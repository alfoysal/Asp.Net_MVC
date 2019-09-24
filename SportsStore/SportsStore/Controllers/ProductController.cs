using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SportsStore.Models;
using SportsStore.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Product
        public ActionResult Index(string category, string search,string sortBy, int? page)
        {
            //instantiate a new view model
            ProductIndexViewModel viewModel = new ProductIndexViewModel();
            //select the products
            var products = db.Products.Include(p => p.Brand).Include(p => p.Category);
            //perform the search and save the search string to the viewModel
           
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.ProductName.Contains(search) || p.Description.Contains(search) || p.Category.CategoryName.Contains(search));
                viewModel.Search = search;
            }
            //group search results into categories and count how many items in each category
            viewModel.CatsWithCount = from matchingProducts in products
                                      where
                                      matchingProducts.CategoryID != null
                                      group matchingProducts by matchingProducts.Category.CategoryName into catGroup
                                      select new CategoryWithCount()
                                      {
                                          CategoryName = catGroup.Key,
                                          ProductCount = catGroup.Count()
                                      };
            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.CategoryName == category);
                viewModel.Category = category;
            }

            //sort the results
            switch (sortBy)
            {
                case "price_lowest":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_highest":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }
            //const int PageItems = 3;
            int currentPage = (page ?? 1);
            viewModel.Products = products.ToPagedList(currentPage, Constants.PageItems);
            viewModel.SortBy = sortBy;

            viewModel.Sorts = new Dictionary<string, string>
            {
                {"Price low to high", "price_lowest" },
                {"Price high to low", "price_highest" }
            };

            return View(viewModel);
        }

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.BrandList = new SelectList(db.Brands, "BrandID", "Brandname");
            viewModel.CategoryList = new SelectList(db.Categories, "CategoryID", "CategoryName");
            viewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ProductImageID", "FileName"));
            }
            return View(viewModel);
            
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( ProductViewModel viewModel)
        {
            Product product = new Product();
            product.ProductName = viewModel.ProductName;
            product.Description = viewModel.Description;
            product.Price = viewModel.Price;
            product.CategoryID = viewModel.CategoryID;
            product.BrandID = viewModel.BrandID;
            product.ProductImageMappings = new List<ProductImageMapping>();
            //get a list of selected images without any blanks
            string[] productImages = viewModel.ProductImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
            for (int i = 0; i < productImages.Length; i++)
            {
                product.ProductImageMappings.Add(new ProductImageMapping
                {
                    ProductImage = db.ProductImages.Find(int.Parse(productImages[i])),
                    ImageNumber = i
                });
            }
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            viewModel.CategoryList = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            viewModel.BrandList = new SelectList(db.Brands, "BrandID", "Brandname", product.BrandID);
            viewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ProductImageID", "FileName", viewModel.ProductImages[i]));
            }
            return View(viewModel);

        }

        // GET: Product/Edit/5
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.CategoryList = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            viewModel.BrandList = new SelectList(db.Brands, "BrandID", "Brandname", product.BrandID);
            viewModel.ImageLists = new List<SelectList>();
            foreach (var imageMapping in product.ProductImageMappings.OrderBy(pim => pim.ImageNumber))
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ProductImageID", "FileName",
                imageMapping.ProductImageID));
            }
            for (int i = viewModel.ImageLists.Count; i < Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ProductImageID", "FileName"));
            }
            viewModel.ProductID = product.ProductID;
            viewModel.ProductName = product.ProductName;
            viewModel.Description = product.Description;
            viewModel.Price = product.Price;
            return View(viewModel);

        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel viewModel)
        {
            //confuse in ProductID
            var productToUpdate = db.Products.Include(p => p.ProductImageMappings).Where(p => p.ProductID == viewModel.ProductID).Single();
            if (TryUpdateModel(productToUpdate, "", new string[] { "ProductName", "Description", "Price","CategoryID", "BrandID" }))
            {
                if (productToUpdate.ProductImageMappings == null)
                {
                    productToUpdate.ProductImageMappings = new List<ProductImageMapping>();
                }
                //get a list of selected images without any blanks
                string[] productImages = viewModel.ProductImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
                for (int i = 0; i < productImages.Length; i++)
                {
                    //get the image currently stored
                    var imageMappingToEdit = productToUpdate.ProductImageMappings.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                    //find the new image
                    var image = db.ProductImages.Find(int.Parse(productImages[i]));
                    //if there is nothing stored then we need to add a new mapping
                    if (imageMappingToEdit == null)
                    {
                        //add image to the imagemappings
                        productToUpdate.ProductImageMappings.Add(new ProductImageMapping
                        {
                            ImageNumber = i,
                            ProductImage = image,
                            ProductImageID = image.ProductImageID
                        });
                    }
                    //else it's not a new file so edit the current mapping
                    else
                    {
                        //ProductImageMappingIDp(164)
                        //if they are not the same
                        if (imageMappingToEdit.ProductImageID != int.Parse(productImages[i]))
                        {
                            //assign image property of the image mapping
                            imageMappingToEdit.ProductImage = image;
                        }
                    }
                }
                //delete any other imagemappings that the user did not include in their
                //selections for the product
                for (int i = productImages.Length; i < Constants.NumberOfProductImages; i++)
                {
                    var imageMappingToEdit = productToUpdate.ProductImageMappings.Where(pim =>
                    pim.ImageNumber == i).FirstOrDefault();
                    //if there is something stored in the mapping
                    if (imageMappingToEdit != null)
                    {
                        //delete the record from the mapping table directly.
                        //just calling productToUpdate.ProductImageMappings.Remove(imageMappingToEdit)
                        //results in a FK error
                        db.ProductImageMappings.Remove(imageMappingToEdit);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Product/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
