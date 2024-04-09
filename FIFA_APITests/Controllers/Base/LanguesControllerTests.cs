using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using FIFA_APITests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public class LanguesControllerTests
    {
        private IActionResult PutTest(int id, Langue? langue, Langue newLangue)
        {
            var mockRepo = new Mock<IManagerLangue>();
            if (langue is not null)
            {
                mockRepo.Setup(m => m.Exists(langue.Id)).ReturnsAsync(true);
                mockRepo.Setup(m => m.GetById(langue.Id)).ReturnsAsync(langue);
                mockRepo.Setup(m => m.Update(newLangue)).Returns(() =>
                    Task.Run(() =>
                    {
                        langue.Id = newLangue.Id;
                        langue.Nom = newLangue.Nom;
                        langue.Utilisateurs = newLangue.Utilisateurs;
                    })
                );
            }

            var controller = new LanguesController(mockRepo.Object)
                .Validating(newLangue);

            return controller.PutLangue(id, newLangue).Result;
        }

        [TestMethod]
        public void LanguesControllerTest_Moq_NoException()
        {
            var mockRepo = new Mock<IManagerLangue>();
            var controller = new LanguesController(mockRepo.Object);
        }

        [TestMethod]
        public void GetLanguesTest_Moq_RightItems()
        {
            List<Langue> langues = new()
            {
                new() { Id = 1, Nom = "Langue1" },
                new() { Id = 2, Nom = "Langue2" },
                new() { Id = 3, Nom = "Langue3" },
            };

            var mockRepo = new Mock<IManagerLangue>();
            mockRepo.Setup(m => m.GetAll()).ReturnsAsync(() => langues);
            var controller = new LanguesController(mockRepo.Object);

            var result = controller.GetLangues().Result;

            TestUtils.ActionResultShouldGive(result, langues);
        }

        [TestMethod]
        public void GetLangueTest_Moq_RightItem()
        {
            Langue langue = new() { Id = 1, Nom = "Langue1" };

            var mockRepo = new Mock<IManagerLangue>();
            mockRepo.Setup(m => m.GetById(langue.Id)).ReturnsAsync(() => langue);
            var controller = new LanguesController(mockRepo.Object);

            var result = controller.GetLangue(langue.Id).Result;

            TestUtils.ActionResultShouldGive(result, langue);
        }

        [TestMethod]
        public void GetLangueTest_Moq_NotFound()
        {
            var mockRepo = new Mock<IManagerLangue>();
            var controller = new LanguesController(mockRepo.Object);

            var result = controller.GetLangue(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutLangueTest_Moq_NoContent()
        {
            Langue langue = new() { Id = 1, Nom = "Langue1" };
            Langue newLangue = new() { Id = 1, Nom = "Langue2" };

            var result = PutTest(langue.Id, langue, newLangue);

            result.Should().BeOfType<NoContentResult>();
            langue.Should().Be(newLangue);
        }

        [TestMethod]
        public void PutLangueTest_Moq_InvalidModelState_BadRequest()
        {
            Langue langue = new() { Id = 1, Nom = "Langue1" };
            Langue newLangue = new() { Id = 1 };

            var result = PutTest(langue.Id, langue, newLangue);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutLangueTest_Moq_InvalidId_BadRequest()
        {
            Langue langue = new() { Id = 1, Nom = "Langue1" };
            Langue newLangue = new() { Id = 2, Nom = "Langue2" };

            var result = PutTest(langue.Id, langue, newLangue);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutLangueTest_Moq_UnknownId_NotFound()
        {
            Langue newLangue = new() { Id = 1, Nom = "Langue2" };

            var result = PutTest(newLangue.Id, null, newLangue);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostLangueTest_Moq_InvalidModelState_BadRequest()
        {
            Langue langue = new() { Id = 1 };

            var mockRepo = new Mock<IManagerLangue>();
            var controller = new LanguesController(mockRepo.Object)
                .Validating(langue);

            var result = controller.PostLangue(langue).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostLangueTest_Moq_CreatedAt()
        {
            Langue langue = new() { Id = 1, Nom = "Langue1" };

            var mockRepo = new Mock<IManagerLangue>();
            var controller = new LanguesController(mockRepo.Object)
                .Validating(langue);

            var result = controller.PostLangue(langue).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Langue>(result, langue);
        }

        [TestMethod]
        public void DeleteLangueTest_Moq_NoContent()
        {
            Langue langue = new() { Id = 1, Nom = "Langue1" };

            var mockRepo = new Mock<IManagerLangue>();
            mockRepo.Setup(m => m.GetById(langue.Id)).ReturnsAsync(() => langue);
            var controller = new LanguesController(mockRepo.Object);

            var result = controller.DeleteLangue(langue.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteLangueTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerLangue>();
            var controller = new LanguesController(mockRepo.Object);

            var result = controller.DeleteLangue(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}