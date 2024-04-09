using FIFA_API.Contracts;
using FIFA_API.Models.EntityFramework;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Claims;

namespace FIFA_APITests.Utils
{
    public static class TestUtils
    {
        public static FifaDbContext DbContextInMemory(string dbName, IConfiguration? config = null)
        {
            var options = new DbContextOptionsBuilder<FifaDbContext>()
                .UseInMemoryDatabase(dbName)
                .ReplaceService<IModelCustomizer, InMemoryModelCustomizer>();

            return new FifaDbContext(options.Options, config);
        }

        public static void ActionResultShouldGive<T>(ActionResult<T> result, T target)
            => ActionResultShouldGive<OkObjectResult, T>(result, target);

        public static void ActionResultShouldGive<TResult, T>(ActionResult<T> result, T target)
            where TResult : ObjectResult
        {
            result.Result.Should().BeOfType<TResult>()
                .Which.Value.Should().BeAssignableTo<T>()
                .And.BeEquivalentTo(target);
        }
    }
}
