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
using FluentAssertions;
using FIFA_API.Models.Controllers;
using Microsoft.AspNetCore.Http;
using FIFA_API.Contracts;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers.Tests
{
    [TestClass()]
    public class CommandesControllerTests
    {
        private ControllerContext MockAuthenticationCTX(Utilisateur? userToReturn)
        {
            ClaimsPrincipal claims = new();

            var mockTokenService = new Mock<ITokenService>();

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.User).Returns(claims);

            mockTokenService.Setup(t => t.GetUserFromPrincipalAsync(claims)).ReturnsAsync(userToReturn);
            mockContext.Setup(c => c.RequestServices.GetService(typeof(ITokenService))).Returns(mockTokenService.Object);

            return new ControllerContext()
            {
                HttpContext = mockContext.Object,
            };
        }

        [TestMethod()]
        public void CommandesControllerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetCommandesTest_Moq_RightItem()
        {
            Commande commande = new()
            {
                Id = 1,
                IdUtilisateur = 1
            };
            CommandeDetails commandeDetails = new() { Commande = commande };
            Utilisateur user = new() { Id = 1 };

            var mockUoW = new Mock<IUnitOfWorkCommande>();
            mockUoW.Setup(m => m.Commandes.GetByIdWithAll(1)).ReturnsAsync(commande);
            mockUoW.Setup(m => m.GetDetails(commande)).ReturnsAsync(commandeDetails);

            var controller = new CommandesController(mockUoW.Object, null) { ControllerContext = MockAuthenticationCTX(user) };

            var result = controller.GetCommande(1).Result;

            result.Result.Should().BeNull();
            result.Value.Should().BeOfType<CommandeDetails>().And.Be(commandeDetails);
        }

        [TestMethod()]
        public void GetCommandeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PutCommandeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PostCommandeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteCommandeTest()
        {
            Assert.Fail();
        }
    }
}