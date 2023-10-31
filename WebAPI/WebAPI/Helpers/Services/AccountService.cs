﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Interface.Services;
using WebAPI.Models.Identity;

namespace WebAPI.Helpers.Services;

public class AccountService : IAccountService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManger;

    public AccountService(IConfiguration configuration, UserManager<AppUser> userManger)
    {
        _configuration = configuration;
        _userManger = userManger;
    }

    public async Task<string> CreateJwsToken(string email)
    {
        var user = await _userManger.Users.FirstOrDefaultAsync(x => x.Email == email);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, email),
                new Claim("unique_id", user!.Id)
            }),
            Expires = DateTime.UtcNow.AddMonths(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = "https://manero.com",
            Audience = "https://manero.com"
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }
}
