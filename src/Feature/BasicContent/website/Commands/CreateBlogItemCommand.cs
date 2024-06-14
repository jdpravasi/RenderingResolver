using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;


namespace RenderingResolver.Commands
{
    public class CreateBlogItemCommand : Command
    {
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");

            Item parentItem = context.Items[0];

            TemplateID templateId = new TemplateID(new ID("{CEBAAA32-3150-4CD1-AE05-D3B5FDE425BC}"));

            parentItem.Add("New Blog Item", templateId);
        }
    }
}