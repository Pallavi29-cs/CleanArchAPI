using System.ComponentModel.DataAnnotations;

namespace CleanArchAPI.Common.DTOs;

// ============================================================
// PAGINATION
// ============================================================
public class PaginationRequest
{
    [Range(1, int.MaxValue)] public int PageNumber { get; set; } = 1;
    [Range(1, 100)] public int PageSize { get; set; } = 10;
}

// ============================================================
// AUTH
// ============================================================
public class LoginRequest
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string UserGUID { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

// ============================================================
// USER
// ============================================================
public class CreateUserRequest
{
    [Required, MaxLength(100)] public string FirstName { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string LastName { get; set; } = string.Empty;
    [Required, EmailAddress, MaxLength(200)] public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    [Required] public int RoleId { get; set; }
}

public class UpdateUserRequest
{
    [Required, MaxLength(100)] public string FirstName { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string LastName { get; set; } = string.Empty;
    [Required] public int RoleId { get; set; }
}

public class UserDto
{
    public Guid GUID { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedOn { get; set; }
}

public class UserFilterRequest : PaginationRequest
{
    public string? SearchTerm { get; set; }
    public int? RoleId { get; set; }
}

public class BulkUserItem
{
    [Required, MaxLength(100)] public string FirstName { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string LastName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    [Required] public int RoleId { get; set; }
}

// ============================================================
// PRODUCT
// ============================================================
public class CreateProductRequest
{
    [Required, MaxLength(200)] public string ProductName { get; set; } = string.Empty;
    [MaxLength(1000)] public string? Description { get; set; }
    [Required, Range(0.01, double.MaxValue)] public decimal Price { get; set; }
    [Range(0, int.MaxValue)] public int Stock { get; set; }
    [Required] public int CategoryId { get; set; }
}

public class UpdateProductRequest
{
    [Required, MaxLength(200)] public string ProductName { get; set; } = string.Empty;
    [MaxLength(1000)] public string? Description { get; set; }
    [Required, Range(0.01, double.MaxValue)] public decimal Price { get; set; }
    [Range(0, int.MaxValue)] public int Stock { get; set; }
    [Required] public int CategoryId { get; set; }
}

public class ProductDto
{
    public Guid GUID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public Guid CategoryGUID { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedOn { get; set; }
}

public class ProductFilterRequest : PaginationRequest
{
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}

public class BulkProductItem
{
    [Required, MaxLength(200)] public string ProductName { get; set; } = string.Empty;
    [MaxLength(1000)] public string? Description { get; set; }
    [Required, Range(0.01, double.MaxValue)] public decimal Price { get; set; }
    [Range(0, int.MaxValue)] public int Stock { get; set; }
    [Required] public int CategoryId { get; set; }
}

// ============================================================
// ORDER
// ============================================================
public class CreateOrderRequest
{
    [Required] public Guid UserGUID { get; set; }
}

public class OrderDto
{
    public Guid GUID { get; set; }
    public Guid UserGUID { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class OrderFilterRequest : PaginationRequest
{
    public Guid? UserGUID { get; set; }
    public string? Status { get; set; }
}
