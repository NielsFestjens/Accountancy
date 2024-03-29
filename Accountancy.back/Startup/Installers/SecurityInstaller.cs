﻿using System.Net;
using System.Security.Cryptography;
using Accountancy.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Accountancy.Startup.Installers;

public class SecurityInstaller
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.Configure<MvcOptions>(config =>
        {
            config.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
        });

        services.AddTransient<ISecurityService, SecurityService>();
        services.AddTransient(serviceProvider => RandomNumberGenerator.Create());
        services.AddTransient<HashAlgorithm>(serviceProvider => new HMACSHA512(new byte[] { 1, 2, 3 })); // todo: key?

        services.AddAuthentication("DatScheme").AddCookie("DatScheme", opt =>
        {
            opt.Events.OnValidatePrincipal += context =>
            {
                return Task.FromResult(0);
            };
            opt.Events.OnRedirectToLogin += context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Task.FromResult(0);
            };
        });
    }

    public static void Configure(IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}