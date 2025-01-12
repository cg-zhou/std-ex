using Newtonsoft.Json;
using System.Collections.Generic;

namespace StdEx.Media.Tmdb.Models
{
    public class TmdbSearchResponse
    {
        [JsonProperty("results")]
        public List<TmdbSearchResult> Results { get; set; } = new List<TmdbSearchResult>();
    }

    public class TmdbSearchResult
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}