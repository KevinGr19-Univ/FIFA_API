using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using FIFA_APITests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public class TypesLivraisonControllerTests
    {
        private IActionResult PutTest(int id, TypeLivraison? typeLivraison, TypeLivraison newTypeLivraison)
        {
            var mockRepo = new Mock<IManagerTypeLivraison>();
            if (typeLivraison is not null)
            {
                mockRepo.Setup(m => m.Exists(typeLivraison.Id)).ReturnsAsync(true);
                mockRepo.Setup(m => m.GetById(typeLivraison.Id)).ReturnsAsync(typeLivraison);
                mockRepo.Setup(m => m.Update(newTypeLivraison)).Returns(() =>
                    Task.Run(() =>
                    {
                        typeLivraison.Id = newTypeLivraison.Id;
                        typeLivraison.Nom = newTypeLivraison.Nom;
                        typeLivraison.MaxBusinessDays = newTypeLivraison.MaxBusinessDays;
                        typeLivraison.Prix = newTypeLivraison.Prix;
                        typeLivraison.Commandes = newTypeLivraison.Commandes;
                    })
                );
            }

            var controller = new TypesLivraisonController(mockRepo.Object)
                .Validating(newTypeLivraison);

            return controller.PutTypeLivraison(id, newTypeLivraison).Result;
        }

        [TestMethod]
        public void TypesLivraisonControllerTest_Moq_NoException()
        {
            var mockRepo = new Mock<IManagerTypeLivraison>();
            var controller = new TypesLivraisonController(mockRepo.Object);
        }

        [TestMethod]
        public void GetTypesLivraisonTest_Moq_RightItems()
        {
            List<TypeLivraison> typeLivraisons = new()
            {
                new() { Id = 1, Nom = "TypeLivraison1", MaxBusinessDays = 1, Prix = 1 },
                new() { Id = 2, Nom = "TypeLivraison2", MaxBusinessDays = 2, Prix = 2 },
                new() { Id = 3, Nom = "TypeLivraison3", MaxBusinessDays = 3, Prix = 3 },
            };

            var mockRepo = new Mock<IManagerTypeLivraison>();
            mockRepo.Setup(m => m.GetAll()).ReturnsAsync(() => typeLivraisons);
            var controller = new TypesLivraisonController(mockRepo.Object);

            var result = controller.GetTypeLivraisons().Result;

            TestUtils.ActionResultShouldGive(result, typeLivraisons);
        }

        [TestMethod]
        public void GetTypeLivraisonTest_Moq_RightItem()
        {
            TypeLivraison typeLivraison = new() { Id = 1, Nom = "TypeLivraison1", MaxBusinessDays = 1, Prix = 1 };

            var mockRepo = new Mock<IManagerTypeLivraison>();
            mockRepo.Setup(m => m.GetById(typeLivraison.Id)).ReturnsAsync(() => typeLivraison);
            var controller = new TypesLivraisonController(mockRepo.Object);

            var result = controller.GetTypeLivraison(typeLivraison.Id).Result;

            TestUtils.ActionResultShouldGive(result, typeLivraison);
        }

        [TestMethod]
        public void GetTypeLivraisonTest_Moq_NotFound()
        {
            var mockRepo = new Mock<IManagerTypeLivraison>();
            var controller = new TypesLivraisonController(mockRepo.Object);

            var result = controller.GetTypeLivraison(1).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutTypeLivraisonTest_Moq_NoContent()
        {
            TypeLivraison typeLivraison = new() { Id = 1, Nom = "TypeLivraison1", MaxBusinessDays = 1, Prix = 1 };
            TypeLivraison newTypeLivraison = new() { Id = 1, Nom = "TypeLivraison2", MaxBusinessDays = 2, Prix = 2 };

            var result = PutTest(typeLivraison.Id, typeLivraison, newTypeLivraison);

            result.Should().BeOfType<NoContentResult>();
            typeLivraison.Should().Be(newTypeLivraison);
        }

        [TestMethod]
        public void PutTypeLivraisonTest_Moq_InvalidModelState_BadRequest()
        {
            TypeLivraison typeLivraison = new() { Id = 1, Nom = "TypeLivraison1", MaxBusinessDays = 1, Prix = 1 };
            TypeLivraison newTypeLivraison = new() { Id = 1, MaxBusinessDays = 2, Prix = 2 };

            var result = PutTest(typeLivraison.Id, typeLivraison, newTypeLivraison);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutTypeLivraisonTest_Moq_InvalidId_BadRequest()
        {
            TypeLivraison typeLivraison = new() { Id = 1, Nom = "TypeLivraison1", MaxBusinessDays = 1, Prix = 1 };
            TypeLivraison newTypeLivraison = new() { Id = 2, Nom = "TypeLivraison2", MaxBusinessDays = 2, Prix = 2 };

            var result = PutTest(typeLivraison.Id, typeLivraison, newTypeLivraison);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutTypeLivraisonTest_Moq_UnknownId_NotFound()
        {
            TypeLivraison newTypeLivraison = new() { Id = 1, Nom = "TypeLivraison2", MaxBusinessDays = 2, Prix = 2 };

            var result = PutTest(newTypeLivraison.Id, null, newTypeLivraison);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PostTypeLivraisonTest_Moq_InvalidModelState_BadRequest()
        {
            TypeLivraison typeLivraison = new() { Id = 1 };

            var mockRepo = new Mock<IManagerTypeLivraison>();
            var controller = new TypesLivraisonController(mockRepo.Object)
                .Validating(typeLivraison);

            var result = controller.PostTypeLivraison(typeLivraison).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostTypeLivraisonTest_Moq_CreatedAt()
        {
            TypeLivraison typeLivraison = new() { Id = 1, Nom = "TypeLivraison1", MaxBusinessDays = 1, Prix = 1 };

            var mockRepo = new Mock<IManagerTypeLivraison>();
            var controller = new TypesLivraisonController(mockRepo.Object)
                .Validating(typeLivraison);

            var result = controller.PostTypeLivraison(typeLivraison).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, TypeLivraison>(result, typeLivraison);
        }

        [TestMethod]
        public void DeleteTypeLivraisonTest_Moq_NoContent()
        {
            TypeLivraison typeLivraison = new() { Id = 1, Nom = "TypeLivraison1", MaxBusinessDays = 1, Prix = 1 };

            var mockRepo = new Mock<IManagerTypeLivraison>();
            mockRepo.Setup(m => m.GetById(typeLivraison.Id)).ReturnsAsync(() => typeLivraison);
            var controller = new TypesLivraisonController(mockRepo.Object);

            var result = controller.DeleteTypeLivraison(typeLivraison.Id).Result;

            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public void DeleteTypeLivraisonTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerTypeLivraison>();
            var controller = new TypesLivraisonController(mockRepo.Object);

            var result = controller.DeleteTypeLivraison(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}