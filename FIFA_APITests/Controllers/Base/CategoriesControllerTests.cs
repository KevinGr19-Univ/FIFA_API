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
using FIFA_APITests.Controllers.Utils;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public class CategoriesControllerTests
    {
        private void GetAllTest(bool onlyVisible)
        {
            List<CategorieProduit> categories = new()
            {
                new() { Id = 1, IdCategorieProduitParent = null, Nom = "Categorie1", Visible = true },
                new() { Id = 2, IdCategorieProduitParent = 1, Nom = "Categorie2", Visible = true },
                new() { Id = 3, IdCategorieProduitParent = 1, Nom = "Categorie3", Visible = false }
            };
            IEnumerable<CategorieProduit> visibles = categories.Where(c => c.Visible);

            var mockRepo = new Mock<IManagerCategorieProduit>();
            mockRepo.Setup(m => m.GetAll(false)).ReturnsAsync(categories);
            mockRepo.Setup(m => m.GetAll(true)).ReturnsAsync(visibles);

            var controller = new CategoriesController(mockRepo.Object);
            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            controller.ControllerContext = mockHttpCtx.ToControllerContext();

            var result = controller.GetCategorieProduits().Result;

            TestUtils.ActionResultShouldGive(result, onlyVisible ? visibles : categories);
        }

        [TestMethod]
        public void CategoriesControllerTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void GetCategorieProduitsTest_Moq_OnlyVisible_RightItems()
        {
            GetAllTest(true);
        }

        [TestMethod]
        public void GetCategorieProduitsTest_Moq_SeeAll_RightItems()
        {
            GetAllTest(false);
        }

        [TestMethod]
        public void GetCategorieProduitTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void PutCategorieProduitTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void PostCategorieProduitTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void DeleteCategorieProduitTest()
        {
            Assert.Fail();
        }
    }
}