using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShopBridgeAPI.Models;
using ShopBridgeAPI.Utility;
using ShopBridgeData.Contracts;
using ShopBridgeData.DBModels;

namespace ShopBridgeAPI.Controllers
{
    /// <summary>
    /// Products Service
    /// </summary>
    [Route("API/Products")]
    public class ProductsController : Controller
    {
        private IRepository<Product> product;
        private IRepository<ErrorLog> log;

        /// <summary>
        /// Product Constructor
        /// </summary>
        /// <param name="product"></param>
        /// <param name="log"></param>
        public ProductsController(IRepository<Product> product, IRepository<ErrorLog> log)
        {
            this.product = product;
            this.log = log;
        }

        /// <summary>
        /// Get Product List
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Route("GetProductList"), HttpPost]
        public ActionResult<ResJsonOutput> GetProductList([FromBody] ProductListFilter param)
        {
            ResJsonOutput result = new ResJsonOutput();
            try
            {
                ProductDatatable productDatatable = new ProductDatatable();
                List<Product> list = product.GetAll().ToList();
                if (list != null && list.Count() > 0)
                {
                    productDatatable.TotalDisplayRecord = productDatatable.TotalRecords = list.Count();
                    if (!string.IsNullOrEmpty(param.Search))
                    {
                        list = list.Where(x => x.ProductName.ToLower().Contains(param.Search.ToLower())
                                                      || x.ProductPrice.ToString().ToLower().Contains(param.Search.ToLower())
                                                       ).ToList();
                        productDatatable.TotalRecords = productDatatable.TotalDisplayRecord = list.Count();
                    }

                    list = SortedDataByColumn(list, param.SortColumn, param.SortOrder);
                    list = list.Skip(param.Start).Take(param.Length).ToList();
                }
                productDatatable.Products = list;
                result.Data = productDatatable;
                result.Status.IsSuccess = true;

            }
            catch (Exception ex)
            {
                result = GetException(ex);
            }
            return result;
        }

        /// <summary>
        /// Sorted Data By Column
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        private static List<Product> SortedDataByColumn(List<Product> list, string sortColumn, string sortDirection)
        {
            switch (sortColumn)
            {
                case Constants.ProductId:
                    Func<Product, long> orderingLongFunction = e => e.ProductId;
                    list = sortDirection.ToLower() == "asc" ? list.OrderBy(orderingLongFunction).ToList() : list.OrderByDescending(orderingLongFunction).ToList();
                    break;
                case Constants.ProductPrice:
                    Func<Product, decimal> orderingDecimalFunction = e => e.ProductPrice;
                    list = sortDirection.ToLower() == "asc" ? list.OrderBy(orderingDecimalFunction).ToList() : list.OrderByDescending(orderingDecimalFunction).ToList();
                    break;
                default:
                    Func<Product, string> orderingStringFunction = e => sortColumn == "productName" ? Convert.ToString(e.ProductName) :
                                                            e.ProductName ;
                    list = sortDirection.ToLower() == "asc" ? list.OrderBy(orderingStringFunction).ToList() : list.OrderByDescending(orderingStringFunction).ToList();
                    break;
            }

            return list;
        }

        /// <summary>
        /// Get Product Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("GetProductDetails"), HttpPost]
        public ActionResult<ResJsonOutput> GetProductDetails([FromBody] RequestById model)
        {
            ResJsonOutput result = new ResJsonOutput();
            try
            {
                result.Data = product.Get(model.ProductId);
                result.Status.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result = GetException(ex);
            }
            return result;
        }

        /// <summary>
        /// Add Product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddProduct"), HttpPost]
        public ActionResult<ResJsonOutput> AddProduct([FromBody] AddProductModel model)
        {
            ResJsonOutput result = new ResJsonOutput();
            try
            {
                Product _product = new Product
                {
                    ProductName = model.ProductName,
                    ProductDescription = model.ProductDescription,
                    ProductPrice = model.ProductPrice,
                    ProductImage = model.ProductImage,
                    CreatedOn = DateTime.UtcNow
                };
                product.Insert(_product);
                result.Data = _product.ProductId;
                result.Status.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result = GetException(ex);
            }
            return result;
        }

        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteProduct"), HttpPost]
        public ActionResult<ResJsonOutput> DeleteProduct([FromBody] RequestById model)
        {
            ResJsonOutput result = new ResJsonOutput();
            try
            {
                Product _product = product.Get(model.ProductId);
                product.Delete(_product);
                result.Status.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result = GetException(ex);
            }
            return result;
        }

        /// <summary>
        /// Get Formatted Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private ResJsonOutput GetException(Exception ex)
        {
            ResJsonOutput result = new ResJsonOutput();
            string ControllerName = RouteData != null ? RouteData.Values["controller"].ToString() : "";
            string ActionName = RouteData != null ? RouteData.Values["action"].ToString() : "";
            ErrorLog errorLog = new ErrorLog
            {
                Exception = Convert.ToString(ex),
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                CreatedOn = DateTime.UtcNow
            };
            log.Insert(errorLog);
            result.Status.StatusCode = "GNERR";
            result.Status.Message = "Error while processing you request.";
            return result;
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