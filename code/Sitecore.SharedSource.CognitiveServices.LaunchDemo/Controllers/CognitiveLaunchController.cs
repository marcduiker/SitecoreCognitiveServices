using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.SpeakerRecognition.Contract.Identification;
using Microsoft.ProjectOxford.Text.Core;
using Microsoft.ProjectOxford.Text.Language;
using Microsoft.ProjectOxford.Text.Sentiment;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Sitecore.SharedSource.CognitiveServices.Enums;
using Sitecore.SharedSource.CognitiveServices.Services.Bing;
using Sitecore.SharedSource.CognitiveServices.Services.Vision;
using Sitecore.SharedSource.CognitiveServices.LaunchDemo.Models;
using Sitecore.SharedSource.CognitiveServices.Models.Language;
using Sitecore.SharedSource.CognitiveServices.Models.Vision.ContentModerator;
using Sitecore.SharedSource.CognitiveServices.Services.Knowledge;
using Sitecore.SharedSource.CognitiveServices.Services.Language;
using Microsoft.ProjectOxford.Video.Contract;
using Sitecore.SharedSource.CognitiveServices.Models.Knowledge;
using Sitecore.SharedSource.CognitiveServices.Models.Language.Translator;
using Sitecore.SharedSource.CognitiveServices.Services.Speech;
using EvaluateResponse = Sitecore.SharedSource.CognitiveServices.Models.Vision.ContentModerator.EvaluateResponse;
using Operation = Microsoft.ProjectOxford.Video.Contract.Operation;

namespace Sitecore.SharedSource.CognitiveServices.LaunchDemo.Controllers
{
    public class CognitiveLaunchController : Controller
    {
        protected readonly IVisionService VisionService;
        protected readonly IAutoSuggestService AutoSuggestService;
        protected readonly IImageSearchService ImageSearchService;
        protected readonly ISpellCheckService SpellCheckService;
        protected readonly IWebSearchService WebSearchService;
        protected readonly INewsSearchService NewsSearchService;
        protected readonly IVideoSearchService VideoSearchService;
        protected readonly IVideoService VideoService;
        protected readonly IEmotionService EmotionService;
        protected readonly IFaceService FaceService;
        protected readonly ILinguisticService LinguisticService;
        protected readonly ISentimentService SentimentService;
        protected readonly IEntityLinkingService EntityLinkingService;
        protected readonly ILanguageService LanguageService;
        protected readonly IContentModeratorService ContentModeratorService;
        protected readonly IAcademicSearchService AcademicSearchService;
        protected readonly IWebLanguageModelService WebLanguageModelService;
        protected readonly ISpeakerIdentificationService SpeakerIdentificationService;
        protected readonly ISpeakerVerificationService SpeakerVerificationService;
        protected readonly ITranslatorService TranslatorService;

        public CognitiveLaunchController(
            IVisionService visionService,
            IAutoSuggestService autoSuggestService,
            IImageSearchService imageSearchService,
            ISpellCheckService spellCheckService,
            IWebSearchService webSearchService,
            INewsSearchService newsSearchService,
            IVideoSearchService videoSearchService,
            IVideoService videoService,
            IEmotionService emotionService,
            IFaceService faceService,
            ILinguisticService linguisticService,
            ISentimentService sentimentService,
            IEntityLinkingService entityLinkingService,
            ILanguageService languageService,
            IContentModeratorService contentModeratorService,
            IAcademicSearchService academicSearchService,
            IWebLanguageModelService webLanguageModelService,
            ISpeakerIdentificationService speakerIdentificationService,
            ISpeakerVerificationService speakerVerificationService,
            ITranslatorService translatorService)
        {
            VisionService = visionService;
            AutoSuggestService = autoSuggestService;
            ImageSearchService = imageSearchService;
            SpellCheckService = spellCheckService;
            WebSearchService = webSearchService;
            NewsSearchService = newsSearchService;
            VideoSearchService = videoSearchService;
            VideoService = videoService;
            EmotionService = emotionService;
            FaceService = faceService;
            LinguisticService = linguisticService;
            SentimentService = sentimentService;
            EntityLinkingService = entityLinkingService;
            LanguageService = languageService;
            ContentModeratorService = contentModeratorService;
            AcademicSearchService = academicSearchService;
            WebLanguageModelService = webLanguageModelService;
            SpeakerIdentificationService = speakerIdentificationService;
            SpeakerVerificationService = speakerVerificationService;
            TranslatorService = translatorService;
        }

