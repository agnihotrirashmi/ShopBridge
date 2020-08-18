using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridgeData.DBModels
{
    public class Product
    {
        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductImage { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
