namespace StdEx.Media.Tmdb.Models
{
    public class TmdbConfig
    {
        public const string DefaultBaseApiUrl = "http://api.tmdb.org/3";
        public const string DefaultBaseImageUrl = "https://image.tmdb.org/t/p/original";
        public const string DefaultLanguage = "zh-CN";

        public string BearerToken { get; set; } = string.Empty;
        public string BaseApiUrl { get; set; } = DefaultBaseApiUrl;
        public string BaseImageUrl { get; set; } = DefaultBaseImageUrl;
        public string Language { get; set; } = DefaultLanguage;
    }
}