using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Data.Abstractions;
using CleanArchAPI.Store.Abstraction;


namespace CleanArchAPI.Store.Implemenation;


public class OrderStore : IOrderStore
{
    private readonly IOrderRepository _repo;


    public OrderStore(IOrderRepository repo)
    {
        _repo = repo;
    }








    public async Task<(IEnumerable<OrderDto> Data, int TotalRecords)>
        GetAllAsync(OrderFilterRequest f)
    {
        try
        {

            return await _repo
                .GetAllAsync(f);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in OrderStore GetAll.",
                ex);
        }
    }










    public async Task<OrderDto?> GetByGuidAsync(
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
                "Error occurred in OrderStore GetByGuid.",
                ex);
        }
    }











    public async Task<Guid> CreateAsync(
        CreateOrderRequest r,
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
                "Error occurred in OrderStore Create.",
                ex);
        }
    }

}