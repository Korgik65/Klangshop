using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Klangshop.Models.ViewModels.Account
{
    public class OrdersForCustomersVM
    {
        [DisplayName("№")]
        public int OrderNumber { get; set; }
        [DisplayName("Загальна сума")]
        public decimal TotalPrice { get; set; }
        [DisplayName("Деталі замовлення")]
        public Dictionary<string, int> ProductsAndAmount { get; set; }
        [DisplayName("Дата замовлення")]
        public DateTime Date { get; set; }
    }
}