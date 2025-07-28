using System.Globalization;
using FluentAssertions;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.Application.Contracts.User;
using NadinSoft.Application.Features.Product.Commands;
using NadinSoft.Application.Features.User.Commands.Register;
using NadinSoft.Application.Test.Extensions;
using NadinSoft.Domain.Entities.Product;
using NadinSoft.Infrastructure.Persistence.Repositories.Common;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NadinSoft.Infrastructure.Persistence.Tests;

public class UnitOfWorkTests : IClassFixture<PersistenceTestSetup>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITestOutputHelper _outputHelper;


    public UnitOfWorkTests(PersistenceTestSetup setup, ITestOutputHelper outputHelper)
    {
        _unitOfWork = setup.UnitOfWork;
        _serviceProvider = setup.ServiceProvider;
        _outputHelper = outputHelper;
    }

    [Fact]
    public async Task Adding_New_Product_Should_Save_To_Database()
    {
        var sender = _serviceProvider.GetRequiredService<ISender>();
        var testUser = new RegisterUserCommand("test", "test", "test", "wsxdr123WS!@", "test@test.com", "wsxdr123WS!@");
        await sender.Send(testUser);

        var manager = _serviceProvider.GetRequiredService<IUserManager>();
        var user = await manager.GetUserByUserNameAsync("test", CancellationToken.None);


        var product = ProductEntity.Create("product", "09332426728", "mopharaz@gmail.com", DateTime.Now,
            user!.Id);

        await _unitOfWork.ProductRepository.CreateAsync(product);

        await _unitOfWork.CommitAsync();

        var productInDb = await _unitOfWork.ProductRepository.GetByNameAsync("product");

        productInDb.Should().NotBeNull();
    }

    [Fact]
    public async Task Getting_User_Product_With_Id_Should_Return_Product()
    {
        var sender = _serviceProvider.GetRequiredService<ISender>();
        var testUser = new RegisterUserCommand("test", "test", "test", "wsxdr123WS!@", "test@test.com", "wsxdr123WS!@");
        await sender.Send(testUser);

        var manager = _serviceProvider.GetRequiredService<IUserManager>();
        var user = await manager.GetUserByUserNameAsync("test", CancellationToken.None);

        var product = ProductEntity.Create("product", "09332426728", "mopharaz@gmail.com", DateTime.Now,
            user!.Id);

        await _unitOfWork.ProductRepository.CreateAsync(product);

        await _unitOfWork.CommitAsync();
        var products =
            await _unitOfWork.ProductRepository.GetUserProductsAsync(user!.Id, 1, 10, CancellationToken.None);

        products.Should().NotBeNull();
    }

    [Fact]
    public async Task Editing_Product_With_Valid_User_Id_Should_Success()
    {
        var sender = _serviceProvider.GetRequiredService<ISender>();
        var testUser = new RegisterUserCommand("test", "test", "test", "wsxdr123WS!@", "test@test.com", "wsxdr123WS!@");
        await sender.Send(testUser);

        var manager = _serviceProvider.GetRequiredService<IUserManager>();
        var user = await manager.GetUserByUserNameAsync("test", CancellationToken.None);

        var product = ProductEntity.Create("product", "09332426728", "mopharaz@gmail.com", DateTime.Now,
            user!.Id);

        await _unitOfWork.ProductRepository.CreateAsync(product);

        await _unitOfWork.CommitAsync();
        var productInDb = await _unitOfWork.ProductRepository.GetByNameAsync("product");
        var myProduct = productInDb.Items[0];

        var editCommand = new EditProductCommand(myProduct.Id, "test2", myProduct.ManufacturePhone,
            myProduct.ManufactureEmail, myProduct.ProduceDate, myProduct.UserId);

        var editResult = await sender.Send(editCommand);
        await _unitOfWork.CommitAsync();

        var productInDb2 = await _unitOfWork.ProductRepository.GetByNameAsync("test2");
        editResult.IsSuccess.Should().BeTrue();
        productInDb2.TotalCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Editing_Product_With_InValid_User_Id_Should_Fail()
    {
        var sender = _serviceProvider.GetRequiredService<ISender>();
        var testUser = new RegisterUserCommand("test", "test", "test", "wsxdr123WS!@", "test@test.com", "wsxdr123WS!@");
        await sender.Send(testUser);

        var manager = _serviceProvider.GetRequiredService<IUserManager>();
        var user = await manager.GetUserByUserNameAsync("test", CancellationToken.None);

        var product = ProductEntity.Create("product", "09332426728", "mopharaz@gmail.com", DateTime.Now,
            user!.Id);

        await _unitOfWork.ProductRepository.CreateAsync(product);

        await _unitOfWork.CommitAsync();
        var productInDb = await _unitOfWork.ProductRepository.GetByNameAsync("product");
        var myProduct = productInDb.Items[0];

        var editCommand = new EditProductCommand(myProduct.Id, "test2", myProduct.ManufacturePhone,
            myProduct.ManufactureEmail, myProduct.ProduceDate, Guid.NewGuid());

        var editResult = await sender.Send(editCommand);
        await _unitOfWork.CommitAsync();

        editResult.IsSuccess.Should().BeFalse();
        _outputHelper.WriteLineOperationResultErrors(editResult);
    }


    [Fact]
    public async Task Adding_Product_Should_Set_Created_Date()
    {
        var sender = _serviceProvider.GetRequiredService<ISender>();
        var testUser = new RegisterUserCommand("test", "test", "test", "wsxdr123WS!@", "test@test.com", "wsxdr123WS!@");
        await sender.Send(testUser);

        var manager = _serviceProvider.GetRequiredService<IUserManager>();
        var user = await manager.GetUserByUserNameAsync("test", CancellationToken.None);


        var product = ProductEntity.Create("product", "09332426728", "mopharaz@gmail.com", DateTime.Now,
            user!.Id);
        var context = _serviceProvider.GetRequiredService<NadinSoftDbContext>();
        context.Add(product);
        await context.SaveChangesAsync();
        var entry = context.Entry(product);
        var createdDate = entry.Property<DateTime>("CreatedDate").CurrentValue;
        _outputHelper.WriteLine(createdDate.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public async Task Editing_Product_Should_Set_Modified_Date()
    {
        var sender = _serviceProvider.GetRequiredService<ISender>();
        var testUser = new RegisterUserCommand("test", "test", "test", "wsxdr123WS!@", "test@test.com", "wsxdr123WS!@");
        await sender.Send(testUser);

        var manager = _serviceProvider.GetRequiredService<IUserManager>();
        var user = await manager.GetUserByUserNameAsync("test", CancellationToken.None);


        var product = ProductEntity.Create("product", "09332426728", "mopharaz@gmail.com", DateTime.Now,
            user!.Id);
        var context = _serviceProvider.GetRequiredService<NadinSoftDbContext>();
        context.Add(product);

        await context.SaveChangesAsync();
        var entry = context.Entry(product);

        var editCommand =
            new EditProductCommand(entry.Entity.Id, "name", "09332426728", "mopharaz@gmail.com", DateTime.Now, user.Id);

        await sender.Send(editCommand);

        await _unitOfWork.CommitAsync();

        var modifiedEntry = context.Entry(product);
        var modifiedDate = modifiedEntry.Property<DateTime>("ModifiedDate").CurrentValue;

        _outputHelper.WriteLine(modifiedDate.ToString(CultureInfo.InvariantCulture));
    }
}