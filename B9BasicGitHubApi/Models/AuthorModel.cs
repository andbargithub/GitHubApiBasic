using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace B9BasicGitHubApi.Models
{
    public class AuthorModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
        
    }
}
