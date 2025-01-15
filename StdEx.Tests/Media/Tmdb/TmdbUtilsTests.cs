using Shouldly;
using StdEx.Media.Tmdb;
using StdEx.Media.Tmdb.Models;
using StdEx.Serialization;

namespace StdEx.Tests.Media.Tmdb;

public class TmdbUtilsTests
{
    private readonly TmdbUtils _tmdbUtils;

    public TmdbUtilsTests()
    {
        var config = LoadTmdbConfig();
        _tmdbUtils = new TmdbUtils(config);
    }

    private TmdbConfig LoadTmdbConfig()
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "tmdbsettings.local.json");
        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException(
                "Please create tmdbsettings.local.json based on tmdbsettings.example.json");
        }

        var json = File.ReadAllText(configPath);
        var config = JsonUtils.Deserialize<TmdbConfig>(json);
        config.ShouldNotBeNull("Failed to load TMDB configuration");

        return config;
    }

    [Fact]
    public async Task GetMovieNfo_ShouldWork()
    {
        var movieName = "Inception";
        var result = await _tmdbUtils.GetMovieNfo(movieName);

        result.ShouldNotBeNull();
        result.Title.ShouldNotBeNullOrEmpty();
        result.Plot.ShouldNotBeNullOrEmpty();
        result.Year.ShouldBeGreaterThan(0);
        result.Ratings.Rating.Value.ShouldBeGreaterThan(0);
        result.Id.ShouldBeGreaterThan(0);
        result.Art.Poster.ShouldNotBeNullOrEmpty();
        result.Art.Fanart.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetMovieNfo_WithInvalidMovie_ShouldThrow()
    {
        var invalidMovieName = "ThisMovieDoesNotExist12345";

        var exception = await Should.ThrowAsync<Exception>(async () =>
            await _tmdbUtils.GetMovieNfo(invalidMovieName));

        exception.Message.ShouldBe($"Movie not found: {invalidMovieName}");
    }

    [Fact]
    public async Task GetMovieNfo_ShouldGenerateValidXml()
    {
        var movieName = "Inception";
        var movieNfo = await _tmdbUtils.GetMovieNfo(movieName);
        var xml = XmlUtils.Serialize(movieNfo);

        xml.ShouldNotBeNullOrEmpty();
        xml.ShouldContain("<title>");
        xml.ShouldContain("<plot>");
        xml.ShouldContain("<year>");
        xml.ShouldContain("<ratings>");
        xml.ShouldContain("<rating name=\"tmdb\" max=\"10\">");
        xml.ShouldContain("<art>");
        xml.ShouldContain("<poster>");
        xml.ShouldContain("<fanart>");
    }
}