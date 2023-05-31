using Klangshop.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Klangshop.Models.ViewModels
{
    public class OrderVM
    {
        public OrderVM()
        {

        }

        public OrderVM(Order row)
        {
            OrderId = row.OrderId;
            CustomerId = row.CustomerId;
            Date = row.Date;
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
    }
}