using ShopApp.Entities;
using System.Collections.Generic;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<Category> SelectedCategories { get; set; }
        

    }
}
