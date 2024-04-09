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
    public class PaysControllerTests
    {
        private IActionResult PutTest(int id, Pays? pays, Pays newPays)
        {
            var mockRepo = new Mock<IManagerPays>();
            if (pays is not null)
            {
                mockRepo.Setup(m => m.Exists(pays.Id)).ReturnsAsync(true);
                mockRepo.Setup(m => m.GetById(pays.Id)).ReturnsAsync(pays);
                mockRepo.Setup(m => m.Update(newPays)).Returns(() =>
                    Task.Run(() =>
                    {
                        pays.Id = newPays.Id;
                        pays.Nom = newPays.Nom;
                        pays.Utilisateurs = newPays.Utilisateurs;
                    })
                );
            }

            var controller = new PaysController(mockRepo.Object)
                .Validating(newPays);

            return controller.PutPays(id, newPays).Result;
        }

        [TestMethod]
        public void PaysControllerTest_Moq_NoException()
        {
            var mockRepo = new Mock<IManagerPays>();
            var controller = new PaysController(mockRepo.Object);
        }

        [TestMethod]
        public void GetPaysTest_Moq_RightItems()
        {
            List<Pays> pays = new()
            {
                new() { Id = 1, Nom = "Pays1" },
                new() { Id = 2, Nom = "Pays2" },
                new() { Id = 3, Nom = "Pays3" },
            };

            var mockRepo = new Mock<IManagerPays>();
            mockRepo.Setup(m => m.GetAll()).ReturnsAsync(() => pays);
            var controller = new PaysController(mockRepo.Object);

            var result = controller.GetPays().Result;

            TestUtils.ActionResultShouldGive(result, pays);
        }

        [TestMethod]
        public void GetPaysTest_Moq_RightItem()
        {
            Pays pays = new() { Id = 1, Nom = "Pays1" };

            var mockRepo = new Mock<IManagerPays>();
            mockRepo.Setup(m => m.GetById(pays.Id)).ReturnsAsync(() => pays);
            var controller = new PaysController(mockRepo.Object);

            var result = controller.GetPays(pays.Id).Result;

            TestUtils.ActionResultShouldGive(result, pays);
        }

        [TestMethod]
        public void GetPaysTest_Moq_NotFound()
        {
            var mockRepo = new Mock<IManagerPays>();
            var controller = new PaysController(mockRepo.Object);

            var result = controller.GetPays(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutPaysTest_Moq_NoContent()
        {
            Pays pays = new() { Id = 1, Nom = "Pays1" };
            Pays newPays = new() { Id = 1, Nom = "Pays2" };

            var result = PutTest(pays.Id, pays, newPays);

            result.Should().BeOfType<NoContentResult>();
            pays.Should().Be(newPays);
        }

        [TestMethod]
        public void PutPaysTest_Moq_InvalidModelState_BadRequest()
        {
            Pays pays = new() { Id = 1, Nom = "Pays1" };
            Pays newPays = new() { Id = 1 };

            var result = PutTest(pays.Id, pays, newPays);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutPaysTest_Moq_InvalidId_BadRequest()
        {
            Pays pays = new() { Id = 1, Nom = "Pays1" };
            Pays newPays = new() { Id = 2, Nom = "Pays2" };

            var result = PutTest(pays.Id, pays, newPays);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutPaysTest_Moq_UnknownId_NotFound()
        {
            Pays newPays = new() { Id = 1, Nom = "Pays2" };

            var result = PutTest(newPays.Id, null, newPays);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostPaysTest_Moq_InvalidModelState_BadRequest()
        {
            Pays pays = new() { Id = 1 };

            var mockRepo = new Mock<IManagerPays>();
            var controller = new PaysController(mockRepo.Object)
                .Validating(pays);

            var result = controller.PostPays(pays).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostPaysTest_Moq_CreatedAt()
        {
            Pays pays = new() { Id = 1, Nom = "Pays1" };

            var mockRepo = new Mock<IManagerPays>();
            var controller = new PaysController(mockRepo.Object)
                .Validating(pays);

            var result = controller.PostPays(pays).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Pays>(result, pays);
        }

        [TestMethod]
        public void DeletePaysTest_Moq_NoContent()
        {
            Pays pays = new() { Id = 1, Nom = "Pays1" };

            var mockRepo = new Mock<IManagerPays>();
            mockRepo.Setup(m => m.GetById(pays.Id)).ReturnsAsync(() => pays);
            var controller = new PaysController(mockRepo.Object);

            var result = controller.DeletePays(pays.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeletePaysTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerPays>();
            var controller = new PaysController(mockRepo.Object);

            var result = controller.DeletePays(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}