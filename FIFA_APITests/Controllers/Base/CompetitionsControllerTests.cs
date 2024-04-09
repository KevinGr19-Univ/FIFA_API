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
    public class CompetitionsControllerTests
    {
        private Competition Generate(int id, bool visible)
        {
            return new()
            {
                Id = id,
                Visible = visible,
                Nom = $"Competition{id}"
            };
        }

        private void GetAllTest(bool onlyVisible)
        {
            List<Competition> competitions = new()
            {
                Generate(1, true),
                Generate(2, true),
                Generate(3, false),
            };
            IEnumerable<Competition> visibles = competitions.Where(c => c.Visible);

            var mockRepo = new Mock<IManagerCompetition>();
            mockRepo.Setup(m => m.GetAll(false)).ReturnsAsync(competitions);
            mockRepo.Setup(m => m.GetAll(true)).ReturnsAsync(visibles);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new CompetitionsController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetCompetitions().Result;

            TestUtils.ActionResultShouldGive(result, onlyVisible ? visibles : competitions);
        }

        private void GetTest(bool competitionVisible, bool onlyVisible)
        {
            Competition competition = Generate(1, competitionVisible);
            bool see = competitionVisible || !onlyVisible;

            var mockRepo = new Mock<IManagerCompetition>();
            mockRepo.Setup(m => m.GetById(competition.Id, false)).ReturnsAsync(competition);
            mockRepo.Setup(m => m.GetById(competition.Id, true)).ReturnsAsync(see ? competition : null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, !onlyVisible);
            var controller = new CompetitionsController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetCompetition(competition.Id).Result;

            if (see) TestUtils.ActionResultShouldGive(result, competition);
            else result.Result.Should().BeOfType<NotFoundResult>();
        }

        private IActionResult PutTest(int id, Competition? competition, Competition newCompetition)
        {
            var mockRepo = new Mock<IManagerCompetition>();
            if (competition is not null)
            {
                mockRepo.Setup(m => m.Exists(competition.Id)).ReturnsAsync(true);
                mockRepo.Setup(m => m.GetById(competition.Id, It.IsAny<bool>())).ReturnsAsync(competition);
                mockRepo.Setup(m => m.Update(newCompetition)).Returns(() =>
                    Task.Run(() =>
                    {
                        competition.Id = newCompetition.Id;
                        competition.Nom = newCompetition.Nom;
                        competition.Produits = newCompetition.Produits;
                        competition.Visible = newCompetition.Visible;
                    })
                );
            }

            var controller = new CompetitionsController(mockRepo.Object)
                .Validating(newCompetition);

            return controller.PutCompetition(id, newCompetition).Result;
        }

        [TestMethod]
        public void CompetitionsControllerTest()
        {
            var mockRepo = new Mock<IManagerCompetition>();
            var controller = new CompetitionsController(mockRepo.Object);
        }

        [TestMethod]
        public void GetCompetitionsTest_Moq_OnlyVisible_RightItems()
        {
            GetAllTest(onlyVisible: true);
        }

        [TestMethod]
        public void GetCompetitionsTest_Moq_SeeAll_RightItems()
        {
            GetAllTest(onlyVisible: false);
        }

        [TestMethod]
        public void GetCompetitionTest_Moq_Visible_Admin_RightItem()
        {
            GetTest(competitionVisible: true, onlyVisible: false);
        }

        [TestMethod]
        public void GetCompetitionTest_Moq_Visible_User_RightItem()
        {
            GetTest(competitionVisible: true, onlyVisible: true);
        }

        [TestMethod]
        public void GetCompetitionTest_Moq_NotVisible_User_NotFound()
        {
            GetTest(competitionVisible: false, onlyVisible: true);
        }

        [TestMethod]
        public void GetCompetitionTest_Moq_NotVisible_Admin_RightItem()
        {
            GetTest(competitionVisible: false, onlyVisible: false);
        }

        [TestMethod]
        public void GetCompetitionTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerCompetition>();
            mockRepo.Setup(m => m.GetById(1, It.IsAny<bool>())).ReturnsAsync(() => null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, true);
            var controller = new CompetitionsController(mockRepo.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetCompetition(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutCompetitionTest_Moq_InvalidModelState_BadRequest()
        {
            Competition competition = Generate(1, false);
            Competition newCompetition = new() { Id = 1 };

            var result = PutTest(competition.Id, competition, newCompetition);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutCompetitionTest_Moq_InvalidId_BadRequest()
        {
            Competition competition = Generate(1, false);
            Competition newCompetition = Generate(2, false);

            var result = PutTest(competition.Id, competition, newCompetition);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutCompetitionTest_Moq_UnknownId_NotFound()
        {
            Competition newCompetition = Generate(1, false);

            var result = PutTest(newCompetition.Id, null, newCompetition);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostCompetitionTest_Moq_InvalidModelState_BadRequest()
        {
            Competition competition = new() { Id = 1 };

            var mockRepo = new Mock<IManagerCompetition>();
            var controller = new CompetitionsController(mockRepo.Object)
                .Validating(competition);

            var result = controller.PostCompetition(competition).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostCompetitionTest_Moq_CreatedAt()
        {
            Competition competition = Generate(1, false);

            var mockRepo = new Mock<IManagerCompetition>();
            var controller = new CompetitionsController(mockRepo.Object)
                .Validating(competition);

            var result = controller.PostCompetition(competition).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Competition>(result, competition);
        }

        [TestMethod]
        public void DeleteCompetitionTest_Moq_NoContent()
        {
            Competition competition = Generate(1, false);

            var mockRepo = new Mock<IManagerCompetition>();
            mockRepo.Setup(m => m.GetById(competition.Id, It.IsAny<bool>())).ReturnsAsync(() => competition);
            var controller = new CompetitionsController(mockRepo.Object);

            var result = controller.DeleteCompetition(competition.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteCompetitionTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerCompetition>();
            var controller = new CompetitionsController(mockRepo.Object);

            var result = controller.DeleteCompetition(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}