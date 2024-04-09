using Microsoft.VisualStudio.TestTools.UnitTesting;
using FIFA_API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using FIFA_API.Repositories.Contracts;
using FIFA_API.Models.EntityFramework;
using FIFA_APITests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public class DocumentsControllerTests
    {
        private Document Generate(int id, bool visible)
        {
            Random r = new();
            return new Document()
            {
                Id = id,
                IdPhoto = null,
                DatePublication = DateTime.Today.AddDays(-r.Next(10)),
                Titre = $"Titre{id}",
                Resume = $"Resume{id}",
                UrlPdf = $"http://fifapi.test.com/{r.Next(100)}",
                Visible = visible
            };
        }

        private void GetTest(bool documentVisible, bool onlyVisible)
        {
            Document produit = Generate(1, documentVisible);
            bool see = documentVisible || !onlyVisible;

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Documents.GetByIdWithAll(produit.Id, false)).ReturnsAsync(produit);
            mockUoW.Setup(m => m.Documents.GetByIdWithAll(produit.Id, true)).ReturnsAsync(see ? produit : null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(PublicationsController.MANAGER_POLICY, !onlyVisible);
            var controller = new DocumentsController(mockUoW.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetDocument(produit.Id).Result;

            if (see) TestUtils.ActionResultShouldGive(result, produit);
            else result.Result.Should().BeOfType<NotFoundResult>();
        }

        private IActionResult PutTest(int id, Document? document, Document newDocument)
        {
            var mockUoW = new Mock<IUnitOfWorkPublication>();
            if (document is not null)
            {
                mockUoW.Setup(m => m.Documents.Exists(document.Id)).ReturnsAsync(true);
                mockUoW.Setup(m => m.Documents.GetById(document.Id, It.IsAny<bool>())).ReturnsAsync(document);
                mockUoW.Setup(m => m.Documents.Update(newDocument)).Returns(() =>
                    Task.Run(() =>
                    {
                        document.Id = newDocument.Id;
                        document.IdPhoto = newDocument.IdPhoto;
                        document.DatePublication = newDocument.DatePublication;
                        document.Titre = newDocument.Titre;
                        document.Resume = newDocument.Resume;
                        document.Visible = newDocument.Visible;
                        document.Joueurs = newDocument.Joueurs;
                        document.UrlPdf = newDocument.UrlPdf;
                    })
                );
            }
            else
            {
                mockUoW.Setup(m => m.Documents.Exists(id)).ReturnsAsync(false);
                mockUoW.Setup(m => m.Documents.GetById(id, It.IsAny<bool>())).ReturnsAsync(() => null);
            }

            var controller = new DocumentsController(mockUoW.Object)
                .Validating(newDocument);

            return controller.PutDocument(id, newDocument).Result;
        }

        [TestMethod]
        public void DocumentsControllerTest_Moq_NoException()
        {
            var mockUoW = new Mock<IUnitOfWorkPublication>();
            var controller = new DocumentsController(mockUoW.Object);
        }

        [TestMethod]
        public void GetDocumentTest_Moq_Visible_Admin_RightItem()
        {
            GetTest(documentVisible: true, onlyVisible: false);
        }

        [TestMethod]
        public void GetDocumentTest_Moq_NotVisible_Admin_RightItem()
        {
            GetTest(documentVisible: false, onlyVisible: false);
        }

        [TestMethod]
        public void GetDocumentTest_Moq_Visible_User_RightItem()
        {
            GetTest(documentVisible: true, onlyVisible: true);
        }

        [TestMethod]
        public void GetDocumentTest_Moq_NotVisible_User_NotFound()
        {
            GetTest(documentVisible: false, onlyVisible: true);
        }

        [TestMethod]
        public void PostDocumentTest_Moq_InvalidModelState_BadRequest()
        {
            Document document = new() { Id = 1 };

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Documents.Add(document));

            var controller = new DocumentsController(mockUoW.Object)
                .Validating(document);

            var result = controller.PostDocument(document).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostDocumentTest_Moq_CreatedAt()
        {
            Document document = Generate(1, false);

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Documents.Add(document));

            var controller = new DocumentsController(mockUoW.Object)
                .Validating(document);

            var result = controller.PostDocument(document).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Document>(result, document);
        }

        [TestMethod]
        public void PutDocumentTest_Moq_InvalidModelState_BadRequest()
        {
            Document document = Generate(1, true);
            Document newDocument = new() { Id = 1 };

            var result = PutTest(document.Id, document, newDocument);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutDocumentTest_Moq_InvalidId_BadRequest()
        {
            Document document = Generate(1, true);
            Document newDocument = Generate(2, true);

            var result = PutTest(document.Id, document, newDocument);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutDocumentTest_Moq_UnknownId_NotFound()
        {
            Document newDocument = Generate(1, true);

            var result = PutTest(newDocument.Id, null, newDocument);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutDocumentTest_Moq_NoContent()
        {
            Document document = Generate(1, true);
            Document newDocument = Generate(1, true);

            var result = PutTest(document.Id, document, newDocument);

            result.Should().BeOfType<NoContentResult>();
            document.Should().Be(newDocument);
        }
    }
}