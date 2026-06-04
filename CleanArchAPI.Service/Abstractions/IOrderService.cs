using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Common.Models;

namespace CleanArchAPI.Service.Abstractions;

public interface IOrderService
{
    Task<(IEnumerable<OrderDto> Data, PaginationMeta Pagination)>
        GetAllAsync(OrderFilterRequest filter);


    Task<OrderDto?> GetByGuidAsync(Guid guid);


    Task<Guid> CreateAsync(
        CreateOrderRequest request,
        string createdBy);
}