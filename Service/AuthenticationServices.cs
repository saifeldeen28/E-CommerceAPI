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
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class AuthenticationServices(UserManager<ApplicationUser> _userManager,IConfiguration _configuration,IMapper _mapper) : IAuthenticationServices
    {
        
        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user =await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                throw new UserNotFoundException(loginDto.Email);
            var IsVaild = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!IsVaild)
                throw new UnauthorizedAccessException("Invalid");
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
                    audience: _configuration["JWTOptions:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null;
        }

        public async Task<AddressDto> GetCurrentUserAddressAsync(string email)
        {
            var user = await _userManager.Users.Include(u=>u.Address)
                .FirstOrDefaultAsync(u=>u.Email == email)
                ?? throw new UserNotFoundException(email);
            if (user.Address is not null) 
            { 
                return _mapper.Map<Address,AddressDto>(user.Address);
            }
            throw new AddressNotFoundException(user.UserName);
        }

        public async Task<UserDto> GetCurrentUserAsync(string email)
        {
            var user =await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException(email);
            return new UserDto()
            {
                Email=user.Email,
                DisplayName=user.DisplayName,
                Token = await createTokenAsync(user),
            };
        }
        public async Task<AddressDto> UpdateCurrentUserAddressAsync(AddressDto addressDto, string email)
        {
            var user = await _userManager.Users.Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new UserNotFoundException(email);

            if (user.Address is not null) 
            { 
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.Street = addressDto.Street;
            }
            else
            {
                user.Address =_mapper.Map<AddressDto,Address>(addressDto);
            }
            await _userManager.UpdateAsync(user);
            return _mapper.Map<AddressDto>(addressDto);
        }


    }
}