        #region Giphy Moderator

        public ActionResult Moderator()
        {
            return View();
        }

        public ActionResult ModerateImage(string url)
        {
            AnalysisResult ar = VisionService.AnalyzeImage(url, new List<VisualFeature>() {VisualFeature.Adult});

            return Json(ar.Adult);
        }

        #endregion Giphy Moderator

        #region Content Moderator

        #region Evaluate

        public ActionResult ContentModeratorEvaluate()
        {
            return View("ContentModerator/Evaluate", new EvaluateResponse());
        }
        
        [HttpPost]
        public ActionResult ContentModeratorEvaluate(string url)
        {
            var result = ContentModeratorService.Evaluate(url);

            return View("ContentModerator/Evaluate", result);
        }

        #endregion Evaluate

        #region Screen

        public ActionResult ContentModeratorScreen()
        {
            return View("ContentModerator/Screen", new ScreenResponse());
        }

        [HttpPost]
        public ActionResult ContentModeratorScreen(string text)
        {
            var result = ContentModeratorService.Screen(text, autocorrect:true, PII: true);

            return View("ContentModerator/Screen", result);
        }

        #endregion Screen

        #region Job

        public ActionResult ContentModeratorJob()
        {
            return View("ContentModerator/Job", new JobResult());
        }

        public ActionResult ContentModeratorJobGet()
        {
            return ContentModeratorJob();
        }

        [HttpPost]
        public ActionResult ContentModeratorJobGet(string teamName, string jobId)
        {
            var result = ContentModeratorService.GetJob(teamName, jobId);
            return View("ContentModerator/Job", new JobResult() { Job = result });
        }

        public ActionResult ContentModeratorJobCreateImage()
        {
            return ContentModeratorJob();
        }

        [HttpPost]
        public ActionResult ContentModeratorJobCreateImage(string imageUrl, string teamName, string workflowName)
        {
            var result = ContentModeratorService.CreateImageJob(imageUrl, teamName, "", workflowName);
            return View("ContentModerator/Job", new JobResult() { JobId = result.JobId });
        }

        public ActionResult ContentModeratorJobCreateText()
        {
            return ContentModeratorJob();
        }

        [HttpPost]
        public ActionResult ContentModeratorJobCreateText(string text, string teamName, string workflowName)
        {
            var result = ContentModeratorService.CreateTextJob(text, teamName, "", workflowName);
            return View("ContentModerator/Job", new JobResult() { JobId = result.JobId });
        }

        #endregion Job

        #region Review

        public ActionResult ContentModeratorReview()
        {
            return View("ContentModerator/Review", new ReviewResult());
        }

        public ActionResult ContentModeratorReviewCreate(string teamName, string imageUrl, string contentId)
        {
            ReviewRequest r = new ReviewRequest();
            r.Content = imageUrl;
            r.ContentId = contentId;
            r.Type = ContentModeratorReviewType.Image;

            var result = ContentModeratorService.CreateReview(teamName, new List<ReviewRequest>() { r });

            return View("ContentModerator/Review", new ReviewResult() { CreateReviews = result });
        }

        public ActionResult ContentModeratorReviewGet(string teamName, string reviewId)
        {
            var result = ContentModeratorService.GetReview(teamName, reviewId);

            return View("ContentModerator/Review", new ReviewResult() { Review = result });
        }

        #endregion Review

        #region Workflow

        public ActionResult ContentModeratorWorkflow()
        {
            return View("ContentModerator/Workflow", new WorkflowResult());
        }

        public ActionResult ContentModeratorWorkflowGetAll()
        {
            return ContentModeratorWorkflow();
        }

        [HttpPost]
        public ActionResult ContentModeratorWorkflowGetAll(string teamName)
        {
            var response = ContentModeratorService.GetAllWorkflows(teamName);

            return View("ContentModerator/Workflow", new WorkflowResult() { Workflows = response });
        }

        public ActionResult ContentModeratorWorkflowGet()
        {
            return ContentModeratorWorkflow();
        }

