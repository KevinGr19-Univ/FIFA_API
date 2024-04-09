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
using FIFA_API.Controllers.Base;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public class VariantesControllerTests
    {
        private VarianteCouleurProduit Generate(int id, bool visible, int? idcouleur = null, int? idproduit = null)
        {
            Random r = new();
            return new()
            {
                Id = id,
                Visible = visible,
                IdCouleur = idcouleur ?? r.Next(1, 10),
                IdProduit = idproduit ?? r.Next(1, 50),
                Prix = (decimal)r.NextDouble()*30+30,
                ImageUrls = new() { $"Image{id}-1", $"Image{id}-2", $"Image{id}-3" }
            };
        }

        private void GetAllTest(bool onlyVisible)
        {
            List<VarianteCouleurProduit> variantes = new()
            {
                Generate(1, true),
                Generate(2, true),
                Generate(3, false),
            };
            IEnumerable<VarianteCouleurProduit> visibles = variantes.Where(c => c.Visible);

            var mockRepo = new Mock<IManagerVarianteCouleurProduit>();
            mockRepo.Setup(m => m.GetAll(false)).ReturnsAsync(variantes);
            mockRepo.Setup(m => m.GetAll(true)).ReturnsAsync(visibles);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new VariantesController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetVariantes().Result;

            TestUtils.ActionResultShouldGive(result, onlyVisible ? visibles : variantes);
        }

        private void GetTest(bool varianteVisible, bool onlyVisible)
        {
            VarianteCouleurProduit variante = Generate(1, varianteVisible);
            bool see = varianteVisible || !onlyVisible;

            var mockRepo = new Mock<IManagerVarianteCouleurProduit>();
            mockRepo.Setup(m => m.GetByIdWithData(variante.Id, false)).ReturnsAsync(variante);
            mockRepo.Setup(m => m.GetByIdWithData(variante.Id, true)).ReturnsAsync(see ? variante : null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new VariantesController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetVariante(variante.Id).Result;

            if (see) TestUtils.ActionResultShouldGive(result, variante);
            else result.Result.Should().BeOfType<NotFoundResult>();
        }

        private IActionResult PutTest(int id, VarianteCouleurProduit? variante, VarianteCouleurProduit newVariante)
        {
            var mockRepo = new Mock<IManagerVarianteCouleurProduit>();
            if (variante is not null)
            {
                mockRepo.Setup(m => m.Exists(variante.Id)).ReturnsAsync(true);
                mockRepo.Setup(m => m.GetById(variante.Id, It.IsAny<bool>())).ReturnsAsync(variante);
                mockRepo.Setup(m => m.Update(newVariante)).Returns(() =>
                    Task.Run(() =>
                    {
                        variante.Id = newVariante.Id;
                        variante.Visible = newVariante.Visible;
                        variante.Couleur = newVariante.Couleur;
                        variante.Produit = newVariante.Produit;
                        variante.Stocks = newVariante.Stocks;
                        variante.ImageUrls = newVariante.ImageUrls;
                        variante.Prix = newVariante.Prix;
                        variante.IdCouleur = newVariante.IdCouleur;
                        variante.IdProduit = newVariante.IdProduit;
                    })
                );
            }

            var controller = new VariantesController(mockRepo.Object)
                .Validating(newVariante);

            return controller.PutVariante(id, newVariante).Result;
        }

        [TestMethod]
        public void VariantesControllerTest()
        {
            var mockRepo = new Mock<IManagerVarianteCouleurProduit>();
            var controller = new VariantesController(mockRepo.Object);
        }

        [TestMethod]
        public void GetVariantesTest_Moq_OnlyVisible_RightItems()
        {
            GetAllTest(onlyVisible: true);
        }

        [TestMethod]
        public void GetVariantesTest_Moq_SeeAll_RightItems()
        {
            GetAllTest(onlyVisible: false);
        }

        [TestMethod]
        public void GetVarianteTest_Moq_Visible_Admin_RightItem()
        {
            GetTest(varianteVisible: true, onlyVisible: false);
        }

        [TestMethod]
        public void GetVarianteTest_Moq_Visible_User_RightItem()
        {
            GetTest(varianteVisible: true, onlyVisible: true);
        }

        [TestMethod]
        public void GetVarianteTest_Moq_NotVisible_User_NotFound()
        {
            GetTest(varianteVisible: false, onlyVisible: true);
        }

        [TestMethod]
        public void GetVarianteTest_Moq_NotVisible_Admin_RightItem()
        {
            GetTest(varianteVisible: false, onlyVisible: false);
        }

        [TestMethod]
        public void GetVarianteTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerVarianteCouleurProduit>();
            mockRepo.Setup(m => m.GetByIdWithData(1, It.IsAny<bool>())).ReturnsAsync(() => null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, true);
            var controller = new VariantesController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetVariante(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutVarianteTest_Moq_InvalidModelState_BadRequest()
        {
            VarianteCouleurProduit variante = Generate(1, false, idcouleur:1, idproduit:1);
            VarianteCouleurProduit newVariante = new() { Id = 1, IdCouleur = 1, IdProduit = 1, Prix = -1 };

            var result = PutTest(variante.Id, variante, newVariante);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutVarianteTest_Moq_InvalidId_BadRequest()
        {
            VarianteCouleurProduit variante = Generate(1, false, idcouleur: 1, idproduit: 1);
            VarianteCouleurProduit newVariante = Generate(2, false, idcouleur: 1, idproduit: 1);

            var result = PutTest(variante.Id, variante, newVariante);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutVarianteTest_Moq_ComboChanged_BadRequest()
        {
            VarianteCouleurProduit variante = Generate(1, false, idcouleur: 1, idproduit: 1);
            VarianteCouleurProduit newVariante = Generate(1, false, idcouleur: 2, idproduit: 1);

            var result = PutTest(variante.Id, variante, newVariante);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutVarianteTest_Moq_UnknownId_NotFound()
        {
            VarianteCouleurProduit newVariante = Generate(1, false);

            var result = PutTest(newVariante.Id, null, newVariante);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostVarianteTest_Moq_InvalidModelState_BadRequest()
        {
            VarianteCouleurProduit variante = new() { Id = 1, IdCouleur = 1, IdProduit = 1, Prix = -1 };

            var mockRepo = new Mock<IManagerVarianteCouleurProduit>();
            mockRepo.Setup(m => m.CombinationExists(variante.IdProduit, variante.IdCouleur)).ReturnsAsync(false);

            var controller = new VariantesController(mockRepo.Object)
                .Validating(variante);

            var result = controller.PostVariante(variante).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostVarianteTest_Moq_ComboExists_Conflict()
        {
            VarianteCouleurProduit variante = Generate(1, false);

            var mockRepo = new Mock<IManagerVarianteCouleurProduit>();
            mockRepo.Setup(m => m.CombinationExists(variante.IdProduit, variante.IdCouleur)).ReturnsAsync(true);

            var controller = new VariantesController(mockRepo.Object)
                .Validating(variante);

            var result = controller.PostVariante(variante).Result;

            result.Result.Should().BeOfType<ConflictResult>();
        }

        [TestMethod]
        public void PostVarianteTest_Moq_CreatedAt()
        {
            VarianteCouleurProduit variante = Generate(1, false);

            var mockRepo = new Mock<IManagerVarianteCouleurProduit>();
            mockRepo.Setup(m => m.CombinationExists(variante.IdProduit, variante.IdCouleur)).ReturnsAsync(false);

            var controller = new VariantesController(mockRepo.Object)
                .Validating(variante);

            var result = controller.PostVariante(variante).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, VarianteCouleurProduit>(result, variante);
        }

        [TestMethod]
        public void DeleteVarianteTest_Moq_NoContent()
        {
            VarianteCouleurProduit variante = Generate(1, false);
            variante.Stocks = new List<StockProduit>() { };

            var mockRepo = new Mock<IManagerVarianteCouleurProduit>();
            mockRepo.Setup(m => m.GetByIdWithStocks(variante.Id, It.IsAny<bool>())).ReturnsAsync(() => variante);
            var controller = new VariantesController(mockRepo.Object);

            var result = controller.DeleteVariante(variante.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteVarianteTest_Moq_WithStocks_Forbid()
        {
            VarianteCouleurProduit variante = Generate(1, false);
            variante.Stocks = new List<StockProduit>()
            {
                new() { IdVCProduit = variante.Id, IdTaille = 1, Stocks = 0 }
            };

            var mockRepo = new Mock<IManagerVarianteCouleurProduit>();
            mockRepo.Setup(m => m.GetByIdWithStocks(variante.Id, It.IsAny<bool>())).ReturnsAsync(() => variante);
            var controller = new VariantesController(mockRepo.Object);

            var result = controller.DeleteVariante(variante.Id).Result;

            result.Should().BeOfType<ForbidResult>();
        }

        [TestMethod]
        public void DeleteVarianteTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerVarianteCouleurProduit>();
            var controller = new VariantesController(mockRepo.Object);

            var result = controller.DeleteVariante(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}