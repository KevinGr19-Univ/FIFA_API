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
    public class NationsControllerTests
    {
        private Nation Generate(int id, bool visible)
        {
            return new()
            {
                Id = id,
                Visible = visible,
                Nom = $"Nation{id}"
            };
        }

        private void GetAllTest(bool onlyVisible)
        {
            List<Nation> nations = new()
            {
                Generate(1, true),
                Generate(2, true),
                Generate(3, false),
            };
            IEnumerable<Nation> visibles = nations.Where(c => c.Visible);

            var mockRepo = new Mock<IManagerNation>();
            mockRepo.Setup(m => m.GetAll(false)).ReturnsAsync(nations);
            mockRepo.Setup(m => m.GetAll(true)).ReturnsAsync(visibles);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new NationsController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetNations().Result;

            TestUtils.ActionResultShouldGive(result, onlyVisible ? visibles : nations);
        }

        private void GetTest(bool nationVisible, bool onlyVisible)
        {
            Nation nation = Generate(1, nationVisible);
            bool see = nationVisible || !onlyVisible;

            var mockRepo = new Mock<IManagerNation>();
            mockRepo.Setup(m => m.GetById(nation.Id, false)).ReturnsAsync(nation);
            mockRepo.Setup(m => m.GetById(nation.Id, true)).ReturnsAsync(see ? nation : null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new NationsController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetNation(nation.Id).Result;

            if (see) TestUtils.ActionResultShouldGive(result, nation);
            else result.Result.Should().BeOfType<NotFoundResult>();
        }

        private IActionResult PutTest(int id, Nation? nation, Nation newNation)
        {
            var mockRepo = new Mock<IManagerNation>();
            if (nation is not null)
            {
                mockRepo.Setup(m => m.Exists(nation.Id)).ReturnsAsync(true);
                mockRepo.Setup(m => m.GetById(nation.Id, It.IsAny<bool>())).ReturnsAsync(nation);
                mockRepo.Setup(m => m.Update(newNation)).Returns(() =>
                    Task.Run(() =>
                    {
                        nation.Id = newNation.Id;
                        nation.Nom = newNation.Nom;
                        nation.Produits = newNation.Produits;
                        nation.Visible = newNation.Visible;
                    })
                );
            }

            var controller = new NationsController(mockRepo.Object);

            return controller.PutNation(id, newNation).Result;
        }

        [TestMethod]
        public void NationsControllerTest()
        {
            var mockRepo = new Mock<IManagerNation>();
            var controller = new NationsController(mockRepo.Object);
        }

        [TestMethod]
        public void GetNationsTest_Moq_OnlyVisible_RightItems()
        {
            GetAllTest(onlyVisible: true);
        }

        [TestMethod]
        public void GetNationsTest_Moq_SeeAll_RightItems()
        {
            GetAllTest(onlyVisible: false);
        }

        [TestMethod]
        public void GetNationTest_Moq_Visible_Admin_RightItem()
        {
            GetTest(nationVisible: true, onlyVisible: false);
        }

        [TestMethod]
        public void GetNationTest_Moq_Visible_User_RightItem()
        {
            GetTest(nationVisible: true, onlyVisible: true);
        }

        [TestMethod]
        public void GetNationTest_Moq_NotVisible_User_NotFound()
        {
            GetTest(nationVisible: false, onlyVisible: true);
        }

        [TestMethod]
        public void GetNationTest_Moq_NotVisible_Admin_RightItem()
        {
            GetTest(nationVisible: false, onlyVisible: false);
        }

        [TestMethod]
        public void GetNationTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerNation>();
            mockRepo.Setup(m => m.GetById(1, It.IsAny<bool>())).ReturnsAsync(() => null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, true);
            var controller = new NationsController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetNation(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutNationTest_Moq_InvalidModelState_BadRequest()
        {
            Nation nation = Generate(1, false);
            Nation newNation = new() { Id = 1 };

            var result = PutTest(nation.Id, nation, newNation);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutNationTest_Moq_InvalidId_BadRequest()
        {
            Nation nation = Generate(1, false);
            Nation newNation = Generate(2, false);

            var result = PutTest(nation.Id, nation, newNation);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutNationTest_Moq_UnknownId_NotFound()
        {
            Nation newNation = Generate(1, false);

            var result = PutTest(newNation.Id, null, newNation);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostNationTest_Moq_InvalidModelState_BadRequest()
        {
            Nation nation = new() { Id = 1 };

            var mockRepo = new Mock<IManagerNation>();
            var controller = new NationsController(mockRepo.Object);

            var result = controller.PostNation(nation).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostNationTest_Moq_CreatedAt()
        {
            Nation nation = Generate(1, false);

            var mockRepo = new Mock<IManagerNation>();
            var controller = new NationsController(mockRepo.Object);

            var result = controller.PostNation(nation).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Nation>(result, nation);
        }

        [TestMethod]
        public void DeleteNationTest_Moq_NoContent()
        {
            Nation nation = Generate(1, false);

            var mockRepo = new Mock<IManagerNation>();
            mockRepo.Setup(m => m.GetById(nation.Id, It.IsAny<bool>())).ReturnsAsync(() => nation);
            var controller = new NationsController(mockRepo.Object);

            var result = controller.DeleteNation(nation.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteNationTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerNation>();
            var controller = new NationsController(mockRepo.Object);

            var result = controller.DeleteNation(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}