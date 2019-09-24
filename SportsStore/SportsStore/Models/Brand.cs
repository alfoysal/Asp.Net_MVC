using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.Models
{
    public class Brand
    {
        public int BrandID { get; set; }
        public string Brandname { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}