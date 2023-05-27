using BusinessLogic.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.AppServices
{
    public interface IAuthAppServices
    {
        Task<ResultDTO<IdentityResult>> CreateUserAsync(UserDTO userModel,string password);
        Task<ResultDTO<string>> PasswordSignInAsync(string email,string password);
        Task<ResultDTO> ChangePasswordAsync(string email, string currPass, string newPass);
        Task SignOutAsync();
    }
}
