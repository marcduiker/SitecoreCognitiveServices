using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.Collections.Generic;
using Microsoft.SharedSource.CognitiveServices.Models.Vision.Computer;

namespace Microsoft.SharedSource.CognitiveServices.Repositories.Vision
{
    public interface IVisionRepository
    {
        Task<AnalysisResult> GetFullAnalysisAsync(Stream stream);
        AnalysisResult GetFullAnalysis(Stream stream);
        Task<AnalysisResult> GetFullAnalysisAsync(string imageUrl);
        AnalysisResult GetFullAnalysis(string imageUrl);
        Task<Adult> GetAdultAnalysisAsync(Stream stream);
        Adult GetAdultAnalysis(Stream stream);
        Task<Adult> GetAdultAnalysisAsync(string imageUrl);
        Adult GetAdultAnalysis(string imageUrl);
        Task<Category[]> GetCategoryAnalysisAsync(Stream stream);
        Category[] GetCategoryAnalysis(Stream stream);
        Task<Category[]> GetCategoryAnalysisAsync(string imageUrl);
        Category[] GetCategoryAnalysis(string imageUrl);
        Task<Color> GetColorAnalysisAsync(Stream stream);
        Color GetColorAnalysis(Stream stream);
        Task<Color> GetColorAnalysisAsync(string imageUrl);
        Color GetColorAnalysis(string imageUrl);
        Task<Face[]> GetFaceAnalysisAsync(Stream stream);
        Face[] GetFaceAnalysis(Stream stream);
        Task<Face[]> GetFaceAnalysisAsync(string imageUrl);
        Face[] GetFaceAnalysis(string imageUrl);
        Task<ImageType> GetImageTypeAnalysisAsync(Stream stream);
        ImageType GetImageTypeAnalysis(Stream stream);
        Task<ImageType> GetImageTypeAnalysisAsync(string imageUrl);
        ImageType GetImageTypeAnalysis(string imageUrl);
        string GetAnalyzeQuerystring(IEnumerable<VisualFeature> visualFeatures, IEnumerable<string> details);
        Task<AnalysisResult> AnalyzeImageAsync(string imageUrl, IEnumerable<VisualFeature> visualFeatures = null, IEnumerable<string> details = null);
        AnalysisResult AnalyzeImage(string imageUrl, IEnumerable<VisualFeature> visualFeatures = null, IEnumerable<string> details = null);
        Task<AnalysisResult> AnalyzeImageAsync(Stream imageStream, IEnumerable<VisualFeature> visualFeatures = null, IEnumerable<string> details = null);
        AnalysisResult AnalyzeImage(Stream imageStream, IEnumerable<VisualFeature> visualFeatures = null, IEnumerable<string> details = null);
        Task<AnalysisInDomainResult> AnalyzeImageInDomainAsync(string url, Model model);
        AnalysisInDomainResult AnalyzeImageInDomain(string url, Model model);
        Task<AnalysisInDomainResult> AnalyzeImageInDomainAsync(string url, string modelName);
        AnalysisInDomainResult AnalyzeImageInDomain(string url, string modelName);
        Task<AnalysisInDomainResult> AnalyzeImageInDomainAsync(Stream imageStream, Model model);
        AnalysisInDomainResult AnalyzeImageInDomain(Stream imageStream, Model model);
        Task<AnalysisInDomainResult> AnalyzeImageInDomainAsync(Stream imageStream, string modelName);
        AnalysisInDomainResult AnalyzeImageInDomain(Stream imageStream, string modelName);
        Task<ModelResult> ListModelsAsync();
        ModelResult ListModels();
        Task<AnalysisResult> DescribeAsync(string url, int maxCandidates = 1);
        AnalysisResult Describe(string url, int maxCandidates = 1);
        Task<AnalysisResult> DescribeAsync(Stream imageStream, int maxCandidates = 1);
        AnalysisResult Describe(Stream imageStream, int maxCandidates = 1);
        Task<byte[]> GetThumbnailAsync(string url, int width, int height, bool smartCropping = true);
        byte[] GetThumbnail(string url, int width, int height, bool smartCropping = true);
        Task<byte[]> GetThumbnailAsync(Stream stream, int width, int height, bool smartCropping = true);
        byte[] GetThumbnail(Stream stream, int width, int height, bool smartCropping = true);
        Task<OcrResults> RecognizeTextAsync(string imageUrl, string languageCode = "unk", bool detectOrientation = true);
        OcrResults RecognizeText(string imageUrl, string languageCode = "unk", bool detectOrientation = true);
        Task<OcrResults> RecognizeTextAsync(Stream imageStream, string languageCode = "unk", bool detectOrientation = true);
        OcrResults RecognizeText(Stream imageStream, string languageCode = "unk", bool detectOrientation = true);
        Task<AnalysisResult> GetTagsAsync(string imageUrl);
        AnalysisResult GetTags(string imageUrl);
        Task<AnalysisResult> GetTagsAsync(Stream imageStream);
        AnalysisResult GetTags(Stream imageStream);
        Task<HandwrittenTextResponse> GetHandwrittenTextOperationResultAsync(string operationId);
        HandwrittenTextResponse GetHandwrittenTextOperationResult(string operationId);
        Task<string> RecognizeHandwrittenTextAsync(string imageUrl, bool handwriting = false);
        string RecognizeHandwrittenText(string imageUrl, bool handwriting = false);
    }
}
