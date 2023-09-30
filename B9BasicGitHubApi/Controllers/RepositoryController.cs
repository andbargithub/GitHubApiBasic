using B9BasicGitHubApi.Models;
using B9BasicGitHubApi.Models.RequestModels;
using B9BasicGitHubApi.Models.ResponseModels;
using B9BasicGitHubApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace B9BasicGitHubApi.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class RepositoryController : ControllerBase
    {
        private IRepositoryService _repositoryService;

        public RepositoryController(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }


        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("List")]
        public async Task<IActionResult> GetRepositories([FromQuery] RepositoriesRequestModel request)
        {
            var response = new List<RepositoryModel>();

            try
            {

                response = _repositoryService.GetRepositories(request.Name).ToList();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetRepository([FromQuery] RepositoriyRequestModel request)
        {
            var response = new RepositoryModel();
            try
            {
                response = _repositoryService.GetRepository(request.Name, request.RepositoryName);
            }
            catch(Exception error) 
            {
                return BadRequest(error.Message);
            }

            return Ok(response);

        }

        [HttpGet, Route("PullRequests/List")]
        public async Task<IActionResult> GetPullrequests([FromQuery] PullRequestRequestModel request)
        {
            var response = new Pullrequests();

            try
            {
                var pullRequests = _repositoryService.GetPullRequests(request.Name, request.RepositoryName, request.LabelTag, request.Search).ToList();
                response = _repositoryService.ProcessPullRequestsToResponseStructure(pullRequests);
                
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }

            return Ok(response);


        }




    }
    }
