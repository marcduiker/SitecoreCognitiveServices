using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Microsoft.SharedSource.CognitiveServices.Repositories.Vision;

namespace Sitecore.SharedSource.CognitiveServices.Services.Vision
{
    public class VisionService : IVisionService
    {
        protected IVisionRepository VisionRepository;
        protected ILogWrapper Logger;

        public VisionService(
            IVisionRepository visionRepository,
            ILogWrapper logger)
        {
            VisionRepository = visionRepository;
            Logger = logger;
        }

        #region Adult

        public virtual Adult GetAdultAnalysis(string imageUrl)
        {
            try
            {
                var result = VisionRepository.GetAdultAnalysis(imageUrl);
                
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"VisionService.GetAdultAnalysis failed on: {imageUrl}", this, ex);
            }

            return null;
        }

        public virtual Adult GetAdultAnalysis(Stream imageStream)
        {
            try
            {
                var result = VisionRepository.GetAdultAnalysis(imageStream);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.GetAdultAnalysis failed", this, ex);
            }

            return null;
        }

        #endregion Adult

        #region Categories

        public virtual Category[] GetCategoryAnalysis(string imageUrl)
        {
            try
            {
                var result = VisionRepository.GetCategoryAnalysis(imageUrl);
                
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"VisionService.GetCategoryAnalysis failed on: {imageUrl}", this, ex);
            }

