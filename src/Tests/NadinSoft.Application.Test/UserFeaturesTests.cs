using Bogus;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.Application.Common;
using NadinSoft.Application.Contracts.User;
using NadinSoft.Application.Contracts.User.Models;
using NadinSoft.Application.Extensions;
using NadinSoft.Application.Features.Common;
using NadinSoft.Application.Features.User.Commands.Register;
using NadinSoft.Application.Features.User.Queries.PasswordLogin;
using NadinSoft.Application.Test.Extensions;
using NadinSoft.Domain.Entities.User;
using NSubstitute;
using Xunit.Abstractions;

namespace NadinSoft.Application.Test;

public class UserFeaturesTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITestOutputHelper _outputHelper;

    public UserFeaturesTests(ITestOutputHelper outputHelper)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.RegisterApplicationValidator();
        _serviceProvider = serviceCollection.BuildServiceProvider();
        _outputHelper = outputHelper;
    }

    [Fact]
    public async Task Creating_New_User_Should_Be_Success()
    {
        // Arrange
        var faker = new Faker();
        var password = faker.Random.String(10);
        var registerUser = new RegisterUserCommand(
            faker.Person.FirstName
            , faker.Person.LastName
            , faker.Person.UserName
            , password
            , faker.Person.Email
            , password
        );

        var userManager = Substitute.For<IUserManager>();

        userManager
            .PasswordCreateAsync(Arg.Any<UserEntity>(), password)
            .Returns(IdentityResult.Success);

        // Act
        var userRegisterCommandHandler = new RegisterUserCommandHandler(userManager);
        var userRegisterResult = await userRegisterCommandHandler.Handle(registerUser);

        // Assert
        userRegisterResult.IsSuccess.Should().Be(true);
    }

    [Fact]
    public async Task User_Register_Email_Should_Be_Valid()
    {
        // Arrange
        var faker = new Faker();
        var password = Guid.NewGuid().ToString("N");
        var registerUser = new RegisterUserCommand(
            faker.Person.FirstName
            , faker.Person.LastName
            , faker.Person.UserName
            , password
            , string.Empty
            , password
        );

        var userManager = Substitute.For<IUserManager>();

        userManager
            .PasswordCreateAsync(Arg.Any<UserEntity>(), password)
            .Returns(IdentityResult.Success);

        // Act
        var userRegisterCommandHandler = new RegisterUserCommandHandler(userManager);
        var validationBehavior =
            new ValidateRequestBehavior<RegisterUserCommand, OperationResult<bool>>(_serviceProvider
                .GetRequiredService<IValidator<RegisterUserCommand>>());

        var userRegisterResult =
            await validationBehavior.Handle(registerUser, CancellationToken.None, userRegisterCommandHandler.Handle);

        // Assert
        userRegisterResult.IsSuccess.Should().Be(false);
        _outputHelper.WriteLineOperationResultErrors(userRegisterResult);
    }

    [Fact]
    public async Task User_Register_Password_And_Repeat_Password_Should_Be_Same()
    {
        // Arrange
        var faker = new Faker();
        var password = Guid.NewGuid().ToString("N");
        var registerUser = new RegisterUserCommand(
            faker.Person.FirstName
            , faker.Person.LastName
            , faker.Person.UserName
            , password
            , faker.Person.Email
            , Guid.NewGuid().ToString("N")
        );

        var userManager = Substitute.For<IUserManager>();

        userManager
            .PasswordCreateAsync(Arg.Any<UserEntity>(), password)
            .Returns(IdentityResult.Success);

        // Act
        var userRegisterCommandHandler = new RegisterUserCommandHandler(userManager);
        var validationBehavior =
            new ValidateRequestBehavior<RegisterUserCommand, OperationResult<bool>>(_serviceProvider
                .GetRequiredService<IValidator<RegisterUserCommand>>());
        var userRegisterResult =
            await validationBehavior.Handle(registerUser, CancellationToken.None, userRegisterCommandHandler.Handle);

        // Assert
        userRegisterResult.IsSuccess.Should().Be(false);
        _outputHelper.WriteLineOperationResultErrors(userRegisterResult);
    }

    [Fact]
    public async Task Login_User_With_UserName_Should_Be_Success()
    {
        // Arrange
        var faker = new Faker();
        var loginQuery = new UserPasswordLoginQuery(faker.Person.UserName, Guid.NewGuid().ToString("N"));

        var userManager = Substitute.For<IUserManager>();

        var userEntity = new UserEntity(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName,
            faker.Person.Email);

        userManager.GetUserByUserNameAsync(loginQuery.UserNameOrEmail, CancellationToken.None)
            .Returns(userEntity);

        userManager.ValidatePasswordAsync(userEntity, loginQuery.Password, CancellationToken.None)
            .Returns(Task.FromResult(IdentityResult.Success));
        var jwtService = Substitute.For<IJwtService>();

        jwtService.GenerateTokenAsync(userEntity, CancellationToken.None)
            .Returns(Task.FromResult(new JwtAccessTokenModel("AccessToken", 3000)));

        // Act
        var loginQueryHandler = new UserPasswordLoginQueryHandler(userManager, jwtService);

        var loginResult = await loginQueryHandler.Handle(loginQuery, CancellationToken.None);

        // Assert 
        loginResult.Result.Should().NotBeNull();
        loginResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Login_User_With_UserName_And_Wrong_Password_Should_Be_Failure()
    {
        // Arrange
        var faker = new Faker();
        var loginQuery = new UserPasswordLoginQuery(faker.Person.UserName, Guid.NewGuid().ToString("N"));

        var userManager = Substitute.For<IUserManager>();

        var userEntity = new UserEntity(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName,
            faker.Person.Email);

        userManager.GetUserByUserNameAsync(loginQuery.UserNameOrEmail, CancellationToken.None)
            .Returns(userEntity);

        userManager.ValidatePasswordAsync(userEntity, loginQuery.Password, CancellationToken.None)
            .Returns(Task.FromResult(IdentityResult.Failed()));

        var jwtService = Substitute.For<IJwtService>();

        jwtService.GenerateTokenAsync(userEntity, CancellationToken.None)
            .Returns(Task.FromResult(new JwtAccessTokenModel("AccessToken", 3000)));

        // Act
        var loginQueryHandler = new UserPasswordLoginQueryHandler(userManager, jwtService);

        var loginResult = await loginQueryHandler.Handle(loginQuery, CancellationToken.None);

        // Assert 
        loginResult.Result.Should().BeNull();
        loginResult.IsSuccess.Should().BeFalse();

        _outputHelper.WriteLineOperationResultErrors(loginResult);
    }

    [Fact]
    public async Task Password_Login_User_With_Email_Should_Be_Success()
    {
        // Arrange
        var faker = new Faker();
        var loginQuery = new UserPasswordLoginQuery(faker.Person.Email, Guid.NewGuid().ToString("N"));

        var userManager = Substitute.For<IUserManager>();

        var userEntity = new UserEntity(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName,
            faker.Person.Email);

        userManager.GetUserByEmailAsync(loginQuery.UserNameOrEmail, CancellationToken.None)
            .Returns(userEntity);

        userManager.ValidatePasswordAsync(userEntity, loginQuery.Password, CancellationToken.None)
            .Returns(Task.FromResult(IdentityResult.Success));
        var jwtService = Substitute.For<IJwtService>();

        jwtService.GenerateTokenAsync(userEntity, CancellationToken.None)
            .Returns(Task.FromResult(new JwtAccessTokenModel("AccessToken", 3000)));

        // Act
        var loginQueryHandler = new UserPasswordLoginQueryHandler(userManager, jwtService);

        var loginResult = await loginQueryHandler.Handle(loginQuery, CancellationToken.None);

        // Assert 
        loginResult.Result.Should().NotBeNull();
        loginResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Password_Login_User_Not_Found_Should_Be_Success()
    {
        // Arrange
        var faker = new Faker();
        var loginQuery = new UserPasswordLoginQuery(faker.Person.UserName, Guid.NewGuid().ToString("N"));

        var userManager = Substitute.For<IUserManager>();

        var userEntity = new UserEntity(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName,
            faker.Person.Email);

        userManager.GetUserByUserNameAsync(loginQuery.UserNameOrEmail, CancellationToken.None)
            .Returns(Task.FromResult<UserEntity?>(null));

        userManager.ValidatePasswordAsync(userEntity, loginQuery.Password, CancellationToken.None)
            .Returns(Task.FromResult(IdentityResult.Success));
        var jwtService = Substitute.For<IJwtService>();

        jwtService.GenerateTokenAsync(userEntity, CancellationToken.None)
            .Returns(Task.FromResult(new JwtAccessTokenModel("AccessToken", 3000)));

        // Act
        var loginQueryHandler = new UserPasswordLoginQueryHandler(userManager, jwtService);

        var loginResult = await loginQueryHandler.Handle(loginQuery, CancellationToken.None);

        // Assert 
        loginResult.Result.Should().BeNull();
        loginResult.IsNotFound.Should().BeTrue();

        _outputHelper.WriteLineOperationResultErrors(loginResult);
    }

    [Fact]
    public async Task Login_User_Input_Should_Be_Valid()
    {
        // Arrange
        var faker = new Faker();
        var loginQuery = new UserPasswordLoginQuery(faker.Person.UserName, string.Empty);

        var userManager = Substitute.For<IUserManager>();

        var userEntity = new UserEntity(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName,
            faker.Person.Email);

        userManager.GetUserByUserNameAsync(loginQuery.UserNameOrEmail, CancellationToken.None)
            .Returns(Task.FromResult<UserEntity?>(userEntity));

        userManager.ValidatePasswordAsync(userEntity, loginQuery.Password, CancellationToken.None)
            .Returns(Task.FromResult(IdentityResult.Success));
        var jwtService = Substitute.For<IJwtService>();

        jwtService.GenerateTokenAsync(userEntity, CancellationToken.None)
            .Returns(Task.FromResult(new JwtAccessTokenModel("AccessToken", 3000)));

        // Act

        var loginQueryHandler = new UserPasswordLoginQueryHandler(userManager, jwtService);

        var validationBehavior =
            new ValidateRequestBehavior<UserPasswordLoginQuery, OperationResult<JwtAccessTokenModel>>(
                _serviceProvider.GetRequiredService<IValidator<UserPasswordLoginQuery>>());

        var loginResult = await validationBehavior.Handle(loginQuery, CancellationToken.None, loginQueryHandler.Handle);

        // Assert 
        loginResult.Result.Should().BeNull();
        loginResult.IsSuccess.Should().BeFalse();

        _outputHelper.WriteLineOperationResultErrors(loginResult);
    }
}