using System.ComponentModel.DataAnnotations;

namespace B9BasicGitHubApi.Models.RequestModels
{
    public class RepositoriesRequestModel
    {
        [Required]
        public string Name { get; set; }
    }

    public class RepositoriyRequestModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string RepositoryName { get; set; }
    }
}
