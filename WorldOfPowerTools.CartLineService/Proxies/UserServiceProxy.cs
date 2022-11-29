using System.Net.Http.Headers;
using System.Text.Json;
using WorldOfPowerTools.CartService.Models;

namespace WorldOfPowerTools.CartService.Proxies
{
    public class UserServiceProxy : IUserServiceProxy
    {
        private readonly string _userServiceUrl;
        public UserServiceProxy(IConfiguration configuration)
        {
            _userServiceUrl = configuration["UserService:URL"];
        }
        public async Task<User?> GetById(Guid userId, string token)
        {
            var tokenScheme = "Bearer";
            if (token.StartsWith(tokenScheme)) token = token.Substring(tokenScheme.Length + 1);
            var url = _userServiceUrl + "/User/" + userId.ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue(tokenScheme, token);
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;
            var jsonUser = await response.Content.ReadAsStringAsync();
            return JsonToUser(jsonUser);
        }

        private static User? JsonToUser(string json)
        {
            var user = JsonSerializer.Deserialize<User>(json);
            return user;
        }
    }
}
