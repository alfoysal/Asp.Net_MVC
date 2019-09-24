using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.ViewModels
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage = "The Product Name cannot be Empty")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a Product Name between 3 and 30 characters in length")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Please enter a Product Name made up of letters and numbers only")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "The product description cannot be blank")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Please enter a product description between 5 and 200 characters in length")]
        [RegularExpression(@"^[,;a-zA-Z0-9'-'\s]*$", ErrorMessage = "Please enter a product description made up of letters and numbers only")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "The price cannot be Empty")]
        [Range(10, 100000, ErrorMessage = "Please enter a price between 10.00 and 100000.00")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Price { get; set; }


        [Display(Name = "Category")]
        public int CategoryID { get; set; }
        public SelectList CategoryList { get; set; }

        [Display(Name = "Brand")]
        public int BrandID { get; set; }
        public SelectList BrandList { get; set; }
        public List<SelectList> ImageLists { get; set; }
        public string[] ProductImages { get; set; }
    }
}