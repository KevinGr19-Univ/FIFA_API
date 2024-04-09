using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIFA_APITests.Utils
{
    internal static class TestExtensions
    {
        internal static T Validating<T>(this T controller, object model) where T : ControllerBase
        {
            controller.ObjectValidator ??= new CustomObjectValidator();
            controller.TryValidateModel(model);
            return controller;
        }
    }
}
