using B9BasicGitHubApi.Models;
using B9BasicGitHubApi.Services.Interfaces;
using Newtonsoft.Json;
using System.Collections;
using System.ComponentModel;
using B9BasicGitHubApi.Extensions;
using B9BasicGitHubApi.Models.ResponseModels;

namespace B9BasicGitHubApi.Services
{
    public class RepositoryService : IRepositoryService
    {
        private Uri _githubApiUrl = new Uri("https://api.github.com");
        private HttpClient _httpClient;
        private IConfiguration _configuration;
        private readonly int AMOUNT_OF_PULL_REQUESTS_IN_LIMITED_CONTEXT = 2;


        public enum PullRequestCategories
        {
            [Description("draft")]
            Draft,
            [Description("active")]
            Active,
            [Description("stale")]
            Stale,
        }

        public RepositoryService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = _githubApiUrl;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "BasicGithubApiInfo");

            _configuration = configuration;

        }

        public IEnumerable<RepositoryModel> GetRepositories(string userName)
        {

            ValidateUser(userName);

            var result = new List<RepositoryModel>();

            String apiRequestUrl = $"{_httpClient.BaseAddress}users/{userName}/repos";

            string repositoryRawData = GetRawDataFromApi(apiRequestUrl);

            result = JsonConvert.DeserializeObject<List<RepositoryModel>>(repositoryRawData);

            return result;

        }

        public RepositoryModel GetRepository(string userName, string repositoryName)
        {
            ValidateRepository(userName, repositoryName);

            var result = new RepositoryModel();

            String apiRequestUrl = $"{_httpClient.BaseAddress}repos/{userName}/{repositoryName}";

            string repositoryRawData = GetRawDataFromApi(apiRequestUrl);

            result = JsonConvert.DeserializeObject<RepositoryModel>(repositoryRawData);

            return result;

        }

        public IEnumerable<PullRequestModel> GetPullRequests(string userName, string repositoryName)
        {
            ValidateRepository(userName, repositoryName);

            var result = new List<PullRequestModel>();

            String apiRequestUrl = $"{_httpClient.BaseAddress}repos/{userName}/{repositoryName}/pulls?state=open";

            string repositoryRawData = GetRawDataFromApi(apiRequestUrl);

            result = JsonConvert.DeserializeObject<List<PullRequestModel>>(repositoryRawData);

            bool githubUnlimitedCalls = false;

            if (Boolean.TryParse(_configuration["UnlimitedGithubCalls"], out githubUnlimitedCalls))
            {
                if (!githubUnlimitedCalls)
                {
                    result = result.GetRange(0, AMOUNT_OF_PULL_REQUESTS_IN_LIMITED_CONTEXT);
                }
            }

            result.ForEach(pr => pr = ProcessPullRequestExtraInfo(pr, userName, repositoryName));

            return result;
        }

        public IEnumerable<PullRequestModel> GetPullRequests(string userName, string repositoryName, string labelTag, string search)
        {
            ValidateRepository(userName, repositoryName);

            var result = this.GetPullRequests(userName, repositoryName);

            if (!String.IsNullOrEmpty(labelTag))
            {
                result = result.Where(pr => pr.Labels.Select(t => t.Name).Contains(labelTag)).ToList();
            }

            if (!String.IsNullOrEmpty(search))
            {
                result = result.Where(pr => pr.body != null && pr.body.Contains(search)).ToList();
            }

            return result;
        }

        public Pullrequests ProcessPullRequestsToResponseStructure(IEnumerable<PullRequestModel> pullRequests)
        {
            var response = new Pullrequests();

            foreach (var pullRequest in pullRequests)
            {
                if (pullRequest.Category.Equals(PullRequestCategories.Active.GetDescription()))
                {
                    response.Active.Add(pullRequest);
                }

                if (pullRequest.Category.Equals(PullRequestCategories.Draft.GetDescription()))
                {
                    response.Draft.Add(pullRequest);
                }

                if (pullRequest.Category.Equals(PullRequestCategories.Stale.GetDescription()))
                {
                    response.Stale.Add(pullRequest);
                }
            }

            response = ProcessExtraPullRequestsInfo(response);

            return response;
        }



        private void ValidateUser(string userName)
        {
            String apiRequestUrl = $"{_httpClient.BaseAddress}users/{userName}";
            HttpResponseMessage getingRepositoryDataFromApi = _httpClient.GetAsync(apiRequestUrl).Result;

            if (!getingRepositoryDataFromApi.IsSuccessStatusCode)
            {
                throw new Exception($"Git Hub user not found: {userName}");
            }

        }

        private void ValidateRepository(string userName, string repositoryName)
        {
            ValidateUser(userName);

            String apiRequestUrl = $"{_httpClient.BaseAddress}repos/{userName}/{repositoryName}";
            HttpResponseMessage getingRepositoryDataFromApi = _httpClient.GetAsync(apiRequestUrl).Result;

            if (!getingRepositoryDataFromApi.IsSuccessStatusCode)
            {
                throw new Exception($"Git Hub repository '{repositoryName}' for user '{userName}' not found.");
            }
        }

        private PullRequestModel ProcessPullRequestExtraInfo(PullRequestModel pullRequest, string userName, string repositoryName)
        {
            var response = pullRequest;

            //response.Commits = GetPullrequestCommits(pullRequest.Number.Value, userName, repositoryName);
            response.Category = GetPullRequestCategory(pullRequest);
            response.DaysInCategory = CalculateDaysInCategory(response);
            response.CommentsQuantity = GetPullRequestCommentsQuantity(pullRequest.Number.Value, userName, repositoryName);

            return response;
        }



        private Pullrequests ProcessExtraPullRequestsInfo(Pullrequests pullRequests)
        {
            var response = pullRequests;
            var allPullRequests = new List<PullRequestModel>();

            allPullRequests.AddRange(pullRequests.Active);
            allPullRequests.AddRange(pullRequests.Draft);
            allPullRequests.AddRange(pullRequests.Stale);

            response.AverageDaysDraft = CalculateAverageDaysOpen(pullRequests.Draft);
            response.AverageDaysStale = CalculateAverageDaysOpen(pullRequests.Stale);
            response.AverageDaysActive = CalculateAverageDaysOpen(pullRequests.Active);
            response.AverageDaysGeneral = CalculateAverageDaysOpen(allPullRequests);

            return response;

        }

        public string GetPullRequestCategory(PullRequestModel pullRequest)
        {
            var response = String.Empty;

            var draftLabel = pullRequest.Labels.Where(l => l.Name.Equals(PullRequestCategories.Draft.GetDescription())).FirstOrDefault();

            if (draftLabel != null)
            {
                response = pullRequest.CreatedAt.AddMonths(1) < DateTime.Today ? PullRequestCategories.Stale.GetDescription() : PullRequestCategories.Draft.GetDescription();
            }
            else
            {
                response = PullRequestCategories.Active.GetDescription();
            }

            return response;
        }

        private int GetPullRequestCommentsQuantity(int pullRequestNumber, string userName, string repositoryName)
        {
            var result = 0;

            String apiRequestUrl = $"{_httpClient.BaseAddress}repos/{userName}/{repositoryName}/pulls/{pullRequestNumber}/comments";

            string repositoryRawData = GetRawDataFromApi(apiRequestUrl);

            var response = JsonConvert.DeserializeObject<List<CommitModel>>(repositoryRawData);

            result = response.Count();

            return result;
        }

        private int CalculateDaysInCategory
            (PullRequestModel pullRequest)
        {
            var startDate = pullRequest.Category.Equals(PullRequestCategories.Stale.GetDescription()) ? pullRequest.CreatedAt.AddMonths(1) : pullRequest.CreatedAt;

            var response = startDate - DateTime.Today;

            return response.Days;
        }

        private int CalculateAverageDaysOpen(IEnumerable<PullRequestModel> pullRequests)
        {
            var response = pullRequests.Count() > 0 ? pullRequests.Select(pr => (pr.CreatedAt - DateTime.Today).Days).ToList().Average() : 0;

            return Convert.ToInt32(response);
        }

        private IEnumerable<CommitModel> GetPullrequestCommits(int pullRequestNumber, string userName, string repositoryName)
        {
            var result = new List<CommitModel>();

            String apiRequestUrl = $"{_httpClient.BaseAddress}repos/{userName}/{repositoryName}/pulls/{pullRequestNumber}/commits";

            string repositoryRawData = GetRawDataFromApi(apiRequestUrl);

            result = JsonConvert.DeserializeObject<List<CommitModel>>(repositoryRawData);

            return result;
        }

        private String GetRawDataFromApi(string requestUrl)
        {
            String result = String.Empty;


            HttpResponseMessage getingRepositoryDataFromApi = _httpClient.GetAsync(requestUrl).Result;

            if (getingRepositoryDataFromApi.IsSuccessStatusCode)
            {
                result = getingRepositoryDataFromApi.Content.ReadAsStringAsync().Result;
            }
            else
            {
                throw new Exception($"Error while querying GitHub Api:{getingRepositoryDataFromApi.ReasonPhrase}");
            }

            return result;
        }

    }
}
