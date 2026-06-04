using System.Data;
using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Data.Abstractions;
using Dapper;
using Microsoft.Data.SqlClient;


namespace CleanArchAPI.Data.Implementations;


public class UserRepository : IUserRepository
{

    private readonly IDbConnectionFactory _db;


    public UserRepository(IDbConnectionFactory db)
    {
        _db = db;
    }




    public async Task<(IEnumerable<UserDto> Data, int TotalRecords)>
        GetAllAsync(UserFilterRequest f)
    {

        using var conn = _db.CreateConnection();


        var p = new DynamicParameters();

        p.Add("@SearchTerm", f.SearchTerm);
        p.Add("@RoleId", f.RoleId);
        p.Add("@PageNumber", f.PageNumber);
        p.Add("@PageSize", f.PageSize);



        var result =
            (await conn.QueryAsync<UserWithCount>(
                "dbo.usp_User_GetAll",
                p,
                commandType: CommandType.StoredProcedure))
            .ToList();



        return (
            result.Select(x => (UserDto)x),
            result.FirstOrDefault()?.TotalRecords ?? 0);

    }







    public async Task<UserDto?> GetByGuidAsync(Guid guid)
    {

        using var conn = _db.CreateConnection();


        return await conn.QueryFirstOrDefaultAsync<UserDto>(
            "dbo.usp_User_GetByGUID",
            new { GUID = guid },
            commandType: CommandType.StoredProcedure);

    }








    public async Task<UserDto?> GetByEmailAsync(string email)
    {

        using var conn = _db.CreateConnection();


        return await conn.QueryFirstOrDefaultAsync<UserDto>(
            "dbo.usp_User_GetByEmail",
            new { Email = email },
            commandType: CommandType.StoredProcedure);

    }








    public async Task<string?> GetPasswordHashByEmailAsync(string email)
    {

        using var conn = _db.CreateConnection();



        var result =
            await conn.QueryFirstOrDefaultAsync<dynamic>(
                "dbo.usp_User_GetByEmail",
                new { Email = email },
                commandType: CommandType.StoredProcedure);



        return result?.PasswordHash;

    }










    public async Task<Guid> CreateAsync(
        CreateUserRequest request,
        string createdBy)
    {

        using var conn = _db.CreateConnection();


        var result =
            await conn.QueryFirstAsync<GuidResult>(
            "dbo.usp_User_Create",

            new
            {
                request.FirstName,
                request.LastName,
                request.Email,

                PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.Password),

                request.RoleId,

                CreatedBy = createdBy
            },

            commandType:
            CommandType.StoredProcedure);



        return result.GUID;

    }









    public async Task UpdateAsync(
        Guid guid,
        UpdateUserRequest request,
        string updatedBy)
    {

        using var conn = _db.CreateConnection();



        await conn.ExecuteAsync(
            "dbo.usp_User_Update",

            new
            {
                GUID = guid,
                request.FirstName,
                request.LastName,
                request.RoleId,
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
            "dbo.usp_User_Delete",

            new
            {
                GUID = guid,
                DeletedBy = deletedBy
            },

            commandType:
            CommandType.StoredProcedure);

    }









    public async Task<int> BulkInsertAsync(
        IEnumerable<BulkUserItem> users,
        string createdBy)
    {

        using var conn =
            _db.CreateConnection()
            as SqlConnection
            ?? throw new InvalidOperationException();



        var table = new DataTable();

        table.Columns.Add("FirstName", typeof(string));
        table.Columns.Add("LastName", typeof(string));
        table.Columns.Add("Email", typeof(string));
        table.Columns.Add("Password", typeof(string));
        table.Columns.Add("RoleId", typeof(int));



        foreach (var u in users)
        {
            table.Rows.Add(
                u.FirstName,
                u.LastName,
                u.Email,
                BCrypt.Net.BCrypt.HashPassword(u.Password),
                u.RoleId);
        }




        var p = new DynamicParameters();

        p.Add(
            "@Users",
            table.AsTableValuedParameter(
                "dbo.UserList_Type"));


        p.Add("@CreatedBy", createdBy);



        var result =
            await conn.QueryFirstAsync<CountResult>(
            "dbo.usp_User_BulkInsert",
            p,
            commandType:
            CommandType.StoredProcedure);



        return result.InsertedCount;

    }








    private class UserWithCount : UserDto
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