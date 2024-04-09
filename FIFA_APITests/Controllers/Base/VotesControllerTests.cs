using Microsoft.VisualStudio.TestTools.UnitTesting;
using FIFA_API.Controllers.Base;
using System;
using Microsoft.AspNetCore.Mvc;
using FIFA_API.Models.EntityFramework;
using Moq;
using FIFA_API.Repositories.Contracts;
using FIFA_APITests.Utils;
using FluentAssertions;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public partial class VotesControllerTests
    {
        private VoteUtilisateur Generate(int idtheme, int iduser)
        {
            Random r = new();
            return new()
            {
                IdTheme = idtheme,
                IdUtilisateur = iduser,
                IdJoueur1 = r.Next(1, 50),
                IdJoueur2 = r.Next(1, 50),
                IdJoueur3 = r.Next(1, 50),
            };
        }

        private ActionResult<VoteUtilisateur> PostTest(VoteUtilisateur vote, bool exists = false, bool isValid = true)
        {
            var mockRepo = new Mock<IUnitOfWorkVote>();
            mockRepo.Setup(m => m.IsVoteValid(vote)).ReturnsAsync(isValid);
            mockRepo.Setup(m => m.Votes.Exists(vote.IdTheme, vote.IdUtilisateur)).ReturnsAsync(exists);
            mockRepo.Setup(m => m.Votes.Add(vote));

            var controller = new VotesController(mockRepo.Object)
                .Validating(vote);

            var result = controller.PostVoteUtilisateur(vote).Result;
            return result;
        }

        private IActionResult PutTest(int idtheme, int idutilisateur, VoteUtilisateur? vote, VoteUtilisateur newVote, bool isValid = true)
        {
            var mockRepo = new Mock<IUnitOfWorkVote>();
            if(vote is not null)
            {
                mockRepo.Setup(m => m.Votes.Exists(vote.IdTheme, vote.IdUtilisateur)).ReturnsAsync(true);
                mockRepo.Setup(m => m.Votes.GetById(vote.IdTheme, vote.IdUtilisateur)).ReturnsAsync(vote);

                mockRepo.Setup(m => m.IsVoteValid(newVote)).ReturnsAsync(isValid);
                mockRepo.Setup(m => m.Votes.Update(newVote)).Returns(() =>
                    Task.Run(() =>
                    {
                        vote.IdTheme = newVote.IdTheme;
                        vote.IdUtilisateur = newVote.IdUtilisateur;
                        vote.IdJoueur1 = newVote.IdJoueur1;
                        vote.IdJoueur2 = newVote.IdJoueur2;
                        vote.IdJoueur3 = newVote.IdJoueur3;
                        vote.ThemeVote = newVote.ThemeVote;
                        vote.Utilisateur = newVote.Utilisateur;
                        vote.Joueur1 = newVote.Joueur1;
                        vote.Joueur2 = newVote.Joueur2;
                        vote.Joueur3 = newVote.Joueur3;
                    })
                );
            }
            else
            {
                mockRepo.Setup(m => m.Votes.Exists(idtheme, idutilisateur)).ReturnsAsync(false);
                mockRepo.Setup(m => m.Votes.GetById(idtheme, idutilisateur)).ReturnsAsync(() => null);
            }

            var controller = new VotesController(mockRepo.Object)
                .Validating(newVote);

            var result = controller.PutVoteUtilisateur(idtheme, idutilisateur, newVote).Result;
            return result;
        }

        [TestMethod]
        public void VotesControllerTest_Moq_NoException()
        {
            var mockRepo = new Mock<IUnitOfWorkVote>();
            var controller = new VotesController(mockRepo.Object);
        }

        [TestMethod]
        public void GetVoteUtilisateursTest_RightItems()
        {
            List<VoteUtilisateur> votes = new()
            {
                Generate(1, 1),
                Generate(1, 2),
                Generate(2, 1),
            };

            var mockRepo = new Mock<IUnitOfWorkVote>();
            mockRepo.Setup(m => m.Votes.GetAll()).ReturnsAsync(votes);
            var controller = new VotesController(mockRepo.Object);

            var result = controller.GetVoteUtilisateurs().Result;

            TestUtils.ActionResultShouldGive(result, votes);
        }

        [TestMethod]
        public void GetVoteUtilisateurTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IUnitOfWorkVote>();
            mockRepo.Setup(m => m.Votes.GetById(1, 1));
            var controller = new VotesController(mockRepo.Object);

            var result = controller.GetVoteUtilisateur(1, 1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void GetVoteUtilisateurTest_Moq_RightItem()
        {
            VoteUtilisateur vote = Generate(1, 3);

            var mockRepo = new Mock<IUnitOfWorkVote>();
            mockRepo.Setup(m => m.Votes.GetById(vote.IdTheme, vote.IdUtilisateur)).ReturnsAsync(vote);
            var controller = new VotesController(mockRepo.Object);

            var result = controller.GetVoteUtilisateur(vote.IdTheme, vote.IdUtilisateur).Result;

            TestUtils.ActionResultShouldGive(result, vote);
        }

        [TestMethod]
        public void PutVoteUtilisateurTest_Moq_VoteIsNotValid_BadRequest()
        {
            VoteUtilisateur vote = Generate(1, 3);
            VoteUtilisateur newVote = Generate(1, 3);

            var result = PutTest(vote.IdTheme, vote.IdUtilisateur, vote, newVote, isValid: false);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutVoteUtilisateurTest_Moq_InvalidId_BadRequest()
        {
            VoteUtilisateur vote = Generate(1, 3);
            VoteUtilisateur newVote = Generate(1, 2);

            var result = PutTest(vote.IdTheme, vote.IdUtilisateur, vote, newVote);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutVoteUtilisateurTest_Moq_UnknownId_NotFound()
        {
            VoteUtilisateur newVote = Generate(1, 3);

            var result = PutTest(newVote.IdTheme, newVote.IdUtilisateur, null, newVote);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutVoteUtilisateurTest_Moq_NoContent()
        {
            VoteUtilisateur vote = Generate(1, 3);
            VoteUtilisateur newVote = Generate(1, 3);

            var result = PutTest(vote.IdTheme, vote.IdUtilisateur, vote, newVote);

            result.Should().BeOfType<NoContentResult>();
            vote.Should().Be(newVote);
        }

        [TestMethod]
        public void PostVoteUtilisateurTest_Moq_VoteIsNotValid_BadRequest()
        {
            VoteUtilisateur vote = Generate(1, 3);

            var result = PostTest(vote, isValid: false);

            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PostVoteUtilisateurTest_Moq_ExistingId_Conflict()
        {
            VoteUtilisateur vote = Generate(1, 3);

            var result = PostTest(vote, exists: true);

            result.Result.Should().BeOfType<ConflictResult>();
        }

        [TestMethod]
        public void PostVoteUtilisateurTest_Moq_CreatedAt()
        {
            VoteUtilisateur vote = Generate(1, 3);

            var result = PostTest(vote);

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, VoteUtilisateur>(result, vote);
        }

        [TestMethod]
        public void DeleteVoteUtilisateurTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IUnitOfWorkVote>();
            mockRepo.Setup(m => m.Votes.GetById(1, 3));
            var controller = new VotesController(mockRepo.Object);

            var result = controller.DeleteVoteUtilisateur(1, 3).Result;

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void DeleteVoteUtilisateurTest_Moq_NoContent()
        {
            VoteUtilisateur vote = Generate(1, 3);

            var mockRepo = new Mock<IUnitOfWorkVote>();
            mockRepo.Setup(m => m.Votes.GetById(vote.IdTheme, vote.IdUtilisateur)).ReturnsAsync(vote);
            mockRepo.Setup(m => m.Votes.Delete(vote));
            var controller = new VotesController(mockRepo.Object);

            var result = controller.DeleteVoteUtilisateur(vote.IdTheme, vote.IdUtilisateur).Result;

            result.Should().BeOfType<NoContentResult>();
        }
    }
}