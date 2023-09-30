using Newtonsoft.Json;

namespace B9BasicGitHubApi.Models
{
    public class HeadModel
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("ref")]
        public string Ref { get; set; }
        [JsonProperty("repo")]
        public RepositoryModel Repository { get; set; }
    }
}
