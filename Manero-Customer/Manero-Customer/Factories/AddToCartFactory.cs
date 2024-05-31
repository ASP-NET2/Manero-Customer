using Manero_Customer.Data.Models;
using Manero_Customer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Manero_Customer.Factories
{
    public class AddToCartFactory(CartService cartService, ProductService productService)
    {
        private readonly CartService cartService = cartService;
        private readonly ProductService productService = productService;

        public async Task<CartItemModel> SingleProduct (Product prod, string userId)
        {
            var prodDict = new Dictionary<string, string>
            {
                { "id", prod.ProductId }
            };
            var productItem = await productService.FilterSingleProduct(prodDict);
            if (productItem != null) {
                var cartItem = new CartItemModel
                {
                    Price = productItem.Price,
                    DiscountPrice = int.TryParse(productItem.DiscountPrice, out int discountPrice) ? discountPrice : 0,
                    ImgUrl = productItem.ImageUrl,
                    Id = productItem.Id,
                    Title = productItem.Title,
                    Format = productItem.FormatName,
                    Quantity = prod.Quantity,
                    UserId = userId
                };
                return cartItem;
            }
            return null;
        }
    }
}
