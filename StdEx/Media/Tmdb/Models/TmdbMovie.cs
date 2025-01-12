#nullable disable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace StdEx.Media.Tmdb.Models
{
    public class TmdbMovie
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("genres")]
        public List<TmdbGenre> Genres { get; set; }

        [JsonProperty("credits")]
        public TmdbCredits Credits { get; set; }
    }

    public class TmdbGenre
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class TmdbCredits
    {
        [JsonProperty("crew")]
        public List<TmdbCrew> Crew { get; set; }
    }

    public class TmdbCrew
    {
        [JsonProperty("job")]
        public string Job { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}