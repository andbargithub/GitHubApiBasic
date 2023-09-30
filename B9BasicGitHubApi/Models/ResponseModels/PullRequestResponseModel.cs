using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace B9BasicGitHubApi.Models.ResponseModels
{
    
    public class Pullrequests
    {
        public List<PullRequestModel> Active { get; set; }
        public List<PullRequestModel> Draft { get; set; }
        public List<PullRequestModel> Stale { get; set; }

        public int? AverageDaysActive { get; set; }
        public int? AverageDaysDraft { get; set; }
        public int? AverageDaysStale { get; set; }
        public int? AverageDaysGeneral { get; set; }

        public Pullrequests()
        {
            Active = new List<PullRequestModel>();
            Draft = new List<PullRequestModel>();
            Stale = new List<PullRequestModel>();
        }

    }
}
