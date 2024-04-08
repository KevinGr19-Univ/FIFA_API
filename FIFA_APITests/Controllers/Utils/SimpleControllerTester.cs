using FIFA_API.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Protocol.Plugins;

namespace FIFA_APITests.Controllers.Utils
{
    public class SimpleControllerTester<TController, TEntity, TKey>
        where TController : ControllerBase
        where TEntity : class
    {
        public delegate ActionResult<IEnumerable<TEntity>> GetAll(TController controller);
        public delegate ActionResult<TEntity> Get(TController controller, TKey key);
        public delegate ActionResult<TEntity> Post(TController controller, TEntity entity);
        public delegate IActionResult Put(TController controller, TKey key, TEntity entity);
        public delegate IActionResult Delete(TController controller, TKey key);
        public delegate TKey KeyGetter(TEntity entity);

        public GetAll GetAllMethod { get; set; }
        public Get GetMethod { get; set; }
        public Post PostMethod { get; set; }
        public Put PutMethod { get; set; }
        public Delete DeleteMethod { get; set; }

        public KeyGetter KeyGetterMethod { get; set; }
        public Func<TController> ControllerFactory { get; set; }

        public void GetAllTest_Moq_RightItems(IEnumerable<TEntity> entities)
        {
            TController controller = ControllerFactory();

            var result = GetAllMethod(controller);

            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<TEntity>>()
                .And.BeEquivalentTo(entities);
        }

        public void GetTest_Moq_RightItem(TEntity entityToGet)
        {
            TController controller = ControllerFactory();
            TKey key = KeyGetterMethod(entityToGet);

            var result = GetMethod(controller, key);

            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<TEntity>()
                .And.Be(entityToGet);
        }

        public void GetTest_Moq_NotFound(TKey key)
        {
            TController controller = ControllerFactory();

            var result = GetMethod(controller, key);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        public void PutTest_Moq_InvalidModelState_BadRequest(TEntity entity)
        {
            TController controller = ControllerFactory();
            TKey key = KeyGetterMethod(entity);

            var result = PutMethod(controller, key, entity);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        public void PutTest_Moq_InvalidId_BadRequest(TKey key, TEntity entity)
        {
            key.Should().NotBe(KeyGetterMethod(entity));
            TController controller = ControllerFactory();

            var result = PutMethod(controller, key, entity);

            result.Should().BeOfType<BadRequestResult>();
        }

        public void PutTest_Moq_UnknownId_NotFound(TEntity entity)
        {
            TController controller = ControllerFactory();
            TKey key = KeyGetterMethod(entity);

            var result = PutMethod(controller, key, entity);
        }
    }
}
