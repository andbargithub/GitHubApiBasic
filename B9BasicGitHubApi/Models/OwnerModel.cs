using System.ComponentModel;
using Newtonsoft.Json;

namespace B9BasicGitHubApi.Models
{
    public class OwnerModel
    {
        [JsonProperty("login")]        
        public string? Name { get; set; }
        [JsonProperty("avatar_url")]        
        public string? AvatarUrl { get; set; }
    }
}
