using FluentAssertions;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.Application.Contracts.User;
using NadinSoft.Application.Features.User.Commands.Register;
using NadinSoft.Application.Features.User.Queries.PasswordLogin;
using NadinSoft.Domain.Entities.User;
using NadinSoft.Infrastructure.Persistence;

namespace NadinSoft.Identity.Tests;

public class IdentityTests : IClassFixture<IdentityTestSetup>
{
    private IServiceProvider _serviceProvider;

    public IdentityTests(IdentityTestSetup setup)
    {
        _serviceProvider = setup.ServiceProvider;
    }

    [Fact]
    public async Task Registering_User_Should_Succeed()
    {
        var testUser = new RegisterUserCommand("test", "test", "test", "qwe123", "test@test.com", "qwe123");

        var sender = _serviceProvider.GetRequiredService<ISender>();
        var registerResult = await sender.Send(testUser);
        registerResult.Result.Should().BeTrue();
    }

    [Fact]
    public async Task Getting_AccessToken_Should_Succeed()
    {
        var sender = _serviceProvider.GetRequiredService<ISender>();

        var testUser = new RegisterUserCommand("test", "test", "TEST3", "qwe123", "test3@test.com", "qwe123");
        await sender.Send(testUser);

        var userManager = _serviceProvider.GetRequiredService<UserManager<UserEntity>>();
        var user = await userManager.FindByNameAsync("TEST3");
        user.Should().NotBeNull();

        var tokenQuery = new UserPasswordLoginQuery("TEST3", "qwe123");
        var tokenQueryResult = await sender.Send(tokenQuery);

        tokenQueryResult.Result.Should().NotBeNull();
        tokenQueryResult.Result!.AccessToken.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Registered_User_Should_Exist_In_Database()
    {
        var testUser = new RegisterUserCommand("test", "test", "TEST3", "qwe123", "test3@test.com", "qwe123");

        var sender = _serviceProvider.GetRequiredService<ISender>();
        var userManager = _serviceProvider.GetRequiredService<UserManager<UserEntity>>();


        await sender.Send(testUser);

        var user = userManager.FindByNameAsync("TEST3");
        user.Should().NotBeNull("User should exist in DB before login.");
    }

    [Fact]
    public async Task Temp_Test()
    {
        var testUser = new RegisterUserCommand("test", "test", "TEST3", "qwe123", "test3@test.com", "qwe123");

        var sender = _serviceProvider.GetRequiredService<ISender>();
        var userManager = _serviceProvider.GetRequiredService<IUserManager>();


        await sender.Send(testUser);

        var user = userManager.GetUserByUserNameAsync("TEST3");
        user.Should().NotBeNull("User should exist in DB before login.");
        
        var tokenQuery = new UserPasswordLoginQuery("TEST3", "qwe123");
        var tokenQueryResult = await sender.Send(tokenQuery);

        tokenQueryResult.Result.Should().NotBeNull();
        tokenQueryResult.Result!.AccessToken.Should().NotBeEmpty();
    }
}