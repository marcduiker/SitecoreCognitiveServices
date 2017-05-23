using Microsoft.Extensions.DependencyInjection;
using Microsoft.SharedSource.CognitiveServices;
using Microsoft.SharedSource.CognitiveServices.CSV;
using Microsoft.SharedSource.CognitiveServices.Repositories;
using Microsoft.SharedSource.CognitiveServices.Repositories.Bing;
using Microsoft.SharedSource.CognitiveServices.Repositories.Knowledge;
using Microsoft.SharedSource.CognitiveServices.Repositories.Language;
using Microsoft.SharedSource.CognitiveServices.Repositories.Speech;
using Microsoft.SharedSource.CognitiveServices.Repositories.Vision;
using Sitecore.DependencyInjection;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Sitecore.SharedSource.CognitiveServices.Factories;
using Sitecore.SharedSource.CognitiveServices.Repositories;
using Sitecore.SharedSource.CognitiveServices.Models.Analysis;
using Sitecore.SharedSource.CognitiveServices.Models.Search;
using Sitecore.SharedSource.CognitiveServices.Models.Utility;
using Sitecore.SharedSource.CognitiveServices.Factories.Intents;
using Sitecore.SharedSource.CognitiveServices.Services.Knowledge;
using Sitecore.SharedSource.CognitiveServices.Services.Language;
using Sitecore.SharedSource.CognitiveServices.Services.Speech;
using Sitecore.SharedSource.CognitiveServices.Services.Vision;
using Sitecore.SharedSource.CognitiveServices.Search;
using Sitecore.SharedSource.CognitiveServices.Services.Bing;
using Sitecore.SharedSource.CognitiveServices.Services.Search;
using Sitecore.SharedSource.CognitiveServices.Ole;
using Sitecore.SharedSource.CognitiveServices.Ole.Intents;

namespace Sitecore.SharedSource.CognitiveServices.Configurator
{
    public class CognitiveConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            //system
            serviceCollection.AddSingleton<IApplicationSettings, ApplicationSettings>();
            serviceCollection.AddSingleton<IApiKeys, ApiKeys>();
            serviceCollection.AddTransient<ISitecoreDataService, SitecoreDataService>();
            serviceCollection.AddTransient<IWebUtilWrapper, WebUtilWrapper>();
            serviceCollection.AddTransient<ILogWrapper, LogWrapper>();
            serviceCollection.AddTransient<IRepositoryClient, RepositoryClient>();
            serviceCollection.AddTransient<ICSVParser, CSVParser>();
            serviceCollection.AddTransient<ITextTranslator, TextTranslator>();
            serviceCollection.AddTransient<IPublishWrapper, PublishWrapper>();

            //repositories
            serviceCollection.AddTransient<IEmotionRepository, EmotionRepository>();
            serviceCollection.AddTransient<IEntityLinkingRepository, EntityLinkingRepository>();
            serviceCollection.AddTransient<IFaceRepository, FaceRepository>();
            serviceCollection.AddTransient<ILanguageRepository, LanguageRepository>();
            serviceCollection.AddTransient<ILinguisticRepository, LinguisticRepository>();
            serviceCollection.AddTransient<ISentimentRepository, SentimentRepository>();
            serviceCollection.AddTransient<ISpeakerIdentificationRepository, SpeakerIdentificationRepository>();
            serviceCollection.AddTransient<ISpeakerVerificationRepository, SpeakerVerificationRepository>();
            serviceCollection.AddTransient<IVideoRepository, VideoRepository>();
            serviceCollection.AddTransient<IVisionRepository, VisionRepository>();
            serviceCollection.AddTransient<IAutoSuggestRepository, AutoSuggestRepository>();
            serviceCollection.AddTransient<IImageSearchRepository, ImageSearchRepository>();
            serviceCollection.AddTransient<ISpellCheckRepository, SpellCheckRepository>();
            serviceCollection.AddTransient<IWebSearchRepository, WebSearchRepository>();
            serviceCollection.AddTransient<INewsSearchRepository, NewsSearchRepository>();
            serviceCollection.AddTransient<IVideoSearchRepository, VideoSearchRepository>();
            serviceCollection.AddTransient<IAcademicSearchRepository, AcademicSearchRepository>();
            serviceCollection.AddTransient<IRecommendationsRepository, RecommendationsRepository>();
            serviceCollection.AddTransient<IContentModeratorRepository, ContentModeratorRepository>();
            serviceCollection.AddTransient<IWebLanguageModelRepository, WebLanguageModelRepository>();
            serviceCollection.AddTransient<ILuisRepository, LuisRepository>();
            serviceCollection.AddTransient<ISpeechRepository, SpeechRepository>();
            serviceCollection.AddTransient<IQnAMakerRepository, QnAMakerRepository>();

