using B9BasicGitHubApi.Controllers;
using B9BasicGitHubApi.Extensions;
using B9BasicGitHubApi.Models;
using B9BasicGitHubApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http;
using System.Net.Http.Json;

namespace B9BasicGitHubApiTest
{
    public class Tests
    {
        
        private RepositoryService _repositoryService;

        WebApplicationFactory<Program> _applicationFactory;
        HttpClient _httpClient;
        public static IConfiguration _configuration { get; set; }


        //Provide a valid username
        private readonly string validUserName = "andbargithub";
        //Provide a valid repository which contains pull requests
        private readonly string validRepositoryName = "callstalker";
        //Provide a valid label/tag
        private readonly string validLabel = "bug";
        //Provide a valid content to search
        private readonly string validSearch = "Proposta";

        


        [SetUp]
        public void Setup()
        {
            _applicationFactory = new WebApplicationFactory<Program>();            
            _httpClient = _applicationFactory.CreateClient();


            _repositoryService = new RepositoryService(_configuration);
            
        }

        [Test]
        public async Task TestRepositoryList()
        {
            var activity = _httpClient.GetAsync($"/api/Repository/list?Name={validUserName}").Result;
            Assert.IsNotNull(activity);
        }

        [Test]
        public async Task TestRepository()
        {
            var activity = _httpClient.GetAsync($"/api/Repository?Name={validUserName}&RepositoryName={validRepositoryName}").Result;
            Assert.IsNotNull(activity);
        }

        [Test]
        public async Task TestPullRequestList()
        {
            var activity = _httpClient.GetAsync($"/api/Repository/PullRequests/List?Name={validUserName}&RepositoryName={validRepositoryName}&LabelTag={validLabel}&Search={validSearch}").Result;
            Assert.IsNotNull(activity);
        }

        [Test]
        public async Task TestAditionalLogicActivePullRequest()
        {
            var pullRequestActive = new PullRequestModel
            {
                Id = 1,
                Title = "Test",
                Labels = new List<LabelModel> { new LabelModel { Name = "test" } },
                CreatedAt = DateTime.Today.AddDays(-7),
            };

            var response = _repositoryService.GetPullRequestCategory(pullRequestActive);


            Assert.AreEqual(response, RepositoryService.PullRequestCategories.Active.GetDescription());

        }

        [Test]
        public async Task TestAditionalLogicDraftPullRequest()
        {
            var pullRequestDraft = new PullRequestModel
            {
                Id = 2,
                Title = "Test 2",
                Labels = new List<LabelModel> { new LabelModel { Name = "draft" } },
                CreatedAt = DateTime.Today.AddDays(-7),
            };

            var response = _repositoryService.GetPullRequestCategory(pullRequestDraft);


            Assert.AreEqual(response, RepositoryService.PullRequestCategories.Draft.GetDescription());

        }

        [Test]
        public async Task TestAditionalLogicStaletPullRequest()
        {
            var pullRequestStale = new PullRequestModel
            {
                Id = 3,
                Title = "Test 3",
                Labels = new List<LabelModel> { new LabelModel { Name = "draft" } },
                CreatedAt = DateTime.Today.AddDays(-40),
            };

            var response = _repositoryService.GetPullRequestCategory(pullRequestStale);


            Assert.AreEqual(response, RepositoryService.PullRequestCategories.Stale.GetDescription());

        }

        



    }
}