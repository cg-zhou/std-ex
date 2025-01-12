using System.Xml.Serialization;

namespace StdEx.Media.Tmdb.Models
{
    [XmlRoot("movie")]
    public class MovieNfo
    {
        [XmlElement("title")]
        public string Title { get; set; } = string.Empty;

        [XmlElement("originaltitle")]
        public string OriginalTitle { get; set; } = string.Empty;

        [XmlElement("sorttitle")]
        public string SortTitle { get; set; } = string.Empty;

        [XmlElement("ratings")]
        public Ratings Ratings { get; set; } = new Ratings();

        [XmlElement("year")]
        public int Year { get; set; }

        [XmlElement("plot")]
        public string Plot { get; set; } = string.Empty;

        [XmlElement("art")]
        public Art Art { get; set; } = new Art();

        [XmlElement("tmdbid")]
        public int Id { get; set; }

        [XmlElement("genre")]
        public string Genre { get; set; } = string.Empty;

        [XmlElement("director")]
        public string Director { get; set; } = string.Empty;

        [XmlElement("premiered")]
        public string Premiered { get; set; } = string.Empty;
    }

    public class Ratings
    {
        [XmlElement("rating")]
        public Rating Rating { get; set; } = new Rating();
    }

    public class Rating
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = "tmdb";

        [XmlAttribute("max")]
        public int Max { get; set; } = 10;

        [XmlText]
        public double Value { get; set; }
    }

    public class Art
    {
        [XmlElement("poster")]
        public string Poster { get; set; } = string.Empty;

        [XmlElement("thumb")]
        public string LocalPoster { get; set; } = string.Empty;

        [XmlElement("fanart")]
        public string Fanart { get; set; } = string.Empty;

        [XmlElement("backdrop")]
        public string LocalFanart { get; set; } = string.Empty;
    }
} 