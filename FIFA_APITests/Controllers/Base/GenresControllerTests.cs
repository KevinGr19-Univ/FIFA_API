using Microsoft.VisualStudio.TestTools.UnitTesting;
using FIFA_API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIFA_API.Models.EntityFramework;
using Moq;
using FIFA_API.Repositories.Contracts;
using FIFA_APITests.Utils;
using FIFA_API.Migrations;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public class GenresControllerTests
    {
        private Genre Generate(int id, bool visible)
        {
            return new()
            {
                Id = id,
                Visible = visible,
                Nom = $"Genre{id}"
            };
        }

        private void GetAllTest(bool onlyVisible)
        {
            List<Genre> genres = new()
            {
                Generate(1, true),
                Generate(2, true),
                Generate(3, false),
            };
            IEnumerable<Genre> visibles = genres.Where(c => c.Visible);

            var mockRepo = new Mock<IManagerGenre>();
            mockRepo.Setup(m => m.GetAll(false)).ReturnsAsync(genres);
            mockRepo.Setup(m => m.GetAll(true)).ReturnsAsync(visibles);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new GenresController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetGenres().Result;

            TestUtils.ActionResultShouldGive(result, onlyVisible ? visibles : genres);
        }

        private void GetTest(bool genreVisible, bool onlyVisible)
        {
            Genre genre = Generate(1, genreVisible);
            bool see = genreVisible || !onlyVisible;

            var mockRepo = new Mock<IManagerGenre>();
            mockRepo.Setup(m => m.GetById(genre.Id, false)).ReturnsAsync(genre);
            mockRepo.Setup(m => m.GetById(genre.Id, true)).ReturnsAsync(see ? genre : null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new GenresController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetGenre(genre.Id).Result;

            if (see) TestUtils.ActionResultShouldGive(result, genre);
            else result.Result.Should().BeOfType<NotFoundResult>();
        }

        private IActionResult PutTest(int id, Genre? genre, Genre newGenre)
        {
            var mockRepo = new Mock<IManagerGenre>();
            if (genre is not null)
            {
                mockRepo.Setup(m => m.Exists(genre.Id)).ReturnsAsync(true);
                mockRepo.Setup(m => m.GetById(genre.Id, It.IsAny<bool>())).ReturnsAsync(genre);
                mockRepo.Setup(m => m.Update(newGenre)).Returns(() =>
                    Task.Run(() =>
                    {
                        genre.Id = newGenre.Id;
                        genre.Nom = newGenre.Nom;
                        genre.Produits = newGenre.Produits;
                        genre.Visible = newGenre.Visible;
                    })
                );
            }

            var controller = new GenresController(mockRepo.Object)
                .Validating(newGenre);

            return controller.PutGenre(id, newGenre).Result;
        }

        [TestMethod]
        public void GenresControllerTest()
        {
            var mockRepo = new Mock<IManagerGenre>();
            var controller = new GenresController(mockRepo.Object);
        }

        [TestMethod]
        public void GetGenresTest_Moq_OnlyVisible_RightItems()
        {
            GetAllTest(onlyVisible: true);
        }

        [TestMethod]
        public void GetGenresTest_Moq_SeeAll_RightItems()
        {
            GetAllTest(onlyVisible: false);
        }

        [TestMethod]
        public void GetGenreTest_Moq_Visible_Admin_RightItem()
        {
            GetTest(genreVisible: true, onlyVisible: false);
        }

        [TestMethod]
        public void GetGenreTest_Moq_Visible_User_RightItem()
        {
            GetTest(genreVisible: true, onlyVisible: true);
        }

        [TestMethod]
        public void GetGenreTest_Moq_NotVisible_User_NotFound()
        {
            GetTest(genreVisible: false, onlyVisible: true);
        }

        [TestMethod]
        public void GetGenreTest_Moq_NotVisible_Admin_RightItem()
        {
            GetTest(genreVisible: false, onlyVisible: false);
        }

        [TestMethod]
        public void GetGenreTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerGenre>();
            mockRepo.Setup(m => m.GetById(1, It.IsAny<bool>())).ReturnsAsync(() => null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, true);
            var controller = new GenresController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetGenre(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutGenreTest_Moq_InvalidModelState_BadRequest()
        {
            Genre genre = Generate(1, false);
            Genre newGenre = new() { Id = 1 };

            var result = PutTest(genre.Id, genre, newGenre);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutGenreTest_Moq_InvalidId_BadRequest()
        {
            Genre genre = Generate(1, false);
            Genre newGenre = Generate(2, false);

            var result = PutTest(genre.Id, genre, newGenre);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutGenreTest_Moq_UnknownId_NotFound()
        {
            Genre newGenre = Generate(1, false);

            var result = PutTest(newGenre.Id, null, newGenre);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostGenreTest_Moq_InvalidModelState_BadRequest()
        {
            Genre genre = new() { Id = 1 };

            var mockRepo = new Mock<IManagerGenre>();
            var controller = new GenresController(mockRepo.Object)
                .Validating(genre);

            var result = controller.PostGenre(genre).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostGenreTest_Moq_CreatedAt()
        {
            Genre genre = Generate(1, false);

            var mockRepo = new Mock<IManagerGenre>();
            var controller = new GenresController(mockRepo.Object)
                .Validating(genre);

            var result = controller.PostGenre(genre).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Genre>(result, genre);
        }

        [TestMethod]
        public void DeleteGenreTest_Moq_NoContent()
        {
            Genre genre = Generate(1, false);

            var mockRepo = new Mock<IManagerGenre>();
            mockRepo.Setup(m => m.GetById(genre.Id, It.IsAny<bool>())).ReturnsAsync(() => genre);
            var controller = new GenresController(mockRepo.Object);

            var result = controller.DeleteGenre(genre.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteGenreTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerGenre>();
            var controller = new GenresController(mockRepo.Object);

            var result = controller.DeleteGenre(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}