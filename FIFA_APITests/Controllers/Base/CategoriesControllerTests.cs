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
    public class CategoriesControllerTests
    {
        private CategorieProduit Generate(int id, bool visible, int? idparent = null)
        {
            return new()
            {
                Id = id,
                IdCategorieProduitParent = idparent,
                Visible = visible,
                Nom = $"Categorie{id}"
            };
        }

        private void GetAllTest(bool onlyVisible)
        {
            List<CategorieProduit> categories = new()
            {
                Generate(1, true),
                Generate(2, true, 1),
                Generate(3, false, 1),
            };
            IEnumerable<CategorieProduit> visibles = categories.Where(c => c.Visible);

            var mockRepo = new Mock<IManagerCategorieProduit>();
            mockRepo.Setup(m => m.GetAll(false)).ReturnsAsync(categories);
            mockRepo.Setup(m => m.GetAll(true)).ReturnsAsync(visibles);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new CategoriesController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetCategorieProduits().Result;

            TestUtils.ActionResultShouldGive(result, onlyVisible ? visibles : categories);
        }

        private void GetTest(bool categorieVisible, bool onlyVisible)
        {
            CategorieProduit categorie = Generate(1, categorieVisible);
            bool see = categorieVisible || !onlyVisible;

            var mockRepo = new Mock<IManagerCategorieProduit>();
            mockRepo.Setup(m => m.GetById(categorie.Id, false)).ReturnsAsync(categorie);
            mockRepo.Setup(m => m.GetById(categorie.Id, true)).ReturnsAsync(see ? categorie : null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new CategoriesController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetCategorieProduit(categorie.Id).Result;

            if (see) TestUtils.ActionResultShouldGive(result, categorie);
            else result.Result.Should().BeOfType<NotFoundResult>();
        }

        private IActionResult PutTest(int id, CategorieProduit? categorie, CategorieProduit newCategorie)
        {
            var mockRepo = new Mock<IManagerCategorieProduit>();
            if (categorie is not null)
            {
                mockRepo.Setup(m => m.Exists(categorie.Id)).ReturnsAsync(true);
                mockRepo.Setup(m => m.GetById(categorie.Id, It.IsAny<bool>())).ReturnsAsync(categorie);
                mockRepo.Setup(m => m.Update(newCategorie)).Returns(() =>
                    Task.Run(() =>
                    {
                        categorie.Id = newCategorie.Id;
                        categorie.Nom = newCategorie.Nom;
                        categorie.IdCategorieProduitParent = newCategorie.IdCategorieProduitParent;
                        categorie.SousCategories = newCategorie.SousCategories;
                        categorie.Parent = newCategorie.Parent;
                        categorie.Visible = newCategorie.Visible;
                    })
                );
            }

            var controller = new CategoriesController(mockRepo.Object)
                .Validating(newCategorie);

            return controller.PutCategorieProduit(id, newCategorie).Result;
        }

        [TestMethod]
        public void CategoriesControllerTest()
        {
            var mockRepo = new Mock<IManagerCategorieProduit>();
            var controller = new CategoriesController(mockRepo.Object);
        }

        [TestMethod]
        public void GetCategorieProduitsTest_Moq_OnlyVisible_RightItems()
        {
            GetAllTest(onlyVisible: true);
        }

        [TestMethod]
        public void GetCategorieProduitsTest_Moq_SeeAll_RightItems()
        {
            GetAllTest(onlyVisible: false);
        }

        [TestMethod]
        public void GetCategorieProduitTest_Moq_Visible_Admin_RightItem()
        {
            GetTest(categorieVisible: true, onlyVisible: false);
        }

        [TestMethod]
        public void GetCategorieProduitTest_Moq_Visible_User_RightItem()
        {
            GetTest(categorieVisible: true, onlyVisible: true);
        }

        [TestMethod]
        public void GetCategorieProduitTest_Moq_NotVisible_User_NotFound()
        {
            GetTest(categorieVisible: false, onlyVisible: true);
        }

        [TestMethod]
        public void GetCategorieProduitTest_Moq_NotVisible_Admin_RightItem()
        {
            GetTest(categorieVisible: false, onlyVisible: false);
        }

        [TestMethod]
        public void GetCategorieProduitTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerCategorieProduit>();
            mockRepo.Setup(m => m.GetById(1, It.IsAny<bool>())).ReturnsAsync(() => null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, true);
            var controller = new CategoriesController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetCategorieProduit(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutCategorieProduitTest_Moq_InvalidModelState_BadRequest()
        {
            CategorieProduit categorie = Generate(1, false);
            CategorieProduit newCategorie = new() { Id = 1 };

            var result = PutTest(categorie.Id, categorie, newCategorie);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutCategorieProduitTest_Moq_InvalidId_BadRequest()
        {
            CategorieProduit categorie = Generate(1, false);
            CategorieProduit newCategorie = Generate(2, false);

            var result = PutTest(categorie.Id, categorie, newCategorie);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutCategorieProduitTest_Moq_UnknownId_NotFound()
        {
            CategorieProduit newCategorie = Generate(1, false);

            var result = PutTest(newCategorie.Id, null, newCategorie);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostCategorieProduitTest_Moq_InvalidModelState_BadRequest()
        {
            CategorieProduit categorie = new() { Id = 1 };

            var mockRepo = new Mock<IManagerCategorieProduit>();
            var controller = new CategoriesController(mockRepo.Object)
                .Validating(categorie);

            var result = controller.PostCategorieProduit(categorie).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostCategorieProduitTest_Moq_CreatedAt()
        {
            CategorieProduit categorie = Generate(1, false);

            var mockRepo = new Mock<IManagerCategorieProduit>();
            var controller = new CategoriesController(mockRepo.Object)
                .Validating(categorie);

            var result = controller.PostCategorieProduit(categorie).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, CategorieProduit>(result, categorie);
        }

        [TestMethod]
        public void DeleteCategorieProduitTest_Moq_NoContent()
        {
            CategorieProduit categorie = Generate(1, false);

            var mockRepo = new Mock<IManagerCategorieProduit>();
            mockRepo.Setup(m => m.GetById(categorie.Id, It.IsAny<bool>())).ReturnsAsync(() => categorie);
            var controller = new CategoriesController(mockRepo.Object);

            var result = controller.DeleteCategorieProduit(categorie.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteCategorieProduitTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerCategorieProduit>();
            var controller = new CategoriesController(mockRepo.Object);

            var result = controller.DeleteCategorieProduit(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}