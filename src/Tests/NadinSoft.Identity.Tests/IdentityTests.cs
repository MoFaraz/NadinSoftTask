using FluentAssertions;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.Application.Contracts.User;
using NadinSoft.Application.Features.User.Commands.Register;
using NadinSoft.Application.Features.User.Queries.PasswordLogin;

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
        var testUser = new RegisterUserCommand("test", "test", "test", "qwe123!@#", "test@test.com", "qwe123!@#");
        var sender = _serviceProvider.GetRequiredService<ISender>();
        await sender.Send(testUser);

        var testUserQuery = new UserPasswordLoginQuery(testUser.Username, testUser.Password);
        var result = await sender.Send(testUserQuery);

        result.Result!.AccessToken.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Registered_User_Should_Exist_In_Database()
    {
        var testUser = new RegisterUserCommand("test", "test", "TEST3", "qwe123", "test3@test.com", "qwe123");

        var sender = _serviceProvider.GetRequiredService<ISender>();
        var userManager = _serviceProvider.GetRequiredService<IUserManager>();


        await sender.Send(testUser);

        var user = userManager.GetUserByUserNameAsync("TEST3");
        user.Should().NotBeNull("User should exist in DB before login.");
    }
}