        [HttpPost]
        public ActionResult ContentModeratorWorkflowGet(string teamName, string workflowName)
        {
            //only text or numbers. no special characters
            if(!workflowName.All(char.IsLetterOrDigit))
                return ContentModeratorWorkflow();

            var response = ContentModeratorService.GetWorkflow(teamName, workflowName);

            return View("ContentModerator/Workflow", new WorkflowResult() { Workflow = response });
        }

        public ActionResult ContentModeratorWorkflowCreate()
        {
            return ContentModeratorWorkflow();
        }

        [HttpPost]
        public ActionResult ContentModeratorWorkflowCreate(string teamName, string workflowName)
        {
            //only text or numbers. no special characters
            if (!workflowName.All(char.IsLetterOrDigit))
                return ContentModeratorWorkflow();

            WorkflowExpression we = new WorkflowExpression() { 
                Description = $"New Sample Workflow: {workflowName}",
                Type = ContentModeratorReviewType.Image,
                Expression = new Condition
                {
                    Operator = "ge",
                    OutputName = "adultscore",
                    Value = "0.1"
                }
            };

            var result = ContentModeratorService.CreateOrUpdateWorkflow(teamName, workflowName, we);
            if (!result)
                return ContentModeratorWorkflow();

            var wer = new WorkflowExpressionResponse()
            {
                Description = we.Description,
                Name = workflowName
            };

            return View("ContentModerator/Workflow", new WorkflowResult() { Workflow = wer });
        }

        #endregion Workflow

        #region List Management

        #region Image Lists

        public ActionResult ContentModeratorImageList()
        {
            return View("ContentModerator/ImageLists", new ImageListResult());
        }

        public ActionResult ContentModeratorImageAdd()
        {
            return ContentModeratorImageList();
        }

        [HttpPost]
        public ActionResult ContentModeratorImageAdd(string imageUrl, string listId)
        {
            var result = ContentModeratorService.AddImage(imageUrl, listId, ContentModeratorTag.Alcohol);

            return View("ContentModerator/ImageLists", new ImageListResult() { Image = result });
        }

        public ActionResult ContentModeratorImageGetAll()
        {
            return ContentModeratorImageList();
        }

        [HttpPost]
        public ActionResult ContentModeratorImageGetAll(string listId)
        {
            var result = ContentModeratorService.GetAllImageIds(listId);

            return View("ContentModerator/ImageLists", new ImageListResult() { Images = result });
        }

        public ActionResult ContentModeratorImageAddList()
        {
            return ContentModeratorImageList();
        }

        [HttpPost]
        public ActionResult ContentModeratorImageAddList(string name, string description)
        {
            var result = ContentModeratorService.CreateList(new ListDetails() { Name = name, Description = description });

            return View("ContentModerator/ImageLists", new ImageListResult() { List = result });
        }

        public ActionResult ContentModeratorImageGetAllLists()
        {
            return ContentModeratorImageList();
        }

        [HttpPost]
        public ActionResult ContentModeratorImageGetAllLists(string placeholder)
        {
            var result = ContentModeratorService.GetAllImageLists();

            return View("ContentModerator/ImageLists", new ImageListResult() { Lists = result });
        }

        #endregion Image Lists

        #region Term Lists

        public ActionResult ContentModeratorTermList()
        {
            return View("ContentModerator/TermLists", new TermListResult());
        }

        public ActionResult ContentModeratorTermListAdd()
        {
            return ContentModeratorTermList();
        }

        [HttpPost]
        public ActionResult ContentModeratorTermListAdd(string listId, string term)
        {
            var result = ContentModeratorService.AddTerm(listId, term, "eng");

            return View("ContentModerator/TermLists", new TermListResult() { EventOccurred = true, EventSuccess = result });
        }

        public ActionResult ContentModeratorTermListGetAll()
        {
            return ContentModeratorTermList();
        }

        [HttpPost]
        public ActionResult ContentModeratorTermListGetAll(string listId)
        {
            var result = ContentModeratorService.GetAllTerms(listId, "eng");

            return View("ContentModerator/TermLists", new TermListResult() { Terms = result });
        }

        public ActionResult ContentModeratorTermListAddList()
        {
            return ContentModeratorTermList();
        }

