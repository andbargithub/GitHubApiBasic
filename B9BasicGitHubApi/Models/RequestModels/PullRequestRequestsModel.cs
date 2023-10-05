using System.ComponentModel.DataAnnotations;

namespace B9BasicGitHubApi.Models.RequestModels
{
    public class PullRequestsRequestModel
    {
        [Required] 
        public string Name { get; set;}
        [Required]
        public string RepositoryName { get; set;}

        public string? LabelTag { get; set;}

        public string? Search { get; set; }
    }
}
