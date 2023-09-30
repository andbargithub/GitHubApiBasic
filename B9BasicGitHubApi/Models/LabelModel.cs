using Newtonsoft.Json;

namespace B9BasicGitHubApi.Models
{
    public class LabelModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
