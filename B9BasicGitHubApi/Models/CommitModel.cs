using Newtonsoft.Json;

namespace B9BasicGitHubApi.Models
{
    public class CommitModel
    {
        [JsonProperty("sha")]
        public string Hash { get; set; }

        [JsonProperty("commit")]
        public CommitDetailModel CommitDetail { get; set; }

        [JsonProperty("author")]
        public OwnerModel Owner { get; set; }


    }
}
