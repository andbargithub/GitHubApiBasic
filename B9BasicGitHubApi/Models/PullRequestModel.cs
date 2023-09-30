using Newtonsoft.Json;

namespace B9BasicGitHubApi.Models
{
    public class PullRequestModel
    {
        [JsonProperty("id")]        
        public long Id { get; set; }
        
        [JsonProperty("html_url")]
        public string Url { get; set; }
        
        [JsonProperty("state")]
        public string? State{ get; set; }
        
        [JsonProperty("title")]
        public string? Title { get; set; }
        
        [JsonProperty("number")]
        public int? Number { get; set; }
        
        [JsonProperty ("user")]
        public OwnerModel Owner { get; set; }
        
        [JsonProperty("body")]
        public string? body { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("labels")]
        public IEnumerable<LabelModel>Labels { get; set; }

        [JsonProperty("head")]
        public HeadModel Head { get; set; }
        public IEnumerable<CommitModel> Commits { get; set; }
        public string Category { get; set; }
        public int? DaysInCategory { get; set; }
        public int? CommentsQuantity { get; set; }
    }
}
