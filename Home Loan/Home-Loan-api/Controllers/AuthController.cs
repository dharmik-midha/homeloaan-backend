using AutoMapper;
using BusinessLogic.AppServices;
using BusinessLogic.DTO;
using DataAccess.Entities;
using Home_Loan_api.Models;
using Logging.NLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace Home_Loan_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AuthController : ControllerBase
    {
        #region Private Variables
        private readonly IAuthAppServices _authAppServices;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private const string loggerPrefix = nameof(AuthController);
        #endregion

        #region ctor
        public AuthController(IAuthAppServices authAppServices, ILogger logger, IMapper mapper)
        {
            _authAppServices = authAppServices;
            _logger = logger;
            _mapper = mapper;
        }
        #endregion

        #region Register
        /// <summary>
        /// Register a New User
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Auth
        ///     {
        ///         "email": "username@email.com",
        ///         "name": "firstName LastName",
        ///         "password": "Abcd@1234",
        ///         "phone": "9876543210",
        ///         "cityCode": "NDL",
        ///         "stateCode": "DEL",
        ///         "countryCode": "IND"
        ///     }
        ///
        /// </remarks>
        /// <param name="registerModal">A json objects that contains all feilds to create a user</param>
        /// <returns>Action Result</returns>
        /// <response code="201">User Created Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Route("signup")]
        [AllowAnonymous]
        public IActionResult Register(RegisterModal registerModal)
        {
            const string localLoggerPrefix = nameof(Register);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {registerModal.Email}|started.");

            UserDTO user = _mapper.Map<RegisterModal, UserDTO>(registerModal);
            ResultDTO<IdentityResult> result = _authAppServices.CreateUserAsync(user, registerModal.Password).Result;
           
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {registerModal.Email}|completed : {result.StatusCode} : {result.Message}");
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region SignIn
        /// <summary>
        /// User SignIn
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Post /Auth
        ///     {
        ///       email = "username@email.com",
        ///       password= "Abcd@1234"
        ///     }
        ///
        /// </remarks>
        /// <param name="signInModal"></param>
        /// <returns>Action Result</returns>
        /// <response code="201">SignIn Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        public IActionResult SignIn(SignInModal signInModal)
        {
            const string localLoggerPrefix = nameof(SignIn);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {signInModal.Email}|started.");
            
            var result = _authAppServices.PasswordSignInAsync(signInModal.Email, signInModal.Password).Result;
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {signInModal.Email}|completed : {result.StatusCode} : {result.Message}.");
           
            if (result.Success)
            {
                return Accepted(new { token = result.Data });
            }
            else
            {
                return Unauthorized();
            }
        }
        #endregion

        #region Change Password 
        /// <summary>
        /// Password Update
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Auth
        ///     {
        ///         "Current Password": "Abcd@1234",
        ///         "New Password": "Wxyz@5678",
        ///     }
        ///
        /// </remarks>
        /// 
        /// <param name="passwordReset"></param>
        /// <returns>Action Result</returns>
        /// <response code="200">Password Updated Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpPut]
        public IActionResult ChangePassword([FromBody] PasswordResetModal passwordReset)
        {
            const string localLoggerPrefix = nameof(ChangePassword);
            var userEmail = User.Claims.Where(a => a.Type == ClaimTypes.Email).FirstOrDefault().Value;
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|started.");
            
            var result = _authAppServices.ChangePasswordAsync(userEmail, passwordReset.CurrentPassword, passwordReset.NewPassword).Result;
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|completed : {result.StatusCode} : {result.Message}");
            
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

    }
}