        [HttpPost]
        public ActionResult ContentModeratorTermListAddList(string name, string description)
        {
            var result = ContentModeratorService.CreateTermList(new ListDetails() { Name = name, Description = description });

            return View("ContentModerator/TermLists", new TermListResult() { List = result });
        }

        public ActionResult ContentModeratorTermListGetAllLists()
        {
            return ContentModeratorTermList();
        }

        [HttpPost]
        public ActionResult ContentModeratorTermListGetAllLists(string placeholder)
        {
            var result = ContentModeratorService.GetAllTermLists();
            
            return View("ContentModerator/TermLists", new TermListResult() { Lists = result });
        }

        #endregion Term Lists

        #endregion List Management

        #endregion Content Moderator

        #region Video

        public ActionResult Video()
        {
            return View("Video", new VideoResult());
        }

        [HttpPost]
        public ActionResult Video(string url)
        {
            var vr = new VideoResult();
            vr.Operation = VideoService.CreateOperation(url, new FaceDetectionOperationSettings());
            
            return View("Video", vr);
        }

        public ActionResult VideoOperation()
        {
            return Video();
        }

        [HttpPost]
        public ActionResult VideoOperation(string operationUrl)
        {
            var vr = new VideoResult();
            vr.Operation = new Operation(operationUrl);
            vr.OperationResult = VideoService.GetOperationResult(new Operation(operationUrl));
            
            return View("Video", vr);
        }

        #endregion Video

        #region Auto Suggest

        public ActionResult Suggestions()
        {
            return View();
        }

        public ActionResult GetSuggestions(string text)
        {
            var results = AutoSuggestService.GetSuggestions(text).SuggestionGroups.FirstOrDefault()?.SearchSuggestions;
            
            return Json(results.Take(5));
        }

        #endregion Auto Suggest

        #region Image Search

        public ActionResult ImageSearch()
        {
            return View();
        }

        public ActionResult GetImages(string query)
        {
            var results = ImageSearchService.GetImages(query);

            return Json(results.Value);
        }

        public ActionResult GetTrendingImages(string query)
        {
            var results = ImageSearchService.GetTrendingImages();

            return Json(results.Categories);
        }

        public ActionResult GetImageInsights(string url)
        {
            var modules = new List<ModulesRequestedOptions> {ModulesRequestedOptions.All};
            var results = ImageSearchService.GetImageInsights(imgUrl:url, modulesRequested:modules);

            return Json(results.VisuallySimilarImages);
        }

        #endregion Image Search

        #region Spell Check

        public ActionResult SpellCheck()
        {
            return View();
        }

        public ActionResult CheckSpelling(string text)
        {
            var results = SpellCheckService.SpellCheck(text);

            return Json(results.FlaggedTokens);
        }

        #endregion Spell Check

        #region Web Search

        public ActionResult WebSearch()
        {
            return View();
        }

        public ActionResult GetWebSearch(string text)
        {
            var results = WebSearchService.WebSearch(text);

            return Json(results.WebPages.Value);
        }

        #endregion Web Search

        #region News Search

        public ActionResult NewsSearch() {
            return View();
        }

        public ActionResult GetNewsSearch(string text) {
            var results = NewsSearchService.NewsSearch(text);

            return Json(results.Value);
        }

        public ActionResult GetNewsTrendSearch() {
            var results = NewsSearchService.TrendingSearch();

            return Json(results.Value.Take(10));
        }

        public ActionResult GetNewsCatSearch(string category)
        {
            var c = (NewsCategoryOptions)Enum.Parse(typeof(NewsCategoryOptions), category);

            var results = NewsSearchService.CategorySearch(c);

            return Json(results.Value);
        }
        #endregion News Search

        #region Video Search

        public ActionResult VideoSearch() {
            return View();
        }

        public ActionResult GetVideoSearch(string text) {
            var results = VideoSearchService.VideoSearch(text);

            return Json(results.Value);
        }

        public ActionResult GetVideoTrendSearch() {
            var results = VideoSearchService.TrendingSearch();

            return Json(results.Categories.Take(5));
        }

        public ActionResult GetVideoDetailsSearch(string videoId) {
            var results = VideoSearchService.VideoDetailsSearch(videoId, VideoDetailsModulesOptions.All);

            return Json(results.VideoResult);
        }

