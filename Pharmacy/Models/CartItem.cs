using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharmacy.Models
{
    public class CartItem
    {
        public Product product { get; set; }
        public int quantity { get; set; }
        public int amount { get; set; }
    }
}