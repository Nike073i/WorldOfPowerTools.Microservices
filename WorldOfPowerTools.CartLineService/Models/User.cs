using System.Text.Json.Serialization;

namespace WorldOfPowerTools.CartService.Models
{
#nullable disable
    public class User
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("password_hash")]
        public string PasswordHash { get; set; }
    }
}
