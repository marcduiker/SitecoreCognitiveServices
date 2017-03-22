extern alias MicrosoftProjectOxfordCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.ProjectOxford.Text.Core;
using Microsoft.WindowsAzure.MediaServices.Client;
using Newtonsoft.Json;
using Sitecore.SharedSource.CognitiveServices.Enums;
using Sitecore.SharedSource.CognitiveServices.Models;
using Sitecore.SharedSource.CognitiveServices.Models.Vision.ContentModerator;
using System.Linq;
using log4net.Repository.Hierarchy;
using Sitecore.ContentSearch.Utilities;
using Sitecore.SharedSource.CognitiveServices.Foundation;

namespace Sitecore.SharedSource.CognitiveServices.Repositories.Vision {
    public class ContentModeratorRepository : TextClient, IContentModeratorRepository {
        protected static readonly string moderatorUrl = "https://westus.api.cognitive.microsoft.com/contentmoderator/moderate/v1.0";
        protected static readonly string reviewUrl = "https://westus.api.cognitive.microsoft.com/contentmoderator/review/v1.0/teams/";
        protected static readonly string listUrl = "https://westus.api.cognitive.microsoft.com/contentmoderator/lists/v1.0/imagelists/";
        protected static readonly string termListUrl = "https://westus.api.cognitive.microsoft.com/contentmoderator/lists/v1.0/termlists/";

        protected static readonly string reviewSessionTokenKey = "ReviewSessionTokenKey";

        protected readonly IApiKeys ApiKeys;
        protected readonly IRepositoryClient RepositoryClient;
        protected readonly ILogWrapper Logger;

        protected HttpContextBase Context { get; set; }

        public ContentModeratorRepository(
            IApiKeys apiKeys,
            IRepositoryClient repoClient,
            HttpContextBase context,
            ILogWrapper logger) : base(apiKeys.ContentModerator) {
            ApiKeys = apiKeys;
            RepositoryClient = repoClient;
            Context = context;
            Logger = logger;
        }

        #region Moderate

        #region Evaluate

        private string GetImageUrlData(string imageUrl) {
            return $"{{\"DataRepresentation\":\"URL\",\"Value\":\"{imageUrl}\"}}";
        }

        public virtual async Task<EvaluateResponse> EvaluateAsync(string imageUrl) {
            var response = await RepositoryClient.SendJsonPostAsync(ApiKeys.ContentModerator, $"{moderatorUrl}/ProcessImage/Evaluate", GetImageUrlData(imageUrl));

            return JsonConvert.DeserializeObject<EvaluateResponse>(response);
        }

        public async Task<EvaluateResponse> EvaluateAsync(Stream stream) {
            var response = await RepositoryClient.SendImagePostAsync(ApiKeys.ContentModerator, $"{moderatorUrl}/ProcessImage/Evaluate", stream);

            return JsonConvert.DeserializeObject<EvaluateResponse>(response);
        }

        #endregion Evaluate

        #region Find Faces

        public virtual async Task<FindFacesResponse> FindFacesAsync(string imageUrl) {
            var response = await RepositoryClient.SendJsonPostAsync(ApiKeys.ContentModerator, $"{moderatorUrl}/ProcessImage/FindFaces", GetImageUrlData(imageUrl));

            return JsonConvert.DeserializeObject<FindFacesResponse>(response);
        }

        public virtual async Task<FindFacesResponse> FindFacesAsync(Stream stream) {
            var response = await RepositoryClient.SendImagePostAsync(ApiKeys.ContentModerator, $"{moderatorUrl}/ProcessImage/FindFaces", stream);

            return JsonConvert.DeserializeObject<FindFacesResponse>(response);
        }

        #endregion Find Faces

        #region Match

        private string GetMatchQuerystring(string listId) {
            return string.IsNullOrEmpty(listId) ? "" : $"?listId={listId}";
        }

        public virtual async Task<MatchResponse> MatchAsync(string imageUrl, string listId = "") {
            var response = await RepositoryClient.SendJsonPostAsync(ApiKeys.ContentModerator, $"{moderatorUrl}/ProcessImage/Match{GetMatchQuerystring(listId)}", GetImageUrlData(imageUrl));

            return JsonConvert.DeserializeObject<MatchResponse>(response);
        }

