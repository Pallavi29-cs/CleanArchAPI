using CleanArchAPI.Common.DTOs;


namespace CleanArchAPI.Data.Abstractions;


public interface IProductRepository
{

    Task<(IEnumerable<ProductDto> Data, int TotalRecords)>
        GetAllAsync(ProductFilterRequest filter);



    Task<ProductDto?> GetByGuidAsync(
        Guid guid);



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