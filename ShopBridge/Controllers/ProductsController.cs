using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShopBridge.Models;

namespace ShopBridge.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductsController : DefaultController
    {
        public ProductsController(IConfiguration config) : base(config)
        {
        }

        /// <summary>
        /// Listing of products
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> List()
        {
            return View();
        }

        /// <summary>
        /// Get records for datatable
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        //[HttpPost]
        public async Task<ActionResult> GetList(JqueryDatatableParam param)
        {
            ProductDatatable productDatatable = new ProductDatatable();
            try
            {
                ProductListFilter model = new ProductListFilter
                {
                    Start = param.Start,
                    Length = param.Length,
                    Search = param.Search.Value,
                    SortColumn = Convert.ToString(param.Columns[param.Order[0].Column].Data),
                    SortOrder = Convert.ToString(param.Order[0].Dir)
                };

                ResJsonOutput result = await PostDataAsync("/API/Products/GetProductList", model);
                if (result.Status.IsSuccess)
                {
                    productDatatable = Utility.ConvertJsonToObject<ProductDatatable>(result.Data);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(new
            {
                draw = param.Draw,
                data = productDatatable.Products,
                recordsFiltered = productDatatable.TotalRecords,
                recordsTotal = productDatatable.TotalDisplayRecord
            });
        }

        /// <summary>
        /// Get method of add product
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Add()
        {
            try
            {
                Product product = new Product();
                return View(product);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add product 
        /// </summary>
        /// <param name="model">Product name, product owner, product price</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add([FromForm] AddProductModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ResJsonOutput result = await PostDataAsync("/API/Products/AddProduct", model);
                    if (result.Status.IsSuccess)
                    {
                        return Json(new AjaxResposnse {Success = true, Message = "Product added successfully.", URL = Url.Action("List", "Products") });
                    }
                    else
                    {
                        return Json(new AjaxResposnse { Message = result.Status.Message });
                    }
                }
                else
                {
                    ViewBag.ErrMsg = GetModelStateErrors(ModelState);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View(model);
        }

        /// <summary>
        /// Get method of product details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Details(long id)
        {
            Product product = new Product();
            try
            {
                ResJsonOutput result = await PostDataAsync("/API/Products/GetProductDetails", new RequestById { ProductId = id });
                if (result.Status.IsSuccess)
                {
                    product = Utility.ConvertJsonToObject<Product>(result.Data);
                }
                else
                {
                    return Json(new AjaxResposnse { Message = result.Status.Message });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View(product);
        }

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="model">ProductId</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Delete(RequestById model)
        {
            ResJsonOutput result = new ResJsonOutput();
            try
            {
                if (ModelState.IsValid)
                {
                    result = await PostDataAsync("/API/Products/DeleteProduct", new RequestById { ProductId = model.ProductId});
                    if (result.Status.IsSuccess)
                    {
                        return Json(new { Success = true, Message = "Product deleted successfully." });
                    }
                }
                else
                {
                    ViewBag.ErrMsg = GetModelStateErrors(ModelState);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(new AjaxResposnse { Message = result.Status.Message });
        }

        /// <summary>
        /// For Disposing db context object
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}