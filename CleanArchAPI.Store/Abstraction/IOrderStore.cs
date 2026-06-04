using CleanArchAPI.Common.DTOs;


namespace CleanArchAPI.Store.Abstraction;


public interface IOrderStore
{

    Task<(IEnumerable<OrderDto> Data, int TotalRecords)>
        GetAllAsync(OrderFilterRequest filter);



    Task<OrderDto?> GetByGuidAsync(
        Guid guid);



    Task<Guid> CreateAsync(
        CreateOrderRequest request,
        string createdBy);

}