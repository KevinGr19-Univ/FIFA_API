using Microsoft.VisualStudio.TestTools.UnitTesting;
using FIFA_API.Controllers;
using System;
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
    public class ProduitsControllerTests
    {
        private Produit Generate(int id, bool visible)
        {
            Random r = new();
            return new()
            {
                Id = id,
                Visible = visible,
                Titre = $"Produit{id}",
                Description = $"Description{r.Next(10)}",
                IdCategorieProduit = r.Next(3),
                IdGenre = r.Next(3),
                IdNation = r.Next(3),
                IdCompetition = r.Next(3)
            };
        }

        private void GetAllTest(bool onlyVisible)
        {
            List<Produit> produits = new()
            {
                Generate(1, true),
                Generate(2, true),
                Generate(3, false),
            };
            IEnumerable<Produit> visibles = produits.Where(c => c.Visible);

            var mockUoW = new Mock<IUnitOfWorkProduit>();
            mockUoW.Setup(m => m.Produits.GetAll(false)).ReturnsAsync(produits);
            mockUoW.Setup(m => m.Produits.GetAll(true)).ReturnsAsync(visibles);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new ProduitsController(mockUoW.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetProduits().Result;

            TestUtils.ActionResultShouldGive(result, onlyVisible ? visibles : produits);
        }

        private void GetTest(bool produitVisible, bool onlyVisible)
        {
            Produit produit = Generate(1, produitVisible);
            bool see = produitVisible || !onlyVisible;

            var mockUoW = new Mock<IUnitOfWorkProduit>();
            mockUoW.Setup(m => m.Produits.GetByIdWithAll(produit.Id, false)).ReturnsAsync(produit);
            mockUoW.Setup(m => m.Produits.GetByIdWithAll(produit.Id, true)).ReturnsAsync(see ? produit : null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new ProduitsController(mockUoW.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetProduit(produit.Id).Result;

            if (see) TestUtils.ActionResultShouldGive(result, produit);
            else result.Result.Should().BeOfType<NotFoundResult>();
        }

        private IActionResult PutTest(int id, Produit? produit, Produit newProduit)
        {
            var mockUoW = new Mock<IUnitOfWorkProduit>();
            if (produit is not null)
            {
                mockUoW.Setup(m => m.Produits.Exists(produit.Id)).ReturnsAsync(true);
                mockUoW.Setup(m => m.Produits.GetById(produit.Id, It.IsAny<bool>())).ReturnsAsync(produit);
                mockUoW.Setup(m => m.Produits.Update(newProduit)).Returns(() =>
                    Task.Run(() =>
                    {
                        produit.Id = newProduit.Id;
                        produit.Titre = newProduit.Titre;
                        produit.Description = newProduit.Description;
                        produit.Visible = newProduit.Visible;
                        produit.IdCategorieProduit = newProduit.IdCategorieProduit;
                        produit.Categorie = newProduit.Categorie;
                        produit.IdNation = newProduit.IdNation;
                        produit.Nation = newProduit.Nation;
                        produit.IdGenre = newProduit.IdGenre;
                        produit.Genre = newProduit.Genre;
                        produit.IdCompetition = newProduit.IdCompetition;
                        produit.Competition = newProduit.Competition;
                        produit.Variantes = newProduit.Variantes;
                        produit.Tailles = newProduit.Tailles;
                    })
                );
            }
            else
            {
                mockUoW.Setup(m => m.Produits.Exists(id)).ReturnsAsync(false);
                mockUoW.Setup(m => m.Produits.GetById(id, It.IsAny<bool>())).ReturnsAsync(() => null);
            }

            var controller = new ProduitsController(mockUoW.Object)
                .Validating(newProduit);

            return controller.PutProduit(id, newProduit).Result;
        }

        [TestMethod]
        public void ProduitsControllerTest()
        {
            var mockUoW = new Mock<IUnitOfWorkProduit>();
            var controller = new ProduitsController(mockUoW.Object);
        }

        [TestMethod]
        public void GetProduitsTest_Moq_RightItems()
        {
            GetAllTest(onlyVisible: false);
        }

        [TestMethod]
        public void GetProduitTest_Moq_Visible_Admin_RightItem()
        {
            GetTest(produitVisible: true, onlyVisible: false);
        }

        [TestMethod]
        public void GetProduitTest_Moq_Visible_User_RightItem()
        {
            GetTest(produitVisible: true, onlyVisible: true);
        }

        [TestMethod]
        public void GetProduitTest_Moq_NotVisible_User_NotFound()
        {
            GetTest(produitVisible: false, onlyVisible: true);
        }

        [TestMethod]
        public void GetProduitTest_Moq_NotVisible_Admin_RightItem()
        {
            GetTest(produitVisible: false, onlyVisible: false);
        }

        [TestMethod]
        public void GetProduitTest_Moq_UnknownId_NotFound()
        {
            var mockUoW = new Mock<IUnitOfWorkProduit>();
            mockUoW.Setup(m => m.Produits.GetById(1, It.IsAny<bool>())).ReturnsAsync(() => null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, true);
            var controller = new ProduitsController(mockUoW.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetProduit(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutProduitTest_Moq_InvalidModelState_BadRequest()
        {
            Produit produit = Generate(1, false);
            Produit newProduit = new() { Id = 1 };

            var result = PutTest(produit.Id, produit, newProduit);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutProduitTest_Moq_InvalidId_BadRequest()
        {
            Produit produit = Generate(1, false);
            Produit newProduit = Generate(2, false);

            var result = PutTest(produit.Id, produit, newProduit);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutProduitTest_Moq_UnknownId_NotFound()
        {
            Produit newProduit = Generate(1, false);

            var result = PutTest(newProduit.Id, null, newProduit);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostProduitTest_Moq_InvalidModelState_BadRequest()
        {
            Produit produit = new() { Id = 1 };

            var mockUoW = new Mock<IUnitOfWorkProduit>();
            mockUoW.Setup(m => m.Produits.Add(produit));

            var controller = new ProduitsController(mockUoW.Object)
                .Validating(produit);

            var result = controller.PostProduit(produit).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostProduitTest_Moq_CreatedAt()
        {
            Produit produit = Generate(1, false);

            var mockUoW = new Mock<IUnitOfWorkProduit>();
            mockUoW.Setup(m => m.Produits.Add(produit));

            var controller = new ProduitsController(mockUoW.Object)
                .Validating(produit);

            var result = controller.PostProduit(produit).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Produit>(result, produit);
        }

        [TestMethod]
        public void DeleteProduitTest_Moq_NoContent()
        {
            Produit produit = Generate(1, false);

            var mockUoW = new Mock<IUnitOfWorkProduit>();
            mockUoW.Setup(m => m.Produits.GetById(produit.Id, It.IsAny<bool>())).ReturnsAsync(() => produit);
            var controller = new ProduitsController(mockUoW.Object);

            var result = controller.DeleteProduit(produit.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteProduitTest_Moq_UnknownId_NotFound()
        {
            var mockUoW = new Mock<IUnitOfWorkProduit>();
            mockUoW.Setup(m => m.Produits.GetById(1, It.IsAny<bool>())).ReturnsAsync(() => null);
            var controller = new ProduitsController(mockUoW.Object);

            var result = controller.DeleteProduit(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}