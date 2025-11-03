using Shared.Dtos.Identity_dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IAuthenticationServices
    {
        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<bool> CheckEmailAsync (string email);
        Task<AddressDto> GetCurrentUserAddressAsync(string email);
        Task<AddressDto> UpdateCurrentUserAddressAsync(AddressDto addressDto,string email);
        Task<UserDto> GetCurrentUserAsync(string email);
    }
}
