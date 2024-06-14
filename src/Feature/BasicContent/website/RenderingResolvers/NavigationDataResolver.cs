using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using RenderingResolver;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Diagnostics;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;

namespace RenderingResolver
{
    public class NavigationDataResolver : RenderingContentsResolver
    {
        private List<Item> items = new List<Item>();

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            Assert.ArgumentNotNull(rendering, nameof(rendering));
            Assert.ArgumentNotNull(renderingConfig, nameof(renderingConfig));

            Item ds = GetContextItem(rendering, renderingConfig);

            var childrenItems = ds.GetChildren().Where(i => i.TemplateID == Templates.Navigation.content.TemplateID && i.Fields["{B2E7F3D5-3FD3-4BEC-90CD-79ED6632A006}"].Value == "1").ToList();


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
