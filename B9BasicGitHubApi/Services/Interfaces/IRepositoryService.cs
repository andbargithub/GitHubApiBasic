using B9BasicGitHubApi.Models;
using B9BasicGitHubApi.Models.ResponseModels;

namespace B9BasicGitHubApi.Services.Interfaces
{
    public interface IRepositoryService
    {

        IEnumerable<RepositoryModel> GetRepositories(string username);
        RepositoryModel GetRepository(string username, string repositoryName);
        IEnumerable<PullRequestModel> GetPullRequests(string userName, string repositoryName);
        IEnumerable<PullRequestModel> GetPullRequests(string userName, string repositoryName, string label, string search);
        Pullrequests ProcessPullRequestsToResponseStructure(IEnumerable<PullRequestModel> pullRequests);

    }
}
