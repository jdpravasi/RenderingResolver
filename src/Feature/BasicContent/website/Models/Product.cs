using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RenderingResolver.Models
{
    public class Product
    {
        public int id { get; set; }
        public string productCategory { get; set; }
        public string productImage { get; set; }
        public string productName { get; set; }
        public string productRatings { get; set; }
    }
}