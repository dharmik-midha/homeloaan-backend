using AutoMapper;
using BusinessLogic.DTO;
using DataAccess.DBContext;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Logging.NLog;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BusinessLogic.AppServices
{
    public class AuthAppServices : IAuthAppServices
    {
        #region Private Variables
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IStaticAppService _staticAppService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private const string loggerPrefix = nameof(AuthAppServices);
        #endregion

        #region ctor
        public AuthAppServices(
            IMapper mapper,
            ILogger logger,
            IStaticAppService staticAppService,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IConfiguration configuration
            )
        {
            _mapper = mapper;
            _logger = logger;
            _staticAppService = staticAppService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }
        #endregion

        #region Change Password
        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="currPass"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        public async Task<ResultDTO> ChangePasswordAsync(string email, string currPass, string newPass)
        {
            const string localLoggerPrefix = nameof(ChangePasswordAsync);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|email : {email}|started.");
            IdentityUser user = userManager.FindByNameAsync(email).Result;
            var res = await userManager.ChangePasswordAsync(user, currPass, newPass);
            if (!res.Succeeded)
            {
                _logger.LogInfo($"{loggerPrefix} | {localLoggerPrefix} : Password Change Failed. : {email} : {res.Errors}");
                return new ResultDTO(res.Succeeded, 400, "Password Change Failed");
            }
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|email : {email}|completed.");
            return new ResultDTO(res.Succeeded,200 , "Password Changed Successfully");
        }
        #endregion

        #region CreateUserAsync
        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<ResultDTO<IdentityResult>> CreateUserAsync(UserDTO userModel, string password)
        {
            const string localLoggerPrefix = nameof(CreateUserAsync);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|email : {userModel.Email}|started.");
            User user = _mapper.Map<UserDTO, User>(userModel);
            //validate city state country
            var validCityStateCountry = _staticAppService.IsValidCityStateCountry(userModel.CityCode, userModel.StateCode, userModel.CountryCode);
            if (!validCityStateCountry.Success)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|email : {userModel.Email}|validation of city, state, country failed.");
                return new ResultDTO<IdentityResult>(false, validCityStateCountry.StatusCode, validCityStateCountry.Message, null);
            }
            user.UserName = userModel.Email;
            user.Id = Guid.NewGuid().ToString();
            user.CountryCode = user.CountryCode.ToUpper();
            user.StateCode = user.StateCode.ToUpper();
            user.CityCode = user.CityCode.ToUpper();


            IdentityResult identity = await userManager.CreateAsync(user, password);
            if (!identity.Succeeded)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|email : {userModel.Email}|failed|error code : 400.");
                return new ResultDTO<IdentityResult>(false, 400, identity.ToString(), identity);
            }

            //Get the currently created user and add role to it
            IdentityUser usr = userManager.FindByNameAsync(userModel.Email).Result;

            _ = await userManager.AddToRoleAsync(usr, "User");
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|email : {userModel.Email}|completed.");
            return new ResultDTO<IdentityResult>(identity.Succeeded, 201, "user created", identity);

        }

        #endregion

        #region PasswordSignInAsync
        /// <summary>
        /// SignIn Password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<ResultDTO<string>> PasswordSignInAsync(string email, string password)
        {
            const string localLoggerPrefix = nameof(PasswordSignInAsync);
            var user = await userManager.FindByNameAsync(email);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|email : {email}|started ");
            if (user != null && await userManager.CheckPasswordAsync(user, password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(5),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                    );
                var response = new JwtSecurityTokenHandler().WriteToken(token);
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Signin successful ");
                return new ResultDTO<string>(true, 200, "Signed In Successfully", response);
            }
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|UnAuthorized ");
            return new ResultDTO<string>(false, 401, "UnAuthorized", null);
        }
        #endregion

        #region SignOutAsync
        /// <summary>
        /// Sign Out
        /// </summary>
        /// <returns></returns>
        public async Task SignOutAsync()
        {
            const string localLoggerPrefix = nameof(SignOutAsync);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Logout Successful. ");
            await signInManager.SignOutAsync();
        }
        #endregion
    }
}
