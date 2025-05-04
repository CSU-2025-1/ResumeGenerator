using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using ResumeGenerator.AuthService.Application.DTO.Requests;
using ResumeGenerator.AuthService.Application.DTO.Responses;
using ResumeGenerator.AuthService.Application.Exceptions;
using ResumeGenerator.AuthService.Application.Services;

namespace ResumeGenerator.AuthService.Web.Controllers;

[ApiController]
[Route("auth/v1")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        try
        {
            var result = await _authService.RegisterUserAsync(request);
            return CreatedAtAction(nameof(Register), result);
        }
        catch (UserAlreadyExistsException)
        {
            return Conflict("Username already exists");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        try
        {
            var result = await _authService.LoginUserAsync(request);
            return Ok(result);
        }
        catch (UserNotFoundException)
        {
            return NotFound("User not found");
        }
        catch (InvalidCredentialsException)
        {
            return BadRequest("Invalid credentials");
        }
        catch (UserNotActiveException)
        {
            return UnprocessableEntity("User account is not activated");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}