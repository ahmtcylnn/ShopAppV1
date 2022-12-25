using ShopApp.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        //[Required]
        //[StringLength(60, MinimumLength =5, ErrorMessage ="Ürün İsmi 5-60 karakter uzunluğunda olmalıdır.")]
        public string Name { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        [StringLength(10000, MinimumLength = 15, ErrorMessage = "Ürün Açıklaması 15-100 karakter uzunluğunda olmalıdır.")]
        public string Description { get; set; }
        [Required (ErrorMessage ="Fiyat Belirtiniz.")]
        [Range(1,10000)]
        public decimal? Price { get; set; }
        public List<Category> SelectedCategories { get; set; }
        

    }
}
