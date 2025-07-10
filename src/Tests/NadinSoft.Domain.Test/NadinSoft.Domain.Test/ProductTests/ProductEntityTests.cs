using FluentAssertions;
using NadinSoft.Domain.Entities.Product;

namespace NadinSoft.Domain.Test.ProductTests;

public class ProductEntityTests
{
    [Fact]
    public void Creating_Product_With_Empty_Id_Should_Throw_Exception()
    {
        // Arrange
        var id = Guid.Empty;
        var name = "test product";
        var manufacturePhone = "+989332426728";
        var manufactureEmail = "test@test.com";
        var produceDate = DateTime.Now;
        var userId = Guid.NewGuid();

        // Act
        Action act = () => ProductEntity.Create(id, name, manufacturePhone, manufactureEmail, produceDate, userId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void Creating_Product_With_Empty_User_Should_Throw_Exception()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "test product";
        var manufacturePhone = "+989332426728";
        var manufactureEmail = "test@test.com";
        var produceDate = DateTime.Now;
        Guid userId = Guid.Empty;

        // Act
        Action act = () => ProductEntity.Create(id, name, manufacturePhone, manufactureEmail, produceDate, userId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Creating_Product_With_Empty_Name_Should_Throw_Exception()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "";
        var manufacturePhone = "+989332426728";
        var manufactureEmail = "test@test.com";
        var produceDate = DateTime.Now;
        Guid userId = Guid.NewGuid();

        // Act
        Action act = () => ProductEntity.Create(id, name, manufacturePhone, manufactureEmail, produceDate, userId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void Creating_Product_With_Null_Name_Should_Throw_Exception()
    {
        // Arrange
        var id = Guid.NewGuid();
        string? name = null;
        var manufacturePhone = "+989332426728";
        var manufactureEmail = "test@test.com";
        var produceDate = DateTime.Now;
        Guid userId = Guid.NewGuid();

        // Act
        Action act = () => ProductEntity.Create(id, name, manufacturePhone, manufactureEmail, produceDate, userId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }


    [Fact]
    public void Creating_Product_With_Empty_ManufacturePhone_Should_Throw_Exception()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "test product";
        var manufacturePhone = "";
        var manufactureEmail = "test@test.com";
        var produceDate = DateTime.Now;
        Guid userId = Guid.NewGuid();

        // Act
        Action act = () => ProductEntity.Create(id, name, manufacturePhone, manufactureEmail, produceDate, userId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void Creating_Product_With_Null_ManufacturePhone_Should_Throw_Exception()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "test product";
        string? manufacturePhone = null;
        var manufactureEmail = "test@test.com";
        var produceDate = DateTime.Now;
        Guid userId = Guid.NewGuid();

        // Act
        Action act = () => ProductEntity.Create(id, name, manufacturePhone, manufactureEmail, produceDate, userId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void Creating_Product_With_Null_ManufactureEmail_Should_Throw_Exception()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "test product";
        var manufacturePhone = "+989332426728";
        string? manufactureEmail = null;
        var produceDate = DateTime.Now;
        Guid userId = Guid.NewGuid();

        // Act
        Action act = () => ProductEntity.Create(id, name, manufacturePhone, manufactureEmail, produceDate, userId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void Creating_Product_With_Empty_ManufactureEmail_Should_Throw_Exception()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "test product";
        var manufacturePhone = "+989332426728";
        var manufactureEmail = "";
        var produceDate = DateTime.Now;
        Guid userId = Guid.NewGuid();

        // Act
        Action act = () => ProductEntity.Create(id, name, manufacturePhone, manufactureEmail, produceDate, userId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Two_Product_With_Same_Id_Should_Be_Equal()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "test product";
        var manufacturePhone = "+989332426728";
        var manufactureEmail = "test@test.com";
        var produceDate = DateTime.Now;
        Guid userId = Guid.NewGuid();

        // Act
        var product1 = ProductEntity.Create(id, name, manufacturePhone, manufactureEmail, produceDate, userId);
        var product2 = ProductEntity.Create(id, name, manufacturePhone, manufactureEmail, produceDate, userId);

        // Assert
        product1.Equals(product2).Should().BeTrue();
    }
    
    [Fact]
    public void Creating_Product_Should_Generate_Slug_Automatically()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        var name = "test product";
        var manufacturePhone = "+989332426728";
        var manufactureEmail = "test@test.com";
        var produceDate = DateTime.Parse("2025/07/08");
        Guid userId = Guid.NewGuid();

        // Act
        var product =  ProductEntity.Create(id, name, manufacturePhone, manufactureEmail, produceDate, userId);

        // Assert
        product.Slug.Equals("test-product-2025-07-08").Should().Be(true);
    }
}