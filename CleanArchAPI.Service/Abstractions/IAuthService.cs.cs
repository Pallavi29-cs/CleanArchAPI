using CleanArchAPI.Common.DTOs;

namespace CleanArchAPI.Service.Abstractions;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}