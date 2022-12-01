using Microsoft.EntityFrameworkCore;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();

            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if(context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(Categories);
                }
                if(context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                }
                context.SaveChanges();
            }

        }
        private static Category[] Categories ={
            new Category() { CategoryName="Telefon"},
            new Category() { CategoryName="Bilgisayar"},
            
        };
        private static Product[] Products = { 
            new Product () { Name="Iphone 11 Pro",Price=1000,ImageUrl="1.jpg"},
            new Product () { Name="Iphone 12 Pro",Price=2000,ImageUrl="2.jpg"},
            new Product () { Name="Iphone 13 Pro",Price=3000,ImageUrl="3.jpg"},
            new Product () { Name="Iphone X ",Price=500,ImageUrl="4.jpg"},
            new Product () { Name="Iphone 11 ",Price=700,ImageUrl="5.jpg"},
        };
    }
}
