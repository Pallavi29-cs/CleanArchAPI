namespace CleanArchAPI.Common.Constants;


/// <summary>
/// Application-wide constants.
/// </summary>
public static class AppConstants
{

    // ==========================
    // Roles
    // ==========================

    public const string AdminRole =
        "Admin";


    public const string ManagerRole =
        "Manager";


    public const string UserRole =
        "User";





    // ==========================
    // Order Status
    // ==========================

    public const string OrderPending =
        "Pending";


    public const string OrderConfirmed =
        "Confirmed";


    public const string OrderShipped =
        "Shipped";


    public const string OrderDelivered =
        "Delivered";


    public const string OrderCancelled =
        "Cancelled";






    // ==========================
    // Common
    // ==========================

    public const string SystemUser =
        "System";


    public const string JwtSettings =
        "JwtSettings";







    // ==========================
    // Response Messages
    // ==========================

    public const string LoginSuccess =
        "Login successful";


    public const string InvalidLogin =
        "Invalid email or password.";



    public const string UserCreated =
        "User created successfully";


    public const string UserUpdated =
        "User updated successfully";


    public const string UserDeleted =
        "User deleted successfully";


    public const string UserNotFound =
        "User not found";




    public const string ProductCreated =
        "Product created successfully";


    public const string ProductUpdated =
        "Product updated successfully";


    public const string ProductDeleted =
        "Product deleted successfully";


    public const string ProductNotFound =
        "Product not found";





    public const string OrderCreated =
        "Order created successfully";


    public const string OrderNotFound =
        "Order not found";

}