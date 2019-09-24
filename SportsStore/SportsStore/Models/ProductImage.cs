using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public class ProductImage
    {
        public int ProductImageID { get; set; }
        [Display(Name = "File")]
        [StringLength(100)]
        [Index(IsUnique =true)]
        public string FileName { get; set; }
        public virtual ICollection<ProductImageMapping> ProductImageMappings { get; set; }
    }
}