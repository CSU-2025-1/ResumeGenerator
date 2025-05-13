using Microsoft.AspNetCore.Mvc;
using ResumeGenerator.AuthService.Application.DTO.Requests;
using ResumeGenerator.AuthService.Application.DTO.Responses;
using ResumeGenerator.AuthService.Application.Exceptions;
using ResumeGenerator.AuthService.Application.Services;

namespace ResumeGenerator.AuthService.Web.Controllers;

[ApiController]
[Route("auth/v1")]
public sealed class AuthController : ControllerBase
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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {

        if (!ModelState.IsValid)
        { 
            return BadRequest("Invalid request"); 
        }
        try
        {
            var result = await _authService.RegisterUserAsync(request);
            //return CreatedAtAction(nameof(Register), new { username = result.Username }, result);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (UserAlreadyExistsException)
        {
            return Conflict("Username already exists");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        if (!ModelState.IsValid)
        { 
            return BadRequest("Invalid request"); 
        }
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
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
}