        #endregion Video Search

        #region Computer Vision

        public ActionResult ComputerVision()
        {
            return View(new ComputerVisionResult());
        }

        [HttpPost]
        public ActionResult ComputerVision(string url)
        {
            ComputerVisionResult cvr = new ComputerVisionResult();
            cvr.ImageUrl = url;
            cvr.Result = VisionService.AnalyzeImage(url, new List<VisualFeature> { VisualFeature.Faces, VisualFeature.Tags });

            return View("ComputerVision", cvr);
        }

        #endregion Computer Vision

        #region Emotion

        public ActionResult Emotion()
        {
            return View(new EmotionResult());
        }

        [HttpPost]
        public ActionResult Emotion(string url)
        {
            EmotionResult er = new EmotionResult();
            er.ImageUrl = url;
            er.Result = EmotionService.Recognize(url);

            return View("Emotion", er);
        }

        #endregion Emotion

        #region Face

        public ActionResult Face()
        {
            return View(new FaceResult());
        }

        [HttpPost]
        public ActionResult Face(string url)
        {
            FaceResult fr = new FaceResult();
            fr.ImageUrl = url;
            fr.Result = FaceService.Detect(url, returnFaceAttributes:new List<FaceAttributeType>()
            {
                FaceAttributeType.Age,
                FaceAttributeType.FacialHair,
                FaceAttributeType.HeadPose,
                FaceAttributeType.Gender,
                FaceAttributeType.Glasses,
                FaceAttributeType.Smile
            }, returnFaceLandmarks:true );

            return View("Face", fr);
        }

        #endregion Face

        #region Linguistic

        public ActionResult Linguistic()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Linguistic(string text)
        {
            TextAnalysisRequest tar = new TextAnalysisRequest()
            {
                Language = "en",
                Text = text
            };
            
            var result = LinguisticService.GetConstituencyTreeTextAnalysis(tar);
            var value = new LinguisticAnalysisResult()
            {
                FieldName = "Sample Text",
                FieldValue = text,
                POSTagsAnalysis = null,
                ConstituencyTreeAnalysis = result,
                TokensAnalysis = null
            };

            return View("Linguistic", value);
        }

        #endregion Linguistic

        #region Key Phrase

        public ActionResult KeyPhrase()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KeyPhrase(string text)
        {
            var sr = new SentimentRequest();
            
            sr.Documents.Add(new Document()
            {
                Text = text,
                Id = "Sample Text"
            });
            
            var result = SentimentService.GetKeyPhrases(sr);
            
            List<KeyPhraseAnalysisResult> fieldResults = result
                .Documents
                .Select(d => new KeyPhraseAnalysisResult()
                {
                    FieldName = d.Id,
                    FieldValue = text,
                    KeyPhraseAnalysis = d
                })
                .ToList();
            
            return View("KeyPhrase", fieldResults);
        }

        #endregion Key Phrase

        #region Link Analysis

        public ActionResult Link()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Link(string text)
        {
            var result = EntityLinkingService.Link(text);

            List<LinkAnalysisResult> fieldResults = new List<LinkAnalysisResult>()
            {
                new LinkAnalysisResult()
                {
                    EntityAnalysis = result,
                    FieldName = "Sample Text",
                    FieldValue = text
                }
            };
            
            return View("Link", fieldResults);
        }

        #endregion Link Analysis

        #region Sentiment Analysis

        public ActionResult Sentiment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sentiment(string text)
        {
            var sr = new SentimentRequest();
            sr.Documents.Add(new Document()
            {
                Text = text,
                Id = "Sample Text"
            });

            var result = SentimentService.GetSentiment(sr);
            
            return View("Sentiment", result);
        }

        #endregion Sentiment Analysis

        #region Language Analysis

        public ActionResult Language()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Language(string text)
        {
            LanguageRequest lr = new LanguageRequest();

            lr.Documents.Add(new Document()
            {
                Text = text,
                Id = "Sample Text"
            });

            var result = LanguageService.GetLanguages(lr);
            return View("Language", result);
        }

        #endregion Language Analysis

        #region Academic Search

        public ActionResult AcademicSearch()
        {
            return View("AcademicSearch", new AcademicResult());
        }

