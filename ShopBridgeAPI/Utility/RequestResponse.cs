using ShopBridgeData.DBModels;
using System.Collections.Generic;
using System.ComponentModel;

namespace ShopBridgeAPI.Utility
{
    public class ResJsonOutput
    {
        public ResJsonOutput()
        {
            Status = new ResStatus();
        }
        public object Data { get; set; }
        public ResStatus Status { get; set; }
    }

    public class ResStatus
    {
        [DefaultValue(false)]
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string StatusCode { get; set; }
    }

    public class RequestById
    {
        public long ProductId { get; set; }
    }

    public class ProductDatatable
    {
        public long TotalRecords { get; set; }
        public long TotalDisplayRecord { get; set; }
        public List<Product> Products { get; set; }
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