            return null;
        }

        public virtual Category[] GetCategoryAnalysis(Stream imageStream)
        {
            try
            {
                var result = VisionRepository.GetCategoryAnalysis(imageStream);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.GetCategoryAnalysis failed", this, ex);
            }

            return null;
        }

        #endregion Categories

        #region Color

        public virtual Color GetColorAnalysis(string imageUrl)
        {
            try
            {
                var result = VisionRepository.GetColorAnalysis(imageUrl);
                
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"VisionService.GetColorAnalysis failed on: {imageUrl}", this, ex);
            }

            return null;
        }

        public virtual Color GetColorAnalysis(Stream imageStream)
        {
            try
            {
                var result = VisionRepository.GetColorAnalysis(imageStream);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.GetColorAnalysis failed", this, ex);
            }

            return null;
        }

        #endregion Color

        #region Face

        public virtual Face[] GetFaceAnalysis(string imageUrl)
        {
            try
            {
                var result = VisionRepository.GetFaceAnalysis(imageUrl);
                
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"VisionService.GetFaceAnalysis failed on: {imageUrl}", this, ex);
            }

            return null;
        }

        public virtual Face[] GetFaceAnalysis(Stream imageStream)
        {
            try
            {
                var result = VisionRepository.GetFaceAnalysis(imageStream);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.GetFaceAnalysis failed", this, ex);
            }

            return null;
        }

        #endregion Face

        #region Full Analysis

        public virtual AnalysisResult GetFullAnalysis(string imageUrl)
        {
            try
            {
                var result = VisionRepository.GetFullAnalysis(imageUrl);
                
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.GetFullAnalysis failed", this, ex);
            }

            return null;
        }

        public virtual AnalysisResult GetFullAnalysis(Stream imageStream)
        {
            try
            {
                var result = VisionRepository.GetFullAnalysis(imageStream);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.GetFullAnalysis failed", this, ex);
            }

            return null;
        }

        #endregion Full Analysis

        #region Image Type

        public virtual ImageType GetImageTypeAnalysis(string imageUrl)
        {
            try
            {
                var result = VisionRepository.GetImageTypeAnalysis(imageUrl);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"VisionService.GetImageTypeAnalysis failed on: {imageUrl}", this, ex);
            }

            return null;
        }

        public virtual ImageType GetImageTypeAnalysis(Stream imageStream)
        {
            try
            {
                var result = VisionRepository.GetImageTypeAnalysis(imageStream);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.GetImageTypeAnalysis failed", this, ex);
            }

            return null;
        }

        #endregion Image Type

        #region Analyze Image
        
        public virtual AnalysisResult AnalyzeImage(Stream stream, List<VisualFeature> features = null, IEnumerable<string> details = null)
        {
            try
            {
                var result =
                    VisionRepository.AnalyzeImage(stream, features, details);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.AnalyzeImage failed", this, ex);
            }

            return null;
        }

        public virtual AnalysisResult AnalyzeImage(string imageUrl, List<VisualFeature> features = null, IEnumerable<string> details = null)
        {
            try
            {
                var result = VisionRepository.AnalyzeImage(imageUrl, features, details);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"VisionService.AnalyzeImage failed on: {imageUrl}", this, ex);
            }

            return null;
        }

        #endregion Analyze Image

        #region Analyze Image In Domain

        public virtual AnalysisInDomainResult AnalyzeImageInDomain(string url, Model model)
        {
            try
            {
                var result = VisionRepository.AnalyzeImageInDomain(url, model);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.AnalyzeImageInDomain failed", this, ex);
            }

            return null;
        }

        public virtual AnalysisInDomainResult AnalyzeImageInDomain(Stream imageStream, Model model)
        {
            try
            {
                var result = VisionRepository.AnalyzeImageInDomain(imageStream, model);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.AnalyzeImageInDomain failed", this, ex);
            }

            return null;
        }

        public virtual AnalysisInDomainResult AnalyzeImageInDomain(string url, string modelName)
        {
            try
            {
                var result = VisionRepository.AnalyzeImageInDomain(url, modelName);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.AnalyzeImageInDomain failed", this, ex);
            }

            return null;
        }

        public virtual AnalysisInDomainResult AnalyzeImageInDomain(Stream imageStream, string modelName)
        {
            try
            {
                var result = VisionRepository.AnalyzeImageInDomain(imageStream, modelName);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.AnalyzeImageInDomain failed", this, ex);
            }

            return null;
        }

        #endregion Analyze Image In Domain

        #region List Models

        public virtual ModelResult ListModels()
        {
            try
            {
                var result = VisionRepository.ListModels();

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.ListModels failed", this, ex);
            }

            return null;
        }

        #endregion List Models

        #region Describe
        
        public virtual Description Describe(string url, int maxCandidates = 1)
        {
            Assert.IsNotNull(url, GetType());

            try
            {
                var result = VisionRepository.Describe(url, maxCandidates).Description;

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.Describe failed", this, ex);
            }

            return null;
        }

        public virtual Description Describe(Stream imageStream, int maxCandidates = 1)
        {
            Assert.IsNotNull(imageStream, GetType());

            try
            {
                var result = VisionRepository.Describe(imageStream, maxCandidates).Description;

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.Describe failed", this, ex);
            }

            return null;
        }

        #endregion Describe

        #region Get Thumbnail
        
        public virtual byte[] GetThumbnail(string url, int width, int height, bool smartCropping = true)
        {
            try
            {
                var result = VisionRepository.GetThumbnail(url, width, height, smartCropping);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.GetThumbnail failed", this, ex);
            }

            return null;
        }

        public virtual byte[] GetThumbnail(Stream stream, int width, int height, bool smartCropping = true)
        {
            try
            {
                var result = VisionRepository.GetThumbnail(stream, width, height, smartCropping);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.GetThumbnail failed", this, ex);
            }

            return null;
        }

        #endregion Get Thumbnail

        #region Recognize Text

        public virtual OcrResults RecognizeText(Stream stream, string language = "unk", bool detectOrientation = true)
        {
            try
            {
                var result = VisionRepository.RecognizeText(stream, language, detectOrientation);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.RecognizeText failed", this, ex);
            }

            return null;
        }

        public virtual OcrResults RecognizeText(string imageUrl, string language = "unk", bool detectOrientation = true)
        {
            try
            {
                var result = VisionRepository.RecognizeText(imageUrl, language, detectOrientation);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"VisionService.RecognizeText failed on: {imageUrl}", this, ex);
            }

            return null;
        }
        
        #endregion Recognize Text

        #region Get Tags

        public virtual Tag[] GetTags(string url)
        {
            try
            {
                var result = VisionRepository.GetTags(url).Tags;

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.GetTags failed", this, ex);
            }

            return null;
        }
        
        public virtual Tag[] GetTags(Stream imageStream)
        {
            try
            {
                var result = VisionRepository.GetTags(imageStream).Tags;

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("VisionService.GetTags failed", this, ex);
            }

            return null;
        }

        #endregion Get Tags
    }
}