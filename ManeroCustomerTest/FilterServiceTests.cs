using System.Collections.Generic;
using Manero_Customer.Data.Models;
using Manero_Customer.Services;
using Xunit;

public class FilterServiceTests
{
    [Fact]
    public void FilterCategory_ReturnsUniqueCategories()
    {
        // Arrange
        var models = new List<ProductCategoryModel>
        {
            new ProductCategoryModel { Category = "Cat1" },
            new ProductCategoryModel { Category = "Cat2" },
            new ProductCategoryModel { Category = "Cat1" }, // Duplicate
            new ProductCategoryModel { Category = "Cat3" }
        };
        var service = new FilterService();
        var expected = new List<string> { "Cat1", "Cat2", "Cat3" };

        // Act
        var result = service.FilterCategory(models);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Count, result.Count);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FilterCategory_ReturnsEmptyList_WhenNoModelsProvided()
    {
        // Arrange
        var models = new List<ProductCategoryModel>();
        var service = new FilterService();

        // Act
        var result = service.FilterCategory(models);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void FilterCategory_ReturnsUniqueCategories_WhenNoDuplicates()
    {
        // Arrange
        var models = new List<ProductCategoryModel>
        {
            new ProductCategoryModel { Category = "Cat1" },
            new ProductCategoryModel { Category = "Cat2" },
            new ProductCategoryModel { Category = "Cat3" }
        };
        var service = new FilterService();
        var expected = new List<string> { "Cat1", "Cat2", "Cat3" };

        // Act
        var result = service.FilterCategory(models);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Count, result.Count);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FilterCategory_IgnoresNullOrEmptyCategories()
    {
        // Arrange
        var models = new List<ProductCategoryModel>
        {
            new ProductCategoryModel { Category = "Cat1" },
            new ProductCategoryModel { Category = null },   // Null Category
            new ProductCategoryModel { Category = "" },     // Empty Category
            new ProductCategoryModel { Category = "Cat2" },
            new ProductCategoryModel { Category = "Cat3" }
        };
        var service = new FilterService();
        var expected = new List<string> { "Cat1", "Cat2", "Cat3" };

        // Act
        var result = service.FilterCategory(models);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Count, result.Count);
        Assert.Equal(expected, result);
    }
}
