using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Common.Models;

namespace CleanArchAPI.Service.Abstractions;

public interface IProductService
{
    Task<(IEnumerable<ProductDto> Data, PaginationMeta Pagination)>
        GetAllAsync(ProductFilterRequest filter);


    Task<ProductDto?> GetByGuidAsync(Guid guid);


    Task<Guid> CreateAsync(
        CreateProductRequest request,
        string createdBy);


    Task UpdateAsync(
        Guid guid,
        UpdateProductRequest request,
        string updatedBy);


    Task DeleteAsync(
        Guid guid,
        string deletedBy);


    Task<int> BulkInsertAsync(
        IEnumerable<BulkProductItem> products,
        string createdBy);
}