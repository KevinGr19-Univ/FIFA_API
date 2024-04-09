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
    public class BlogsControllerTests
    {
        private Blog Generate(int id, bool visible)
        {
            Random r = new();
            return new Blog()
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

        private void GetTest(bool blogVisible, bool onlyVisible)
        {
            Blog produit = Generate(1, blogVisible);
            bool see = blogVisible || !onlyVisible;

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Blogs.GetByIdWithAll(produit.Id, false)).ReturnsAsync(produit);
            mockUoW.Setup(m => m.Blogs.GetByIdWithAll(produit.Id, true)).ReturnsAsync(see ? produit : null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(PublicationsController.MANAGER_POLICY, !onlyVisible);
            var controller = new BlogsController(mockUoW.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetBlog(produit.Id).Result;

            if (see) TestUtils.ActionResultShouldGive(result, produit);
            else result.Result.Should().BeOfType<NotFoundResult>();
        }

        private IActionResult PutTest(int id, Blog? blog, Blog newBlog)
        {
            var mockUoW = new Mock<IUnitOfWorkPublication>();
            if (blog is not null)
            {
                mockUoW.Setup(m => m.Blogs.Exists(blog.Id)).ReturnsAsync(true);
                mockUoW.Setup(m => m.Blogs.GetById(blog.Id, It.IsAny<bool>())).ReturnsAsync(blog);
                mockUoW.Setup(m => m.Blogs.Update(newBlog)).Returns(() =>
                    Task.Run(() =>
                    {
                        blog.Id = newBlog.Id;
                        blog.IdPhoto = newBlog.IdPhoto;
                        blog.Texte = newBlog.Texte;
                        blog.DatePublication = newBlog.DatePublication;
                        blog.Titre = newBlog.Titre;
                        blog.Resume = newBlog.Resume;
                        blog.Visible = newBlog.Visible;
                        blog.Joueurs = newBlog.Joueurs;
                        blog.Commentaires = newBlog.Commentaires;
                        blog.Photos = newBlog.Photos;
                    })
                );
            }
            else
            {
                mockUoW.Setup(m => m.Blogs.Exists(id)).ReturnsAsync(false);
                mockUoW.Setup(m => m.Blogs.GetById(id, It.IsAny<bool>())).ReturnsAsync(() => null);
            }

            var controller = new BlogsController(mockUoW.Object)
                .Validating(newBlog);

            return controller.PutBlog(id, newBlog).Result;
        }

        [TestMethod]
        public void BlogsControllerTest_Moq_NoException()
        {
            var mockUoW = new Mock<IUnitOfWorkPublication>();
            var controller = new BlogsController(mockUoW.Object);
        }

        [TestMethod]
        public void GetBlogTest_Moq_Visible_Admin_RightItem()
        {
            GetTest(blogVisible: true, onlyVisible: false);
        }

        [TestMethod]
        public void GetBlogTest_Moq_NotVisible_Admin_RightItem()
        {
            GetTest(blogVisible: false, onlyVisible: false);
        }

        [TestMethod]
        public void GetBlogTest_Moq_Visible_User_RightItem()
        {
            GetTest(blogVisible: true, onlyVisible: true);
        }

        [TestMethod]
        public void GetBlogTest_Moq_NotVisible_User_NotFound()
        {
            GetTest(blogVisible: false, onlyVisible: true);
        }

        [TestMethod]
        public void PostBlogTest_Moq_InvalidModelState_BadRequest()
        {
            Blog blog = new() { Id = 1 };

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Blogs.Add(blog));

            var controller = new BlogsController(mockUoW.Object)
                .Validating(blog);

            var result = controller.PostBlog(blog).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostBlogTest_Moq_CreatedAt()
        {
            Blog blog = Generate(1, false);

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Blogs.Add(blog));

            var controller = new BlogsController(mockUoW.Object)
                .Validating(blog);

            var result = controller.PostBlog(blog).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Blog>(result, blog);
        }

        [TestMethod]
        public void PutBlogTest_Moq_InvalidModelState_BadRequest()
        {
            Blog blog = Generate(1, true);
            Blog newBlog = new() { Id = 1 };

            var result = PutTest(blog.Id, blog, newBlog);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutBlogTest_Moq_InvalidId_BadRequest()
        {
            Blog blog = Generate(1, true);
            Blog newBlog = Generate(2, true);

            var result = PutTest(blog.Id, blog, newBlog);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutBlogTest_Moq_UnknownId_NotFound()
        {
            Blog newBlog = Generate(1, true);

            var result = PutTest(newBlog.Id, null, newBlog);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutBlogTest_Moq_NoContent()
        {
            Blog blog = Generate(1, true);
            Blog newBlog = Generate(1, true);

            var result = PutTest(blog.Id, blog, newBlog);

            result.Should().BeOfType<NoContentResult>();
            blog.Should().Be(newBlog);
        }
    }
}