using Shouldly;
using StdEx.Media.Tmdb;
using StdEx.Media.Tmdb.Models;
using StdEx.Serialization;
using System.IO;

namespace StdEx.Tests.Media.Tmdb
{
    public class TmdbUtilsTests
    {
        private readonly TmdbUtils _tmdbUtils;

        public TmdbUtilsTests()
        {
            var config = LoadTmdbConfig();
            _tmdbUtils = new TmdbUtils(config.BearerToken);
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
        public async Task GenerateMovieNfo_ShouldWork()
        {
            // Arrange
            var movieName = "Inception";

            // Act
            var result = await _tmdbUtils.GenerateMovieNfo(movieName);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain("<title>");
            result.ShouldContain("<plot>");
            result.ShouldContain("<year>");
        }

        [Fact]
        public async Task GenerateMovieNfo_WithInvalidMovie_ShouldThrow()
        {
            // Arrange
            var invalidMovieName = "ThisMovieDoesNotExist12345";

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(async () =>
                await _tmdbUtils.GenerateMovieNfo(invalidMovieName));

            exception.Message.ShouldBe($"Movie not found: {invalidMovieName}");
        }
    }
}