using Newtonsoft.Json;

namespace StdEx.Media.Tmdb.Models
{
    public class TmdbSearchResult
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}