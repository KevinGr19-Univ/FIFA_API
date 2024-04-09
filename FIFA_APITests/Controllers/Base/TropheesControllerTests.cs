using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using FIFA_APITests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sprache;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public class TropheesControllerTests
    {
        private IActionResult PutTest(int id, Trophee? trophee, Trophee newTrophee)
        {
            var mockRepo = new Mock<IManagerTrophee>();
            if (trophee is not null)
            {
                mockRepo.Setup(m => m.Exists(trophee.Id)).ReturnsAsync(true);
                mockRepo.Setup(m => m.GetById(trophee.Id)).ReturnsAsync(trophee);
                mockRepo.Setup(m => m.Update(newTrophee)).Returns(() =>
                    Task.Run(() =>
                    {
                        trophee.Id = newTrophee.Id;
                        trophee.Nom = newTrophee.Nom;
                        trophee.Joueurs = newTrophee.Joueurs;
                    })
                );
            }

            var controller = new TropheesController(mockRepo.Object);

            return controller.PutTrophee(id, newTrophee).Result;
        }

        [TestMethod]
        public void TropheesControllerTest_Moq_NoException()
        {
            var mockRepo = new Mock<IManagerTrophee>();
            var controller = new TropheesController(mockRepo.Object);
        }

        [TestMethod]
        public void GetTropheesTest_Moq_RightItems()
        {
            List<Trophee> trophees = new()
            {
                new() { Id = 1, Nom = "Trophee1" },
                new() { Id = 2, Nom = "Trophee2" },
                new() { Id = 3, Nom = "Trophee3" },
            };

            var mockRepo = new Mock<IManagerTrophee>();
            mockRepo.Setup(m => m.GetAll()).ReturnsAsync(() => trophees);
            var controller = new TropheesController(mockRepo.Object);

            var result = controller.GetTrophees().Result;

            TestUtils.ActionResultShouldGive(result, trophees);
        }

        [TestMethod]
        public void GetTropheeTest_Moq_RightItem()
        {
            Trophee trophee = new() { Id = 1, Nom = "Trophee1" };

            var mockRepo = new Mock<IManagerTrophee>();
            mockRepo.Setup(m => m.GetById(trophee.Id)).ReturnsAsync(() => trophee);
            var controller = new TropheesController(mockRepo.Object);

            var result = controller.GetTrophee(trophee.Id).Result;

            TestUtils.ActionResultShouldGive(result, trophee);
        }

        [TestMethod]
        public void GetTropheeTest_Moq_NotFound()
        {
            var mockRepo = new Mock<IManagerTrophee>();
            var controller = new TropheesController(mockRepo.Object);

            var result = controller.GetTrophee(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutTropheeTest_Moq_NoContent()
        {
            Trophee trophee = new() { Id = 1, Nom = "Trophee1" };
            Trophee newTrophee = new() { Id = 1, Nom = "Trophee2" };

            var result = PutTest(trophee.Id, trophee, newTrophee);

            result.Should().BeOfType<NoContentResult>();
            trophee.Should().Be(newTrophee);
        }

        [TestMethod]
        public void PutTropheeTest_Moq_InvalidModelState_BadRequest()
        {
            Trophee trophee = new() { Id = 1, Nom = "Trophee1" };
            Trophee newTrophee = new() { Id = 1 };

            var result = PutTest(trophee.Id, trophee, newTrophee);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutTropheeTest_Moq_InvalidId_BadRequest()
        {
            Trophee trophee = new() { Id = 1, Nom = "Trophee1" };
            Trophee newTrophee = new() { Id = 2, Nom = "Trophee2" };

            var result = PutTest(trophee.Id, trophee, newTrophee);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutTropheeTest_Moq_UnknownId_NotFound()
        {
            Trophee newTrophee = new() { Id = 1, Nom = "Trophee2" };

            var result = PutTest(newTrophee.Id, null, newTrophee);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostTropheeTest_Moq_InvalidModelState_BadRequest()
        {
            Trophee trophee = new() { Id = 1 };

            var mockRepo = new Mock<IManagerTrophee>();
            var controller = new TropheesController(mockRepo.Object);

            var result = controller.PostTrophee(trophee).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostTropheeTest_Moq_CreatedAt()
        {
            Trophee trophee = new() { Id = 1, Nom = "Trophee1" };

            var mockRepo = new Mock<IManagerTrophee>();
            var controller = new TropheesController(mockRepo.Object);

            var result = controller.PostTrophee(trophee).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Trophee>(result, trophee);

        }

        [TestMethod]
        public void DeleteTropheeTest_Moq_NoContent()
        {
            Trophee trophee = new() { Id = 1, Nom = "Trophee1" };

            var mockRepo = new Mock<IManagerTrophee>();
            mockRepo.Setup(m => m.GetById(trophee.Id)).ReturnsAsync(() => trophee);
            var controller = new TropheesController(mockRepo.Object);

            var result = controller.DeleteTrophee(trophee.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteTropheeTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerTrophee>();
            var controller = new TropheesController(mockRepo.Object);

            var result = controller.DeleteTrophee(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void GetTropheeJoueursTest_Moq_RightItems()
        {
            List<Joueur> joueurs = new()
            {
                new() { Id = 1, Nom = "Joueur1" },
                new() { Id = 2, Nom = "Joueur2" },
                new() { Id = 3, Nom = "Joueur3" },
            };
            Trophee trophee = new() { Id = 1, Nom = "Trophee1", Joueurs = joueurs };

            var mockRepo = new Mock<IManagerTrophee>();
            mockRepo.Setup(m => m.GetByIdWithJoueurs(trophee.Id)).ReturnsAsync(trophee);
            var controller = new TropheesController(mockRepo.Object);

            var result = controller.GetTropheeJoueurs(trophee.Id).Result;

            TestUtils.ActionResultShouldGive(result, joueurs);
        }
    }
}