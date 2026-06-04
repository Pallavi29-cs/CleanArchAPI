using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Common.Models;
using CleanArchAPI.Service.Abstractions;
using CleanArchAPI.Store.Abstraction;


namespace CleanArchAPI.Service.Implementations;


public class OrderService : IOrderService
{
    private readonly IOrderStore _store;


    public OrderService(IOrderStore store)
    {
        _store = store;
    }






    public async Task<(IEnumerable<OrderDto> Data, PaginationMeta Pagination)>
        GetAllAsync(OrderFilterRequest f)
    {
        try
        {

            var (data, total) =
                await _store
                .GetAllAsync(f);



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
                "Error occurred while getting orders.",
                ex);
        }
    }









    public async Task<OrderDto?> GetByGuidAsync(
        Guid g)
    {
        try
        {

            return await _store
                .GetByGuidAsync(g);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while getting order.",
                ex);
        }
    }










    public async Task<Guid> CreateAsync(
        CreateOrderRequest r,
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
                "Error occurred while creating order.",
                ex);
        }
    }


}