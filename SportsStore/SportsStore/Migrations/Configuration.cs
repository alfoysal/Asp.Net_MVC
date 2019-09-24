namespace SportsStore.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<SportsStore.Models.StoreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SportsStore.Models.StoreContext";
        }

        protected override void Seed(SportsStore.Models.StoreContext context)
        {
            var categories = new List<Category>
            {
            new Category { CategoryName = "Hocky" },
            new Category { CategoryName = "HandBall" },
            };
            categories.ForEach(c => context.Categories.AddOrUpdate(p => p.CategoryName, c));
            context.SaveChanges();

            var brands = new List<Brand>
            {
            new Brand { Brandname = "Nike" },
            new Brand { Brandname = "Adidas" },
            new Brand { Brandname = "Mitre" },
            new Brand { Brandname = "Voit" },
            new Brand { Brandname= "Spalding" },
            new Brand { Brandname= "Wilson" }
            };
            brands.ForEach(c => context.Brands.AddOrUpdate(p => p.Brandname, c));
            context.SaveChanges();

            //var products = new List<Product>
            //{
            //    new Product { ProductName = "Soccer Ball", Description="For sleeping or general wear",
            //      Price=4.99M, CategoryID=categories.Single( c => c.CategoryName == "Football").CategoryID,BrandID=brands.Single( c => c.Brandname == "Adidas").BrandID }
            //};
            //products.ForEach(c => context.Products.AddOrUpdate(p => p.ProductName, c));
            //context.SaveChanges();
            
        }
    }
}
