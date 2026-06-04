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
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;


    public OrdersController(
        IOrderService orderService)
    {
        _orderService = orderService;
    }







    /// <summary>
    /// Get all orders (Async)
    /// </summary>
    [HttpGet(nameof(GetAllAsync))]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] OrderFilterRequest filter)
    {

        var (data, pagination) =
            await _orderService
            .GetAllAsync(filter);



        return Ok(
            ApiResponse<IEnumerable<OrderDto>>
            .Ok(
                data,
                "Orders retrieved successfully",
                pagination));
    }









    /// <summary>
    /// Get order by GUID (Async)
    /// </summary>
    [HttpGet(nameof(GetByGuidAsync) + "/{guid:guid}")]
    public async Task<IActionResult> GetByGuidAsync(
        Guid guid)
    {

        var order =
            await _orderService
            .GetByGuidAsync(guid);



        if (order == null)
        {
            return NotFound(
                ApiResponse<OrderDto>
                .NotFound(
                    "Order not found"));
        }




        return Ok(
            ApiResponse<OrderDto>
            .Ok(order));

    }










    /// <summary>
    /// Create new order (Async)
    /// </summary>
    [HttpPost(nameof(CreateAsync))]
    public async Task<IActionResult> CreateAsync(
        CreateOrderRequest request)
    {

        var createdBy =
            JwtHelper.GetUserEmail(User)
            ?? "System";



        var guid =
            await _orderService
            .CreateAsync(
                request,
                createdBy);



        return CreatedAtAction(
            nameof(GetByGuidAsync),
            new { guid },
            ApiResponse<object>
            .Created(
                new { guid },
                "Order created successfully"));

    }

}