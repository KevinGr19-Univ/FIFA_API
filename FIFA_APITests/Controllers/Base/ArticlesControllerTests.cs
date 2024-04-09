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
using System.Reflection.Metadata;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public class ArticlesControllerTests
    {
        private Article Generate(int id, bool visible)
        {
            Random r = new();
            return new Article()
            {
                Id = id,
                IdPhoto = null,
                Texte = $"Texte{id}-{r.Next(100)}",
                DatePublication = DateTime.Today.AddDays(-r.Next(10)),
                Titre = $"Titre{id}",
                Resume = $"Resume{id}",
                Visible = visible
            };
        }

        private void GetTest(bool articleVisible, bool onlyVisible)
        {
            Article produit = Generate(1, articleVisible);
            bool see = articleVisible || !onlyVisible;

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Articles.GetByIdWithAll(produit.Id, false)).ReturnsAsync(produit);
            mockUoW.Setup(m => m.Articles.GetByIdWithAll(produit.Id, true)).ReturnsAsync(see ? produit : null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(PublicationsController.MANAGER_POLICY, !onlyVisible);
            var controller = new ArticlesController(mockUoW.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetArticle(produit.Id).Result;

            if (see) TestUtils.ActionResultShouldGive(result, produit);
            else result.Result.Should().BeOfType<NotFoundResult>();
        }

        private IActionResult PutTest(int id, Article? article, Article newArticle)
        {
            var mockUoW = new Mock<IUnitOfWorkPublication>();
            if (article is not null)
            {
                mockUoW.Setup(m => m.Articles.Exists(article.Id)).ReturnsAsync(true);
                mockUoW.Setup(m => m.Articles.GetById(article.Id, It.IsAny<bool>())).ReturnsAsync(article);
                mockUoW.Setup(m => m.Articles.Update(newArticle)).Returns(() =>
                    Task.Run(() =>
                    {
                        article.Id = newArticle.Id;
                        article.IdPhoto = newArticle.IdPhoto;
                        article.Texte = newArticle.Texte;
                        article.DatePublication = newArticle.DatePublication;
                        article.Titre = newArticle.Titre;
                        article.Resume = newArticle.Resume;
                        article.Visible = newArticle.Visible;
                        article.Joueurs = newArticle.Joueurs;
                        article.Videos = newArticle.Videos;
                        article.Photos = newArticle.Photos;
                    })
                );
            }
            else
            {
                mockUoW.Setup(m => m.Articles.Exists(id)).ReturnsAsync(false);
                mockUoW.Setup(m => m.Articles.GetById(id, It.IsAny<bool>())).ReturnsAsync(() => null);
            }

            var controller = new ArticlesController(mockUoW.Object)
                .Validating(newArticle);

            return controller.PutArticle(id, newArticle).Result;
        }

        [TestMethod]
        public void ArticlesControllerTest_Moq_NoException()
        {
            var mockUoW = new Mock<IUnitOfWorkPublication>();
            var controller = new ArticlesController(mockUoW.Object);
        }

        [TestMethod]
        public void GetArticleTest_Moq_Visible_Admin_RightItem()
        {
            GetTest(articleVisible: true, onlyVisible: false);
        }

        [TestMethod]
        public void GetArticleTest_Moq_NotVisible_Admin_RightItem()
        {
            GetTest(articleVisible: false, onlyVisible: false);
        }

        [TestMethod]
        public void GetArticleTest_Moq_Visible_User_RightItem()
        {
            GetTest(articleVisible: true, onlyVisible: true);
        }

        [TestMethod]
        public void GetArticleTest_Moq_NotVisible_User_NotFound()
        {
            GetTest(articleVisible: false, onlyVisible: true);
        }

        [TestMethod]
        public void PostArticleTest_Moq_InvalidModelState_BadRequest()
        {
            Article article = new() { Id = 1 };

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Articles.Add(article));

            var controller = new ArticlesController(mockUoW.Object)
                .Validating(article);

            var result = controller.PostArticle(article).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostArticleTest_Moq_CreatedAt()
        {
            Article article = Generate(1, false);

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Articles.Add(article));

            var controller = new ArticlesController(mockUoW.Object)
                .Validating(article);

            var result = controller.PostArticle(article).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Article>(result, article);
        }

        [TestMethod]
        public void PutArticleTest_Moq_InvalidModelState_BadRequest()
        {
            Article article = Generate(1, true);
            Article newArticle = new() { Id = 1 };

            var result = PutTest(article.Id, article, newArticle);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutArticleTest_Moq_InvalidId_BadRequest()
        {
            Article article = Generate(1, true);
            Article newArticle = Generate(2, true);

            var result = PutTest(article.Id, article, newArticle);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutArticleTest_Moq_UnknownId_NotFound()
        {
            Article newArticle = Generate(1, true);

            var result = PutTest(newArticle.Id, null, newArticle);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutArticleTest_Moq_NoContent()
        {
            Article article = Generate(1, true);
            Article newArticle = Generate(1, true);

            var result = PutTest(article.Id, article, newArticle);

            result.Should().BeOfType<NoContentResult>();
            article.Should().Be(newArticle);
        }
    }
}