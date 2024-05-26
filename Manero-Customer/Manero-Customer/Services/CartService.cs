using Manero_Customer.Data.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Manero_Customer.Services
{
          
    public class CartService(HttpClient httpClient, IConfiguration configuration)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _configuration = configuration;

        public async Task<Cart?> GetCartList(string id)
        {
            try
            {
                var url = $"https://maneroproductsfunction.azurewebsites.net/api/GetCustomerCart/{id}?code=m7ibFqN_Rsi3NTYCkJetWZ90JJYiDk7lLkeHufoSZH3CAzFugGfWTQ%3D%3D";
                var result = await _httpClient.GetFromJsonAsync<Cart>(url);
                return result ?? null;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public string GetGuid()
        {
            var GuidId = Guid.NewGuid().ToString();
            return GuidId;
        }

        public async Task<Cart> CreateCustomerCart(string userCartID)
        {

            var url = "https://maneroproductsfunction.azurewebsites.net/api/CreateUserCart?code=mYHQQl3mKEjURud_TG8moekTucux1jMqY4WG0kisweEAAzFuCTG6PA%3D%3D";
            var payload = new { id = userCartID };
            var response = await _httpClient.PostAsJsonAsync(url, payload);
            if (response.IsSuccessStatusCode)
            {
                var cart = await response.Content.ReadFromJsonAsync<Cart>();
                return cart!;
            }
            return null!;
            
        }

        public async Task <Cart> AddToCart(Product prod)
        {
            try
            {
                var userCart = new Cart();
                var test3 = "4f063da4-b64a-4478-ae76-3159858eb86c";
                var prodList = await GetCartList(test3);
                if (prodList == null)
                {
                    userCart = await CreateCustomerCart(GetGuid());
                }

                var userId = prodList!.Id ?? userCart.Id;
                var payLoad = new Product
                {
                    ProductName = prod.ProductName,
                    ProductId = prod.ProductId,
                    Quantity = prod.Quantity,
                    Price = prod.Price,
                };
                var url = $"https://maneroproductsfunction.azurewebsites.net/api/AddProdToCart/{userId}?code=hPdhi5Dyr3U8Il5Lh9c6QSNfTG_8AlPeedbYRVUbV4joAzFuWsspUg%3D%3D";
                var response = await _httpClient.PostAsJsonAsync(url, payLoad);
                if (response.IsSuccessStatusCode)
                {

                    var upDatedCart = await response.Content.ReadFromJsonAsync<Cart>();
                    return upDatedCart!;
                }
            }
            catch (Exception ex)
            {
                {
                    Console.WriteLine(ex.Message);
                    return null!;
                }
            }
            return null!;

        }

        public async Task <Cart> DeqraeasproductQuantity(string cartId, string productId)
        {
            var url = $"https://maneroproductsfunction.azurewebsites.net/api/DecreaseCart/{cartId}/product/{productId}?code=pic7fOZjFv_uFAioumE36W0LIjUd_IQll6EsMh30e_bQAzFufO9jTQ%3D%3D";
            var response = await _httpClient.PostAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                var updatedCart = await response.Content.ReadFromJsonAsync<Cart>();
                return updatedCart!;
            }
            return null!;
        }
    }
}
