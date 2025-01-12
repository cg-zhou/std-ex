using Newtonsoft.Json;
using System.Collections.Generic;

namespace StdEx.Media.Tmdb.Models
{
    public class TmdbCredits
    {
        [JsonProperty("crew")]
        public List<TmdbCrew> Crew { get; set; }
    }
}