using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Common.Models;
using CleanArchAPI.Service.Abstractions;
using CleanArchAPI.Store.Abstraction;


namespace CleanArchAPI.Service.Implementations;


public class UserService : IUserService
{
    private readonly IUserStore _store;


    public UserService(IUserStore store)
    {
        _store = store;
    }





    public async Task<(IEnumerable<UserDto> Data, PaginationMeta Pagination)>
        GetAllAsync(UserFilterRequest f)
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
                "Error occurred while getting users.",
                ex);
        }
    }








    public async Task<UserDto?> GetByGuidAsync(Guid g)
    {
        try
        {

            return await _store
                .GetByGuidAsync(g);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while getting user.",
                ex);
        }
    }









    public async Task<Guid> CreateAsync(
        CreateUserRequest r,
        string c)
    {
        try
        {

            return await _store
                .CreateAsync(r, c);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while creating user.",
                ex);
        }
    }










    public async Task UpdateAsync(
        Guid g,
        UpdateUserRequest r,
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
                "Error occurred while updating user.",
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
                "Error occurred while deleting user.",
                ex);
        }
    }









    public async Task<int> BulkInsertAsync(
        IEnumerable<BulkUserItem> u,
        string c)
    {
        try
        {

            return await _store
                .BulkInsertAsync(
                    u,
                    c);

        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred while bulk inserting users.",
                ex);
        }
    }

}