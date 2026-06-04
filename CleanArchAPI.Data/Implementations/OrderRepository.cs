using System.Data;
using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Data.Abstractions;
using Dapper;


namespace CleanArchAPI.Data.Implementations;


public class OrderRepository : IOrderRepository
{

    private readonly IDbConnectionFactory _db;



    public OrderRepository(IDbConnectionFactory db)
    {
        _db = db;
    }








    public async Task<(IEnumerable<OrderDto> Data, int TotalRecords)>
        GetAllAsync(OrderFilterRequest f)
    {

        using var conn =
            _db.CreateConnection();



        var p =
            new DynamicParameters();



        p.Add("@UserGUID", f.UserGUID);
        p.Add("@Status", f.Status);
        p.Add("@PageNumber", f.PageNumber);
        p.Add("@PageSize", f.PageSize);





        var result =
            (await conn.QueryAsync<OrderWithCount>(
                "dbo.usp_Order_GetAll",
                p,
                commandType:
                CommandType.StoredProcedure))
            .ToList();




        return (
            result.Select(x => (OrderDto)x),
            result.FirstOrDefault()?.TotalRecords ?? 0
        );

    }










    public async Task<OrderDto?> GetByGuidAsync(Guid guid)
    {

        using var conn =
            _db.CreateConnection();




        return await conn.QueryFirstOrDefaultAsync<OrderDto>(
            "dbo.usp_Order_GetByGUID",

            new
            {
                GUID = guid
            },

            commandType:
            CommandType.StoredProcedure);

    }












    public async Task<Guid> CreateAsync(
        CreateOrderRequest request,
        string createdBy)
    {

        using var conn =
            _db.CreateConnection();




        var result =
            await conn.QueryFirstAsync<GuidResult>(

            "dbo.usp_Order_Create",


            new
            {
                UserGUID = request.UserGUID,

                CreatedBy = createdBy
            },


            commandType:
            CommandType.StoredProcedure);





        return result.GUID;

    }











    private class OrderWithCount : OrderDto
    {
        public int TotalRecords { get; set; }
    }






    private class GuidResult
    {
        public Guid GUID { get; set; }
    }


}