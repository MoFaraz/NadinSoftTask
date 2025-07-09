using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.Application.Common;
using NadinSoft.Application.Extensions;
using NadinSoft.Application.Features.Common;
using NadinSoft.Application.Features.Product.Commands;
using NadinSoft.Application.Features.Product.Queries;
using NadinSoft.Application.Repositories.Common;
using NadinSoft.Application.Repositories.ProductRepository;
using NadinSoft.Application.Test.Extensions;
using NadinSoft.Domain.Entities.Product;

namespace NadinSoft.Application.Test;

public class ProductFeaturesTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITestOutputHelper _outputHelper;

    public ProductFeaturesTests(ITestOutputHelper outputHelper)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.RegisterApplicationValidator();
        _serviceProvider = serviceCollection.BuildServiceProvider();
        _outputHelper = outputHelper;
    }


    [Fact]
    public async Task Add_Product_With_Valid_Parameters_Should_Succeed()
    {
        // Arrange
        var faker = new Faker();
        var product = new CreateProductCommand(Guid.NewGuid(), "name", faker.Person.Phone, faker.Person.Email,
            DateTime.Now, Guid.NewGuid());

        var productRepository = Substitute.For<IProductRepository>();

        productRepository.IsProductExistsAsync(product.Name).Returns(Task.FromResult(false));

        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.ProductRepository.Returns(productRepository);
        var validationBehavior =
            new ValidateRequestBehavior<CreateProductCommand, OperationResult<bool>>(_serviceProvider
                .GetRequiredService<IValidator<CreateProductCommand>>());

        var createProductHandler = new CreateProductCommandHandler(unitOfWork);

        // Act
        var createProductResult =
            await validationBehavior.Handle(product, CancellationToken.None, createProductHandler.Handle);

        // Assert
        createProductResult.Result.Should().BeTrue();
    }

    [Fact]
    public async Task Exists_Product_Can_Not_Be_Created()
    {
        // Arrange
        var faker = new Faker();
        var product = new CreateProductCommand(Guid.NewGuid(), "name", faker.Person.Phone, faker.Person.Email,
            DateTime.Now, Guid.NewGuid());

        var productRepository = Substitute.For<IProductRepository>();

        productRepository.IsProductExistsAsync(product.Name).Returns(Task.FromResult(true));

        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.ProductRepository.Returns(productRepository);
        var validationBehavior =
            new ValidateRequestBehavior<CreateProductCommand, OperationResult<bool>>(_serviceProvider
                .GetRequiredService<IValidator<CreateProductCommand>>());

        var createProductHandler = new CreateProductCommandHandler(unitOfWork);

        // Act
        var createProductResult =
            await validationBehavior.Handle(product, CancellationToken.None, createProductHandler.Handle);

        // Assert
        createProductResult.Result.Should().BeFalse();
        _outputHelper.WriteLineOperationResultErrors(createProductResult);
    }

    [Fact]
    public async Task Getting_List_Of_Products_Should_Be_Success()
    {
        // Arrange
        var faker = new Faker();
        var product = new GetProductByNameQuery("name");
        var fakeProduct1 = ProductEntity.Create(Guid.NewGuid(), "name1", faker.Person.Phone, faker.Person.Email,
            DateTime.Now,
            Guid.NewGuid());
        var fakeProduct2 = ProductEntity.Create(Guid.NewGuid(), "name2", faker.Person.Phone, faker.Person.Email,
            DateTime.Now,
            Guid.NewGuid());
        List<ProductEntity> fakeProducts = [fakeProduct1, fakeProduct2];

        var productRepository = Substitute.For<IProductRepository>();

        productRepository.GetByNameAsync(product.SearchTerm).Returns(Task.FromResult(fakeProducts));

        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.ProductRepository.Returns(productRepository);
        var validationBehavior =
            new ValidateRequestBehavior<GetProductByNameQuery, OperationResult<List<GetProductByNameQueryResult>>>(
                _serviceProvider
                    .GetRequiredService<IValidator<GetProductByNameQuery>>());

        var getProductByNameQueryHandler = new GetProductByNameQueryHandler(unitOfWork);

        // Act
        var getProductResult =
            await validationBehavior.Handle(product, CancellationToken.None, getProductByNameQueryHandler.Handle);

        // Assert
        getProductResult.Result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Searching_For_Products_Should_Have_At_Least_Three_Characters()
    {
        // Arrange
        var faker = new Faker();
        var product = new GetProductByNameQuery("na");
        var fakeProduct1 = ProductEntity.Create(Guid.NewGuid(), "name1", faker.Person.Phone, faker.Person.Email,
            DateTime.Now,
            Guid.NewGuid());
        var fakeProduct2 = ProductEntity.Create(Guid.NewGuid(), "name2", faker.Person.Phone, faker.Person.Email,
            DateTime.Now,
            Guid.NewGuid());
        List<ProductEntity> fakeProducts = [fakeProduct1, fakeProduct2];

        var productRepository = Substitute.For<IProductRepository>();

        productRepository.GetByNameAsync(product.SearchTerm).Returns(Task.FromResult(fakeProducts));

        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.ProductRepository.Returns(productRepository);
        var validationBehavior =
            new ValidateRequestBehavior<GetProductByNameQuery, OperationResult<List<GetProductByNameQueryResult>>>(
                _serviceProvider
                    .GetRequiredService<IValidator<GetProductByNameQuery>>());

        var getProductByNameQueryHandler = new GetProductByNameQueryHandler(unitOfWork);

        // Act
        var getProductResult=
            await validationBehavior.Handle(product, CancellationToken.None, getProductByNameQueryHandler.Handle);

        // Assert
        getProductResult.IsSuccess.Should().BeFalse();
        _outputHelper.WriteLineOperationResultErrors(getProductResult);
    }
}