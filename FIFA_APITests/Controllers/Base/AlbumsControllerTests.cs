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
    public class AlbumsControllerTests
    {
        private Album Generate(int id, bool visible)
        {
            Random r = new();
            return new Album()
            {
                Id = id,
                IdPhoto = null,
                DatePublication = DateTime.Today.AddDays(-r.Next(10)),
                Titre = $"Titre{id}",
                Resume = $"Resume{id}",
                Visible = visible
            };
        }

        private void GetTest(bool albumVisible, bool onlyVisible)
        {
            Album produit = Generate(1, albumVisible);
            bool see = albumVisible || !onlyVisible;

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Albums.GetByIdWithPhotos(produit.Id, false)).ReturnsAsync(produit);
            mockUoW.Setup(m => m.Albums.GetByIdWithPhotos(produit.Id, true)).ReturnsAsync(see ? produit : null);

            var mockHttpCtx = new MockHttpContext().MockMatchingPolicy(PublicationsController.MANAGER_POLICY, !onlyVisible);
            var controller = new AlbumsController(mockUoW.Object) { ControllerContext = mockHttpCtx.ToControllerContext() };

            var result = controller.GetAlbum(produit.Id).Result;

            if (see) TestUtils.ActionResultShouldGive(result, produit);
            else result.Result.Should().BeOfType<NotFoundResult>();
        }

        private IActionResult PutTest(int id, Album? album, Album newAlbum)
        {
            var mockUoW = new Mock<IUnitOfWorkPublication>();
            if (album is not null)
            {
                mockUoW.Setup(m => m.Albums.Exists(album.Id)).ReturnsAsync(true);
                mockUoW.Setup(m => m.Albums.GetById(album.Id, It.IsAny<bool>())).ReturnsAsync(album);
                mockUoW.Setup(m => m.Albums.Update(newAlbum)).Returns(() =>
                    Task.Run(() =>
                    {
                        album.Id = newAlbum.Id;
                        album.IdPhoto = newAlbum.IdPhoto;
                        album.DatePublication = newAlbum.DatePublication;
                        album.Titre = newAlbum.Titre;
                        album.Resume = newAlbum.Resume;
                        album.Visible = newAlbum.Visible;
                        album.Joueurs = newAlbum.Joueurs;
                        album.Photos = newAlbum.Photos;
                    })
                );
            }
            else
            {
                mockUoW.Setup(m => m.Albums.Exists(id)).ReturnsAsync(false);
                mockUoW.Setup(m => m.Albums.GetById(id, It.IsAny<bool>())).ReturnsAsync(() => null);
            }

            var controller = new AlbumsController(mockUoW.Object)
                .Validating(newAlbum);

            return controller.PutAlbum(id, newAlbum).Result;
        }

        [TestMethod]
        public void AlbumsControllerTest_Moq_NoException()
        {
            var mockUoW = new Mock<IUnitOfWorkPublication>();
            var controller = new AlbumsController(mockUoW.Object);
        }

        [TestMethod]
        public void GetAlbumTest_Moq_Visible_Admin_RightItem()
        {
            GetTest(albumVisible: true, onlyVisible: false);
        }

        [TestMethod]
        public void GetAlbumTest_Moq_NotVisible_Admin_RightItem()
        {
            GetTest(albumVisible: false, onlyVisible: false);
        }

        [TestMethod]
        public void GetAlbumTest_Moq_Visible_User_RightItem()
        {
            GetTest(albumVisible: true, onlyVisible: true);
        }

        [TestMethod]
        public void GetAlbumTest_Moq_NotVisible_User_NotFound()
        {
            GetTest(albumVisible: false, onlyVisible: true);
        }

        [TestMethod]
        public void PostAlbumTest_Moq_InvalidModelState_BadRequest()
        {
            Album album = new() { Id = 1 };

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Albums.Add(album));

            var controller = new AlbumsController(mockUoW.Object)
                .Validating(album);

            var result = controller.PostAlbum(album).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostAlbumTest_Moq_CreatedAt()
        {
            Album album = Generate(1, false);

            var mockUoW = new Mock<IUnitOfWorkPublication>();
            mockUoW.Setup(m => m.Albums.Add(album));

            var controller = new AlbumsController(mockUoW.Object)
                .Validating(album);

            var result = controller.PostAlbum(album).Result;

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, Album>(result, album);
        }

        [TestMethod]
        public void PutAlbumTest_Moq_InvalidModelState_BadRequest()
        {
            Album album = Generate(1, true);
            Album newAlbum = new() { Id = 1 };

            var result = PutTest(album.Id, album, newAlbum);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutAlbumTest_Moq_InvalidId_BadRequest()
        {
            Album album = Generate(1, true);
            Album newAlbum = Generate(2, true);

            var result = PutTest(album.Id, album, newAlbum);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutAlbumTest_Moq_UnknownId_NotFound()
        {
            Album newAlbum = Generate(1, true);

            var result = PutTest(newAlbum.Id, null, newAlbum);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutAlbumTest_Moq_NoContent()
        {
            Album album = Generate(1, true);
            Album newAlbum = Generate(1, true);

            var result = PutTest(album.Id, album, newAlbum);

            result.Should().BeOfType<NoContentResult>();
            album.Should().Be(newAlbum);
        }
    }
}