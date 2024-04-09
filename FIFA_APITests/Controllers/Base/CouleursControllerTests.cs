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
    public class CouleursControllerTests
    {
        private Couleur Generate(int id, bool visible)
        {
            Random r = new Random();
            return new()
            {
                Id = id,
                CodeHexa = r.Next((int)Math.Pow(256, 3)).ToString("X6"),
                Visible = visible,
                Nom = $"Couleur{id}"
            };
        }

        private void GetAllTest(bool onlyVisible)
        {
            List<Couleur> couleurs = new()
            {
                Generate(1, true),
                Generate(2, true),
                Generate(3, false),
            };
            IEnumerable<Couleur> visibles = couleurs.Where(c => c.Visible);

            var mockRepo = new Mock<IManagerCouleur>();
            mockRepo.Setup(m => m.GetAll(false)).ReturnsAsync(couleurs);
            mockRepo.Setup(m => m.GetAll(true)).ReturnsAsync(visibles);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new CouleursController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetCouleurs().Result;

            TestUtils.ActionResultShouldGive(result, onlyVisible ? visibles : couleurs);
        }

        private void GetTest(bool couleurVisible, bool onlyVisible)
        {
            Couleur couleur = Generate(1, couleurVisible);
            bool see = couleurVisible || !onlyVisible;

            var mockRepo = new Mock<IManagerCouleur>();
            mockRepo.Setup(m => m.GetById(couleur.Id, false)).ReturnsAsync(couleur);
            mockRepo.Setup(m => m.GetById(couleur.Id, true)).ReturnsAsync(see ? couleur : null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new CouleursController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetCouleur(couleur.Id).Result;

            if (see) TestUtils.ActionResultShouldGive(result, couleur);
            else result.Result.Should().BeOfType<NotFoundResult>();
        }

        private IActionResult PutTest(int id, Couleur? couleur, Couleur newCouleur)
        {
            var mockRepo = new Mock<IManagerCouleur>();
            if (couleur is not null)
            {
                mockRepo.Setup(m => m.Exists(couleur.Id)).ReturnsAsync(true);
                mockRepo.Setup(m => m.GetById(couleur.Id, It.IsAny<bool>())).ReturnsAsync(couleur);
                mockRepo.Setup(m => m.Update(newCouleur)).Returns(() =>
                    Task.Run(() =>
                    {
                        couleur.Id = newCouleur.Id;
                        couleur.Nom = newCouleur.Nom;
                        couleur.CodeHexa = newCouleur.CodeHexa;
                        couleur.Visible = newCouleur.Visible;
                        couleur.VariantesProduits = newCouleur.VariantesProduits;
                    })
                );
            }

            var controller = new CouleursController(mockRepo.Object)
                .Validating(newCouleur);

            return controller.PutCouleur(id, newCouleur).Result;
        }

        [TestMethod]
        public void CouleursControllerTest()
        {
            var mockRepo = new Mock<IManagerCouleur>();
            var controller = new CouleursController(mockRepo.Object);
        }

        [TestMethod]
        public void GetCouleursTest_Moq_OnlyVisible_RightItems()
        {
            GetAllTest(onlyVisible: true);
        }

        [TestMethod]
        public void GetCouleursTest_Moq_SeeAll_RightItems()
        {
            GetAllTest(onlyVisible: false);
        }

        [TestMethod]
        public void GetCouleurTest_Moq_Visible_Admin_RightItem()
        {
            GetTest(couleurVisible: true, onlyVisible: false);
        }

        [TestMethod]
        public void GetCouleurTest_Moq_Visible_User_RightItem()
        {
            GetTest(couleurVisible: true, onlyVisible: true);
        }

        [TestMethod]
        public void GetCouleurTest_Moq_NotVisible_User_NotFound()
        {
            GetTest(couleurVisible: false, onlyVisible: true);
        }

        [TestMethod]
        public void GetCouleurTest_Moq_NotVisible_Admin_RightItem()
        {
            GetTest(couleurVisible: false, onlyVisible: false);
        }

        [TestMethod]
        public void GetCouleurTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerCouleur>();
            mockRepo.Setup(m => m.GetById(1, It.IsAny<bool>())).ReturnsAsync(() => null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, true);
            var controller = new CouleursController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetCouleur(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutCouleurTest_Moq_InvalidModelState_BadRequest()
        {
            Couleur couleur = Generate(1, false);
            Couleur newCouleur = new() { Id = 1 };

            var result = PutTest(couleur.Id, couleur, newCouleur);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutCouleurTest_Moq_InvalidId_BadRequest()
        {
            Couleur couleur = Generate(1, false);
            Couleur newCouleur = Generate(2, false);

            var result = PutTest(couleur.Id, couleur, newCouleur);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutCouleurTest_Moq_UnknownId_NotFound()
        {
            Couleur newCouleur = Generate(1, false);

            var result = PutTest(newCouleur.Id, null, newCouleur);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostCouleurTest_Moq_InvalidModelState_BadRequest()
        {
            Couleur couleur = new() { Id = 1 };

            var mockRepo = new Mock<IManagerCouleur>();
            var controller = new CouleursController(mockRepo.Object)
                .Validating(couleur);

            var result = controller.PostCouleur(couleur).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostCouleurTest_Moq_CreatedAt()
        {
            Couleur couleur = Generate(1, false);

            var mockRepo = new Mock<IManagerCouleur>();
            var controller = new CouleursController(mockRepo.Object)
                .Validating(couleur);

            var result = controller.PostCouleur(couleur).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Couleur>(result, couleur);
        }

        [TestMethod]
        public void DeleteCouleurTest_Moq_NoContent()
        {
            Couleur couleur = Generate(1, false);

            var mockRepo = new Mock<IManagerCouleur>();
            mockRepo.Setup(m => m.GetById(couleur.Id, It.IsAny<bool>())).ReturnsAsync(() => couleur);
            var controller = new CouleursController(mockRepo.Object);

            var result = controller.DeleteCouleur(couleur.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteCouleurTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerCouleur>();
            var controller = new CouleursController(mockRepo.Object);

            var result = controller.DeleteCouleur(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}