namespace Blog.Core.Contracts.Controllers.Users;

public sealed record LoginRequest(string Username, string Password);