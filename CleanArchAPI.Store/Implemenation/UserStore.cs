using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Data.Abstractions;
using CleanArchAPI.Store.Abstraction;


namespace CleanArchAPI.Store.Implemenation;


public class UserStore : IUserStore
{
    private readonly IUserRepository _repo;


    public UserStore(IUserRepository repo)
    {
        _repo = repo;
    }






    public async Task<(IEnumerable<UserDto> Data, int TotalRecords)>
        GetAllAsync(UserFilterRequest f)
    {
        try
        {
            return await _repo.GetAllAsync(f);
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in UserStore GetAll.",
                ex);
        }
    }







    public async Task<UserDto?> GetByGuidAsync(Guid g)
    {
        try
        {
            return await _repo.GetByGuidAsync(g);
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in UserStore GetByGuid.",
                ex);
        }
    }








    public async Task<UserDto?> GetByEmailAsync(string e)
    {
        try
        {
            return await _repo.GetByEmailAsync(e);
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in UserStore GetByEmail.",
                ex);
        }
    }








    public async Task<string?> GetPasswordHashByEmailAsync(string e)
    {
        try
        {
            return await _repo
                .GetPasswordHashByEmailAsync(e);
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in UserStore PasswordHash.",
                ex);
        }
    }








    public async Task<Guid> CreateAsync(
        CreateUserRequest r,
        string c)
    {
        try
        {
            return await _repo.CreateAsync(r, c);
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in UserStore Create.",
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
            await _repo.UpdateAsync(g, r, u);
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in UserStore Update.",
                ex);
        }
    }








    public async Task DeleteAsync(
        Guid g,
        string d)
    {
        try
        {
            await _repo.DeleteAsync(g, d);
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in UserStore Delete.",
                ex);
        }
    }








    public async Task<int> BulkInsertAsync(
        IEnumerable<BulkUserItem> u,
        string c)
    {
        try
        {
            return await _repo.BulkInsertAsync(u, c);
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Error occurred in UserStore BulkInsert.",
                ex);
        }
    }

}