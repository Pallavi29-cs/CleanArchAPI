using CleanArchAPI.Common.DTOs;


namespace CleanArchAPI.Store.Abstraction;


public interface IUserStore
{

    Task<(IEnumerable<UserDto> Data, int TotalRecords)>
        GetAllAsync(UserFilterRequest filter);



    Task<UserDto?> GetByGuidAsync(
        Guid guid);



    Task<UserDto?> GetByEmailAsync(
        string email);



    Task<string?> GetPasswordHashByEmailAsync(
        string email);



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