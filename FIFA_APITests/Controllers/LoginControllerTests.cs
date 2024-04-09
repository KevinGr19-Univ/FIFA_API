using Microsoft.VisualStudio.TestTools.UnitTesting;
using FIFA_API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIFA_API.Models.EntityFramework;
using Moq;
using FIFA_API.Repositories.Contracts;
using FIFA_API.Contracts;
using FluentAssertions;
using FIFA_APITests.Utils;
using FIFA_API.Models.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace FIFA_API.Controllers.Tests
{
    [TestClass]
    public class LoginControllerTests
    {
        private Mock<IManagerUtilisateur> MockManagerUtilisateur(Utilisateur user)
        {
            var mockRepo = new Mock<IManagerUtilisateur>();
            mockRepo.Setup(m => m.GetById(user.Id)).ReturnsAsync(user);
            mockRepo.Setup(m => m.GetByEmail(user.Mail)).ReturnsAsync(user);
            return mockRepo;
        }

        private (Mock<ITokenService>, APITokenInfo) MockTokenService(Utilisateur user)
        {
            Random r = new();
            var mockToken = new Mock<ITokenService>();
            var tokens = new APITokenInfo()
            {
                AccessToken = r.Next(1_000_000).ToString(),
                RefreshToken = r.Next(1_000_000).ToString(),
            };
            mockToken.Setup(m => m.GenerateAccessToken(user)).Returns(tokens.AccessToken);
            mockToken.Setup(m => m.GenerateRefreshToken()).Returns(tokens.RefreshToken);
            return (mockToken, tokens);
        }

        private (LoginController, APITokenInfo) SetupLoginTest(Utilisateur user, string password)
        {
            var mockRepo = MockManagerUtilisateur(user);
            var mock2FA = new Mock<ILogin2FAService>();
            var mockHasher = new Mock<IPasswordHasher>();
            (var mockToken, var tokens) = MockTokenService(user);

            mock2FA.Setup(m => m.Send2FACodeAsync(user)).ReturnsAsync($"2fa_token_test_{user.Id}");
            mockHasher.Setup(m => m.Verify(user.HashMotDePasse, password)).Returns(true);

            var controller = new LoginController(mockRepo.Object, mockHasher.Object, mockToken.Object, mock2FA.Object);

            return (controller, tokens);
        }

        private (LoginController, APITokenInfo) SetupLogin2FA(Utilisateur user, Login2FAInfo loginInfo)
        {
            var mockRepo = MockManagerUtilisateur(user);
            var mock2FA = new Mock<ILogin2FAService>();
            var mockHasher = new Mock<IPasswordHasher>();
            (var mockToken, var tokens) = MockTokenService(user);

            mock2FA.Setup(m => m.AuthenticateAsync(loginInfo.Token, loginInfo.Code)).ReturnsAsync(user);

            var controller = new LoginController(mockRepo.Object, mockHasher.Object, mockToken.Object, mock2FA.Object);

            return (controller, tokens);
        }

        private (LoginController, APITokenInfo) SetupRefresh(Utilisateur user, APITokenInfo credentials)
        {
            var mockRepo = MockManagerUtilisateur(user);
            var mock2FA = new Mock<ILogin2FAService>();
            var mockHasher = new Mock<IPasswordHasher>();
            (var mockToken, var tokens) = MockTokenService(user);

            mockToken.Setup(m => m.GetUserFromExpiredAsync(credentials.AccessToken)).ReturnsAsync(user);

            var controller = new LoginController(mockRepo.Object, mockHasher.Object, mockToken.Object, mock2FA.Object);
            return (controller, tokens);
        }

        private void LoginTest(Utilisateur user, string userPassword, string mail, string password, bool unauthorized)
        {
            (var controller, var tokens) = SetupLoginTest(user, userPassword);

            var result = controller.Login(new()
            {
                Mail = mail,
                Password = password
            }).Result;

            if (unauthorized) result.Should().BeOfType<UnauthorizedResult>();
            else
            {
                TestUtils.ActionResultShouldGive<OkObjectResult, APITokenInfo>(result, tokens);
                user.RefreshToken.Should().Be(tokens.RefreshToken);
                user.DerniereConnexion.Should().NotBeNull();
            }
        }

        private void Login2FATest(Utilisateur user, Login2FAInfo loginInfoSetup, Login2FAInfo loginInfo, bool unauthorized)
        {
            (var controller, var tokens) = SetupLogin2FA(user, loginInfoSetup);

            var result = controller.Login2FA(new()
            {
                Token = loginInfo.Token,
                Code = loginInfo.Code
            }).Result;

            if (unauthorized) result.Result.Should().BeOfType<UnauthorizedResult>();
            else
            {
                TestUtils.ActionResultShouldGive(result, tokens);
                user.DateVerif2FA.Should().NotBeNull();
                user.RefreshToken.Should().Be(tokens.RefreshToken);
                user.DerniereConnexion.Should().NotBeNull();
            }
        }

        [TestMethod]
        public void LoginTest_Moq_AuthenticationSuccess_No2FA_ReturnsTokens()
        {
            Utilisateur user = new() { Id = 1, Mail = "test@gmail.com", HashMotDePasse = "hashDeMotDePasse" };
            LoginTest(user, "password", user.Mail, "password", false);
        }

        [TestMethod]
        public void LoginTest_Moq_AuthenticationSuccess_With2FA_Returns2FAToken()
        {
            Utilisateur user = new() { Id = 1, Mail = "test@gmail.com", HashMotDePasse = "hashDeMotDePasse",
                Telephone = "0123456789", DoubleAuthentification = true, DateVerif2FA = DateTime.Today };

            (var controller, var _) = SetupLoginTest(user, "password");

            var result = controller.Login(new()
            {
                Mail = user.Mail,
                Password = "password"
            }).Result;

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().NotBeNull().And.GetType()
                .GetMember("token").Should().NotBeNull();
        }

        [TestMethod]
        public void LoginTest_Moq_AuthenticationFailure_WrongPassword_Unauthorized()
        {
            Utilisateur user = new() { Id = 1, Mail = "test@gmail.com", HashMotDePasse = "hashDeMotDePasse" };
            LoginTest(user, "password", user.Mail, "password2", true);

        }

        [TestMethod]
        public void LoginTest_Moq_AuthenticationFailure_WrongMail_Unauthorized()
        {
            Utilisateur user = new() { Id = 1, Mail = "test@gmail.com", HashMotDePasse = "hashDeMotDePasse" };
            LoginTest(user, "password", "test2@gmail.com", "password", true);
        }

        [TestMethod]
        public void LoginTest_Moq_AuthenticationSuccess_Anonymized_Unauthorized()
        {
            Utilisateur user = new() { Id = 1, Mail = "test@gmail.com", HashMotDePasse = "hashDeMotDePasse", Anonyme = true };
            LoginTest(user, "password", user.Mail, "password", true);

        }

        [TestMethod]
        public void Login2FATest_Moq_AuthenticationSuccess_ReturnsTokens()
        {
            Utilisateur user = new() { Id = 1, Mail = "test@gmail.com", DoubleAuthentification = true, DateVerif2FA = null };
            Login2FAInfo loginInfo = new() { Token = "token", Code = "code" };
            Login2FATest(user, loginInfo, loginInfo, false);
        }

        [TestMethod]
        public void Login2FATest_Moq_AuthenticationSuccess_Anonymized_Unauthorized()
        {
            Utilisateur user = new() { Id = 1, Mail = "test@gmail.com", DoubleAuthentification = true, Anonyme = true };
            Login2FAInfo loginInfo = new() { Token = "token", Code = "code" };
            Login2FATest(user, loginInfo, loginInfo, true);
        }

        [TestMethod]
        public void Login2FATest_Moq_AuthenticationSuccess_2FADisabled_Unauthorized()
        {
            Utilisateur user = new() { Id = 1, Mail = "test@gmail.com", DoubleAuthentification = false };
            Login2FAInfo loginInfo = new() { Token = "token", Code = "code" };
            Login2FATest(user, loginInfo, loginInfo, true);
        }

        [TestMethod]
        public void Login2FATest_Moq_AuthenticationFailure_Unauthorized()
        {
            Utilisateur user = new() { Id = 1, Mail = "test@gmail.com", DoubleAuthentification = true };
            Login2FAInfo loginInfoSetup = new() { Token = "token", Code = "code" };
            Login2FAInfo loginInfo = new() { Token = "token2", Code = "code" };
            Login2FATest(user, loginInfoSetup, loginInfo, true);
        }

        [TestMethod]
        public void RefreshTest_Moq_ValidCredentials_ReturnsTokens()
        {
            Utilisateur user = new() { Id = 1 };
            APITokenInfo setup = new() { AccessToken = "old_token", RefreshToken = "old_refresh" };
            user.RefreshToken = setup.RefreshToken;

            (var controller, var tokens) = SetupRefresh(user, setup);
            APITokenInfo credentials = new() { AccessToken = "old_token", RefreshToken = "old_refresh" };

            var result = controller.Refresh(credentials).Result;

            TestUtils.ActionResultShouldGive(result, tokens);
            user.RefreshToken.Should().Be(tokens.RefreshToken);
        }

        [TestMethod]
        public void RefreshTest_Moq_ValidCredentials_Anonymized_NotFound()
        {
            Utilisateur user = new() { Id = 1, Anonyme = true };
            APITokenInfo setup = new() { AccessToken = "old_token", RefreshToken = "old_refresh" };
            user.RefreshToken = setup.RefreshToken;

            (var controller, var _) = SetupRefresh(user, setup);
            APITokenInfo credentials = new() { AccessToken = "old_token", RefreshToken = "old_refresh" };

            var result = controller.Refresh(credentials).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void RefreshTest_Moq_InvalidAccess_NotFound()
        {
            Utilisateur user = new() { Id = 1 };
            APITokenInfo setup = new() { AccessToken = "old_token", RefreshToken = "old_refresh" };
            user.RefreshToken = setup.RefreshToken;

            (var controller, var _) = SetupRefresh(user, setup);
            APITokenInfo credentials = new() { AccessToken = "old_token2", RefreshToken = "old_refresh" };

            var result = controller.Refresh(credentials).Result;

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void RefreshTest_Moq_InvalidRefresh_Forbid()
        {
            Utilisateur user = new() { Id = 1 };
            APITokenInfo setup = new() { AccessToken = "old_token", RefreshToken = "old_refresh" };
            user.RefreshToken = setup.RefreshToken;

            (var controller, var _) = SetupRefresh(user, setup);
            APITokenInfo credentials = new() { AccessToken = "old_token", RefreshToken = "old_refresh2" };

            var result = controller.Refresh(credentials).Result;

            result.Result.Should().BeOfType<ForbidResult>();
        }

        [TestMethod]
        public void SendEmailVerificationTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void VerifyEmailTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void SendResetPasswordTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void ResetPasswordTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void SendVerify2FATest()
        {
            Assert.Fail();
        }
    }
}