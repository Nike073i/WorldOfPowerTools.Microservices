using System.Net.Http.Headers;
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
        public async Task<Product> GetById(Guid productId, string token)
        {
            var url = _productServiceUrl + "/product/" + productId.ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.SendAsync(request);
            var jsonProduct = await response.Content.ReadAsStringAsync();
            return new Product();
        }
    }
}
