using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using FIFA_APITests.Controllers.Utils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FIFA_API.Controllers.Tests
{
    [TestClass()]
    public class CommandesControllerTests
    {
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

            var controller = new CommandesController(mockUoW.Object, null) { ControllerContext = TestUtils.MockAuthenticationCTX(user) };

            var result = controller.GetCommande(1).Result;

            result.Result.Should().BeNull();
            result.Value.Should().BeOfType<CommandeDetails>().And.Be(commandeDetails);
        }

        [TestMethod]
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