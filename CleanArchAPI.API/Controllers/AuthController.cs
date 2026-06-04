using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Common.Models;
using CleanArchAPI.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;


namespace CleanArchAPI.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    private readonly ILogger<AuthController> _logger;



    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger)
    {
        _authService = authService;

        _logger = logger;
    }






    /// <summary>
    /// User Login - Generate JWT Token (Async)
    /// </summary>
    [HttpPost(nameof(LoginAsync))]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginRequest request)
    {


        if (!ModelState.IsValid)
        {

            var errors =
                string.Join("; ",
                ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));



            return BadRequest(
                ApiResponse<object>
                .BadRequest(errors));
        }






        _logger.LogInformation(
            "Login attempt: {Email}",
            request.Email);







        var result =
            await _authService
            .LoginAsync(request);







        if (result == null)
        {

            return Unauthorized(
                ApiResponse<object>
                .Unauthorized(
                    "Invalid email or password."));

        }








        return Ok(

            ApiResponse<LoginResponse>
            .Ok(
                result,
                "Login successful")

        );


    }

}