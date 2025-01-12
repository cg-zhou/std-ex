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

        [XmlElement("rating")]
        public double Rating { get; set; }

        [XmlElement("year")]
        public int Year { get; set; }

        [XmlElement("plot")]
        public string Plot { get; set; } = string.Empty;

        [XmlElement("thumb")]
        public string Thumb { get; set; } = string.Empty;

        [XmlElement("fanart")]
        public string Fanart { get; set; } = string.Empty;

        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("genre")]
        public string Genre { get; set; } = string.Empty;

        [XmlElement("director")]
        public string Director { get; set; } = string.Empty;

        [XmlElement("premiered")]
        public string Premiered { get; set; } = string.Empty;
    }
} 