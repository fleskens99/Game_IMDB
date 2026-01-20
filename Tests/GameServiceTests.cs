using DTOs;
using Interfaces;
using Moq;
using Services;
using FluentAssertions;
using System;
using System.Collections.Generic;

namespace ServiceTests
{
    public class GameServiceTests
    {
        // --------------------
        // ADD GAME
        // --------------------

        [Fact]
        public void AddGame_ValidGame_TrimsFieldsAndCallsRepo()
        {
            var game = new GameDTO
            {
                Name = "  Test Game  ",
                Category = "  RPG  ",
                Description = "  Description  "
            };

            var repo = new Mock<IGameRepo>();
            repo.Setup(r => r.AddGame(It.IsAny<GameDTO>())).Returns(1);

            var service = new GameService(repo.Object);

            var result = service.AddGame(game);

            Assert.Equal(1, result);

            repo.Verify(r => r.AddGame(It.Is<GameDTO>(g =>
                g.Name == "Test Game" &&
                g.Category == "RPG" &&
                g.Description == "Description"
            )), Times.Once);
        }

        [Fact]
        public void AddGame_NullGame_ThrowsArgumentNullException()
        {
            var service = new GameService(new Mock<IGameRepo>().Object);

            Assert.Throws<ArgumentNullException>(() => service.AddGame(null));
        }

        [Fact]
        public void AddGame_EmptyName_ThrowsArgumentException()
        {
            var game = new GameDTO
            {
                Name = "",
                Category = "RPG",
                Description = "Desc"
            };

            var service = new GameService(new Mock<IGameRepo>().Object);

            Assert.Throws<ArgumentException>(() => service.AddGame(game));
        }

        [Fact]
        public void AddGame_EmptyCategory_ThrowsArgumentException()
        {
            var game = new GameDTO
            {
                Name = "Game",
                Category = " ",
                Description = "Desc"
            };

            var service = new GameService(new Mock<IGameRepo>().Object);

            Assert.Throws<ArgumentException>(() => service.AddGame(game));
        }

        [Fact]
        public void AddGame_EmptyDescription_ThrowsArgumentException()
        {
            var game = new GameDTO
            {
                Name = "Game",
                Category = "RPG",
                Description = null
            };

            var service = new GameService(new Mock<IGameRepo>().Object);

            Assert.Throws<ArgumentException>(() => service.AddGame(game));
        }

        // --------------------
        // GET GAMES
        // --------------------

        [Fact]
        public void GetGames_GamesExist_ReturnsGames()
        {
            var games = new List<GameDTO>
            {
                new GameDTO { Id = 1, Name = "Game", Category = "Cat", Description = "Desc" }
            };

            var repo = new Mock<IGameRepo>();
            repo.Setup(r => r.GetGames()).Returns(games);

            var service = new GameService(repo.Object);

            var result = service.GetGames();

            Assert.Single(result);
            repo.Verify(r => r.GetGames(), Times.Once);
        }

        [Fact]
        public void GetGames_NoGames_ThrowsKeyNotFoundException()
        {
            var repo = new Mock<IGameRepo>();
            repo.Setup(r => r.GetGames()).Returns(new List<GameDTO>());

            var service = new GameService(repo.Object);

            Assert.Throws<KeyNotFoundException>(() => service.GetGames());
        }

        // --------------------
        // GET GAME BY ID
        // --------------------

        [Fact]
        public void GetGameById_InvalidId_ThrowsArgumentException()
        {
            var service = new GameService(new Mock<IGameRepo>().Object);

            Assert.Throws<ArgumentException>(() => service.GetGameById(0));
        }

        [Fact]
        public void GetGameById_GameNotFound_ThrowsKeyNotFoundException()
        {
            var repo = new Mock<IGameRepo>();
            repo.Setup(r => r.GetGameById(1)).Returns((GameDTO)null!);

            var service = new GameService(repo.Object);
            
        
            Assert.Throws<KeyNotFoundException>(() => service.GetGameById(1));
        }

