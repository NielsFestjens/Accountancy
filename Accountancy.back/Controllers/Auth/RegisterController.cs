﻿using Accountancy.Domain.Auth;
using Accountancy.Infrastructure.Database;
using Accountancy.Infrastructure.Exceptions;
using Accountancy.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accountancy.Controllers.Auth;

public record RegisterCommand
{
    public string Username { get; set; }
    public string Password { get; set; }
}

[Route("api/Auth/[controller]")]
[AllowAnonymous]
public class RegisterController : Controller
{
    private readonly ISecurityService _securityService;
    private readonly IRepository _repository;

    public RegisterController(ISecurityService securityService, IRepository repository)
    {
        _securityService = securityService;
        _repository = repository;
    }

    [HttpPost]
    public async Task Register([FromBody] RegisterCommand command)
    {
        var salt = _securityService.GetSalt();
        var password = _securityService.CalculateHash(command.Password, salt);

        if (_repository.Query<User>().Any(x => x.Username == command.Username))
            throw new UsernameAlreadyExistsException();

        var user = new User { Username = command.Username, Password = password, PasswordSalt = salt };

        _repository.AddAndSave(user);

        await _securityService.SignIn(HttpContext, user);
    }
}

public class UsernameAlreadyExistsException : KnownException
{
    public UsernameAlreadyExistsException() : base("This username already exists")
    {
    }
}