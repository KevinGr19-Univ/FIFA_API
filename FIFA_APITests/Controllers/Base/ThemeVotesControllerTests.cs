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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public class ThemeVotesControllerTests
    {
        private ThemeVote Generate(int id, bool visible)
        {
            Random r = new();
            return new()
            {
                Id = id,
                Visible = visible,
                NomTheme = $"ThemeVote{id}-{r.Next(100)}"
            };
        }

        private void GetAllTest(bool onlyVisible)
        {
            List<ThemeVote> themeVotes = new()
            {
                Generate(1, true),
                Generate(2, true),
                Generate(3, false),
            };
            IEnumerable<ThemeVote> visibles = themeVotes.Where(c => c.Visible);

            var mockUoW = new Mock<IUnitOfWorkVote>();
            mockUoW.Setup(m => m.Themes.GetAll(false)).ReturnsAsync(themeVotes);
            mockUoW.Setup(m => m.Themes.GetAll(true)).ReturnsAsync(visibles);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ThemeVotesController.MANAGER_POLICY, !onlyVisible);
            var controller = new ThemeVotesController(mockUoW.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetThemeVotes().Result;

            TestUtils.ActionResultShouldGive(result, onlyVisible ? visibles : themeVotes);
        }

        private void GetTest(bool themeVoteVisible, bool onlyVisible)
        {
            ThemeVote themeVote = Generate(1, themeVoteVisible);
            bool see = themeVoteVisible || !onlyVisible;

            var mockUoW = new Mock<IUnitOfWorkVote>();
            mockUoW.Setup(m => m.Themes.GetById(themeVote.Id, false)).ReturnsAsync(themeVote);
            mockUoW.Setup(m => m.Themes.GetById(themeVote.Id, true)).ReturnsAsync(see ? themeVote : null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ThemeVotesController.MANAGER_POLICY, !onlyVisible);
            var controller = new ThemeVotesController(mockUoW.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetThemeVote(themeVote.Id).Result;

            if (see) TestUtils.ActionResultShouldGive(result, themeVote);
            else result.Result.Should().BeOfType<NotFoundResult>();
        }

        private IActionResult PutTest(int id, ThemeVote? themeVote, ThemeVote newThemeVote)
        {
            var mockUoW = new Mock<IUnitOfWorkVote>();
            if (themeVote is not null)
            {
                mockUoW.Setup(m => m.Themes.Exists(themeVote.Id)).ReturnsAsync(true);
                mockUoW.Setup(m => m.Themes.GetById(themeVote.Id, It.IsAny<bool>())).ReturnsAsync(themeVote);
                mockUoW.Setup(m => m.Themes.Update(newThemeVote)).Returns(() =>
                    Task.Run(() =>
                    {
                        themeVote.Id = newThemeVote.Id;
                        themeVote.NomTheme = newThemeVote.NomTheme;
                        themeVote.AssocJoueurs = newThemeVote.AssocJoueurs;
                        themeVote.Visible = newThemeVote.Visible;
                    })
                );
            }
            else
            {
                mockUoW.Setup(m => m.Themes.Exists(id)).ReturnsAsync(false);
                mockUoW.Setup(m => m.Themes.GetById(id, It.IsAny<bool>())).ReturnsAsync(() => null);
            }

            var controller = new ThemeVotesController(mockUoW.Object)
                .Validating(newThemeVote);

            return controller.PutThemeVote(id, newThemeVote).Result;
        }

        [TestMethod]
        public void ThemeVotesControllerTest()
        {
            var mockUoW = new Mock<IUnitOfWorkVote>();
            var controller = new ThemeVotesController(mockUoW.Object);
        }

        [TestMethod]
        public void GetThemeVotesTest_Moq_OnlyVisible_RightItems()
        {
            GetAllTest(onlyVisible: true);
        }

        [TestMethod]
        public void GetThemeVotesTest_Moq_SeeAll_RightItems()
        {
            GetAllTest(onlyVisible: false);
        }

        [TestMethod]
        public void GetThemeVoteTest_Moq_Visible_Admin_RightItem()
        {
            GetTest(themeVoteVisible: true, onlyVisible: false);
        }

        [TestMethod]
        public void GetThemeVoteTest_Moq_Visible_User_RightItem()
        {
            GetTest(themeVoteVisible: true, onlyVisible: true);
        }

        [TestMethod]
        public void GetThemeVoteTest_Moq_NotVisible_User_NotFound()
        {
            GetTest(themeVoteVisible: false, onlyVisible: true);
        }

        [TestMethod]
        public void GetThemeVoteTest_Moq_NotVisible_Admin_RightItem()
        {
            GetTest(themeVoteVisible: false, onlyVisible: false);
        }

        [TestMethod]
        public void GetThemeVoteTest_Moq_UnknownId_NotFound()
        {
            var mockUoW = new Mock<IUnitOfWorkVote>();
            mockUoW.Setup(m => m.Themes.GetById(1, It.IsAny<bool>())).ReturnsAsync(() => null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(ProduitsController.SEE_POLICY, true);
            var controller = new ThemeVotesController(mockUoW.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetThemeVote(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutThemeVoteTest_Moq_InvalidModelState_BadRequest()
        {
            ThemeVote themeVote = Generate(1, false);
            ThemeVote newThemeVote = new() { Id = 1 };

            var result = PutTest(themeVote.Id, themeVote, newThemeVote);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutThemeVoteTest_Moq_InvalidId_BadRequest()
        {
            ThemeVote themeVote = Generate(1, false);
            ThemeVote newThemeVote = Generate(2, false);

            var result = PutTest(themeVote.Id, themeVote, newThemeVote);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutThemeVoteTest_Moq_UnknownId_NotFound()
        {
            ThemeVote newThemeVote = Generate(1, false);

            var result = PutTest(newThemeVote.Id, null, newThemeVote);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostThemeVoteTest_Moq_InvalidModelState_BadRequest()
        {
            ThemeVote themeVote = new() { Id = 1 };

            var mockUoW = new Mock<IUnitOfWorkVote>();
            mockUoW.Setup(m => m.Themes.Add(themeVote));

            var controller = new ThemeVotesController(mockUoW.Object)
                .Validating(themeVote);

            var result = controller.PostThemeVote(themeVote).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostThemeVoteTest_Moq_CreatedAt()
        {
            ThemeVote themeVote = Generate(1, false);

            var mockUoW = new Mock<IUnitOfWorkVote>();
            mockUoW.Setup(m => m.Themes.Add(themeVote));

            var controller = new ThemeVotesController(mockUoW.Object)
                .Validating(themeVote);

            var result = controller.PostThemeVote(themeVote).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, ThemeVote>(result, themeVote);
        }

        [TestMethod]
        public void DeleteThemeVoteTest_Moq_NoContent()
        {
            ThemeVote themeVote = Generate(1, false);

            var mockUoW = new Mock<IUnitOfWorkVote>();
            mockUoW.Setup(m => m.Themes.GetById(themeVote.Id, It.IsAny<bool>())).ReturnsAsync(() => themeVote);
            var controller = new ThemeVotesController(mockUoW.Object);

            var result = controller.DeleteThemeVote(themeVote.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteThemeVoteTest_Moq_UnknownId_NotFound()
        {
            var mockUoW = new Mock<IUnitOfWorkVote>();
            mockUoW.Setup(m => m.Themes.GetById(1, It.IsAny<bool>())).ReturnsAsync(() => null);
            var controller = new ThemeVotesController(mockUoW.Object);

            var result = controller.DeleteThemeVote(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}