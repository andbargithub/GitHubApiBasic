# GitHubApiBasic

Repository to host source code of Technical Test.


The Requirements are

	1) Create a Method that searches Github and display the latest open pull requests
		1.1) Input Parameters: 
			1.1.1) Repository Owner
			1.1.2) Repository Name
			1.1.3) Pull Request Label
			1.1.4) Custom Search Key words
			
		1.2) The output of the method must contain a list of open pull requets:
			1.2.1) Array of Commits
			1.2.2) URL to Pull Request
			1.2.3) Title
			1.2.4) Short Description
			1.2.5) Number of Comments
			1.2.6) Creation Date
			1.2.7) Creator's Name
			1.2.8) Creator's Email
			1.2.9) Creator's Avatar Link

	2) Aditional Logic
	
		2.1) Pull Requests must be classified as Draft, Stale and Active
			2.1.1) If a pull request has not a Label named "Draft" then it is classified as "Active"			
			2.1.2) If a Pull Request has the Label named "Draft" and is open for less than a month and marked as Draft, than it is Draft
			2.1.3) If a Pull Request has the Label named "Draft" and is open for more than a month and marked as Draft, than it is Stale
			
		2.2) The response must group the Pull Requets by its category (Active, Draft or Stale)
		
		2.3) The response must provide a field to show how many days the Pull Request is Stale
		
		2.4) Average Days
			2.4.1) Calculate Average Days Open for Active
			2.4.2) Calculate Average Days Open for Active
			2.4.3) Calculate Average Days Open for Active
			
	3) Unit Test
		The requirement 2.1 must be unit tested
			
The Solution is composed by 2 Projects:

	B9BasicGitHubApi.csproj: The API that has the Method "/api​/Repository​/PullRequests​/List" which meets requirements 1 and 2 above. It can be executed using Swagger (https://localhost:7169/swagger/index.html)
	
	B9BasicGitHubApiTest.csproj: The Tests Project which meets the requirement 3 above
	
	Where to find the approach for each requirement
		Requirement 1.1 is implemented in Models\RequestModels\PullRequestRequestModel.cs
		Requirement 1.2 is implemented in Models\PullRequestModel.cs				
		Requirement 2.1 is implemented in Services\RepositoryService.cs, method "GetPullRequestCategory"
		Requirement 2.2 is implemented in Models\RequestModels\PullRequestResponseModel.cs
		Requirement 2.3 is implemented in Models\PullRequestModel.cs, DaysInCategory Attribute
		Requirement 2.4 is implemented in Models\ResponseModels\PullRequestResponseModel.cs, Attributes AverageDaysActive, AverageDaysDraft, AverageDaysStale, AverageDaysGeneral
		Requirement 3 is implemented in UnitTests.cs, Methods TestAditionalLogicActivePullRequest, TestAditionalLogicDraftPullRequest, TestAditionalLogicStaletPullRequest
		
	Please note that not all approaches chosen were the best for the scenario, but were implemented to show how I work, organize and know concepts of OO, Extension Methods (see Extensions\EnumExtension.cs file, implementation of GetDescritpion Method), Dependency Injection and others.
	
	Thank you for your time and feel free to let your impression about these tasks in this repository.