        public virtual async Task<MatchResponse> MatchAsync(Stream stream, string listId = "") {
            var response = await RepositoryClient.SendImagePostAsync(ApiKeys.ContentModerator, $"{moderatorUrl}/ProcessImage/Match{GetMatchQuerystring(listId)}", stream);

            return JsonConvert.DeserializeObject<MatchResponse>(response);
        }

        #endregion Match

        #region OCR

        private string GetOCRQuerystring(string language, bool enhanced) {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(language))
                sb.Append($"?language={language}");

            if (enhanced) {
                var concat = (sb.Length > 0) ? "&" : "?";
                sb.Append($"{concat}enhanced={enhanced}");
            }

            return sb.ToString();
        }

        public virtual async Task<OCRResult> OCRAsync(string imageUrl, string language = "", bool enhanced = false) {
            var response = await RepositoryClient.SendJsonPostAsync(ApiKeys.ContentModerator, $"{moderatorUrl}/ProcessImage/OCR{GetOCRQuerystring(language, enhanced)}", GetImageUrlData(imageUrl));

            return JsonConvert.DeserializeObject<OCRResult>(response);
        }

        public virtual async Task<OCRResult> OCRAsync(Stream stream, string language = "", bool enhanced = false) {
            var response = await RepositoryClient.SendImagePostAsync(ApiKeys.ContentModerator, $"{moderatorUrl}/ProcessImage/OCR{GetOCRQuerystring(language, enhanced)}", stream);

            return JsonConvert.DeserializeObject<OCRResult>(response);
        }

        #endregion OCR

        #region Detect Language

        public virtual async Task<DetectLanguageResponse> DetectLanguageAsync(string text) {
            var response = await RepositoryClient.SendTextPostAsync(ApiKeys.ContentModerator, $"{moderatorUrl}/ProcessText/DetectLanguage", text);

            return JsonConvert.DeserializeObject<DetectLanguageResponse>(response);
        }

        #endregion Detect Language

        #region Screen

        public virtual async Task<ScreenResponse> ScreenAsync(string text, string language = "eng", bool autocorrect = false, bool urls = false, bool PII = false, string listId = "") {
            StringBuilder sb = new StringBuilder();
            sb.Append($"?language={language}");

            if (autocorrect)
                sb.Append($"&autocorrect={autocorrect}");

            if (urls)
                sb.Append($"&urls={urls}");

            if (PII)
                sb.Append($"&PII={PII}");

            if (!string.IsNullOrEmpty(listId))
                sb.Append($"&listId={listId}");

            var response = await RepositoryClient.SendTextPostAsync(ApiKeys.ContentModerator, $"{moderatorUrl}/ProcessText/Screen{sb}", text);

            return JsonConvert.DeserializeObject<ScreenResponse>(response);
        }

        #endregion Screen

        #endregion Moderate

        #region Review

        protected string GetToken() {
            if (Context?.Session?[reviewSessionTokenKey] != null) {
                var sessionToken = (TokenResponse)Context.Session[reviewSessionTokenKey];
                if (sessionToken.Expires_On != null && sessionToken.ExpirationDate >= DateTime.Now)
                    return sessionToken.Access_Token;
            }

            var token = RepositoryClient.SendReviewTokenRequest(ApiKeys.ContentModeratorPrivateKey, ApiKeys.ContentModeratorClientId);
            Context.Session.Add(reviewSessionTokenKey, token);

            return token.Access_Token;
        }

        #region Create Job

        private string GetCreateJobQuerystring(string callbackEndpoint) {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(callbackEndpoint))
                sb.Append($"&CallBackEndpoint={callbackEndpoint}");

