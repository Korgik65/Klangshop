using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Klangshop.Models.Data
{
    [Table("OrderDetails")]
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customers { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Orders { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Products { get; set; }
    }
}