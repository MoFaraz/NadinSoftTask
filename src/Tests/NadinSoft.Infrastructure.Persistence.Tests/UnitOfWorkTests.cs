using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.Domain.Entities.Product;
using NadinSoft.Infrastructure.Persistence.Repositories.Common;

namespace NadinSoft.Infrastructure.Persistence.Tests;

public class UnitOfWorkTests : IClassFixture<PersistenceTestSetup>
{
    private readonly UnitOfWork _unitOfWork;

    
    public UnitOfWorkTests(PersistenceTestSetup setup)
    {
        _unitOfWork = setup.UnitOfWork;
    }

    [Fact]
    public async Task Adding_New_Product_Should_Save_To_Database()
    {
        var product = ProductEntity.Create("product", "09332426728", "mopharaz@gmail.com", DateTime.Now,
            Guid.NewGuid());
        
        await _unitOfWork.ProductRepository.CreateAsync(product);

        await _unitOfWork.CommitAsync();
        
        var productInDb = await _unitOfWork.ProductRepository.GetByNameAsync("product");
        
        productInDb.Should().NotBeNull();
    }

    // [Fact]
    // public async Task Getting_Product_With_Id_Should_Return_Product()
    // {
    //     var product = await _unitOfWork.ProductRepository.Get
    // }
}