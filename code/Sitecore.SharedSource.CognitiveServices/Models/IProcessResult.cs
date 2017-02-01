﻿using Microsoft.ProjectOxford.EntityLinking.Contract;
using Microsoft.ProjectOxford.Text.Language;
using Microsoft.ProjectOxford.Text.Sentiment;
using Microsoft.ProjectOxford.Vision.Contract;

namespace Sitecore.SharedSource.CognitiveServices.Models
{
    public interface IProcessResult
    {
        int ItemCount { get; set; }
        string ItemId { get; set; }
        string Database { get; set; }
        string Language { get; set; }
    }
}