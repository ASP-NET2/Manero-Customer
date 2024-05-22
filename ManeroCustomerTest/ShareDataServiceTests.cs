using Manero_Customer.Data.Models;
using Manero_Customer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManeroCustomerTest
{
    public class ShareDataServiceTests
    {
        [Fact]
        public void SetCategories_ShouldStoreCategories()
        {
            // Arrange
            var service = new SharesDataService();
            var categories = new List<CategoryModel>
        {
            new CategoryModel { CategoryName = "Category1", ImageLink = "Link1" },
            new CategoryModel { CategoryName = "Category2", ImageLink = "Link2" }
        };

            // Act
            service.SetCategorie(categories);

            // Assert
            var result = service.GetCategorie();
            Assert.Equal(2, result.Count);
            Assert.Equal("Category1", result[0].CategoryName);
            Assert.Equal("Link1", result[0].ImageLink);
            Assert.Equal("Category2", result[1].CategoryName);
            Assert.Equal("Link2", result[1].ImageLink);
        }

        [Fact]
        public void GetCategories_ShouldReturnEmptyList_IfNotSet()
        {
            // Arrange
            var service = new SharesDataService();

            // Act
            var result = service.GetCategorie();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
