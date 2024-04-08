using FIFA_API.Contracts;
using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FIFA_APITests.Controllers.Utils
{
    public class MockHttpContext
    {
        private Mock<HttpContext> _mock;
        private ClaimsPrincipal _user;

        private Mock<ICustomAuthorizationService>? _authService;
        private Mock<ITokenService>? _tokenService;

        public MockHttpContext()
        {
            _mock = new Mock<HttpContext>();
            _user = new ClaimsPrincipal();

            _mock.Setup(c => c.User).Returns(_user);
        }

        public MockHttpContext MockAuthentication(Utilisateur? user)
        {
            if (_tokenService is null)
            {
                _tokenService = new Mock<ITokenService>();
                _mock.Setup(c => c.RequestServices.GetService(typeof(ITokenService))).Returns(_tokenService.Object);
            }

            _tokenService.Setup(t => t.GetUserFromPrincipalAsync(_user)).ReturnsAsync(user);
            return this;
        }

        public MockHttpContext MockMatchingPolicy(string policy, bool result)
        {
            if (_authService is null)
            {
                _authService = new Mock<ICustomAuthorizationService>();
                _mock.Setup(c => c.RequestServices.GetService(typeof(ICustomAuthorizationService))).Returns(_authService.Object);
            }

            _authService.Setup(s => s.MatchPolicyAsync(_user, policy)).ReturnsAsync(result);
            return this;
        }

        public ControllerContext ToControllerContext()
        {
            return new ControllerContext() { HttpContext = _mock.Object };
        }
    }
}
