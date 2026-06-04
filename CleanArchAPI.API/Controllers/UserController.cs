using CleanArchAPI.API.Utilities;
using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Common.Models;
using CleanArchAPI.Service.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CleanArchAPI.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;


    public UsersController(
        IUserService userService)
    {
        _userService = userService;
    }




    /// <summary>
    /// Get all users (Async)
    /// </summary>
    [HttpGet(nameof(GetAllAsync))]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] UserFilterRequest filter)
    {
        var (data, pagination) =
            await _userService.GetAllAsync(filter);


        return Ok(
            ApiResponse<IEnumerable<UserDto>>
            .Ok(
                data,
                "Users retrieved successfully",
                pagination));
    }







    /// <summary>
    /// Get user by GUID (Async)
    /// </summary>
    [HttpGet(nameof(GetByGuidAsync) + "/{guid:guid}")]
    public async Task<IActionResult> GetByGuidAsync(
        Guid guid)
    {

        var user =
            await _userService.GetByGuidAsync(guid);


        if (user == null)
        {
            return NotFound(
                ApiResponse<UserDto>
                .NotFound("User not found"));
        }


        return Ok(
            ApiResponse<UserDto>
            .Ok(user));
    }








    /// <summary>
    /// Create new user (Async)
    /// </summary>
    [HttpPost(nameof(CreateAsync))]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateAsync(
        CreateUserRequest request)
    {

        var createdBy =
            JwtHelper.GetUserEmail(User)
            ?? "System";


        var guid =
            await _userService
            .CreateAsync(
                request,
                createdBy);



        return CreatedAtAction(
            nameof(GetByGuidAsync),
            new { guid },
            ApiResponse<object>
            .Created(
                new { guid },
                "User created successfully"));
    }









    /// <summary>
    /// Update user (Async)
    /// </summary>
    [HttpPut(nameof(UpdateAsync) + "/{guid:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAsync(
        Guid guid,
        UpdateUserRequest request)
    {

        var updatedBy =
            JwtHelper.GetUserEmail(User)
            ?? "System";



        await _userService
            .UpdateAsync(
                guid,
                request,
                updatedBy);



        return Ok(
            ApiResponse<object>
            .Ok(
                new { guid },
                "User updated successfully"));
    }








    /// <summary>
    /// Delete user (Async)
    /// </summary>
    [HttpDelete(nameof(DeleteAsync) + "/{guid:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAsync(
        Guid guid)
    {

        var deletedBy =
            JwtHelper.GetUserEmail(User)
            ?? "System";


        await _userService
            .DeleteAsync(
                guid,
                deletedBy);



        return Ok(
            ApiResponse<object>
            .Ok(
                new { guid },
                "User deleted successfully"));
    }

}