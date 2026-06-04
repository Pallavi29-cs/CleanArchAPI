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
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;


    public ProductsController(
        IProductService productService)
    {
        _productService = productService;
    }



    /// <summary>
    /// Get all products (Async)
    /// </summary>
    [HttpGet(nameof(GetAllAsync))]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] ProductFilterRequest filter)
    {
        var (data, pagination) =
            await _productService.GetAllAsync(filter);


        return Ok(
            ApiResponse<IEnumerable<ProductDto>>
            .Ok(
                data,
                "Products retrieved successfully",
                pagination));
    }








    /// <summary>
    /// Get product by GUID (Async)
    /// </summary>
    [HttpGet(nameof(GetByGuidAsync) + "/{guid:guid}")]
    public async Task<IActionResult> GetByGuidAsync(
        Guid guid)
    {
        var product =
            await _productService
            .GetByGuidAsync(guid);


        if (product == null)
        {
            return NotFound(
                ApiResponse<ProductDto>
                .NotFound("Product not found"));
        }


        return Ok(
            ApiResponse<ProductDto>
            .Ok(product));
    }








    /// <summary>
    /// Create new product (Async)
    /// </summary>
    [HttpPost(nameof(CreateAsync))]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateProductRequest request)
    {

        var createdBy =
            JwtHelper.GetUserEmail(User)
            ?? "System";


        var guid =
            await _productService
            .CreateAsync(
                request,
                createdBy);



        return CreatedAtAction(
            nameof(GetByGuidAsync),
            new { guid },
            ApiResponse<object>
            .Created(
                new { guid },
                "Product created successfully"));
    }









    /// <summary>
    /// Update product (Async)
    /// </summary>
    [HttpPut(nameof(UpdateAsync) + "/{guid:guid}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UpdateAsync(
        Guid guid,
        [FromBody] UpdateProductRequest request)
    {

        var updatedBy =
            JwtHelper.GetUserEmail(User)
            ?? "System";



        await _productService
        .UpdateAsync(
            guid,
            request,
            updatedBy);



        return Ok(
            ApiResponse<object>
            .Ok(
                new { guid },
                "Product updated successfully"));
    }









    /// <summary>
    /// Delete product (Async)
    /// </summary>
    [HttpDelete(nameof(DeleteAsync) + "/{guid:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAsync(
        Guid guid)
    {

        var deletedBy =
            JwtHelper.GetUserEmail(User)
            ?? "System";



        await _productService
        .DeleteAsync(
            guid,
            deletedBy);



        return Ok(
            ApiResponse<object>
            .Ok(
                new { guid },
                "Product deleted successfully"));
    }










    /// <summary>
    /// Bulk insert products (Async)
    /// </summary>
    [HttpPost(nameof(BulkInsertAsync))]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> BulkInsertAsync(
        IEnumerable<BulkProductItem> products)
    {

        var list =
            products.ToList();



        var createdBy =
            JwtHelper.GetUserEmail(User)
            ?? "System";



        var count =
            await _productService
            .BulkInsertAsync(
                list,
                createdBy);



        return Ok(
            ApiResponse<object>
            .Created(
                new { count },
                $"{count} products inserted successfully"));
    }

}