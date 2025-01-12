using StdEx.Media.Tmdb;

namespace StdEx.Tests.Media.Tmdb
{
    public class TmdbUtilsTests
    {
        private readonly TmdbUtils _tmdbUtils;

        public TmdbUtilsTests()
        {
            // Bearer token here
            const string yourApiBearerToken = "eyJhbGci...";
            _tmdbUtils = new TmdbUtils(yourApiBearerToken);
        }

        [Fact]
        public async Task GenerateMovieNfo_ShouldReturnValidXml()
        {
            // Arrange
            var movieName = "Inception";

            // Act
            var result = await _tmdbUtils.GenerateMovieNfo(movieName);

            // Assert
            Assert.Contains("<title>", result);
            Assert.Contains("<plot>", result);
            Assert.Contains("<year>", result);
        }

        [Fact]
        public async Task GenerateMovieNfo_WithInvalidMovie_ShouldThrowException()
        {
            // Arrange
            var invalidMovieName = "ThisMovieDoesNotExist12345";

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _tmdbUtils.GenerateMovieNfo(invalidMovieName));
        }
    }
}