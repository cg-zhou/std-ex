using Newtonsoft.Json;

namespace StdEx.Media.Tmdb.Models
{
    public class TmdbGenre
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}