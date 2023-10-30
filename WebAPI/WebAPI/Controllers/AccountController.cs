﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Interface.Services;
using WebAPI.Models.Dtos;
using WebAPI.Models.Identity;
using WebAPI.Models.Schemas;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IAccountService _accountService;

    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration, IAccountService accountService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _accountService = accountService;
    }


    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp(SignUpSchema schema)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _userManager.Users.AnyAsync(x => x.Email == schema.Email))
                return Conflict("An account with that email already exists");

            if ((await _userManager.CreateAsync(schema, schema.Password)).Succeeded)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == schema.Email);
                return Created("", (UserDto)user!);
            }

            return StatusCode(502, "Something went wrong when signing up. Make sure all of the provided information was correct and try again.");
        }
        catch
        {
            return StatusCode(502, "Something went wrong when signing up. Please try again.");
        }
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn(SignInSchema schema)
    {
        try
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if ((await _signInManager.PasswordSignInAsync(schema.Email, schema.Password, false, false)).Succeeded)
            {
                var tokenString = _accountService.CreateJwsToken(schema.Email);

                return Ok(new { Token = tokenString });
            }

            return Unauthorized("Invalid login credentials, please try again.");
        }
        catch
        {
            return StatusCode(502, "Something went wrong when signing in. Please try again.");
        }
    }
}