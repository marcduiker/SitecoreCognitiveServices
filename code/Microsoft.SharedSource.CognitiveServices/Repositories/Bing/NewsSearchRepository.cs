﻿using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.SharedSource.CognitiveServices.Enums;
using Microsoft.SharedSource.CognitiveServices.Models.Bing.NewsSearch;

namespace Microsoft.SharedSource.CognitiveServices.Repositories.Bing {
    public class NewsSearchRepository : INewsSearchRepository {

        public static readonly string categoryUrl = "https://api.cognitive.microsoft.com/bing/v5.0/news/";
        public static readonly string trendingUrl = "https://api.cognitive.microsoft.com/bing/v5.0/news/trendingtopics";
        public static readonly string newsUrl = "https://api.cognitive.microsoft.com/bing/v5.0/news/search";

        protected readonly IApiKeys ApiKeys;
        protected readonly IRepositoryClient RepositoryClient;

        public NewsSearchRepository(
            IApiKeys apiKeys,
            IRepositoryClient repositoryClient)
        {
            ApiKeys = apiKeys;
            RepositoryClient = repositoryClient;
        }

        #region Category Search

        public virtual NewsSearchCategoryResponse CategorySearch(NewsCategoryOptions category) {
            var catName = Enum.GetName(typeof(NewsCategoryOptions), category).Replace("USUK", "US/UK");

            var response = RepositoryClient.SendGet(ApiKeys.BingSearch, $"{categoryUrl}?Category={catName}");

            return JsonConvert.DeserializeObject<NewsSearchCategoryResponse>(response);
        }

        public virtual async Task<NewsSearchCategoryResponse> CategorySearchAsync(NewsCategoryOptions category)
        {
            var catName = Enum.GetName(typeof (NewsCategoryOptions), category).Replace("USUK", "US/UK");

            var response = await RepositoryClient.SendGetAsync(ApiKeys.BingSearch, $"{categoryUrl}?Category={catName}");

            return JsonConvert.DeserializeObject<NewsSearchCategoryResponse>(response);
        }

        #endregion Category Search

        #region Trending Search

        public virtual NewsSearchTrendResponse TrendingSearch() {
            var response = RepositoryClient.SendGet(ApiKeys.BingSearch, trendingUrl);

            return JsonConvert.DeserializeObject<NewsSearchTrendResponse>(response);
        }

        public virtual async Task<NewsSearchTrendResponse> TrendingSearchAsync() {
            var response = await RepositoryClient.SendGetAsync(ApiKeys.BingSearch, trendingUrl);

            return JsonConvert.DeserializeObject<NewsSearchTrendResponse>(response);
        }

        #endregion Trending Search

        #region News Search

        protected virtual string GetNewsSearchQuerystring(int countOffset, string languageCode, SafeSearchOptions safeSearch)
        {
            StringBuilder sb = new StringBuilder();

            if (countOffset > 0)
                sb.Append($"?countoffset={countOffset}");

            if (!string.IsNullOrEmpty(languageCode)) {
                var concat = (sb.Length > 0) ? "?" : "&";
                sb.Append($"{concat}mkt={languageCode}");
            }

            if (safeSearch != SafeSearchOptions.Off) {
                var concat = (sb.Length > 0) ? "?" : "&";
                sb.Append($"{concat}safeSearch={Enum.GetName(typeof(SafeSearchOptions), safeSearch)}");
            }

            return sb.ToString();
        }

        public virtual NewsSearchResponse NewsSearch(string text, int countOffset = 0, string languageCode = "", SafeSearchOptions safeSearch = SafeSearchOptions.Off) {
            var qs = GetNewsSearchQuerystring(countOffset, languageCode, safeSearch);

            var response = RepositoryClient.SendGet(ApiKeys.BingSearch, $"{newsUrl}?q={text}{qs}");

            return JsonConvert.DeserializeObject<NewsSearchResponse>(response);
        }

        public virtual async Task<NewsSearchResponse> NewsSearchAsync(string text, int countOffset = 0, string languageCode = "", SafeSearchOptions safeSearch = SafeSearchOptions.Off)
        {
            var qs = GetNewsSearchQuerystring(countOffset, languageCode, safeSearch);

            var response = await RepositoryClient.SendGetAsync(ApiKeys.BingSearch, $"{newsUrl}?q={text}{qs}");
            
            return JsonConvert.DeserializeObject<NewsSearchResponse>(response);
        }

        #endregion News Search
    }
}