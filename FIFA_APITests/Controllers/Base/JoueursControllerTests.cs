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
    public class JoueursControllerTests
    {
        private Joueur Generate(int id)
        {
            Random r = new();
            return new()
            {
                Id = id,
                Prenom = $"Prenom{id}",
                Nom = $"Joueur{id}",
                Biographie = $"Biographie{id}\nLigne1\nLigne2",
                DateNaissance = new DateTime(1982, 07, 13),
                LieuNaissance = $"Lieu{id}",
                Poids = r.Next(20) + 65,
                Taille = r.Next(30) + 160,
                ImageUrl = $"URL{id}",
                Pied = (PiedJoueur)r.Next(3),
                Poste = (PosteJoueur)r.Next(4),
                IdClub = id,
                IdPays = id
            };
        }

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
                        joueur.Prenom = newJoueur.Prenom;
                        joueur.Nom = newJoueur.Nom;
                        joueur.Biographie = newJoueur.Biographie;
                        joueur.DateNaissance = newJoueur.DateNaissance;
                        joueur.LieuNaissance = newJoueur.LieuNaissance;
                        joueur.Poids = newJoueur.Poids;
                        joueur.Taille = newJoueur.Taille;
                        joueur.ImageUrl = newJoueur.ImageUrl;
                        joueur.Pied = newJoueur.Pied;
                        joueur.Poste = newJoueur.Poste;
                        joueur.IdClub = newJoueur.IdClub;
                        joueur.IdPays = newJoueur.IdPays;

                        joueur.Pays = newJoueur.Pays;
                        joueur.Club = newJoueur.Club;
                        joueur.Stats = newJoueur.Stats;
                        joueur.FaqJoueurs = newJoueur.FaqJoueurs;
                        joueur.Trophees = newJoueur.Trophees;
                    })
                );
            }
            else
            {
                mockUoW.Setup(m => m.Joueurs.Exists(id)).ReturnsAsync(false);
                mockUoW.Setup(m => m.Joueurs.GetById(id)).ReturnsAsync(() => null);
            }

            var controller = new JoueursController(mockUoW.Object)
                .Validating(newJoueur);

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
                Generate(1),
                Generate(2),
                Generate(3),
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
            Joueur joueur = Generate(1);

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
            Joueur joueur = Generate(1);
            Joueur newJoueur = Generate(1);

            var result = PutTest(joueur.Id, joueur, newJoueur);

            result.Should().BeOfType<NoContentResult>();
            joueur.Should().Be(newJoueur);
        }

        [TestMethod]
        public void PutJoueurTest_Moq_InvalidModelState_BadRequest()
        {
            Joueur joueur = Generate(1);
            Joueur newJoueur = new() { Id = 1 };

            var result = PutTest(joueur.Id, joueur, newJoueur);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutJoueurTest_Moq_InvalidId_BadRequest()
        {
            Joueur joueur = Generate(1);
            Joueur newJoueur = Generate(2);

            var result = PutTest(joueur.Id, joueur, newJoueur);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutJoueurTest_Moq_UnknownId_NotFound()
        {
            Joueur newJoueur = Generate(1);

            var result = PutTest(newJoueur.Id, null, newJoueur);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostJoueurTest_Moq_InvalidModelState_BadRequest()
        {
            Joueur joueur = new() { Id = 1 };

            var mockUoW = new Mock<IUnitOfWorkJoueur>();
            mockUoW.Setup(m => m.Joueurs.Add(joueur));

            var controller = new JoueursController(mockUoW.Object)
                .Validating(joueur);

            var result = controller.PostJoueur(joueur).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostJoueurTest_Moq_CreatedAt()
        {
            Joueur joueur = Generate(1);

            var mockUoW = new Mock<IUnitOfWorkJoueur>();
            mockUoW.Setup(m => m.Joueurs.Add(joueur));

            var controller = new JoueursController(mockUoW.Object)
                .Validating(joueur);

            var result = controller.PostJoueur(joueur).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Joueur>(result, joueur);
        }

        [TestMethod]
        public void DeleteJoueurTest_Moq_NoContent()
        {
            Joueur joueur = Generate(1);

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