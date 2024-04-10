using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using FIFA_APITests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIFA_API.Controllers.Tests
{
    public partial class VotesControllerTests
    {
        private (VotesController, Mock<IUnitOfWorkVote>) SetupUserTest(Utilisateur? user)
        {
            var mockUoW = new Mock<IUnitOfWorkVote>();
            if(user is not null)
            {
                mockUoW.Setup(m => m.Votes.GetUserVotes(user.Id)).ReturnsAsync(user.Votes);
            }

            var mockHttpCtx = new MockHttpContext()
                .MockAuthentication(user);

            var controller = new VotesController(mockUoW.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };
            return (controller, mockUoW);
        }

        [TestMethod]
        public void GetMyVotesTest_Moq_User_RightItems()
        {
            Utilisateur user = new() { Id = 1 };
            user.Votes = new List<VoteUtilisateur>()
            {
                Generate(1, user.Id),
                Generate(2, user.Id),
                Generate(3, user.Id),
            };

            (var controller, var mockUoW) = SetupUserTest(user);

            var result = controller.GetMyVotes().Result;

            TestUtils.ActionResultShouldGive(result, user.Votes);
        }

        [TestMethod]
        public void GetMyVoteTest_Moq_User_IsOwnVote_RightItem()
        {
            Utilisateur user = new() { Id = 1 };
            VoteUtilisateur vote = Generate(2, user.Id);

            (var controller, var mockUoW) = SetupUserTest(user);
            mockUoW.Setup(m => m.Votes.GetById(vote.IdTheme, vote.IdUtilisateur)).ReturnsAsync(vote);

            var result = controller.GetMyVote(vote.IdTheme).Result;

            TestUtils.ActionResultShouldGive(result, vote);
        }

        [TestMethod]
        public void GetMyVoteTest_Moq_User_IsNotOwnVote_RightItem()
        {
            Utilisateur user = new() { Id = 1 };
            VoteUtilisateur vote = Generate(2, user.Id + 1);

            (var controller, var mockUoW) = SetupUserTest(user);
            mockUoW.Setup(m => m.Votes.GetById(vote.IdTheme, vote.IdUtilisateur)).ReturnsAsync(vote);

            var result = controller.GetMyVote(vote.IdTheme).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void GetMyVoteTest_Moq_User_UnknownId_NotFound()
        {
            Utilisateur user = new() { Id = 1 };

            (var controller, var mockUoW) = SetupUserTest(user);
            mockUoW.Setup(m => m.Votes.GetById(1, user.Id));

            var result = controller.GetMyVote(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }
    }
}
