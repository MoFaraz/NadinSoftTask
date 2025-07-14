using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.MappingConfiguration;
using NadinSoft.Application.Extensions;
using NadinSoft.Application.Features.Common;
using NadinSoft.Application.Features.Product.Commands;
using NadinSoft.Application.Features.Product.Queries;
using NadinSoft.Application.Repositories.Common;
using NadinSoft.Application.Repositories.ProductRepository;
using NadinSoft.Application.Test.Extensions;
using NadinSoft.Domain.Entities.Product;
using NadinSoft.Domain.Entities.User;

namespace NadinSoft.Application.Test;

public class ProductFeaturesTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITestOutputHelper _outputHelper;

    public ProductFeaturesTests(ITestOutputHelper outputHelper)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.RegisterApplicationValidator().AddApplicationAutoMapper();
        _serviceProvider = serviceCollection.BuildServiceProvider();
        _outputHelper = outputHelper;
    }


    [Fact]
    public async Task Add_Product_With_Valid_Parameters_Should_Succeed()
    {
        // Arrange
        var faker = new Faker();
        var product = new CreateProductCommand("name", faker.Person.Phone, faker.Person.Email,
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
        var product = new CreateProductCommand("name", faker.Person.Phone, faker.Person.Email,
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
        var getProductResult =
            await validationBehavior.Handle(product, CancellationToken.None, getProductByNameQueryHandler.Handle);

        // Assert
        getProductResult.IsSuccess.Should().BeFalse();
        _outputHelper.WriteLineOperationResultErrors(getProductResult);
    }

    [Fact]
    public async Task Editing_A_Product_With_Valid_Parameters_Should_Be_Success()
    {
        var faker = new Faker();
        var mockId = Guid.NewGuid();
        var mockUserId = Guid.NewGuid();
        var productEntityMock = ProductEntity.Create(mockId, "name", faker.Person.Phone, faker.Person.Email,
            DateTime.Now, mockUserId);

        var productRepository = Substitute.For<IProductRepository>();

        productRepository.GetProductByIdForUpdateAsync(productEntityMock.Id)!.Returns(
            Task.FromResult(productEntityMock));

        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.ProductRepository.Returns(productRepository);

        var editedPhoneNumber = faker.Phone.PhoneNumber();
        var editedEmail = faker.Person.Email;
        var editProductCommand =
            new EditProductCommand(mockId, "name2", editedPhoneNumber, editedEmail, DateTime.Now, mockUserId);


        var validationBehavior =
            new ValidateRequestBehavior<EditProductCommand, OperationResult<bool>>(
                _serviceProvider
                    .GetRequiredService<IValidator<EditProductCommand>>());

        var editProductCommandHandler = new EditProductCommandHandler(unitOfWork);

        // Act
        var editResult = await validationBehavior.Handle(editProductCommand, CancellationToken.None,
            editProductCommandHandler.Handle);

        // Assert
        editResult.IsSuccess.Should().BeTrue();
        productEntityMock.Name.Should().BeEquivalentTo("name2");
        productEntityMock.ManufacturePhone.Should().BeEquivalentTo(editedPhoneNumber);
        productEntityMock.ManufactureEmail.Should().BeEquivalentTo(editedEmail);
    }

    [Fact]
    public async Task Getting_Product_Detail_With_Valid_Parameters_Should_Be_Success()
    {
        var faker = new Faker();
        var mockUser = new UserEntity(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName,
            faker.Person.Email);
        var productEntityMock = ProductEntity.Create("name", faker.Person.Phone, faker.Person.Email,
            DateTime.Now, mockUser);

        var productRepository = Substitute.For<IProductRepository>();

        productRepository.GetProductDetailByIdAsync(productEntityMock.Id)!.Returns(Task.FromResult(productEntityMock));

        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.ProductRepository.Returns(productRepository);

        var getProductDetailQuery =
            new GetProductDetailByIdQuery(productEntityMock.Id);


        var validationBehavior =
            new ValidateRequestBehavior<GetProductDetailByIdQuery, OperationResult<GetProductDetailByIdQueryResult>>(
                _serviceProvider
                    .GetRequiredService<IValidator<GetProductDetailByIdQuery>>());

        var mapper = _serviceProvider.GetRequiredService<IMapper>();
        var getProductDetailHandler = new GetProductDetailByIdHandler(unitOfWork, mapper);

        // Act
        var result =
            await Helpers.ValidateAndExecuteAsync(getProductDetailQuery, getProductDetailHandler, _serviceProvider);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Result!.OwnerUsername.Should().BeEquivalentTo(mockUser.UserName);
    }

    [Fact]
    public async Task Changing_Product_Availability_With_Valid_Parameters_Should_Be_Success()
    {
        var faker = new Faker();
        var mockId = Guid.NewGuid();
        var productEntityMock = ProductEntity.Create(mockId, "name", faker.Person.Phone, faker.Person.Email,
            DateTime.Now, Guid.NewGuid());

        var productRepository = Substitute.For<IProductRepository>();

        productRepository.GetProductByIdForUpdateAsync(productEntityMock.Id)!.Returns(
            Task.FromResult(productEntityMock));

        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.ProductRepository.Returns(productRepository);

        var changeCommand =
            new ChangeProductAvailabilityCommand(mockId, false);

        var changeHandler = new ChangeProductAvailabilityCommandHandler(unitOfWork);

        // Act
        var changeResult = await Helpers.ValidateAndExecuteAsync(changeCommand, changeHandler, _serviceProvider);

        // Assert
        changeResult.IsSuccess.Should().BeTrue();
        productEntityMock.IsAvailable.Should().BeFalse();
    }

    [Fact]
    public async Task Getting_User_Products_With_Valid_Parameters_Should_Be_Success()
    {
        // Arrange
        var faker = new Faker();
        var userProductQuery = new GetProductByNameQuery("name");
        var mockUserId = Guid.NewGuid();
        var fakeProduct1 = ProductEntity.Create(Guid.NewGuid(), "name1", faker.Person.Phone, faker.Person.Email,
            DateTime.Now,
            mockUserId);
        var fakeProduct2 = ProductEntity.Create(Guid.NewGuid(), "name2", faker.Person.Phone, faker.Person.Email,
            DateTime.Now,
            mockUserId);
        List<ProductEntity> fakeProducts = [fakeProduct1, fakeProduct2];

        var productRepository = Substitute.For<IProductRepository>();

        productRepository.GetByNameAsync(userProductQuery.SearchTerm).Returns(Task.FromResult(fakeProducts));

        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.ProductRepository.Returns(productRepository);

        var userProductHandler = new GetProductByNameQueryHandler(unitOfWork);

        // Act
        var getProductResult =
            await Helpers.ValidateAndExecuteAsync(userProductQuery, userProductHandler, _serviceProvider);

        // Assert
        getProductResult.Result.Should().NotBeEmpty();
        getProductResult.Result.Count.Should().Be(fakeProducts.Count);
    }
}