            //services
            serviceCollection.AddTransient<ISearchService, SearchService>();
            serviceCollection.AddTransient<IEmotionService, EmotionService>();
            serviceCollection.AddTransient<IEntityLinkingService, EntityLinkingService>();
            serviceCollection.AddTransient<IFaceService, FaceService>();
            serviceCollection.AddTransient<ILanguageService, LanguageService>();
            serviceCollection.AddTransient<ILinguisticService, LinguisticService>();
            serviceCollection.AddTransient<ISentimentService, SentimentService>();
            serviceCollection.AddTransient<ISpeakerIdentificationService, SpeakerIdentificationService>();
            serviceCollection.AddTransient<ISpeakerVerificationService, SpeakerVerificationService>();
            serviceCollection.AddTransient<IVideoService, VideoService>();
            serviceCollection.AddTransient<IVisionService, VisionService>();
            serviceCollection.AddTransient<IAutoSuggestService, AutoSuggestService>();
            serviceCollection.AddTransient<IImageSearchService, ImageSearchService>();
            serviceCollection.AddTransient<ISpellCheckService, SpellCheckService>();
            serviceCollection.AddTransient<IWebSearchService, WebSearchService>();
            serviceCollection.AddTransient<INewsSearchService, NewsSearchService>();
            serviceCollection.AddTransient<IVideoSearchService, VideoSearchService>();
            serviceCollection.AddTransient<IAcademicSearchService, AcademicSearchService>();
            serviceCollection.AddTransient<IRecommendationsService, RecommendationsService>();
            serviceCollection.AddTransient<IContentModeratorService, ContentModeratorService>();
            serviceCollection.AddTransient<IWebLanguageModelService, WebLanguageModelService>();
            serviceCollection.AddTransient<ILuisService, LuisService>();
            serviceCollection.AddTransient<ISpeechService, SpeechService>();
            serviceCollection.AddTransient<IQnAMakerService, QnAMakerService>();

            //factory models
            serviceCollection.AddTransient<ICognitiveImageAnalysis, CognitiveImageAnalysis>();
            serviceCollection.AddTransient<ICognitiveMediaSearch, CognitiveMediaSearch>();
            serviceCollection.AddTransient<ICognitiveTextAnalysis, CognitiveTextAnalysis>();
            serviceCollection.AddTransient<IImageDescription, ImageDescription>();
            serviceCollection.AddTransient<IReanalyzeAll, ReanalyzeAll>();
            serviceCollection.AddTransient<ISetAltTagsAll, SetAltTagsAll>();
            serviceCollection.AddTransient<ICognitiveMediaSearchJsonResult, CognitiveMediaSearchJsonResult>();

            //factories
            serviceCollection.AddTransient<ICognitiveImageAnalysisFactory, CognitiveImageAnalysisFactory>();
            serviceCollection.AddTransient<ICognitiveTextAnalysisFactory, CognitiveTextAnalysisFactory>();
            serviceCollection.AddTransient<IImageDescriptionFactory, ImageDescriptionFactory>();
            serviceCollection.AddTransient<IReanalyzeAllFactory, ReanalyzeAllFactory>();
            serviceCollection.AddTransient<ISetAltTagsAllFactory, SetAltTagsAllFactory>();
            serviceCollection.AddTransient<ICognitiveMediaSearchFactory, CognitiveMediaSearchFactory>();

            //intents
            serviceCollection.AddTransient<IDefaultIntent, DefaultIntent>();
            serviceCollection.AddTransient<IGreetIntent, GreetIntent>();
            serviceCollection.AddTransient<IKickUserIntent, KickUserIntent>();
            serviceCollection.AddTransient<ILoggedInUsersIntent, LoggedInUsersIntent>();
            serviceCollection.AddTransient<IPublishIntent, PublishIntent>();
            serviceCollection.AddTransient<IVersionIntent, VersionIntent>();
            serviceCollection.AddTransient<IAboutIntent, AboutIntent>();
            serviceCollection.AddTransient<IUnlockItemsIntent, UnlockItemsIntent>();
            serviceCollection.AddTransient<ILockedItemCountIntent, LockedItemCountIntent>();

            //intent factories
            serviceCollection.AddTransient<IIntentFactory<IIntent>, DefaultIntentFactory>();
            serviceCollection.AddTransient<IIntentFactory<IIntent>, GreetIntentFactory>();
            serviceCollection.AddTransient<IIntentFactory<IIntent>, KickUserIntentFactory>();
            serviceCollection.AddTransient<IIntentFactory<IIntent>, LoggedInUsersIntentFactory>();
            serviceCollection.AddTransient<IIntentFactory<IIntent>, PublishIntentFactory>();
            serviceCollection.AddTransient<IIntentFactory<IIntent>, VersionIntentFactory>();
            serviceCollection.AddTransient<IIntentFactory<IIntent>, AboutIntentFactory>();
            serviceCollection.AddTransient<IIntentFactory<IIntent>, UnlockItemsIntentFactory>();
            serviceCollection.AddTransient<IIntentFactory<IIntent>, LockedItemCountIntentFactory>();

            //intent provider
            serviceCollection.AddSingleton<IIntentProvider, IntentProvider>();

            //search
            serviceCollection.AddTransient<ICognitiveSearchContext, CognitiveSearchContext>();
            serviceCollection.AddTransient<ICognitiveSearchResult, CognitiveSearchResult>();
            
            serviceCollection.AddMvcControllersInCurrentAssembly();
        }
    }
}