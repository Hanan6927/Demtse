﻿using AutoMapper;
using Demጽ.Entities;
using Demጽ.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;



namespace Demጽ.Repository.AuthenticationRepository
{
    public class AuthentiactionRepository : IAuthenticationRepository
    {
        private readonly UserManager<User> _userManger;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AuthentiactionRepository(UserManager<User> userManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            this._mapper = mapper;
            this._roleManager = roleManager;
            this._userManger = userManager;
            this._configuration = configuration;
        }
        public async Task<User> UpdateProfile(User user)
        {
            await _userManger.UpdateAsync(user);
            return user;
        }
        public async Task<User> Get(string userId)
        {
            //throw new NotImplementedException();
            var user = await _userManger.FindByIdAsync(userId);
            if(user == null)
            {
                return null;
            }
            return user;
        }
        public async Task<IEnumerable<string>> GetUserRoels(User user)
        {
            if (user == null)
            {
                return null;

            }
            var roles = await _userManger.GetRolesAsync(user);
            return roles.ToList();
        }
        public async Task<bool> Exist(string userName)
        {
            var user = await _userManger.FindByNameAsync(userName);
            if (user == null)
            {
                return  false;
            }
            return true;
        }

        //public async Task<User> Get(string userId)
        //{
        //    //throw new NotImplementedException();
        //    var user = await _userManger.FindByIdAsync(userId);
        //    if(user == null)
        //    {
        //        return null;
        //    }
        //    return user;
        //}

        //public async Task<IEnumerable<string>> GetUserRoels(User user)
        //{
        //    if (user == null)
        //    {
        //        return null;

        //    }
        //    var roles = await _userManger.GetRolesAsync(user);
        //    return roles.ToList();

        //}
        

        public async Task<LoginReturn> Login(LoginDto userCred)

        {
            var user = await _userManger.FindByNameAsync(userName: userCred.UserName);
            if (user != null && await _userManger.CheckPasswordAsync(user, userCred.Password))
            {
                var userRoles = await _userManger.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                   new Claim (ClaimTypes.Name,user.UserName),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(5),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                    );

                return new LoginReturn()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    User = _mapper.Map<UserDto>(user)

                };

            }
            return null;
        }

        public async Task<User> Register(User user , String Password)
        {
            Console.WriteLine(user.UserName);

            var result = await _userManger.CreateAsync(user, Password);
            if (!result.Succeeded)

            {
                return null;

            }

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }
            var some = await _userManger.AddToRoleAsync(user, "User");
            var roles = await _userManger.GetRolesAsync(user);
            return user;
        }

        public Task<User> RegisterAdmin(User user, string Password)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> RemoveUserFromCreateRole(String userId)
        {
            var user = await _userManger.FindByIdAsync(userId);
            if(user == null)
            {
                return null;

            }
            var roles = await _userManger.GetRolesAsync(user);
            var roleToRemove = roles.Where(r => r.Equals("Creator",StringComparison.InvariantCultureIgnoreCase
                ));
            if(roleToRemove == null)
            {
                return null;
            }
            var result = await _userManger.RemoveFromRolesAsync(user, roleToRemove);

            return await _userManger.GetRolesAsync(user);
        }

        public async Task<User> Update(UserUpdateDto userUpdate,String userId)
        {
            var user = await  _userManger.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            var userCheck = await _userManger.FindByNameAsync(userUpdate.UserName);
            if(userCheck != null && userCheck.Id != userId)
            {
                return null;
            }
            user.Email = userUpdate.Email;
            user.UserName = userUpdate.UserName;

            await _userManger.UpdateAsync(user);

            var token = await _userManger.GeneratePasswordResetTokenAsync(user);
            var toke = await _userManger.ResetPasswordAsync(user, token, userUpdate.Password);

            return user;
        }

        //public async  Task<User> UpdateProfile(User user)
        //{
        //     await _userManger.UpdateAsync(user);
        //    return user;
        //}
        public async Task<IEnumerable<string>> AddUserToCreateRole(User user)
        {
            if (!await _roleManager.RoleExistsAsync("Creator"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Creator"));
            }
            var some = await _userManger.AddToRoleAsync(user, "Creator");
            var roles = await _userManger.GetRolesAsync(user);
            return roles;
        }

        public async Task<User> DeleteUser(string userId)
        {
            var user = await _userManger.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            await _userManger.DeleteAsync(user);
            return user;
        }


    }
}
