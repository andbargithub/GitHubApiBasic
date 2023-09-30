using Newtonsoft.Json;

namespace B9BasicGitHubApi.Models
{
    public class CommitDetailModel
    {
        [JsonProperty("author")]
        public AuthorModel Author { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }


    }
}
