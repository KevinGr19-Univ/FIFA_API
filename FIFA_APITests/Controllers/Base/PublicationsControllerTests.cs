using Microsoft.VisualStudio.TestTools.UnitTesting;
using FIFA_API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIFA_API.Models.EntityFramework;
using System.Reflection;
using Moq;
using FIFA_API.Repositories.Contracts;
using FIFA_APITests.Utils;
using FluentAssertions;
using Npgsql.Internal.TypeHandlers.FullTextSearchHandlers;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public class PublicationsControllerTests
    {
        private T Generate<T>(int id, bool visible) where T : Publication, new()
        {
            ConstructorInfo? ctor = typeof(T).GetConstructor(new Type[0]);
            if (ctor is null) throw new ArgumentException($"{nameof(T)} needs a parameterless constructor");

            T pub = (T)ctor.Invoke(parameters: null);

            pub.Id = id;
            pub.Visible = visible;
            pub.Titre = $"Titre{id}";
            pub.Resume = $"Resume{id}";
            pub.DatePublication = DateTime.Now.AddDays(-20);
            pub.IdPhoto = null;

            return pub;
        }

        private void GetTest(bool publicationVisible, bool onlyVisible)
        {
            Publication publication = Generate<Blog>(1, publicationVisible);
            bool see = publicationVisible || !onlyVisible;

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Publications.GetByIdWithPhoto(publication.Id, false)).ReturnsAsync(publication);
            mockUoW.Setup(m => m.Publications.GetByIdWithPhoto(publication.Id, true)).ReturnsAsync(publicationVisible ? publication : null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(PublicationsController.MANAGER_POLICY, !onlyVisible);
            var controller = new PublicationsController(mockUoW.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetPublication(publication.Id).Result;

            if (see) TestUtils.ActionResultShouldGive(result, publication);
            else result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PublicationsControllerTest_Moq_NoException()
        {
            var mockUoW = new Mock<IUnitOfWorkPublication>();
            var controller = new PublicationsController(mockUoW.Object);
        }

        [TestMethod]
        public void GetAllPublicationsTest_Moq_RightItems()
        {
            List<Publication> publications = new()
            {
                Generate<Album>(1, true),
                Generate<Document>(2, true),
                Generate<Article>(3, false),
            };
            IEnumerable<Publication> visibles = publications.Where(p => p.Visible);

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Publications.GetAll(false)).ReturnsAsync(publications);
            mockUoW.Setup(m => m.Publications.GetAll(true)).ReturnsAsync(visibles);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(PublicationsController.MANAGER_POLICY, true);
            var controller = new PublicationsController(mockUoW.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetAllPublications().Result;

            TestUtils.ActionResultShouldGive(result, publications);
        }

        [TestMethod]
        public void GetPublicationTest_Moq_NotVisible_User_NotFound()
        {
            GetTest(publicationVisible: false, onlyVisible: true);
        }

        [TestMethod]
        public void GetPublicationTest_Moq_NotVisible_Admin_NotFound()
        {
            GetTest(publicationVisible: false, onlyVisible: false);
        }

        [TestMethod]
        public void GetPublicationTest_Moq_Visible_User_NoContent()
        {
            GetTest(publicationVisible: true, onlyVisible: true);
        }

        [TestMethod]
        public void GetPublicationTest_Moq_Visible_Admin_NoContent()
        {
            GetTest(publicationVisible: true, onlyVisible: false);
        }

        public void GetPublicationTest_Moq_UnknownId_NotFound()
        {
            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Publications.GetByIdWithPhoto(1, It.IsAny<bool>())).ReturnsAsync(() => null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(PublicationsController.MANAGER_POLICY, true);
            var controller = new PublicationsController(mockUoW.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetPublication(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void DeletePublicationTest_Moq_UnknownId_NotFound()
        {
            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Publications.GetById(1, It.IsAny<bool>())).ReturnsAsync(() => null);
            var controller = new PublicationsController(mockUoW.Object);

            var result = controller.DeletePublication(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void DeletePublicationTest_Moq_NoContent()
        {
            Publication publication = Generate<Document>(1, false);

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Publications.GetById(1, false)).ReturnsAsync(publication);
            mockUoW.Setup(m => m.Publications.GetById(1, true)).ReturnsAsync(() => null);
            var controller = new PublicationsController(mockUoW.Object);

            var result = controller.DeletePublication(1).Result;

            result.Should().BeOfType<NoContentResult>();
        }
    }
}