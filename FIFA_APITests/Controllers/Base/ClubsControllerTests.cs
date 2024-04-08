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
using Microsoft.AspNetCore.Mvc;
using FIFA_APITests;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using Sprache;

namespace FIFA_API.Controllers.Tests
{
    [TestClass()]
    public class ClubsControllerTests
    {
        [TestMethod()]
        public void ClubsControllerTest_Moq_NoException()
        {
            var mockRepo = new Mock<IManagerClub>();
            var controller = new ClubsController(mockRepo.Object);
        }

        [TestMethod()]
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

            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<Club>>()
                .And.BeEquivalentTo(clubs);
        }

        [TestMethod()]
        public void GetClubTest_Moq_RightItem()
        {
            Club club = new() { Id = 1, Nom = "Club1" };

            var mockRepo = new Mock<IManagerClub>();
            mockRepo.Setup(m => m.GetById(1)).ReturnsAsync(() => club);
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.GetClub(1).Result;

            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<Club>()
                .And.Be(club);
        }

        [TestMethod()]
        public void GetClubTest_Moq_NotFound()
        {
            var mockRepo = new Mock<IManagerClub>();
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.GetClub(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod()]
        public void PutClubTest_Moq_NoContent()
        {
            Club club = new() { Id = 1, Nom = "Club1" };
            Club newClub = new() { Id = 1, Nom = "Club2" };

            var mockRepo = new Mock<IManagerClub>();
            mockRepo.Setup(m => m.Exists(1)).ReturnsAsync(true);
            mockRepo.Setup(m => m.GetById(1)).ReturnsAsync(club);
            mockRepo.Setup(m => m.Update(newClub)).Returns(() =>
                Task.Run(() =>
                {
                    club.Id = newClub.Id;
                    club.Nom = newClub.Nom;
                })
            );
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.PutClub(1, newClub).Result;

            result.Should().BeOfType<NoContentResult>();
            club.Should().Be(newClub);
        }

        [TestMethod()]
        public void PutClubTest_Moq_BadRequest()
        {
            Club club = new() { Id = 1, Nom = "Club1" };
            Club newClub = new() { Id = 1 };

            var mockRepo = new Mock<IManagerClub>();
            mockRepo.Setup(m => m.Exists(1)).ReturnsAsync(true);
            mockRepo.Setup(m => m.GetById(1)).ReturnsAsync(club);
            mockRepo.Setup(m => m.Update(newClub)).Returns(() =>
                Task.Run(() =>
                {
                    club.Id = newClub.Id;
                    club.Nom = newClub.Nom;
                })
            );
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.PutClub(1, newClub).Result;

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod()]
        public void PutClubTest_Moq_NotFound()
        {
            Club newClub = new() { Id = 1, Nom = "Club2" };

            var mockRepo = new Mock<IManagerClub>();
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.PutClub(1, newClub).Result;

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod()]
        public void PostClubTest_Moq_BadRequest()
        {
            Club club = new() { Id = 1, Nom = new string('a', Club.MAX_NOM_LENGTH + 2) };

            var mockRepo = new Mock<IManagerClub>();
            var controller = new ClubsController(mockRepo.Object);

            controller.TryValidateModel(club);
            var result = controller.PostClub(club).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod()]
        public void DeleteClubTest_Moq_NoContent()
        {
            Club club = new() { Id = 1, Nom = "Club1" };

            var mockRepo = new Mock<IManagerClub>();
            mockRepo.Setup(m => m.GetById(1)).ReturnsAsync(() => club);
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.DeleteClub(1).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod()]
        public void DeleteClubTest_Moq_NotFound()
        {
            var mockRepo = new Mock<IManagerClub>();
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.DeleteClub(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}