            return sb.ToString();
        }

        private string GetCreateJobData(string content) {
            return $"{{ \"ContentValue\": \"{content}\" }}";
        }

        public virtual async Task<CreateJobResponse> CreateImageJobAsync(string imageUrl, string teamName, string contentId, string workflowName, string callbackEndpoint = "") {
            var response = await RepositoryClient.SendAsync(
                ApiKeys.ContentModerator,
                $"{reviewUrl}{teamName}/jobs?ContentType=Image&ContentId={contentId}&WorkflowName={workflowName}{GetCreateJobQuerystring(callbackEndpoint)}",
                GetCreateJobData(imageUrl),
                "application/json",
                "POST",
                GetToken());

            return JsonConvert.DeserializeObject<CreateJobResponse>(response);
        }

        public virtual async Task<CreateJobResponse> CreateImageJobAsync(Stream stream, string teamName, string contentId, string workflowName, string callbackEndpoint = "") {
            var response = await RepositoryClient.SendAsync(
                ApiKeys.ContentModerator,
                $"{reviewUrl}{teamName}/jobs?ContentType=Image&ContentId={contentId}&WorkflowName={workflowName}{GetCreateJobQuerystring(callbackEndpoint)}",
                RepositoryClient.GetStreamString(stream),
                RepositoryClient.GetImageStreamContentType(stream),
                "POST",
                GetToken());

            return JsonConvert.DeserializeObject<CreateJobResponse>(response);
        }

        public virtual async Task<CreateJobResponse> CreateTextJobAsync(string text, string teamName, string contentId, string workflowName, string callbackEndpoint = "") {
            var response = await RepositoryClient.SendAsync(
                ApiKeys.ContentModerator,
                $"{reviewUrl}{teamName}/jobs?ContentType=Text&ContentId={contentId}&WorkflowName={workflowName}{GetCreateJobQuerystring(callbackEndpoint)}",
                GetCreateJobData(text),
                "application/json",
                "POST",
                GetToken());

            return JsonConvert.DeserializeObject<CreateJobResponse>(response);
        }
        
        public void RunContentModeratorJob(string mediaProcessorName, string inputFilePath, string outputDirectory, VideoModerationConfiguration configuration) {

            ITokenProvider tp = new MediaServicesCredentials(ApiKeys.ContentModeratorClientId, ApiKeys.ContentModeratorPrivateKey);
            CloudMediaContext cloudContext = new CloudMediaContext(tp);

            // create asset with input file
            IAsset asset = cloudContext
                .Assets
                .CreateFromFile(inputFilePath, AssetCreationOptions.None);

            // grab instance of Azure Media Content Moderator MP
            IMediaProcessor mp = cloudContext
                .MediaProcessors
                .GetLatestMediaProcessorByName(mediaProcessorName);

            // create Job with Content Moderator task
            IJob job = cloudContext
                .Jobs
                .Create($"Content Moderator {Path.GetFileName(inputFilePath)}_{Guid.NewGuid()}");

            ITask contentModeratorTask = job
                .Tasks
                .AddNew("Adult classifier task", mp, JsonConvert.SerializeObject(configuration), TaskOptions.None);

            contentModeratorTask
                .InputAssets
                .Add(asset);

            contentModeratorTask
                .OutputAssets
                .AddNew("Adult classifier output", AssetCreationOptions.None);

            job.Submit();
            
            Task progressPrintTask = new Task(() => {
                IJob jobQuery = null;
                do {
                    var progressContext = cloudContext;
                    jobQuery = progressContext
                        .Jobs
                        .First(j => j.Id == job.Id);
                    Logger.Info($"{DateTime.Now}\t{jobQuery.State}", this);
                    Thread.Sleep(10000);
                }
                while (jobQuery.State != JobState.Finished
                && jobQuery.State != JobState.Error
                && jobQuery.State != JobState.Canceled);
            });
            progressPrintTask.Start();

            Task progressJobTask = job.GetExecutionProgressTask(CancellationToken.None);
            progressJobTask.Wait();

            if (job.State == JobState.Error) {
                ErrorDetail error = job.Tasks.First().ErrorDetails.First();
                Logger.Error($"{error.Code}. {error.Message}", this);
            }
            
            job.OutputMediaAssets
                .First()
                .AssetFiles
                .ForEach(file => file.Download(Path.Combine(outputDirectory, file.Name)));
        }
        
        #endregion Create Job

        #region Get Job

        public virtual async Task<GetJobResponse> GetJobAsync(string teamName, string jobId) {
            var response = await RepositoryClient.SendAsync(ApiKeys.ContentModerator, $"{reviewUrl}{teamName}/jobs/{jobId}", "", "application/json", "GET", GetToken());

            return JsonConvert.DeserializeObject<GetJobResponse>(response);
        }

        #endregion Get Job

        #region Create Review

        public virtual async Task<List<string>> CreateReviewAsync(string teamName, List<ReviewRequest> requests, string subTeam = "") {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(subTeam))
                sb.Append($"?subTeam={subTeam}");

            var response = await RepositoryClient.SendAsync(
                ApiKeys.ContentModerator,
                $"{reviewUrl}{teamName}/reviews{sb}",
                JsonConvert.SerializeObject(requests),
                "application/json",
                "POST",
                GetToken());

            return JsonConvert.DeserializeObject<List<string>>(response);
        }

        #endregion Create Review

        #region Get Review

        public virtual async Task<GetReviewResponse> GetReviewAsync(string teamName, string reviewId) {
            var response = await RepositoryClient.SendAsync(ApiKeys.ContentModerator, $"{reviewUrl}{teamName}/reviews/{reviewId}", "", "application/json", "GET", GetToken());

            return JsonConvert.DeserializeObject<GetReviewResponse>(response);
        }

        #endregion Get Review

        #region Create or Update Workflow

        public virtual async Task<bool> CreateOrUpdateWorkflowAsync(string teamName, string workflowName, WorkflowExpression expression) {
            var data = JsonConvert.SerializeObject(expression);

            var response = await RepositoryClient.SendAsync(
                ApiKeys.ContentModerator,
                $"{reviewUrl}{teamName}/workflows/{workflowName}",
                data,
                "application/json",
                "PUT",
                GetToken());

            return JsonConvert.DeserializeObject<bool>(response);
        }

        #endregion Create or Update Workflow

        #region Get Workflow

        public virtual async Task<WorkflowExpressionResponse> GetWorkflowAsync(string teamName, string workflowName) {
            var response = await RepositoryClient.SendAsync(ApiKeys.ContentModerator, $"{reviewUrl}{teamName}/workflows/{workflowName}", "", "application/json", "GET", GetToken());

            return JsonConvert.DeserializeObject<WorkflowExpressionResponse>(response);
        }

        public virtual async Task<List<WorkflowExpressionResponse>> GetAllWorkflowsAsync(string teamName) {
            var response = await RepositoryClient.SendAsync(ApiKeys.ContentModerator, $"{reviewUrl}{teamName}/workflows", "", "application/json", "GET", GetToken());

            return JsonConvert.DeserializeObject<List<WorkflowExpressionResponse>>(response);
        }

        #endregion Get Workflow

        #endregion Review

        #region List Management

        #region Image

        private string GetAddImageQuerystring(ContentModeratorTag tag, string label) {
            StringBuilder sb = new StringBuilder();
            if (tag != ContentModeratorTag.None)
                sb.Append($"?tag={(int)tag}");

            if (!string.IsNullOrEmpty(label)) {
                var concat = sb.Length > 0 ? "&" : "?";
                sb.Append($"{concat}label={label}");
            }

            return sb.ToString();
        }

        public virtual async Task<AddImageResponse> AddImageAsync(string imageUrl, string listId, ContentModeratorTag tag = ContentModeratorTag.None, string label = "") {
            string data = $"{{ \"DataRepresentation\":\"URL\", \"Value\":\"{imageUrl}\" }}";

            var response = await RepositoryClient.SendJsonPostAsync(ApiKeys.ContentModerator, $"{listUrl}{listId}/images{GetAddImageQuerystring(tag, label)}", data);

            return JsonConvert.DeserializeObject<AddImageResponse>(response);
        }

        public virtual async Task<AddImageResponse> AddImageAsync(Stream stream, string listId, ContentModeratorTag tag = ContentModeratorTag.None, string label = "") {
            var response = await RepositoryClient.SendImagePostAsync(ApiKeys.ContentModerator, $"{listUrl}{listId}/images{GetAddImageQuerystring(tag, label)}", stream);

            return JsonConvert.DeserializeObject<AddImageResponse>(response);
        }

        public virtual async Task DeleteImageAsync(string listId, string imageId) {
            await RepositoryClient.SendJsonDeleteAsync(ApiKeys.ContentModerator, $"{listUrl}{listId}/images/{imageId}");
        }

        public virtual async Task DeleteAllImageAsync(string listId) {
            await RepositoryClient.SendJsonDeleteAsync(ApiKeys.ContentModerator, $"{listUrl}{listId}/images");
        }

        public virtual async Task<GetImagesResponse> GetAllImageIdsAsync(string listId) {
            var response = await SendGetAsync($"{listUrl}{listId}/images");

            return JsonConvert.DeserializeObject<GetImagesResponse>(response);
        }

        #endregion Image

        #region Image Lists

        public virtual async Task<ListDetails> GetImageListDetailsAsync(string listId) {
            var response = await SendGetAsync($"{listUrl}{listId}");

            return JsonConvert.DeserializeObject<ListDetails>(response);
        }

        public virtual async Task<ListDetails> CreateListAsync(ListDetails details) {
            var response = await RepositoryClient.SendJsonPostAsync(ApiKeys.ContentModerator, $"{listUrl}", JsonConvert.SerializeObject(details));

            return JsonConvert.DeserializeObject<ListDetails>(response);
        }

        public virtual async Task DeleteImageListAsync(string listId) {
            await RepositoryClient.SendJsonDeleteAsync(ApiKeys.ContentModerator, $"{listUrl}{listId}");
        }

        public virtual async Task<List<ListDetails>> GetAllImageListsAsync() {
            var response = await SendGetAsync(listUrl);

            return JsonConvert.DeserializeObject<List<ListDetails>>(response);
        }

        public virtual async Task<RefreshSearchResponse> RefreshImageSearchIndexAsync(string listId) {
            var response = await RepositoryClient.SendJsonPostAsync(ApiKeys.ContentModerator, $"{listUrl}{listId}/RefreshIndex", "");

            return JsonConvert.DeserializeObject<RefreshSearchResponse>(response);
        }

        public virtual async Task<ListDetails> UpdateImageListDetailsAsync(string listId, ListDetails details) {
            var response = await RepositoryClient.SendJsonPutAsync(ApiKeys.ContentModerator, $"{listUrl}{listId}", JsonConvert.SerializeObject(details));

            return JsonConvert.DeserializeObject<ListDetails>(response);
        }

        #endregion Image Lists

        #region Term

        public virtual async Task AddTermAsync(string listId, string term, string language) {
            var response = await RepositoryClient.SendJsonPostAsync(ApiKeys.ContentModerator, $"{termListUrl}{listId}/terms/{term}?language={language}", "");
        }

        public virtual async Task DeleteTermAsync(string listId, string term, string language) {
            await RepositoryClient.SendJsonDeleteAsync(ApiKeys.ContentModerator, $"{listUrl}{listId}/terms/{term}?language={language}");
        }

        public virtual async Task DeleteAllTermsAsync(string listId, string language) {
            await RepositoryClient.SendJsonDeleteAsync(ApiKeys.ContentModerator, $"{listUrl}{listId}/terms?language={language}");
        }

        public virtual async Task<GetTermsResponse> GetAllTermsAsync(string listId, string language) {
            var response = await SendGetAsync($"{termListUrl}{listId}/terms?language={language}");

            return JsonConvert.DeserializeObject<GetTermsResponse>(response);
        }

        #endregion Term

        #region Term Lists

        public virtual async Task<ListDetails> CreateTermListAsync(ListDetails details) {
            var response = await RepositoryClient.SendJsonPostAsync(ApiKeys.ContentModerator, termListUrl, JsonConvert.SerializeObject(details));

            return JsonConvert.DeserializeObject<ListDetails>(response);
        }

        public virtual async Task DeleteTermListAsync(string listId) {
            await RepositoryClient.SendJsonDeleteAsync(ApiKeys.ContentModerator, $"{termListUrl}{listId}");
        }

        public virtual async Task<List<ListDetails>> GetAllTermListsAsync() {
            var response = await SendGetAsync(termListUrl);

            return JsonConvert.DeserializeObject<List<ListDetails>>(response);
        }

        public virtual async Task<ListDetails> GetTermListDetailsAsync(string listId) {
            var response = await SendGetAsync($"{termListUrl}{listId}");

            return JsonConvert.DeserializeObject<ListDetails>(response);
        }

        public virtual async Task<RefreshSearchResponse> RefreshTermSearchIndexAsync(string listId, string language) {
            var response = await RepositoryClient.SendJsonPostAsync(ApiKeys.ContentModerator, $"{termListUrl}{listId}/RefreshIndex?lanaguage={language}", "");

            return JsonConvert.DeserializeObject<RefreshSearchResponse>(response);
        }

        public virtual async Task<ListDetails> UpdateTermListDetailsAsync(string listId, ListDetails details) {
            var response = await RepositoryClient.SendJsonPutAsync(ApiKeys.ContentModerator, $"{termListUrl}{listId}", JsonConvert.SerializeObject(details));

            return JsonConvert.DeserializeObject<ListDetails>(response);
        }

        #endregion Term Lists

        #endregion List Management
    }
}