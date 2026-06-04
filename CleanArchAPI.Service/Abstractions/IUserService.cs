using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Common.Models;

namespace CleanArchAPI.Service.Abstractions;

public interface IUserService
{
    Task<(IEnumerable<UserDto> Data, PaginationMeta Pagination)>
        GetAllAsync(UserFilterRequest filter);


    Task<UserDto?> GetByGuidAsync(Guid guid);


    Task<Guid> CreateAsync(
        CreateUserRequest request,
        string createdBy);


    Task UpdateAsync(
        Guid guid,
        UpdateUserRequest request,
        string updatedBy);


    Task DeleteAsync(
        Guid guid,
        string deletedBy);


    Task<int> BulkInsertAsync(
        IEnumerable<BulkUserItem> users,
        string createdBy);
}