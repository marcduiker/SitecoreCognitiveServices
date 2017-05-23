using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Sitecore.Data.Managers;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Sitecore.SharedSource.CognitiveServices.Models.Ole;
using Sitecore.SharedSource.CognitiveServices.Ole;
using Sitecore.SharedSource.CognitiveServices.Services.Language;

namespace Sitecore.SharedSource.CognitiveServices.Controllers {

    public class CognitiveBotController : Controller
    {
        protected readonly IIntentProvider IntentProvider;
        protected readonly ILuisService LuisService;
        protected readonly IWebUtilWrapper WebUtil;
        protected readonly IApplicationSettings ApplicationSettings;

        protected readonly ItemContextParameters Parameters;

        protected readonly Guid AppId;

        public CognitiveBotController(
            IIntentProvider intentProvider, 
            ILuisService luisService,
            IWebUtilWrapper webUtil,
            IApplicationSettings applicationSettings)
        {
            IntentProvider = intentProvider;
            LuisService = luisService;
            WebUtil = webUtil;
            ApplicationSettings = applicationSettings;

            AppId = ApplicationSettings.OleApplicationId;

            Parameters = new ItemContextParameters()
            {
                Id = WebUtil.GetQueryString("id"),
                Language = WebUtil.GetQueryString("language"),
                Database = WebUtil.GetQueryString("db")
            };

            ThemeManager.GetImage("Office/32x32/man_8.png", 32, 32);
        }

        public ActionResult OleChat()
        {
            return View("OleChat", Parameters);
        }

        public ActionResult Post([FromBody] Activity activity)
        {
            var s = JsonConvert.SerializeObject(activity.ChannelData);
            var d = JsonConvert.DeserializeObject<List<string>>(s);
            ItemContextParameters parameters = (d.Any())
                ? JsonConvert.DeserializeObject<ItemContextParameters>(d[0])
                : new ItemContextParameters();

            if (activity.Type == ActivityTypes.Message)
                return HandleMessage(activity, parameters);

            return null;
        }

        public ActionResult HandleMessage(Activity activity, ItemContextParameters parameters) { 

            var result = LuisService.Query(AppId, activity.Text); // determine which intent to use
            var intent = IntentProvider.GetIntent(AppId, result.TopScoringIntent.Intent); // try to find the matching intent object

            var text = (intent != null) // respond with the selected intent or fallback to default
                ? intent.Respond(result, parameters)
                : IntentProvider.GetDefaultResponse(AppId);

            var reply = activity.CreateReply(text, "en-US");
            return Json(reply);
        }
    }
}