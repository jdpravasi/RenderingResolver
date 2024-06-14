using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RenderingResolver.RenderingResolvers.News
{
    public class BreakingNewsListResolver : RenderingContentsResolver
    {
        private List<Item> items = new List<Item>();

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            Assert.ArgumentNotNull(rendering, nameof(rendering));
            Assert.ArgumentNotNull(renderingConfig, nameof(renderingConfig));

            Item ds = GetContextItem(rendering, renderingConfig);

            var childrenItems = ds.GetChildren().Where(i => i.TemplateID == Templates.News.content.TemplateID && i.Fields[Templates.News.content.Category].Value == "Breaking News").ToList();

            if (!childrenItems.Any())
                return null;

            JObject jobject = new JObject()
            {
                ["items"] = (JToken)new JArray()
            };

            jobject["items"] = ProcessItems(childrenItems, rendering, renderingConfig);
            return jobject;
        }
    }
}