        [Fact]
        public void GetGameById_ValidId_ReturnsGame()
        {
            var game = new GameDTO { Id = 1, Name = "Game"};

            var repo = new Mock<IGameRepo>();
            repo.Setup(r => r.GetGameById(1)).Returns(game);

            var service = new GameService(repo.Object);

            var result = service.GetGameById(1);

            game.Should().BeEquivalentTo(result);

        }

        // --------------------
        // EDIT GAME
        // --------------------

        [Fact]
        public void EditGame_ValidGame_TrimsAndCallsRepo()
        {
            var game = new GameDTO
            {
                Name = " Game ",
                Category = " Cat ",
                Description = " Desc "
            };

            var repo = new Mock<IGameRepo>();
            var service = new GameService(repo.Object);

            service.EditGame(game);

            repo.Verify(r => r.EditGame(It.Is<GameDTO>(g =>
                g.Name == "Game" &&
                g.Category == "Cat" &&
                g.Description == "Desc"
            )), Times.Once);
        }

        [Fact]
        public void EditGame_NullGame_ThrowsArgumentNullException()
        {
            var service = new GameService(new Mock<IGameRepo>().Object);

            Assert.Throws<ArgumentNullException>(() => service.EditGame(null));
        }

        // --------------------
        // DELETE GAME
        // --------------------

        [Fact]
        public void DeleteGame_InvalidId_ThrowsArgumentException()
        {
            var service = new GameService(new Mock<IGameRepo>().Object);

            Assert.Throws<ArgumentException>(() => service.DeleteGame(-1));
        }

        [Fact]
        public void DeleteGame_GameNotFound_ThrowsKeyNotFoundException()
        {
            var repo = new Mock<IGameRepo>();
            repo.Setup(r => r.GetGameById(1)).Returns((GameDTO)null);

            var service = new GameService(repo.Object);

            Assert.Throws<KeyNotFoundException>(() => service.DeleteGame(1));
        }

        [Fact]
        public void DeleteGame_ValidGame_CallsDelete()
        {
            var repo = new Mock<IGameRepo>();
            repo.Setup(r => r.GetGameById(1)).Returns(new GameDTO { Id = 1 });

            var service = new GameService(repo.Object);

            service.DeleteGame(1);

            repo.Verify(r => r.DeleteGame(1), Times.Once);
        }

        // --------------------
        // GET IMAGE BLOB
        // --------------------

        [Fact]
        public void GetImageBlob_ValidId_ReturnsImage()
        {
            var bytes = new byte[] { 1, 2, 3 };

            var repo = new Mock<IGameRepo>();
            repo.Setup(r => r.GetImageBlob(1)).Returns(bytes);

            var service = new GameService(repo.Object);

            var result = service.GetImageBlob(1);

            Assert.Equal(bytes, result);
        }

        [Fact]
        public void GetImageBlob_InvalidId_ThrowsArgumentException()
        {
            var service = new GameService(new Mock<IGameRepo>().Object);

            Assert.Throws<ArgumentException>(() => service.GetImageBlob(0));
        }

        // --------------------
        // CAN EDIT GAME
        // --------------------

        [Fact]
        public void CanEditGame_Admin_ReturnsTrue()
        {
            var service = new GameService(new Mock<IGameRepo>().Object);

            var result = service.CanEditGame(1, 1, true);

            Assert.True(result);
        }

        [Fact]
        public void CanEditGame_Owner_ReturnsTrue()
        {
            var repo = new Mock<IGameRepo>();
            repo.Setup(r => r.GetGameById(1))
                .Returns(new GameDTO { CreatedByUserId = 10 });

            var service = new GameService(repo.Object);

            var result = service.CanEditGame(1, 10, false);

            Assert.True(result);
        }

        [Fact]
        public void CanEditGame_NotOwner_ReturnsFalse()
        {
            var repo = new Mock<IGameRepo>();
            repo.Setup(r => r.GetGameById(1))
                .Returns(new GameDTO { CreatedByUserId = 5 });

            var service = new GameService(repo.Object);

            var result = service.CanEditGame(1, 10, false);

            Assert.False(result);
        }
    }
}