        public ActionResult AcademicSearchCalcHistogram()
        {
            return AcademicSearch();
        }

        [HttpPost]
        public ActionResult AcademicSearchCalcHistogram(string expression)
        {
            var result = AcademicSearchService.CalcHistogram(expression, AcademicModelOptions.latest, "Y,F.FN,AA.AuN");

            return View("AcademicSearch", new AcademicResult() { Histogram = result });
        }

        public ActionResult AcademicSearchEvaluate()
        {
            return AcademicSearch();
        }

        [HttpPost]
        public ActionResult AcademicSearchEvaluate(string expression)
        {
            var result = AcademicSearchService.Evaluate(expression, AcademicModelOptions.latest);

            return View("AcademicSearch", new AcademicResult() { Evaluate = result });
        }

        public ActionResult AcademicSearchGraphSearch()
        {
            return AcademicSearch();
        }

        [HttpPost]
        public ActionResult AcademicSearchGraphSearch(string author)
        {
            GraphSearchRequest g = new GraphSearchRequest()
            {
                Author = new AcademicAuthor() { 
                    Return = new AcademicReturn() { 
                        Name = author,
                        Type = "Author"
                    }
                },
                Paper = new AcademicPaper() { 
                    NormalizedTitle = "graph engine",
                    Select = new List<string>() { "OriginalTitle" },
                    Type = "Paper"
                },
                Path = "/paper/AuthorIDs/author"
            };

            var result = AcademicSearchService.GraphSearch(g);

            return View("AcademicSearch", new AcademicResult() { GraphSearch = result });
        }

        public ActionResult AcademicSearchInterpret()
        {
            return AcademicSearch();
        }

        [HttpPost]
        public ActionResult AcademicSearchInterpret(string query)
        {
            var result = AcademicSearchService.Interpret(query);
            
            return View("AcademicSearch", new AcademicResult() { Interpret = result });
        }

        public ActionResult AcademicSearchSimilarity()
        {
            return AcademicSearch();
        }

        [HttpPost]
        public ActionResult AcademicSearchSimilarity(string text, string text2)
        {
            var result = AcademicSearchService.Similarity(text, text2);

            return View("AcademicSearch", new AcademicResult() { Similarity = result, SimilarityRun = "" });
        }

        #endregion Academic Search

        #region Web Language Model

        public ActionResult WebLanguageModel()
        {
            return View("WebLanguageModel", new WebLanguageModelResult());
        }

        public ActionResult WebLanguageModelBreakIntoWords()
        {
            return WebLanguageModel();
        }

        [HttpPost]
        public ActionResult WebLanguageModelBreakIntoWords(string text)
        {
            var result = WebLanguageModelService.BreakIntoWords(WebLMModelOptions.title, text);

            return View("WebLanguageModel", new WebLanguageModelResult() { BreakIntoWords = result });
        }

        public ActionResult WebLanguageModelCalculateConditionalProbability()
        {
            return WebLanguageModel();
        }

        [HttpPost]
        public ActionResult WebLanguageModelCalculateConditionalProbability(string phrase, string word)
        {
            ConditionalProbabilityRequest c = new ConditionalProbabilityRequest()
            {
                Queries = new List<ConditionalProbabilityQuery>()
                {
                    new ConditionalProbabilityQuery()
                    {
                        Words = phrase,
                        Word = word
                    }
                }
            };
            var result = WebLanguageModelService.CalculateConditionalProbability(WebLMModelOptions.title, c);

            return View("WebLanguageModel", new WebLanguageModelResult() { ConditionalProbability = result });
        }

        public ActionResult WebLanguageModelCalculateJointProbability()
        {
            return WebLanguageModel();
        }

        [HttpPost]
        public ActionResult WebLanguageModelCalculateJointProbability(string phrase1, string phrase2)
        {
            JointProbabilityRequest j = new JointProbabilityRequest()
            {
                Queries = new List<string>() { phrase1, phrase2 }
            };
            var result = WebLanguageModelService.CalculateJointProbability(WebLMModelOptions.title, j);

            return View("WebLanguageModel", new WebLanguageModelResult() { JointProbability = result });
        }

        public ActionResult WebLanguageModelGenerateNextWords()
        {
            return WebLanguageModel();
        }

