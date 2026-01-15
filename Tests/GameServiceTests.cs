using DTOs;
using Interfaces;
using Moq;
using Services;

public class GameServiceTests
{
    // ---------- AddGame ----------

    [Fact]
    public void AddGame_Valid_ReturnsId()
    {
        // Arrange
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);
        var game = new GameDTO { Name = "Halo", Category = "Shooter" };
        repoMock.Setup(r => r.AddGame(game)).Returns(1);

        // Act
        int result = service.AddGame(game);

        // Assert
        Assert.Equal(1, result);
        repoMock.Verify(r => r.AddGame(game), Times.Once);
        repoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void AddGame_Null_Throws()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);

        Assert.Throws<ArgumentNullException>(() => service.AddGame(null!));
        repoMock.VerifyNoOtherCalls();
    }

    // ---------- DeleteGame ----------

    [Fact]
    public void DeleteGame_Valid_CallsRepository()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);

        service.DeleteGame(5);

        repoMock.Verify(r => r.DeleteGame(5), Times.Once);
        repoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void DeleteGame_InvalidId_Throws()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);

        Assert.Throws<ArgumentException>(() => service.DeleteGame(0));
        repoMock.VerifyNoOtherCalls();
    }

    // ---------- GetGames ----------

    [Fact]
    public void GetGames_ReturnsList()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);
        var games = new List<GameDTO> { new GameDTO { Id = 1, Name = "Halo", Category = "Shooter" } };
        repoMock.Setup(r => r.GetGames()).Returns(games);

        var result = service.GetGames();

        Assert.Single(result);
        Assert.Equal("Halo", result[0].Name);
        repoMock.Verify(r => r.GetGames(), Times.Once);
        repoMock.VerifyNoOtherCalls();
    }

    // ---------- EditGame ----------

    [Fact]
    public void EditGame_Valid_CallsRepository()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);
        var game = new GameDTO { Id = 10, Name = "Celeste", Category = "Platformer" };

        service.EditGame(game);

        repoMock.Verify(r => r.EditGame(game), Times.Once);
        repoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void EditGame_Null_Throws()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);

        Assert.Throws<ArgumentNullException>(() => service.EditGame(null!));
        repoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void EditGame_EmptyName_Throws()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);
        var game = new GameDTO { Id = 10, Name = "   ", Category = "Platformer" };

        Assert.Throws<ArgumentException>(() => service.EditGame(game));
        repoMock.VerifyNoOtherCalls();
    }

    // ---------- GetGameById ----------

    [Fact]
    public void GetGameById_Found_ReturnsGame()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);
        var game = new GameDTO { Id = 5, Name = "Stardew", Category = "Farming" };
        repoMock.Setup(r => r.GetGameById(5)).Returns(game);

        var result = service.GetGameById(5);

        Assert.NotNull(result);
        Assert.Equal(5, result!.Id);
        repoMock.Verify(r => r.GetGameById(5), Times.Once);
        repoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void GetGameById_NotFound_ReturnsNull()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);
        repoMock.Setup(r => r.GetGameById(999)).Returns((GameDTO?)null);

        var result = service.GetGameById(999);

        Assert.Null(result);
        repoMock.Verify(r => r.GetGameById(999), Times.Once);
        repoMock.VerifyNoOtherCalls();
    }

    // ---------- GetImageBlob ----------

    [Fact]
    public void GetImageBlob_Found_ReturnsBytes()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);
        byte[] blob = { 1, 2, 3 };
        repoMock.Setup(r => r.GetImageBlob(3)).Returns(blob);

        var result = service.GetImageBlob(3);

        Assert.NotNull(result);
        Assert.Equal(blob, result!);
        repoMock.Verify(r => r.GetImageBlob(3), Times.Once);
        repoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void GetImageBlob_NotFound_ReturnsNull()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);
        repoMock.Setup(r => r.GetImageBlob(4)).Returns((byte[]?)null);

        var result = service.GetImageBlob(4);

        Assert.Null(result);
        repoMock.Verify(r => r.GetImageBlob(4), Times.Once);
        repoMock.VerifyNoOtherCalls();
    }

    // ---------- CanEditGame ----------

    [Fact]
    public void CanEditGame_Admin_True()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);

        bool can = service.CanEditGame(gameId: 1, userId: 2, isAdmin: true);

        Assert.True(can);
        repoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void CanEditGame_InvalidUser_NotAdmin_False()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);

        bool can = service.CanEditGame(gameId: 10, userId: 0, isAdmin: false);

        Assert.False(can);
        repoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void CanEditGame_UserMatchesCreator_True()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);
        var game = new GameDTO { Id = 77, CreatedByUserId = 15 };
        repoMock.Setup(r => r.GetGameById(77)).Returns(game);

        bool can = service.CanEditGame(gameId: 77, userId: 15, isAdmin: false);

        Assert.True(can);
        repoMock.Verify(r => r.GetGameById(77), Times.Once);
        repoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void CanEditGame_UserDoesNotMatch_False()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);
        var game = new GameDTO { Id = 77, CreatedByUserId = 15 };
        repoMock.Setup(r => r.GetGameById(77)).Returns(game);

        bool can = service.CanEditGame(gameId: 77, userId: 99, isAdmin: false);

        Assert.False(can);
        repoMock.Verify(r => r.GetGameById(77), Times.Once);
        repoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void CanEditGame_GameNotFound_False()
    {
        var repoMock = new Mock<IGameRepo>();
        var service = new GameService(repoMock.Object);
        repoMock.Setup(r => r.GetGameById(10)).Returns((GameDTO?)null);

        bool can = service.CanEditGame(gameId: 10, userId: 5, isAdmin: false);

        Assert.False(can);
        repoMock.Verify(r => r.GetGameById(10), Times.Once);
        repoMock.VerifyNoOtherCalls();
    }
}
