using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace FIFA_APITests
{
    public static class TestUtils
    {
        public static T ToType<T>(object? obj, string? assertMessage = null, [CallerArgumentExpression(nameof(obj))] string? objName = null)
        {
            assertMessage ??= $"{objName} n'est pas du bon type.";
            Assert.IsNotNull(obj, assertMessage);
            Assert.IsInstanceOfType<T>(obj, assertMessage);
            return (T)obj;
        }

        public static T? ToTypeNullable<T>(object? obj, string? assertMessage = null, [CallerArgumentExpression(nameof(obj))] string? objName = null)
            where T : class
        {
            if (obj is null) return null;

            assertMessage ??= $"{objName} n'est ni null, ni du bon type.";
            Assert.IsInstanceOfType<T>(obj, assertMessage);
            return (T)obj;
        }
    }
}
