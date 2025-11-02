using DomainLayer.Models;
using Microsoft.AspNetCore.Identity;
using ServiceAbstraction;
using Shared.Dtos.Identity_dtos;
using DomainLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace Service
{
    public class AuthenticationServices(UserManager<ApplicationUser> _userManager,IConfiguration _configuration) : IAuthenticationServices
    {
        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user =await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                throw new UserNotFoundException(loginDto.Email);
            var IsVaild = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!IsVaild)
                throw new Exception("Invalid");
            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,     
                Token=await createTokenAsync(user)
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                DisplayName = registerDto.DisplayName,
                PhoneNumber = registerDto.PhoneNumber
            };
            var result =await  _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) 
            {
                var errors = result.Errors.Select(e=>e.Description).ToList();
                throw new BadRequestException(errors);
            }
               
            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token =await createTokenAsync(user)
            };
        }
        private async Task<string> createTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
            };
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var secretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    issuer: _configuration.GetSection("JWTOptions")["Issuer"],
                    audience: _configuration["JWTOptions.Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
