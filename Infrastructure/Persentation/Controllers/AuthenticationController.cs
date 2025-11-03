using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.Dtos.Identity_dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Persentation.Controllers
{
    public class AuthenticationController(IServiceManger _serviceManger):APIBaseController
    {
       
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
           var user =await _serviceManger.AuthenticationService.LoginAsync(loginDto);
            return Ok(user);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user =await _serviceManger.AuthenticationService.RegisterAsync(registerDto);
            return Ok(user);
        }
        [HttpGet("CheckEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var res =await _serviceManger.AuthenticationService.CheckEmailAsync(email);
            return Ok(res);
        }
        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var res =await _serviceManger.AuthenticationService.GetCurrentUserAsync(email);
            return Ok(res);
        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var adrs = await _serviceManger.AuthenticationService.GetCurrentUserAddressAsync(email);
            return Ok(adrs);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress (AddressDto address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var adrs = await _serviceManger.AuthenticationService.UpdateCurrentUserAddressAsync(address, email);
            return Ok(adrs);
        }

    }
}
