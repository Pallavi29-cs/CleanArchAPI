using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanArchAPI.Common.DTOs;
using CleanArchAPI.Service.Abstractions;
using CleanArchAPI.Store.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace CleanArchAPI.Service.Implementations;


public class AuthService : IAuthService
{
    private readonly IUserStore _store;
    private readonly IConfiguration _config;


    public AuthService(
        IUserStore store,
        IConfiguration config)
    {
        _store = store;
        _config = config;
    }



    public async Task<LoginResponse?> LoginAsync(
        LoginRequest request)
    {
        try
        {

            var user =
                await _store
                .GetByEmailAsync(request.Email);



            if (user == null ||
                !user.IsActive)
            {
                return null;
            }




            var hash =
                await _store
                .GetPasswordHashByEmailAsync(
                    request.Email);




            if (hash == null ||
                !BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    hash))
            {
                return null;
            }






            var jwt =
                _config.GetSection(
                    "JwtSettings");




            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        jwt["SecretKey"]!));





            var expiry =
                DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(
                        jwt["ExpiryInMinutes"]));






            var claims = new[]
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.GUID.ToString()),


                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.GUID.ToString()),


                new Claim(
                    ClaimTypes.Email,
                    user.Email),


                new Claim(
                    ClaimTypes.Role,
                    user.RoleName),


                new Claim(
                    "FullName",
                    $"{user.FirstName} {user.LastName}"),


                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            };






            var token =
                new JwtSecurityToken(

                    issuer:
                    jwt["Issuer"],

                    audience:
                    jwt["Audience"],

                    claims:
                    claims,

                    expires:
                    expiry,

                    signingCredentials:
                    new SigningCredentials(
                        key,
                        SecurityAlgorithms.HmacSha256));







            return new LoginResponse
            {
                Token =
                new JwtSecurityTokenHandler()
                .WriteToken(token),


                UserGUID =
                user.GUID.ToString(),


                FullName =
                $"{user.FirstName} {user.LastName}",


                Role =
                user.RoleName,


                ExpiresAt =
                expiry
            };

        }
        catch (Exception ex)
        {

            throw new Exception(
                "Error occurred while login user.",
                ex);

        }
    }

}