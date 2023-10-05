using System.ComponentModel.DataAnnotations;

namespace B9BasicGitHubApi.Models.RequestModels
{
    public class PullRequestRequestModel
    {
        [Required] 
        public string? Name { get; set;}
        [Required]
        public string? RepositoryName { get; set;}
        [Required]
        public int PullRequestNumber { get; set;}

        
    }
}
