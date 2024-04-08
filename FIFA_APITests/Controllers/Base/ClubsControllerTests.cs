using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using FIFA_APITests.Controllers.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static System.Reflection.Metadata.BlobBuilder;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public class ClubsControllerTests
    {
        private IActionResult PutTest(int id, Club? club, Club newClub)
        {
            var mockRepo = new Mock<IManagerClub>();
            if (club is not null)
            {
                mockRepo.Setup(m => m.Exists(club.Id)).ReturnsAsync(true);
                mockRepo.Setup(m => m.GetById(club.Id)).ReturnsAsync(club);
                mockRepo.Setup(m => m.Update(newClub)).Returns(() =>
                    Task.Run(() =>
                    {
                        club.Id = newClub.Id;
                        club.Nom = newClub.Nom;
                    })
                );
            }

            var controller = new ClubsController(mockRepo.Object);

            return controller.PutClub(id, newClub).Result;
        }

        [TestMethod]
        public void ClubsControllerTest_Moq_NoException()
        {
            var mockRepo = new Mock<IManagerClub>();
            var controller = new ClubsController(mockRepo.Object);
        }

        [TestMethod]
        public void GetClubsTest_Moq_RightItems()
        {
            List<Club> clubs = new()
            {
                new() { Id = 1, Nom = "Club1" },
                new() { Id = 2, Nom = "Club2" },
                new() { Id = 3, Nom = "Club3" },
            };

            var mockRepo = new Mock<IManagerClub>();
            mockRepo.Setup(m => m.GetAll()).ReturnsAsync(() => clubs);
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.GetClubs().Result;

            TestUtils.ActionResultShouldGive(result, clubs);
        }

        [TestMethod]
        public void GetClubTest_Moq_RightItem()
        {
            Club club = new() { Id = 1, Nom = "Club1" };

            var mockRepo = new Mock<IManagerClub>();
            mockRepo.Setup(m => m.GetById(club.Id)).ReturnsAsync(() => club);
            var controller = new ClubsController(mockRepo.Object);
            var result = controller.GetClub(club.Id).Result;

            TestUtils.ActionResultShouldGive(result, club);
        }

        [TestMethod]
        public void GetClubTest_Moq_NotFound()
        {
            var mockRepo = new Mock<IManagerClub>();
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.GetClub(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutClubTest_Moq_NoContent()
        {
            Club club = new() { Id = 1, Nom = "Club1" };
            Club newClub = new() { Id = 1, Nom = "Club2" };

            var result = PutTest(club.Id, club, newClub);

            result.Should().BeOfType<NoContentResult>();
            club.Should().Be(newClub);
        }

        [TestMethod]
        public void PutClubTest_Moq_InvalidModelState_BadRequest()
        {
            Club club = new() { Id = 1, Nom = "Club1" };
            Club newClub = new() { Id = 1 };

            var result = PutTest(club.Id, club, newClub);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutClubTest_Moq_InvalidId_BadRequest()
        {
            Club club = new() { Id = 1, Nom = "Club1" };
            Club newClub = new() { Id = 2, Nom = "Club2" };

            var result = PutTest(club.Id, club, newClub);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutClubTest_Moq_UnknownId_NotFound()
        {
            Club newClub = new() { Id = 1, Nom = "Club2" };

            var result = PutTest(newClub.Id, null, newClub);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostClubTest_Moq_InvalidModelState_BadRequest()
        {
            Club club = new() { Id = 1 };

            var mockRepo = new Mock<IManagerClub>();
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.PostClub(club).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostClubTest_Moq_CreatedAt()
        {
            Club club = new() { Id = 1, Nom = "Club1" };

            var mockRepo = new Mock<IManagerClub>();
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.PostClub(club).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Club>(result, club);
        }

        [TestMethod]
        public void DeleteClubTest_Moq_NoContent()
        {
            Club club = new() { Id = 1, Nom = "Club1" };

            var mockRepo = new Mock<IManagerClub>();
            mockRepo.Setup(m => m.GetById(club.Id)).ReturnsAsync(() => club);
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.DeleteClub(club.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteClubTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerClub>();
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.DeleteClub(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}