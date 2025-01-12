using Newtonsoft.Json;

namespace StdEx.Media.Tmdb.Models
{
    public class TmdbCrew
    {
        [JsonProperty("job")]
        public string Job { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}