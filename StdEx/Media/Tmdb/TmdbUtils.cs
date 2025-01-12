using Newtonsoft.Json;
using StdEx.Media.Tmdb.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StdEx.Media.Tmdb
{
    public class TmdbUtils
    {
        private readonly HttpClient _httpClient;
        private readonly string _bearerToken;
        private const string BaseApiUrl = "http://api.tmdb.org/3";
        private const string BaseImageUrl = "https://image.tmdb.org/t/p/original";

        public TmdbUtils(string bearerToken, int timeoutSeconds = 10)
        {
            _bearerToken = bearerToken;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };

            // TMDB API v4 standard headers
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");
        }

        public async Task<string> GenerateMovieNfo(string movieName)
        {
            var searchUrl = $"{BaseApiUrl}/search/movie?query={Uri.EscapeDataString(movieName)}";
            var searchResponse = await GetJsonAsync<TmdbSearchResponse>(searchUrl);

            if (searchResponse?.Results == null || !searchResponse.Results.Any())
            {
                throw new Exception($"Movie not found: {movieName}");
            }

            var movieId = searchResponse.Results.First().Id;
            var movieUrl = $"{BaseApiUrl}/movie/{movieId}?append_to_response=credits";
            var movie = await GetJsonAsync<TmdbMovie>(movieUrl);

            return CreateNfoXml(movie);
        }

        private async Task<T> GetJsonAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"API request failed: {response.StatusCode}, Content: {content}");
                }

                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                throw new Exception($"Request failed: {url}", ex);
            }
        }

        private string CreateNfoXml(TmdbMovie movie)
        {
            var doc = new XDocument(
                new XElement("movie",
                    new XElement("title", movie.Title),
                    new XElement("originaltitle", movie.OriginalTitle),
                    new XElement("sorttitle", movie.Title),
                    new XElement("rating", movie.VoteAverage),
                    new XElement("year", DateTime.Parse(movie.ReleaseDate).Year),
                    new XElement("plot", movie.Overview),
                    new XElement("thumb", $"{BaseImageUrl}{movie.PosterPath}"),
                    new XElement("fanart", $"{BaseImageUrl}{movie.BackdropPath}"),
                    new XElement("id", movie.Id),
                    new XElement("genre", string.Join(" / ", movie.Genres.Select(g => g.Name))),
                    new XElement("director", string.Join(" / ", movie.Credits.Crew
                        .Where(c => c.Job == "Director")
                        .Select(c => c.Name))),
                    new XElement("premiered", movie.ReleaseDate)
                ));

            return doc.ToString();
        }
    }
}