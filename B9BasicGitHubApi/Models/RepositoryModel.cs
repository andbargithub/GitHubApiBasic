

using Newtonsoft.Json;

namespace B9BasicGitHubApi.Models
{
    public class RepositoryModel
    {        

        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("owner")]
        public OwnerModel? Owner { get; set; }
        [JsonProperty("html_url")]
        public string? Url { get; set; }
        [JsonProperty("description")]
        public string? Description { get; set; }
        



        

    }
}
