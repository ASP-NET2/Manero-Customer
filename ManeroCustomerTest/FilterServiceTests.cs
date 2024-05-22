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
                new ProductModel { CategoryName = "Cat1" },
                new ProductModel { CategoryName = "Cat2" },
                new ProductModel { CategoryName = "Cat1" }, // Duplicate
                new ProductModel { CategoryName = "Cat3" }
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
                new ProductModel { CategoryName = "Cat1" },
                new ProductModel { CategoryName = "Cat2" },
                new ProductModel { CategoryName = "Cat3" }
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
                new ProductModel { CategoryName = "Cat1" },
                new ProductModel { CategoryName = null },   // Null Category
                new ProductModel { CategoryName = "" },     // Empty Category
                new ProductModel { CategoryName = "Cat2" },
                new ProductModel { CategoryName = "Cat3" }
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
                new ProductModel { SubCategoryName = "SubCat1" },
                new ProductModel { SubCategoryName = "SubCat2" },
                new ProductModel { SubCategoryName = "SubCat1" }, // Duplicate
                new ProductModel { SubCategoryName = "SubCat3" }
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
                new ProductModel { SubCategoryName = "SubCat1" },
                new ProductModel { SubCategoryName = "SubCat2" },
                new ProductModel { SubCategoryName = "SubCat3" }
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
                new ProductModel { SubCategoryName = "SubCat1" },
                new ProductModel { SubCategoryName = null },   // Null SubCategory
                new ProductModel { SubCategoryName = "" },     // Empty SubCategory
                new ProductModel { SubCategoryName = "SubCat2" },
                new ProductModel { SubCategoryName = "SubCat3" }
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
