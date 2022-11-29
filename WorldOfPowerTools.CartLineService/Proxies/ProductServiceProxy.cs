using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using WorldOfPowerTools.CartService.Models;

namespace WorldOfPowerTools.CartService.Proxies
{
    public class ProductServiceProxy : IProductServiceProxy
    {
        private readonly string _productServiceUrl;
        public ProductServiceProxy(IConfiguration configuration)
        {
            _productServiceUrl = configuration["ProductService:URL"];
        }
        public async Task<Product?> GetById(Guid productId, string token)
        {
            var tokenScheme = "Bearer";
            if (token.StartsWith(tokenScheme)) token = token.Substring(tokenScheme.Length + 1);
            var url = _productServiceUrl + "/Product/" + productId.ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue(tokenScheme, token);
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;
            var jsonProduct = await response.Content.ReadAsStringAsync();
            return JsonToProduct(jsonProduct);
        }

        private static Product? JsonToProduct(string json)
        {
            var options = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
            var product = JsonSerializer.Deserialize<Product>(json, options);
            return product;
        }
    }
}
