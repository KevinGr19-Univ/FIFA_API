using Microsoft.VisualStudio.TestTools.UnitTesting;
using FIFA_API.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIFA_API.Models.EntityFramework;
using Moq;
using FIFA_API.Repositories.Contracts;
using FIFA_APITests.Utils;
using FluentAssertions;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public class StocksControllerTests
    {
        private ActionResult<StockProduit> PostTest(StockProduit stock, bool exists = false)
        {
            var mockRepo = new Mock<IManagerStockProduit>();
            mockRepo.Setup(m => m.Exists(stock.IdVCProduit, stock.IdTaille)).ReturnsAsync(exists);
            mockRepo.Setup(m => m.Add(stock));

            var controller = new StocksController(mockRepo.Object)
                .Validating(stock);

            var result = controller.PostStockProduit(stock).Result;
            return result;
        }

        private IActionResult PutTest(int idvariante, int idtaille, StockProduit? stock, StockProduit newStock)
        {
            var mockRepo = new Mock<IManagerStockProduit>();
            if(stock is not null)
            {
                mockRepo.Setup(m => m.Exists(stock.IdVCProduit, stock.IdTaille)).ReturnsAsync(true);
                mockRepo.Setup(m => m.GetById(stock.IdVCProduit, stock.IdTaille)).ReturnsAsync(stock);
                mockRepo.Setup(m => m.Update(newStock)).Returns(() =>
                    Task.Run(() =>
                    {
                        stock.IdVCProduit = newStock.IdVCProduit;
                        stock.IdTaille = newStock.IdTaille;
                        stock.Stocks = newStock.Stocks;
                        stock.VCProduit = newStock.VCProduit;
                        stock.Taille = newStock.Taille;
                    })
                );
            }

            var controller = new StocksController(mockRepo.Object)
                .Validating(newStock);

            var result = controller.PutStockProduit(idvariante, idtaille, newStock).Result;
            return result;
        }

        [TestMethod]
        public void StocksControllerTest_Moq_NoException()
        {
            var mockRepo = new Mock<IManagerStockProduit>();
            var controller = new StocksController(mockRepo.Object);
        }

        [TestMethod]
        public void GetStockProduitsTest_RightItems()
        {
            List<StockProduit> stocks = new()
            {
                new() { IdVCProduit = 1, IdTaille = 1, Stocks = 2 },
                new() { IdVCProduit = 1, IdTaille = 2, Stocks = 4 },
                new() { IdVCProduit = 2, IdTaille = 1, Stocks = 3 },
            };

            var mockRepo = new Mock<IManagerStockProduit>();
            mockRepo.Setup(m => m.GetAll()).ReturnsAsync(stocks);
            var controller = new StocksController(mockRepo.Object);

            var result = controller.GetStockProduits().Result;

            TestUtils.ActionResultShouldGive(result, stocks);
        }

        [TestMethod]
        public void GetStockProduitTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerStockProduit>();
            var controller = new StocksController(mockRepo.Object);

            var result = controller.GetStockProduit(1, 3).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void GetStockProduitTest_Moq_RightItem()
        {
            StockProduit stock = new()
            {
                IdVCProduit = 1,
                IdTaille = 3,
                Stocks = 3
            };

            var mockRepo = new Mock<IManagerStockProduit>();
            mockRepo.Setup(m => m.GetById(stock.IdVCProduit, stock.IdTaille)).ReturnsAsync(stock);
            var controller = new StocksController(mockRepo.Object);

            var result = controller.GetStockProduit(stock.IdVCProduit, stock.IdTaille).Result;

            TestUtils.ActionResultShouldGive(result, stock);
        }

        [TestMethod]
        public void PutStockProduitTest_Moq_InvalidModelState_BadRequest()
        {
            StockProduit stock = new() { IdVCProduit = 1, IdTaille = 3, Stocks = 1 };
            StockProduit newStock = new() { IdVCProduit = 1, IdTaille = 3, Stocks = -1 };

            var result = PutTest(stock.IdVCProduit, stock.IdTaille, stock, newStock);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PutStockProduitTest_Moq_InvalidId_BadRequest()
        {
            StockProduit stock = new() { IdVCProduit = 1, IdTaille = 3, Stocks = 1 };
            StockProduit newStock = new() { IdVCProduit = 1, IdTaille = 2, Stocks = 2 };

            var result = PutTest(stock.IdVCProduit, stock.IdTaille, stock, newStock);

            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void PutStockProduitTest_Moq_UnknownId_NotFound()
        {
            StockProduit newStock = new() { IdVCProduit = 1, IdTaille = 3, Stocks = 2 };

            var result = PutTest(newStock.IdVCProduit, newStock.IdTaille, null, newStock);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void PutStockProduitTest_Moq_NoContent()
        {
            StockProduit stock = new() { IdVCProduit = 1, IdTaille = 3, Stocks = 1 };
            StockProduit newStock = new() { IdVCProduit = 1, IdTaille = 3, Stocks = 2 };

            var result = PutTest(stock.IdVCProduit, stock.IdTaille, stock, newStock);

            result.Should().BeOfType<NoContentResult>();
            stock.Should().Be(newStock);
        }

        [TestMethod]
        public void PostStockProduitTest_Moq_InvalidModelState_BadRequest()
        {
            StockProduit stock = new() { IdVCProduit = 1, IdTaille = 3, Stocks = -1 };

            var result = PostTest(stock);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void PostStockProduitTest_Moq_ExistingId_Conflict()
        {
            StockProduit stock = new() { IdVCProduit = 1, IdTaille = 3, Stocks = 1 };

            var result = PostTest(stock, exists: true);

            result.Result.Should().BeOfType<ConflictResult>();
        }

        [TestMethod]
        public void PostStockProduitTest_Moq_CreatedAt()
        {
            StockProduit stock = new() { IdVCProduit = 1, IdTaille = 3, Stocks = 1 };

            var result = PostTest(stock);

            TestUtils.ActionResultShouldGive<CreatedAtActionResult, StockProduit>(result, stock);
        }

        [TestMethod]
        public void DeleteStockProduitTest_Moq_UnknownId_NotFound()
        {
            var mockRepo = new Mock<IManagerStockProduit>();
            var controller = new StocksController(mockRepo.Object);

            var result = controller.DeleteStockProduit(1, 3).Result;

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void DeleteStockProduitTest_Moq_NoContent()
        {
            StockProduit stock = new() { IdVCProduit = 1, IdTaille = 3, Stocks = 1 };

            var mockRepo = new Mock<IManagerStockProduit>();
            mockRepo.Setup(m => m.GetById(stock.IdVCProduit, stock.IdTaille)).ReturnsAsync(stock);
            var controller = new StocksController(mockRepo.Object);

            var result = controller.DeleteStockProduit(stock.IdVCProduit, stock.IdTaille).Result;

            result.Should().BeOfType<NoContentResult>();
        }
    }
}