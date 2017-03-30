﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.SharedSource.CognitiveServices;
using Microsoft.SharedSource.CognitiveServices.CSV;
using Sitecore.DependencyInjection;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Sitecore.SharedSource.CognitiveServices.Factories;
using Sitecore.SharedSource.CognitiveServices.Repositories;
using Microsoft.SharedSource.CognitiveServices.Repositories.Knowledge;
using Microsoft.SharedSource.CognitiveServices.Repositories.Language;
using Microsoft.SharedSource.CognitiveServices.Repositories.Speech;
using Microsoft.SharedSource.CognitiveServices.Repositories.Vision;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Sitecore.SharedSource.CognitiveServices.Models.Analysis;
using Sitecore.SharedSource.CognitiveServices.Models.Search;
using Sitecore.SharedSource.CognitiveServices.Models.Utility;
using Microsoft.SharedSource.CognitiveServices.Repositories;
using Microsoft.SharedSource.CognitiveServices.Repositories.Bing;
using Sitecore.SharedSource.CognitiveServices.Services.Knowledge;
using Sitecore.SharedSource.CognitiveServices.Services.Language;
using Sitecore.SharedSource.CognitiveServices.Services.Speech;
using Sitecore.SharedSource.CognitiveServices.Services.Vision;
using Sitecore.SharedSource.CognitiveServices.Search;
using Sitecore.SharedSource.CognitiveServices.Services.Bing;
using Sitecore.SharedSource.CognitiveServices.Services.Search;
using Sitecore.SharedSource.CognitiveServices.Ole;

namespace Sitecore.SharedSource.CognitiveServices.Configurator
{
    public class CognitiveConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            //system
            serviceCollection.AddSingleton<IApiKeys, ApiKeys>();
            serviceCollection.AddTransient<ISitecoreDataService, SitecoreDataService>();
            serviceCollection.AddTransient<IWebUtilWrapper, WebUtilWrapper>();
            serviceCollection.AddTransient<ISettingsWrapper, SettingsWrapper>();
            serviceCollection.AddTransient<ILogWrapper, LogWrapper>();
            serviceCollection.AddTransient<IRepositoryClient, RepositoryClient>();
            serviceCollection.AddTransient<ICSVParser, CSVParser>();
            serviceCollection.AddTransient<ITextTranslator, TextTranslator>();
            serviceCollection.AddTransient<IIntentProvider, IntentProvider>();

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

            //search
            serviceCollection.AddTransient<ICognitiveSearchContext, CognitiveSearchContext>();
            serviceCollection.AddTransient<ICognitiveSearchResult, CognitiveSearchResult>();
            
            serviceCollection.AddMvcControllersInCurrentAssembly();
        }
    }
}