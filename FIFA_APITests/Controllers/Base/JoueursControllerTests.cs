using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using FIFA_APITests.Controllers.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public class JoueursControllerTests
    {
        private IActionResult PutTest(int id, Joueur? joueur, Joueur newJoueur)
        {
            var mockUoW = new Mock<IUnitOfWorkJoueur>();
            if (joueur is not null)
            {
                mockUoW.Setup(m => m.Joueurs.Exists(joueur.Id)).ReturnsAsync(true);
                mockUoW.Setup(m => m.Joueurs.GetById(joueur.Id)).ReturnsAsync(joueur);
                mockUoW.Setup(m => m.Joueurs.Update(newJoueur)).Returns(() =>
                    Task.Run(() =>
                    {
                        joueur.Id = newJoueur.Id;
                        joueur.Nom = newJoueur.Nom;
                    })
                );
            }
            else
            {
                mockUoW.Setup(m => m.Joueurs.Exists(id)).ReturnsAsync(false);
                mockUoW.Setup(m => m.Joueurs.GetById(id)).ReturnsAsync(() => null);
            }

            var controller = new JoueursController(mockUoW.Object);

            return controller.PutJoueur(id, newJoueur).Result;
        }

        [TestMethod]
        public void JoueursControllerTest_Moq_NoException()
        {
            var mockUoW = new Mock<IUnitOfWorkJoueur>();
            var controller = new JoueursController(mockUoW.Object);
        }

        [TestMethod]
        public void GetJoueursTest_Moq_RightItems()
        {
            List<Joueur> joueurs = new()
            {
                new() { Id = 1, Nom = "Joueur1" },
                new() { Id = 2, Nom = "Joueur2" },
                new() { Id = 3, Nom = "Joueur3" },
            };

            var mockUoW = new Mock<IUnitOfWorkJoueur>();
            mockUoW.Setup(m => m.Joueurs.GetAll()).ReturnsAsync(() => joueurs);
            var controller = new JoueursController(mockUoW.Object);

            var result = controller.GetJoueurs().Result;

            TestUtils.ActionResultShouldGive(result, joueurs);
        }

        [TestMethod]
        public void GetJoueurTest_Moq_RightItem()
        {
            Joueur joueur = new() { Id = 1, Nom = "Joueur1" };

            var mockUoW = new Mock<IUnitOfWorkJoueur>();
            mockUoW.Setup(m => m.Joueurs.GetByIdWithData(joueur.Id)).ReturnsAsync(() => joueur);
            var controller = new JoueursController(mockUoW.Object);

            var result = controller.GetJoueur(joueur.Id).Result;

            TestUtils.ActionResultShouldGive(result, joueur);
        }

        [TestMethod]
        public void GetJoueurTest_Moq_NotFound()
        {
            var mockUoW = new Mock<IUnitOfWorkJoueur>();
            mockUoW.Setup(m => m.Joueurs.GetByIdWithData(1)).ReturnsAsync(() => null);
            var controller = new JoueursController(mockUoW.Object);

            var result = controller.GetJoueur(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutJoueurTest_Moq_NoContent()
        {
            Joueur joueur = new() { Id = 1, Nom = "Joueur1" };
            Joueur newJoueur = new() { Id = 1, Nom = "Joueur2" };

            var result = PutTest(joueur.Id, joueur, newJoueur);

            result.Should().BeOfType<NoContentResult>();
            joueur.Should().Be(newJoueur);
        }

        [TestMethod]
        public void PutJoueurTest_Moq_InvalidModelState_BadRequest()
        {
            Joueur joueur = new() { Id = 1, Nom = "Joueur1" };
            Joueur newJoueur = new() { Id = 1 };

            var result = PutTest(joueur.Id, joueur, newJoueur);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutJoueurTest_Moq_InvalidId_BadRequest()
        {
            Joueur joueur = new() { Id = 1, Nom = "Joueur1" };
            Joueur newJoueur = new() { Id = 2, Nom = "Joueur2" };

            var result = PutTest(joueur.Id, joueur, newJoueur);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutJoueurTest_Moq_UnknownId_NotFound()
        {
            Joueur newJoueur = new() { Id = 1, Nom = "Joueur2" };

            var result = PutTest(newJoueur.Id, null, newJoueur);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostJoueurTest_Moq_InvalidModelState_BadRequest()
        {
            Joueur joueur = new() { Id = 1 };

            var mockUoW = new Mock<IUnitOfWorkJoueur>();
            mockUoW.Setup(m => m.Joueurs.Add(joueur));
            var controller = new JoueursController(mockUoW.Object);

            var result = controller.PostJoueur(joueur).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostJoueurTest_Moq_CreatedAt()
        {
            Joueur joueur = new() { Id = 1, Nom = "Joueur1" };

            var mockUoW = new Mock<IUnitOfWorkJoueur>();
            mockUoW.Setup(m => m.Joueurs.Add(joueur));
            var controller = new JoueursController(mockUoW.Object);

            var result = controller.PostJoueur(joueur).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Joueur>(result, joueur);
        }

        [TestMethod]
        public void DeleteJoueurTest_Moq_NoContent()
        {
            Joueur joueur = new() { Id = 1, Nom = "Joueur1" };

            var mockUoW = new Mock<IUnitOfWorkJoueur>();
            mockUoW.Setup(m => m.Joueurs.GetById(joueur.Id)).ReturnsAsync(() => joueur);
            var controller = new JoueursController(mockUoW.Object);

            var result = controller.DeleteJoueur(joueur.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteJoueurTest_Moq_UnknownId_NotFound()
        {
            var mockUoW = new Mock<IUnitOfWorkJoueur>();
            mockUoW.Setup(m => m.Joueurs.GetById(1)).ReturnsAsync(() => null);
            var controller = new JoueursController(mockUoW.Object);

            var result = controller.DeleteJoueur(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}