using Klangshop.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Klangshop.Models.ViewModels
{
    public class CategoryVM
    {
        public CategoryVM()
        {

        }

        public CategoryVM(Category row)
        {
            Id = row.Id;
            Type = row.Type;
        }

        public int Id { get; set; }
        public string Type { get; set; }
    }
}