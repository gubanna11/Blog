namespace Blog.Core.Contracts.Controllers.Users;

public sealed record RegisterRequest(string FirstName, string LastName, string Username, string Email,
    string PhoneNumber, string Password, string ConfirmPassword);