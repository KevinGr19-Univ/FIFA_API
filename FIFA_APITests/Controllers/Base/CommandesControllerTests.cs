using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using FIFA_APITests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FIFA_API.Controllers.Tests
{
    [TestClass()]
    public class CommandesControllerTests
    {
        private Commande Generate(int id, int idUser)
        {
            Random r = new();
            return new()
            {
                Id = id,
                IdUtilisateur = idUser,
                IdTypeLivraison = r.Next(5),
                PrixLivraison = r.Next(500) + 40,
                DateLivraison = DateTime.Now.AddDays(-r.Next(40)),
                UrlFacture = $"URL{id}",
                CodePostalFacturation = "74210",
                CodePostalLivraison = "74210",
                VilleFacturation = "Faverges",
                VilleLivraison = "Faverges",
                RueFacturation = "9 rue de l'Arc-en-Ciel",
                RueLivraison = "9 rue de l'Arc-en-Ciel",
            };
        }

        private void GetTest(bool isOwnCommande, bool isManager)
        {
            Commande commande = Generate(1, 1);
            CommandeDetails commandeDetails = new() { Commande = commande };
            Utilisateur user = new() { Id = isOwnCommande ? 1 : 2 };

            var mockUoW = new Mock<IUnitOfWorkCommande>();
            mockUoW.Setup(m => m.Commandes.GetByIdWithAll(commande.Id)).ReturnsAsync(commande);
            mockUoW.Setup(m => m.GetDetails(commande)).ReturnsAsync(commandeDetails);

            var mockHttpCtx = new MockHttpContext()
                .MockAuthentication(user)
                .MockMatchingPolicy(CommandesController.MANAGER_POLICY, isManager);

            var controller = new CommandesController(mockUoW.Object, null) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetCommande(1).Result;

            if (isOwnCommande || isManager)
                TestUtils.ActionResultShouldGive(result, commandeDetails);
            else
                result.Result.Should().BeOfType<NotFoundResult>();
        }

        private IActionResult PutTest(int id, Commande? commande, Commande newCommande)
        {
            var mockUoW = new Mock<IUnitOfWorkCommande>();
            if (commande is not null)
            {
                mockUoW.Setup(m => m.Commandes.Exists(commande.Id)).ReturnsAsync(true);
                mockUoW.Setup(m => m.Commandes.GetById(commande.Id)).ReturnsAsync(commande);
                mockUoW.Setup(m => m.Commandes.Update(newCommande)).Returns(() =>
                    Task.Run(() =>
                    {
                        commande.Id = newCommande.Id;
                        commande.CodePostalFacturation = newCommande.CodePostalFacturation;
                        commande.CodePostalLivraison = newCommande.CodePostalLivraison;
                        commande.DateCommande = newCommande.DateCommande;
                        commande.RueLivraison = newCommande.RueLivraison;
                        commande.PrixLivraison = newCommande.PrixLivraison;
                        commande.DateExpedition = newCommande.DateExpedition;
                        commande.DateLivraison = newCommande.DateLivraison;
                        commande.IdTypeLivraison = newCommande.IdTypeLivraison;
                        commande.IdUtilisateur = newCommande.IdUtilisateur;
                        commande.UrlFacture = newCommande.UrlFacture;
                        commande.Utilisateur = newCommande.Utilisateur;
                        commande.Status = newCommande.Status;
                        commande.Lignes = newCommande.Lignes;
                        commande.VilleFacturation = newCommande.VilleFacturation;
                        commande.VilleLivraison = newCommande.VilleLivraison;
                    })
                );
            }
            else
            {
                mockUoW.Setup(m => m.Commandes.Exists(id)).ReturnsAsync(false);
                mockUoW.Setup(m => m.Commandes.GetById(id)).ReturnsAsync(() => null);
            }

            var controller = new CommandesController(mockUoW.Object, null)
                .Validating(newCommande);

            return controller.PutCommande(id, newCommande).Result;
        }

        [TestMethod()]
        public void CommandesControllerTest_Moq_NoException()
        {
            var mockUoW = new Mock<IUnitOfWorkCommande>();
            var controller = new CommandesController(mockUoW.Object, null);
        }

        [TestMethod()]
        public void GetCommandesTest_Moq_RightItems()
        {
            List<Commande> commandes = new()
            {
                Generate(1, 1),
                Generate(2, 1),
                Generate(3, 2),
            };

            var mockUoW = new Mock<IUnitOfWorkCommande>();
            mockUoW.Setup(m => m.Commandes.GetAll()).ReturnsAsync(commandes);
            var controller = new CommandesController(mockUoW.Object, null);

            var result = controller.GetCommandes().Result;

            TestUtils.ActionResultShouldGive(result, commandes);
        }

        [TestMethod()]
        public void GetCommandesTest_Moq_User_OwnCommande_RightItem()
        {
            GetTest(isOwnCommande: true, isManager: false);
        }

        [TestMethod()]
        public void GetCommandesTest_Moq_User_NotOwnCommande_NotFound()
        {
            GetTest(isOwnCommande: false, isManager: false);
        }

        [TestMethod()]
        public void GetCommandesTest_Moq_Admin_NotOwnCommande_RightItem()
        {
            GetTest(isOwnCommande: false, isManager: true);
        }

        [TestMethod]
        public void GetCommandesTest_Moq_UnknownId_NotFound()
        {
            Utilisateur user = new() { Id = 1 };

            var mockUoW = new Mock<IUnitOfWorkCommande>();
            mockUoW.Setup(m => m.Commandes.GetByIdWithAll(1));

            var mockHttpCtx = new MockHttpContext().MockAuthentication(user);
            var controller = new CommandesController(mockUoW.Object, null) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetCommande(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod()]
        public void PutCommandeTest_Moq_InvalidModelState_BadRequest()
        {
            Commande commande = Generate(1, 1);
            Commande newCommande = new() { Id = 1 };

            var result = PutTest(commande.Id, commande, newCommande);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod()]
        public void PutCommandeTest_Moq_InvalidId_BadRequest()
        {
            Commande commande = Generate(1, 1);
            Commande newCommande = Generate(2, 1);

            var result = PutTest(commande.Id, commande, newCommande);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod()]
        public void PutCommandeTest_Moq_UnknownId_NotFound()
        {
            Commande newCommande = Generate(1, 1);

            var result = PutTest(newCommande.Id, null, newCommande);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod()]
        public void PutCommandeTest_Moq_NoContent()
        {
            Commande commande = Generate(1, 1);
            Commande newCommande = Generate(1, 1);

            var result = PutTest(commande.Id, commande, newCommande);

            result.Should().BeOfType<NoContentResult>();
            commande.Should().Be(newCommande);
        }

        [TestMethod()]
        public void PostCommandeTest_Moq_InvalidModelState_BadRequest()
        {
            Commande commande = Generate(1, 1);
            commande.CodePostalFacturation = "abcde";

            var mockUoW = new Mock<IUnitOfWorkCommande>();
            mockUoW.Setup(m => m.Commandes.Add(commande));

            var controller = new CommandesController(mockUoW.Object, null)
                .Validating(commande);

            var result = controller.PostCommande(commande).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod()]
        public void PostCommandeTest_Moq_CreatedAt()
        {
            Commande commande = Generate(1, 1);

            var mockUoW = new Mock<IUnitOfWorkCommande>();
            mockUoW.Setup(m => m.Commandes.Add(commande));

            var controller = new CommandesController(mockUoW.Object, null)
                .Validating(commande);

            var result = controller.PostCommande(commande).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Commande>(result, commande);
        }

        [TestMethod()]
        public void DeleteCommandeTest_Moq_UnknownId_NotFound()
        {
            var mockUoW = new Mock<IUnitOfWorkCommande>();
            mockUoW.Setup(m => m.Commandes.GetById(1));
            var controller = new CommandesController(mockUoW.Object, null);

            var result = controller.DeleteCommande(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod()]
        public void DeleteCommandeTest_Moq_NoContent()
        {
            Commande commande = Generate(1, 1);

            var mockUoW = new Mock<IUnitOfWorkCommande>();
            mockUoW.Setup(m => m.Commandes.GetById(commande.Id)).ReturnsAsync(commande);
            mockUoW.Setup(m => m.Commandes.Delete(commande));
            var controller = new CommandesController(mockUoW.Object, null);

            var result = controller.DeleteCommande(commande.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }
    }
}