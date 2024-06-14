using Newtonsoft.Json;
using RenderingResolver.Models;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using System.Collections.Generic;
using System.Net.Http;
using Sitecore.Data.Items;
using System.Net;
namespace RenderingResolver.Commands
{
    public class SyncProducts : Command
    {
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");

            CreateItemsFromApiData();
        }
       
        private void CreateItemsFromApiData()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (HttpClient client = new HttpClient())
            {
                // Make the asynchronous call synchronously
                HttpResponseMessage response = client.GetAsync("https://localhost:7189/Product/GetProducts").ConfigureAwait(false).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    List<Product> products = JsonConvert.DeserializeObject<List<Product>>(json);

                    Database masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
                    Item parentItem = masterDb.GetItem(new ID("{DFB6DAA8-C71B-496F-BC27-FF8F24D619E1}")); 

                    foreach (Product product in products)
                    {
                        CreateItem(product, parentItem);
                    }
                }
            }
        }

        private void CreateItem(Product product, Item parentItem)
        {
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                TemplateID templateId = new TemplateID(new ID("{22A81966-9AE7-495D-9BA8-FFB16909E8C7}")); 
                Item newItem = parentItem.Add(ItemUtil.ProposeValidItemName(product.productName), templateId);

                newItem.Editing.BeginEdit();
                try
                {
                    newItem.Fields[new ID("{3E64848E-E2BA-47A9-A3CA-D9F859B5A2B4}")].Value = product.id.ToString(); 
                    newItem.Fields[new ID("{88202E4F-0FDB-4E1E-A6DD-EA05E15C5437}")].Value = product.productCategory; 
                    newItem.Fields[new ID("{C5EA5D83-F889-460B-B20E-27F23A1A68FE}")].Value = product.productImage; 
                    newItem.Fields[new ID("{B58C15C9-DBAF-4ED7-A62B-437F9885DADD}")].Value = product.productName; 
                    newItem.Fields[new ID("{461B8CFB-3EC0-4D6F-8E13-7AB2C3F60FE0}")].Value = product.productRatings; 
                    newItem.Editing.EndEdit();
                }
                catch
                {
                    newItem.Editing.CancelEdit();
                    throw;
                }
            }
        }
    }
}