        [HttpPost]
        public ActionResult WebLanguageModelGenerateNextWords(string phrase)
        {
            var result = WebLanguageModelService.GenerateNextWords(WebLMModelOptions.title, phrase);

            return View("WebLanguageModel", new WebLanguageModelResult() { NextWords = result });
        }

        public ActionResult WebLanguageModelListAvailable()
        {
            return WebLanguageModel();
        }

        [HttpPost]
        public ActionResult WebLanguageModelListAvailable(string placeholder)
        {
            var result = WebLanguageModelService.ListAvailableModels();

            return View("WebLanguageModel", new WebLanguageModelResult() { WebLMModels = result });
        }

        #endregion Web Language Model

        #region Speaker Identification

        public ActionResult SpeakerIdentification()
        {
            return View("SpeakerIdentification", new SpeakerIdentificationResult());
        }

        public ActionResult SpeakerIdentificationGetProfiles()
        {
            return SpeakerIdentification();
        }

        [HttpPost]
        public ActionResult SpeakerIdentificationGetProfiles(string placeholder)
        {
            var result = SpeakerIdentificationService.GetProfiles();
            
            return View("SpeakerIdentification", new SpeakerIdentificationResult() { Profiles = result });
        }

        public ActionResult SpeakerIdentificationGetProfile()
        {
            return SpeakerIdentification();
        }

        [HttpPost]
        public ActionResult SpeakerIdentificationGetProfile(Guid profileId)
        {
            var result = SpeakerIdentificationService.GetProfile(profileId);
            
            return View("SpeakerIdentification", new SpeakerIdentificationResult() { Profile = result });
        }

        public ActionResult SpeakerIdentificationCreateProfile()
        {
            return SpeakerIdentification();
        }

        [HttpPost]
        public ActionResult SpeakerIdentificationCreateProfile(string locale)
        {
            var result = SpeakerIdentificationService.CreateProfile(locale);
            
            return View("SpeakerIdentification", new SpeakerIdentificationResult() { NewProfile = result });
        }

        public ActionResult SpeakerIdentificationEnroll()
        {
            return SpeakerIdentification();
        }

        [HttpPost]
        public ActionResult SpeakerIdentificationEnroll(Guid profileId, HttpPostedFileBase file)
        {
            var result = SpeakerIdentificationService.Enroll(file.InputStream, profileId);
            
            return View("SpeakerIdentification", new SpeakerIdentificationResult() { EnrollOperation = result });
        }

        public ActionResult SpeakerIdentificationCheckIdentificationStatus()
        {
            return SpeakerIdentification();
        }

        [HttpPost]
        public ActionResult SpeakerIdentificationCheckIdentificationStatus(string url)
        {
            var result = SpeakerIdentificationService.CheckIdentificationStatus(new OperationLocation() { Url = url });
            
            return View("SpeakerIdentification", new SpeakerIdentificationResult() { IdentificationStatus = result });
        }

        public ActionResult SpeakerIdentificationCheckEnrollmentStatus()
        {
            return SpeakerIdentification();
        }

        [HttpPost]
        public ActionResult SpeakerIdentificationCheckEnrollmentStatus(string url)
        {
            var result = SpeakerIdentificationService.CheckEnrollmentStatus(new OperationLocation() { Url = url });
            
            return View("SpeakerIdentification", new SpeakerIdentificationResult() { EnrollmentStatus = result });
        }

        public ActionResult SpeakerIdentificationIdentify()
        {
            return SpeakerIdentification();
        }

        [HttpPost]
        public ActionResult SpeakerIdentificationIdentify(Guid profileId, HttpPostedFileBase file)
        {
            var result = SpeakerIdentificationService.Identify(file.InputStream, new []{ profileId });

            return View("SpeakerIdentification", new SpeakerIdentificationResult() { IdentifyOperation = result });
        }

        public ActionResult SpeakerIdentificationDeleteProfile()
        {
            return SpeakerIdentification();
        }

        [HttpPost]
        public ActionResult SpeakerIdentificationDeleteProfile(Guid profileId)
        {
            SpeakerIdentificationService.DeleteProfile(profileId);

            return View("SpeakerIdentification", new SpeakerIdentificationResult());
        }

