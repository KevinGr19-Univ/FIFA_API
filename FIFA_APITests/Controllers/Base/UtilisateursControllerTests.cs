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
    public class UtilisateursControllerTests
    {
        private Utilisateur Generate(int id)
        {
            string prenom = PRENOMS.Random(), nom = NOMS.Random();

            byte[] hash = new byte[20];
            new Random().NextBytes(hash);

            return new()
            {
                Id = id,
                Prenom = prenom,
                Surnom = $"{prenom} {nom.ToUpper()}",
                Telephone = "0123456789",
                Mail = $"{prenom.ToLower()}.{nom.ToLower()}@gmail.com",
                HashMotDePasse = Convert.ToBase64String(hash),
                DoubleAuthentification = false,
                IdLangue = 1,
                IdPays = 1
            };
        }

        private IActionResult PutTest(int id, Utilisateur? utilisateur, Utilisateur newUtilisateur)
        {
            var mockRepo = new Mock<IManagerUtilisateur>();
            if (utilisateur is not null)
            {
                mockRepo.Setup(m => m.Exists(utilisateur.Id)).ReturnsAsync(true);
                mockRepo.Setup(m => m.GetById(utilisateur.Id)).ReturnsAsync(utilisateur);
                mockRepo.Setup(m => m.Update(newUtilisateur)).Returns(() =>
                    Task.Run(() =>
                    {
                        utilisateur.Id = newUtilisateur.Id;
                        utilisateur.Prenom = newUtilisateur.Prenom;
                        utilisateur.Surnom = newUtilisateur.Surnom;
                        utilisateur.DateVerificationEmail = newUtilisateur.DateVerificationEmail;
                        utilisateur.DateNaissance = newUtilisateur.DateNaissance;
                        utilisateur.Anonyme = newUtilisateur.Anonyme;
                        utilisateur.DateVerif2FA = newUtilisateur.DateVerif2FA;
                        utilisateur.HashMotDePasse = newUtilisateur.HashMotDePasse;
                        utilisateur.IdLangue = newUtilisateur.IdLangue;
                        utilisateur.IdPays = newUtilisateur.IdPays;
                        utilisateur.Pays = newUtilisateur.Pays;
                        utilisateur.Langue = newUtilisateur.Langue;
                        utilisateur.IdPaysFavori = newUtilisateur.IdPaysFavori;
                        utilisateur.PaysFavori = newUtilisateur.PaysFavori;
                        utilisateur.DoubleAuthentification = newUtilisateur.DoubleAuthentification;
                        utilisateur.Telephone = newUtilisateur.Telephone;
                        utilisateur.Token2FA = newUtilisateur.Token2FA;
                        utilisateur.Commandes = newUtilisateur.Commandes;
                        utilisateur.DerniereConnexion = newUtilisateur.DerniereConnexion;
                        utilisateur.Mail = newUtilisateur.Mail;
                        utilisateur.RefreshToken = newUtilisateur.RefreshToken;
                        utilisateur.Role = newUtilisateur.Role;
                        utilisateur.RefreshToken = newUtilisateur.RefreshToken;
                        utilisateur.Votes = newUtilisateur.Votes;

                        // Automatisable avec reflection mais flemme + mauvaise idée à long terme
                    })
                );
            }

            var controller = new UtilisateursController(mockRepo.Object)
                .Validating(newUtilisateur);

            return controller.PutUtilisateur(id, newUtilisateur).Result;
        }

        [TestMethod]
        public void UtilisateursControllerTest_Moq_NoException()
        {
            var mockRepo = new Mock<IManagerUtilisateur>();
            var controller = new UtilisateursController(mockRepo.Object);
        }

        [TestMethod]
        public void GetUtilisateursTest_Moq_RightItems()
        {
            List<Utilisateur> utilisateurs = new()
            {
                Generate(1),
                Generate(2),
                Generate(3),
            };

            var mockRepo = new Mock<IManagerUtilisateur>();
            mockRepo.Setup(m => m.GetAll()).ReturnsAsync(() => utilisateurs);
            var controller = new UtilisateursController(mockRepo.Object);

            var result = controller.GetUtilisateurs().Result;

            TestUtils.ActionResultShouldGive(result, utilisateurs);
        }

        [TestMethod]
        public void GetUtilisateurTest_Moq_RightItem()
        {
            Utilisateur utilisateur = Generate(1);

            var mockRepo = new Mock<IManagerUtilisateur>();
            mockRepo.Setup(m => m.GetById(utilisateur.Id)).ReturnsAsync(() => utilisateur);
            var controller = new UtilisateursController(mockRepo.Object);
            var result = controller.GetUtilisateur(utilisateur.Id).Result;

            TestUtils.ActionResultShouldGive(result, utilisateur);
        }

        [TestMethod]
        public void GetUtilisateurTest_Moq_NotFound()
        {
            var mockRepo = new Mock<IManagerUtilisateur>();
            var controller = new UtilisateursController(mockRepo.Object);

            var result = controller.GetUtilisateur(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutUtilisateurTest_Moq_NoContent()
        {
            Utilisateur utilisateur = Generate(1);
            Utilisateur newUtilisateur = Generate(1);

            var result = PutTest(utilisateur.Id, utilisateur, newUtilisateur);

            result.Should().BeOfType<NoContentResult>();
            utilisateur.Should().Be(newUtilisateur);
        }

        [TestMethod]
        public void PutUtilisateurTest_Moq_InvalidModelState_BadRequest()
        {
            Utilisateur utilisateur = Generate(1);
            Utilisateur newUtilisateur = new() { Id = 1 };

            var result = PutTest(utilisateur.Id, utilisateur, newUtilisateur);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutUtilisateurTest_Moq_InvalidId_BadRequest()
        {
            Utilisateur utilisateur = Generate(1);
            Utilisateur newUtilisateur = Generate(2);

            var result = PutTest(utilisateur.Id, utilisateur, newUtilisateur);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutUtilisateurTest_Moq_UnknownId_NotFound()
        {
            Utilisateur newUtilisateur = Generate(1);

            var result = PutTest(newUtilisateur.Id, null, newUtilisateur);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostUtilisateurTest_Moq_InvalidModelState_BadRequest()
        {
            Utilisateur utilisateur = new() { Id = 1 };

            var mockRepo = new Mock<IManagerUtilisateur>();
            var controller = new UtilisateursController(mockRepo.Object)
                .Validating(utilisateur);

            var result = controller.PostUtilisateur(utilisateur).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostUtilisateurTest_Moq_CreatedAt()
        {
            Utilisateur utilisateur = Generate(1);

            var mockRepo = new Mock<IManagerUtilisateur>();
            var controller = new UtilisateursController(mockRepo.Object)
                .Validating(utilisateur);

            var result = controller.PostUtilisateur(utilisateur).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Utilisateur>(result, utilisateur);
        }

        [TestMethod]
        public void DeleteUtilisateurTest_Moq_NoContent()
        {
            Utilisateur utilisateur = Generate(1);

            var mockRepo = new Mock<IManagerUtilisateur>();
            mockRepo.Setup(m => m.GetById(utilisateur.Id)).ReturnsAsync(() => utilisateur);
            var controller = new UtilisateursController(mockRepo.Object);

            var result = controller.DeleteUtilisateur(utilisateur.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteUtilisateurTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerUtilisateur>();
            var controller = new UtilisateursController(mockRepo.Object);

            var result = controller.DeleteUtilisateur(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }

        public static readonly string[] PRENOMS =
        {
            "Kévin", "Noa", "Loïc", "Dylan", "Imene"
        };
        public static readonly string[] NOMS =
        {
            "Grandjean", "Guillot", "Chandon", "Battig", "Hidri"
        };
    }
}