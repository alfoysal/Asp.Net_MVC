using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.Models
{
    public class ProductImageMapping
    {
        public int ProductImageMappingID { get; set; }
        public int ImageNumber { get; set; }
        public int ProductID { get; set; }
        public int ProductImageID { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductImage ProductImage { get; set; }
    }
}