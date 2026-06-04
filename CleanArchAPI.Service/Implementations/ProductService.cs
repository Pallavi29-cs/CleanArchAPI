using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Common.Models;
using CleanArchAPI.Service.Abstractions;
using CleanArchAPI.Store.Abstraction;


namespace CleanArchAPI.Service.Implementations;


public class ProductService : IProductService
{
    private readonly IProductStore _store;


    public ProductService(IProductStore store)
    {
        _store = store;
    }







    public async Task<(IEnumerable<ProductDto> Data, PaginationMeta Pagination)>
        GetAllAsync(ProductFilterRequest f)
    {
        try
        {

            var (data, total) =
                await _store.GetAllAsync(f);



            return (
                data,
                new PaginationMeta
                {
                    PageNumber = f.PageNumber,
                    PageSize = f.PageSize,
                    TotalRecords = total
                });

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while getting products.",
                ex);
        }
    }









    public async Task<ProductDto?> GetByGuidAsync(Guid g)
    {
        try
        {

            return await _store
                .GetByGuidAsync(g);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while getting product.",
                ex);
        }
    }










    public async Task<Guid> CreateAsync(
        CreateProductRequest r,
        string c)
    {
        try
        {

            return await _store
                .CreateAsync(
                    r,
                    c);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while creating product.",
                ex);
        }
    }










    public async Task UpdateAsync(
        Guid g,
        UpdateProductRequest r,
        string u)
    {
        try
        {

            await _store
                .UpdateAsync(
                    g,
                    r,
                    u);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while updating product.",
                ex);
        }
    }











    public async Task DeleteAsync(
        Guid g,
        string d)
    {
        try
        {

            await _store
                .DeleteAsync(
                    g,
                    d);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while deleting product.",
                ex);
        }
    }











    public async Task<int> BulkInsertAsync(
        IEnumerable<BulkProductItem> p,
        string c)
    {
        try
        {

            return await _store
                .BulkInsertAsync(
                    p,
                    c);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while bulk inserting products.",
                ex);
        }
    }


}