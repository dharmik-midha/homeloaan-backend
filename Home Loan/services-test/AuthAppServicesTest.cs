using AutoMapper;
using BusinessLogic.AppServices;
using BusinessLogic.DTO;
using DataAccess.DBContext;
using DataAccess.Entities;
using Logging.NLog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class AuthAppServicesTest
    {
        private IStaticAppService _staticAppService;
        private IMapper _mapper;
        private ILogger _logger;
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;
        private AuthAppServices _authAppServices;

        [SetUp]
        public void SetUp()
        {
            _staticAppService = Substitute.For<IStaticAppService>();
            _mapper = Substitute.For<IMapper>();
            _logger = Substitute.For<ILogger>();
            _signInManager = Substitute.For<SignInManager<IdentityUser>>(
                Substitute.For<UserManager<IdentityUser>>(
                    Substitute.For<IUserStore<IdentityUser>>(),
                    Substitute.For<IOptions<IdentityOptions>>(),
                    Substitute.For<IPasswordHasher<IdentityUser>>(),
                    new List<IUserValidator<IdentityUser>>(),
                    new List<IPasswordValidator<IdentityUser>>(),
                    Substitute.For<ILookupNormalizer>(),
                    Substitute.For<IdentityErrorDescriber>(),
                    Substitute.For<IServiceProvider>(),
                    Substitute.For<ILogger>()
                ),
                Substitute.For<IHttpContextAccessor>(),
                Substitute.For<IUserClaimsPrincipalFactory<IdentityUser>>(),
                Substitute.For<IOptions<IdentityOptions>>(),
                Substitute.For<Microsoft.Extensions.Logging.ILogger<SignInManager<IdentityUser>>>()
            );
            _userManager = Substitute.For<UserManager<IdentityUser>>(
                Substitute.For<IUserStore<IdentityUser>>(),
                Substitute.For<IOptions<IdentityOptions>>(),
                Substitute.For<IPasswordHasher<IdentityUser>>(),
                new List<IUserValidator<IdentityUser>>(),
                new List<IPasswordValidator<IdentityUser>>(),
                Substitute.For<ILookupNormalizer>(),
                Substitute.For<IdentityErrorDescriber>(),
                Substitute.For<IServiceProvider>(),
                Substitute.For<ILogger>()
            );
            _roleManager = Substitute.For<RoleManager<IdentityRole>>(
                Substitute.For<IRoleStore<IdentityRole>>(),
                Substitute.For<IEnumerable<IRoleValidator<IdentityRole>>>(),
                Substitute.For<ILookupNormalizer>(),
                Substitute.For<IdentityErrorDescriber>(),
                Substitute.For<ILogger>()
            );
            _configuration = Substitute.For<IConfiguration>();

            _authAppServices = new AuthAppServices(
                _mapper,
                _logger,
                _staticAppService,
                _signInManager,
                _userManager,
                _configuration
            );
        }

        //[Test]
        //public async Task ChangePasswordAsync_PasswordChanged_ReturnsSuccessResult()
        //{
        //    // Arrange
        //    var email = "test@example.com";
        //    var currPass = "oldPassword";
        //    var newPass = "newPassword";
        //    var user = new IdentityUser
        //    {
        //        UserName = email,
        //        Email = email
        //    };
        //    _userManager.FindByNameAsync(email).Returns(user);

        //    // Act
        //    var result = await _authAppServices.ChangePasswordAsync(email, currPass, newPass);

        //    // Assert
        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual(200, result.StatusCode);
        //    Assert.AreEqual("Password Changed Successfully", result.Message);
        //    _logger.Received(1).LogInfo($"{nameof(AuthAppServices)} | {nameof(AuthAppServices.ChangePasswordAsync)} : Password Changed. : {email}");
        //}

        //[Test]
        //public async Task CreateUserAsync_ValidUser_ReturnsSuccessResult()
        //{
        //    // Arrange
        //    var password = "abcdefgh";
        //    var userDto = new UserDTO
        //    {
        //        Name = "John Doe",
        //        Email = "johndoe@example.com",
        //        CityCode = "HW",
        //        CountryCode = "IN",
        //        StateCode = "UK"
        //    };
        //    var user = new User
        //    {
        //        Name = "John Doe",
        //        Email = "johndoe@example.com",
        //        CityCode = "HW",
        //        CountryCode = "IN",
        //        StateCode = "UK"
        //    };
        //    _staticAppService.IsValidCityStateCountry(user.CityCode, user.StateCode, user.CountryCode).Returns(new ResultDTO(true, StatusCodes.Status200OK, ""));
        //    _mapper.Map<UserDTO, User>(userDto).Returns(user);

        //    // Act
        //    var result = await _authAppServices.CreateUserAsync(userDto, password);

        //    // Assert
        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual(201, result.StatusCode);
        //    Assert.AreEqual("user created", result.Message);
        //}
        //[Test]
        //public async Task PasswordSignInAsync_ReturnsSuccessResult_WhenSignInIsSuccessful()
        //{
        //    // Arrange
        //    var user = new User { UserName = "testuser@example.com", Email = "testuser@example.com" };
        //    _userManager.FindByNameAsync(user.UserName).Returns(user);
        //    _userManager.CheckPasswordAsync(user, "password").Returns(true);
        //    _userManager.GetRolesAsync(user).Returns(new List<string> { "User" });

        //    var expectedToken = "test_token";
        //    var jwtTokenHandler = new JwtSecurityTokenHandler();
        //    var token = jwtTokenHandler.CreateJwtSecurityToken();
        //    jwtTokenHandler.WriteToken(token).Returns(expectedToken);

        //    // Act
        //    var result = await _authAppServices.PasswordSignInAsync(user.UserName, "password");

        //    // Assert
        //    Assert.That(result.Success, Is.True);
        //    Assert.That(result.StatusCode, Is.EqualTo(200));
        //    Assert.That(result.Message, Is.EqualTo("Signed In Successfully"));
        //    Assert.That(result.Data, Is.EqualTo(expectedToken));
        //}
    }
}
