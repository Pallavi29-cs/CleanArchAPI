using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Data.Abstractions;
using CleanArchAPI.Store.Abstraction;


namespace CleanArchAPI.Store.Implemenation;


public class ProductStore : IProductStore
{
    private readonly IProductRepository _repo;


    public ProductStore(IProductRepository repo)
    {
        _repo = repo;
    }







    public async Task<(IEnumerable<ProductDto> Data, int TotalRecords)>
        GetAllAsync(ProductFilterRequest f)
    {
        try
        {

            return await _repo
                .GetAllAsync(f);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in ProductStore GetAll.",
                ex);
        }
    }









    public async Task<ProductDto?> GetByGuidAsync(
        Guid g)
    {
        try
        {

            return await _repo
                .GetByGuidAsync(g);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in ProductStore GetByGuid.",
                ex);
        }
    }










    public async Task<Guid> CreateAsync(
        CreateProductRequest r,
        string c)
    {
        try
        {

            return await _repo
                .CreateAsync(
                    r,
                    c);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in ProductStore Create.",
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

            await _repo
                .UpdateAsync(
                    g,
                    r,
                    u);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in ProductStore Update.",
                ex);
        }
    }











    public async Task DeleteAsync(
        Guid g,
        string d)
    {
        try
        {

            await _repo
                .DeleteAsync(
                    g,
                    d);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in ProductStore Delete.",
                ex);
        }
    }











    public async Task<int> BulkInsertAsync(
        IEnumerable<BulkProductItem> p,
        string c)
    {
        try
        {

            return await _repo
                .BulkInsertAsync(
                    p,
                    c);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in ProductStore BulkInsert.",
                ex);
        }
    }


}