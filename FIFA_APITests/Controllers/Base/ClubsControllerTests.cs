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

namespace FIFA_API.Controllers.Tests
{
    [TestClass()]
    public class ClubsControllerTests
    {
        [TestMethod()]
        public void ClubsControllerTest_NoException()
        {
            var mockRepo = new Mock<IManagerClub>();
            var controller = new ClubsController(mockRepo.Object);
        }

        [TestMethod()]
        public void GetClubsTest_RightItems()
        {
            List<Club> clubs = new()
            {
                new() { Id = 1, Nom = "Club1" },
                new() { Id = 2, Nom = "Club2" },
                new() { Id = 3, Nom = "Club3" },
            };

            var mockRepo = new Mock<IManagerClub>();
            mockRepo.Setup(m => m.GetAll().Result).Returns(() => clubs);
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.GetClubs().Result;

            var okObjRes = TestUtils.ToType<OkObjectResult>(result.Result);
            var value = TestUtils.ToType<IEnumerable<Club>>(okObjRes.Value);
            CollectionAssert.AreEquivalent(clubs, value.ToList());
        }

        [TestMethod()]
        public void GetClubTest_RightItem()
        {
            Club club = new() { Id = 1, Nom = "Club1" };

            var mockRepo = new Mock<IManagerClub>();
            mockRepo.Setup(m => m.GetById(1).Result).Returns(() => club);
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.GetClub(1).Result;

            var okObjRes = TestUtils.ToType<OkObjectResult>(result.Result);
            var value = TestUtils.ToType<Club>(okObjRes.Value);
            Assert.AreEqual(club, value, $"L'objet {nameof(Club)} retourné n'est pas celui attendu.");
        }

        [TestMethod()]
        public void GetClubTest_NotFound()
        {
            var mockRepo = new Mock<IManagerClub>();
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.GetClub(1).Result;
            Assert.IsInstanceOfType<NotFoundResult>(result.Result);
        }

        [TestMethod()]
        public void PutClubTest_NoContent()
        {
            Club club = new() { Id = 1, Nom = "Club1" };
            Club newClub = new() { Id = 1, Nom = "Club2" };

            var mockRepo = new Mock<IManagerClub>();
            mockRepo.Setup(m => m.GetById(1).Result).Returns(() => club);
            mockRepo.Setup(m => m.Update(newClub)).Returns(() =>
                Task.Run(() =>
                {
                    club.Id = newClub.Id;
                    club.Nom = newClub.Nom;
                })
            );
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.PutClub(1, newClub).Result;

            Assert.IsInstanceOfType<NoContentResult>(result);
            Assert.AreEqual(club, newClub);
        }

        [TestMethod()]
        public void PutClubTest_NotFound()
        {
            Club newClub = new() { Id = 1, Nom = "Club2" };

            var mockRepo = new Mock<IManagerClub>();
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.PutClub(1, newClub).Result;

            Assert.IsInstanceOfType<NotFoundResult>(result);
        }

        [TestMethod()]
        public void PostClubTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteClubTest_NoContent()
        {
            Club club = new() { Id = 1, Nom = "Club1" };

            var mockRepo = new Mock<IManagerClub>();
            mockRepo.Setup(m => m.GetById(1).Result).Returns(() => club);
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.DeleteClub(1).Result;

            Assert.IsInstanceOfType<NoContentResult>(result);
        }

        [TestMethod()]
        public void DeleteClubTest_NotFound()
        {
            var mockRepo = new Mock<IManagerClub>();
            var controller = new ClubsController(mockRepo.Object);

            var result = controller.DeleteClub(1).Result;

            Assert.IsInstanceOfType<NotFoundResult>(result);
        }
    }
}