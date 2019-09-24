using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models
{
    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {
    }
    public partial class ProductMetaData
    {
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
    }
}