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
    public class TaillesControllerTests
    {
        private TailleProduit Generate(int id, bool visible)
        {
            return new()
            {
                Id = id,
                Visible = visible,
                Nom = $"TailleProduit{id}"
            };
        }

        private void GetAllTest(bool onlyVisible)
        {
            List<TailleProduit> tailles = new()
            {
                Generate(1, true),
                Generate(2, true),
                Generate(3, false),
            };
            IEnumerable<TailleProduit> visibles = tailles.Where(c => c.Visible);

            var mockRepo = new Mock<IManagerTailleProduit>();
            mockRepo.Setup(m => m.GetAll(false)).ReturnsAsync(tailles);
            mockRepo.Setup(m => m.GetAll(true)).ReturnsAsync(visibles);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new TaillesController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetTailleProduits().Result;

            TestUtils.ActionResultShouldGive(result, onlyVisible ? visibles : tailles);
        }

        private void GetTest(bool tailleVisible, bool onlyVisible)
        {
            TailleProduit taille = Generate(1, tailleVisible);
            bool see = tailleVisible || !onlyVisible;

            var mockRepo = new Mock<IManagerTailleProduit>();
            mockRepo.Setup(m => m.GetById(taille.Id, false)).ReturnsAsync(taille);
            mockRepo.Setup(m => m.GetById(taille.Id, true)).ReturnsAsync(see ? taille : null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new TaillesController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetTailleProduit(taille.Id).Result;

            if (see) TestUtils.ActionResultShouldGive(result, taille);
            else result.Result.Should().BeOfType<NotFoundResult>();
        }

        private IActionResult PutTest(int id, TailleProduit? taille, TailleProduit newTailleProduit)
        {
            var mockRepo = new Mock<IManagerTailleProduit>();
            if (taille is not null)
            {
                mockRepo.Setup(m => m.Exists(taille.Id)).ReturnsAsync(true);
                mockRepo.Setup(m => m.GetById(taille.Id, It.IsAny<bool>())).ReturnsAsync(taille);
                mockRepo.Setup(m => m.Update(newTailleProduit)).Returns(() =>
                    Task.Run(() =>
                    {
                        taille.Id = newTailleProduit.Id;
                        taille.Nom = newTailleProduit.Nom;
                        taille.Produits = newTailleProduit.Produits;
                        taille.Visible = newTailleProduit.Visible;
                    })
                );
            }

            var controller = new TaillesController(mockRepo.Object);

            return controller.PutTailleProduit(id, newTailleProduit).Result;
        }

        [TestMethod]
        public void TaillesControllerTest()
        {
            var mockRepo = new Mock<IManagerTailleProduit>();
            var controller = new TaillesController(mockRepo.Object);
        }

        [TestMethod]
        public void GetTaillesTest_Moq_OnlyVisible_RightItems()
        {
            GetAllTest(onlyVisible: true);
        }

        [TestMethod]
        public void GetTaillesTest_Moq_SeeAll_RightItems()
        {
            GetAllTest(onlyVisible: false);
        }

        [TestMethod]
        public void GetTailleTest_Moq_Visible_Admin_RightItem()
        {
            GetTest(tailleVisible: true, onlyVisible: false);
        }

        [TestMethod]
        public void GetTailleTest_Moq_Visible_User_RightItem()
        {
            GetTest(tailleVisible: true, onlyVisible: true);
        }

        [TestMethod]
        public void GetTailleTest_Moq_NotVisible_User_NotFound()
        {
            GetTest(tailleVisible: false, onlyVisible: true);
        }

        [TestMethod]
        public void GetTailleTest_Moq_NotVisible_Admin_RightItem()
        {
            GetTest(tailleVisible: false, onlyVisible: false);
        }

        [TestMethod]
        public void GetTailleTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerTailleProduit>();
            mockRepo.Setup(m => m.GetById(1, It.IsAny<bool>())).ReturnsAsync(() => null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, true);
            var controller = new TaillesController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetTailleProduit(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutTailleTest_Moq_InvalidModelState_BadRequest()
        {
            TailleProduit taille = Generate(1, false);
            TailleProduit newTailleProduit = new() { Id = 1 };

            var result = PutTest(taille.Id, taille, newTailleProduit);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutTailleTest_Moq_InvalidId_BadRequest()
        {
            TailleProduit taille = Generate(1, false);
            TailleProduit newTailleProduit = Generate(2, false);

            var result = PutTest(taille.Id, taille, newTailleProduit);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutTailleTest_Moq_UnknownId_NotFound()
        {
            TailleProduit newTailleProduit = Generate(1, false);

            var result = PutTest(newTailleProduit.Id, null, newTailleProduit);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostTailleTest_Moq_InvalidModelState_BadRequest()
        {
            TailleProduit taille = new() { Id = 1 };

            var mockRepo = new Mock<IManagerTailleProduit>();
            var controller = new TaillesController(mockRepo.Object);

            var result = controller.PostTailleProduit(taille).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostTailleTest_Moq_CreatedAt()
        {
            TailleProduit taille = Generate(1, false);

            var mockRepo = new Mock<IManagerTailleProduit>();
            var controller = new TaillesController(mockRepo.Object);

            var result = controller.PostTailleProduit(taille).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, TailleProduit>(result, taille);
        }

        [TestMethod]
        public void DeleteTailleTest_Moq_NoContent()
        {
            TailleProduit taille = Generate(1, false);

            var mockRepo = new Mock<IManagerTailleProduit>();
            mockRepo.Setup(m => m.GetById(taille.Id, It.IsAny<bool>())).ReturnsAsync(() => taille);
            var controller = new TaillesController(mockRepo.Object);

            var result = controller.DeleteTailleProduit(taille.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteTailleTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerTailleProduit>();
            var controller = new TaillesController(mockRepo.Object);

            var result = controller.DeleteTailleProduit(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}