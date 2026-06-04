using System.Data;
using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Data.Abstractions;
using Dapper;
using Microsoft.Data.SqlClient;


namespace CleanArchAPI.Data.Implementations;


public class ProductRepository : IProductRepository
{

    private readonly IDbConnectionFactory _db;



    public ProductRepository(IDbConnectionFactory db)
    {
        _db = db;
    }







    public async Task<(IEnumerable<ProductDto> Data, int TotalRecords)>
        GetAllAsync(ProductFilterRequest f)
    {

        using var conn = _db.CreateConnection();


        var p = new DynamicParameters();


        p.Add("@SearchTerm", f.SearchTerm);
        p.Add("@CategoryId", f.CategoryId);
        p.Add("@MinPrice", f.MinPrice);
        p.Add("@MaxPrice", f.MaxPrice);
        p.Add("@PageNumber", f.PageNumber);
        p.Add("@PageSize", f.PageSize);



        var result =
            (await conn.QueryAsync<ProductWithCount>(
                "dbo.usp_Product_GetAll",
                p,
                commandType: CommandType.StoredProcedure))
            .ToList();



        return (
            result.Select(x => (ProductDto)x),
            result.FirstOrDefault()?.TotalRecords ?? 0);

    }








    public async Task<ProductDto?> GetByGuidAsync(Guid guid)
    {

        using var conn = _db.CreateConnection();


        return await conn.QueryFirstOrDefaultAsync<ProductDto>(
            "dbo.usp_Product_GetByGUID",
            new { GUID = guid },
            commandType: CommandType.StoredProcedure);

    }









    public async Task<Guid> CreateAsync(
        CreateProductRequest request,
        string createdBy)
    {

        using var conn = _db.CreateConnection();


        var result =
            await conn.QueryFirstAsync<GuidResult>(
            "dbo.usp_Product_Create",

            new
            {
                request.ProductName,
                request.Description,
                request.Price,
                request.Stock,
                request.CategoryId,

                CreatedBy = createdBy
            },

            commandType:
            CommandType.StoredProcedure);



        return result.GUID;

    }









    public async Task UpdateAsync(
        Guid guid,
        UpdateProductRequest request,
        string updatedBy)
    {

        using var conn = _db.CreateConnection();


        await conn.ExecuteAsync(
            "dbo.usp_Product_Update",

            new
            {
                GUID = guid,
                request.ProductName,
                request.Description,
                request.Price,
                request.Stock,
                request.CategoryId,
                UpdatedBy = updatedBy
            },

            commandType:
            CommandType.StoredProcedure);

    }








    public async Task DeleteAsync(
        Guid guid,
        string deletedBy)
    {

        using var conn =
            _db.CreateConnection();



        await conn.ExecuteAsync(
            "dbo.usp_Product_Delete",

            new
            {
                GUID = guid,
                DeletedBy = deletedBy
            },

            commandType:
            CommandType.StoredProcedure);

    }









    public async Task<int> BulkInsertAsync(
        IEnumerable<BulkProductItem> products,
        string createdBy)
    {


        using var conn =
            _db.CreateConnection()
            as SqlConnection
            ?? throw new InvalidOperationException();



        var table = new DataTable();


        table.Columns.Add("ProductName", typeof(string));
        table.Columns.Add("Description", typeof(string));
        table.Columns.Add("Price", typeof(decimal));
        table.Columns.Add("Stock", typeof(int));
        table.Columns.Add("CategoryId", typeof(int));



        foreach (var p in products)
        {
            table.Rows.Add(
                p.ProductName,
                p.Description ?? (object)DBNull.Value,
                p.Price,
                p.Stock,
                p.CategoryId);
        }





        var dp = new DynamicParameters();


        dp.Add(
            "@Products",
            table.AsTableValuedParameter(
            "dbo.ProductList_Type"));


        dp.Add("@CreatedBy", createdBy);



        var result =
            await conn.QueryFirstAsync<CountResult>(
            "dbo.usp_Product_BulkInsert",
            dp,
            commandType:
            CommandType.StoredProcedure);



        return result.InsertedCount;

    }








    private class ProductWithCount : ProductDto
    {
        public int TotalRecords { get; set; }
    }



    private class GuidResult
    {
        public Guid GUID { get; set; }
    }



    private class CountResult
    {
        public int InsertedCount { get; set; }
    }


}