using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Klangshop.Areas.Admin.ViewModels
{
    public class OrdersForAdminVM
    {
        [DisplayName("№")]
        public int OrderNumber { get; set; }
        [DisplayName("Замовник")]
        public Dictionary<string, string> CustomerName { get; set; }
        [DisplayName("Загальна сума")]
        public decimal TotalPrice { get; set; }
        [DisplayName("Деталі замовлення")]
        public Dictionary<string, int> ProductsAndAmount { get; set; }
        [DisplayName("Дата замовлення")]
        public DateTime Date { get; set; }

    }
}