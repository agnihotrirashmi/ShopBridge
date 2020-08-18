using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopBridge.Models
{
    public class AddProductModel
    {
        [Required]
        [StringLength(200)]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [Required]
        [DisplayName("Product Price")]
        public decimal? ProductPrice { get; set; }

        [Required]
        [StringLength(500)]
        [DisplayName("Product Description")]
        public string ProductDescription { get; set; }

        [DisplayName("Product Image")]
        [DataType(DataType.Upload)]
        [FromForm(Name = "file")]
        public IFormFile file { get; set; }
    }

    public class Product
    {
        [Required]
        [DisplayName("Product Id")]
        public long ProductId { get; set; }

        [Required]
        [StringLength(200)]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [Required]
        [DisplayName("Product Price")]
        public decimal? ProductPrice { get; set; }

        [Required]
        [StringLength(500)]
        [DisplayName("Product Description")]
        public string ProductDescription { get; set; }

        [DisplayName("Product Image")]
        public string ProductImage { get; set; }

        [DisplayName("Product Created On")]
        public DateTime? CreatedOn { get; set; }
    }
     
    public class JqueryDatatableParam
    {
        public int Draw { get; set; }
        public DTColumn[] Columns { get; set; }
        public DTOrder[] Order { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public DTSearch Search { get; set; }
        public string SortOrder
        {
            get
            {
                return Columns != null && Order != null && Order.Length > 0
                    ? (Columns[Order[0].Column].Data + (Order[0].Dir == DTOrderDir.DESC ? " " + Order[0].Dir : string.Empty))
                    : null;
            }
        }
    }

    public class DTColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public DTSearch Search { get; set; }
    }

    public class DTOrder
    {
        public int Column { get; set; }
        public DTOrderDir Dir { get; set; }
    }

    public enum DTOrderDir
    {
        ASC,
        DESC
    }

    public class DTSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class ProductDatatable
    {
        public long TotalRecords { get; set; }
        public long TotalDisplayRecord { get; set; }
        public List<Product> Products { get; set; }
    }

    public class AjaxResposnse
    {
        public string Message { get; set; }
        public string URL { get; set; }
        [DefaultValue(false)]
        public bool Success { get; set; }
    }

    public class ProductListFilter
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public string Search { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
    }
}
