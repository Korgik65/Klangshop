using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Klangshop.Models.ViewModels
{
    public class CartVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice 
        { 
            get { return Amount * Price; } 
        }
    }
}