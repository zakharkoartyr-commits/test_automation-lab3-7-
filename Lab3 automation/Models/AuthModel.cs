using Newtonsoft.Json;

namespace Lab3_automation.Models
{
    public class AuthRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}