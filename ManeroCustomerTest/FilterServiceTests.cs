using System.Collections.Generic;
using Manero_Customer.Data.Models;
using Manero_Customer.Services;
using Xunit;

namespace ManeroCustomerTest
{
    public class FilterServiceTests
    {
        [Fact]
        public void FilterCategory_ReturnsUniqueCategories()
        {
            // Arrange
            var models = new List<ProductModel>
            {
                new ProductModel { Category = "Cat1" },
                new ProductModel { Category = "Cat2" },
                new ProductModel { Category = "Cat1" }, // Duplicate
                new ProductModel { Category = "Cat3" }
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
            var models = new List<ProductModel>();
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
            var models = new List<ProductModel>
            {
                new ProductModel { Category = "Cat1" },
                new ProductModel { Category = "Cat2" },
                new ProductModel { Category = "Cat3" }
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
            var models = new List<ProductModel>
            {
                new ProductModel { Category = "Cat1" },
                new ProductModel { Category = null },   // Null Category
                new ProductModel { Category = "" },     // Empty Category
                new ProductModel { Category = "Cat2" },
                new ProductModel { Category = "Cat3" }
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
        public void FilterCategory_ReturnsEmptyList_WhenModelsIsNull()
        {
            // Arrange
            List<ProductModel> models = null;
            var service = new FilterService();

            // Act
            var result = service.FilterCategory(models);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void FilterSubCategory_ReturnsUniqueSubCategories()
        {
            // Arrange
            var models = new List<ProductModel>
            {
                new ProductModel { SubCategory = "SubCat1" },
                new ProductModel { SubCategory = "SubCat2" },
                new ProductModel { SubCategory = "SubCat1" }, // Duplicate
                new ProductModel { SubCategory = "SubCat3" }
            };
            var service = new FilterService();
            var expected = new List<string> { "SubCat1", "SubCat2", "SubCat3" };

            // Act
            var result = service.FilterSubCategory(models);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Count, result.Count);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FilterSubCategory_ReturnsEmptyList_WhenNoModelsProvided()
        {
            // Arrange
            var models = new List<ProductModel>();
            var service = new FilterService();

            // Act
            var result = service.FilterSubCategory(models);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void FilterSubCategory_ReturnsUniqueSubCategories_WhenNoDuplicates()
        {
            // Arrange
            var models = new List<ProductModel>
            {
                new ProductModel { SubCategory = "SubCat1" },
                new ProductModel { SubCategory = "SubCat2" },
                new ProductModel { SubCategory = "SubCat3" }
            };
            var service = new FilterService();
            var expected = new List<string> { "SubCat1", "SubCat2", "SubCat3" };

            // Act
            var result = service.FilterSubCategory(models);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Count, result.Count);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FilterSubCategory_IgnoresNullOrEmptySubCategories()
        {
            // Arrange
            var models = new List<ProductModel>
            {
                new ProductModel { SubCategory = "SubCat1" },
                new ProductModel { SubCategory = null },   // Null SubCategory
                new ProductModel { SubCategory = "" },     // Empty SubCategory
                new ProductModel { SubCategory = "SubCat2" },
                new ProductModel { SubCategory = "SubCat3" }
            };
            var service = new FilterService();
            var expected = new List<string> { "SubCat1", "SubCat2", "SubCat3" };

            // Act
            var result = service.FilterSubCategory(models);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Count, result.Count);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FilterSubCategory_ReturnsEmptyList_WhenModelsIsNull()
        {
            // Arrange
            List<ProductModel> models = null;
            var service = new FilterService();

            // Act
            var result = service.FilterSubCategory(models);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