        public ActionResult SpeakerIdentificationResetEnrollments()
        {
            return SpeakerIdentification();
        }

        [HttpPost]
        public ActionResult SpeakerIdentificationResetEnrollments(Guid profileId)
        {
            SpeakerIdentificationService.ResetEnrollments(profileId);

            return View("SpeakerIdentification", new SpeakerIdentificationResult());
        }

        #endregion Speaker Identification

        #region Speaker Verification

        public ActionResult SpeakerVerification()
        {
            return View("SpeakerVerification", new SpeakerVerificationResult());
        }

        public ActionResult SpeakerVerificationCreateProfile()
        {
            return SpeakerVerification();
        }

        [HttpPost]
        public ActionResult SpeakerVerificationCreateProfile(string locale)
        {
            var result = SpeakerVerificationService.CreateProfile(locale);
            
            return View("SpeakerVerification", new SpeakerVerificationResult() { NewProfile = result });
        }

        public ActionResult SpeakerVerificationDeleteProfile()
        {
            return SpeakerVerification();
        }

        [HttpPost]
        public ActionResult SpeakerVerificationDeleteProfile(Guid profileId)
        {
            SpeakerVerificationService.DeleteProfile(profileId);
            
            return View("SpeakerVerification", new SpeakerVerificationResult());
        }

        public ActionResult SpeakerVerificationEnroll()
        {
            return SpeakerVerification();
        }

        [HttpPost]
        public ActionResult SpeakerVerificationEnroll(Guid profileId, HttpPostedFileBase file)
        {
            var result = SpeakerVerificationService.Enroll(file.InputStream, profileId);
            
            return View("SpeakerVerification", new SpeakerVerificationResult() { Enrollment = result });
        }

        public ActionResult SpeakerVerificationGetPhrases()
        {
            return SpeakerVerification();
        }

        [HttpPost]
        public ActionResult SpeakerVerificationGetPhrases(string locale)
        {
            var result = SpeakerVerificationService.GetPhrases(locale);
            
            return View("SpeakerVerification", new SpeakerVerificationResult() { Phrases = result });
        }

        public ActionResult SpeakerVerificationGetProfile()
        {
            return SpeakerVerification();
        }

        [HttpPost]
        public ActionResult SpeakerVerificationGetProfile(Guid profileId)
        {
            var result = SpeakerVerificationService.GetProfile(profileId);
            
            return View("SpeakerVerification", new SpeakerVerificationResult() { Profile = result });
        }

        public ActionResult SpeakerVerificationGetProfiles()
        {
            return SpeakerVerification();
        }

        [HttpPost]
        public ActionResult SpeakerVerificationGetProfiles(string placeholder)
        {
            var result = SpeakerVerificationService.GetProfiles();
            
            return View("SpeakerVerification", new SpeakerVerificationResult() { Profiles = result });
        }

        public ActionResult SpeakerVerificationResetEnrollments()
        {
            return SpeakerVerification();
        }

        [HttpPost]
        public ActionResult SpeakerVerificationResetEnrollments(Guid profileId)
        {
            SpeakerVerificationService.ResetEnrollments(profileId);
            
            return View("SpeakerVerification", new SpeakerVerificationResult());
        }

        public ActionResult SpeakerVerificationVerify()
        {
            return SpeakerVerification();
        }

        [HttpPost]
        public ActionResult SpeakerVerificationVerify(Guid profileId, HttpPostedFileBase file)
        {
            var result = SpeakerVerificationService.Verify(file.InputStream, profileId);

            return View("SpeakerVerification", new SpeakerVerificationResult() { Verification = result });
        }

        #endregion Speaker Verification

        #region Translator

        public ActionResult Translator()
        {
            var result = TranslatorService.GetLanguages(new List<TranslateScopeOptions>() { TranslateScopeOptions.speech });

            return View("Translator", new TranslatorResult() { Languages = result });
        }

        public ActionResult TranslatorTranslate() {
            return Translator();
        }

        [HttpPost]
        public ActionResult TranslatorTranslate(string from, string to, HttpPostedFileBase file) {

            var result = TranslatorService.Translate(from, to, file.InputStream);

            return View("Translator", new TranslatorResult() { Translation = result });
        }

        #endregion Translator
    }
}