using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeAPI.Models
{
    public class AddProductModel
    {
        [Required]
        [StringLength(200)]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [Required]
        [DisplayName("Product Price")]
        public decimal ProductPrice { get; set; }

        [Required]
        [StringLength(500)]
        [DisplayName("Product Description")]
        public string ProductDescription { get; set; }

        [DisplayName("Product Image")]
        public IFormFile ProductImage { get; set; }
    }
}
