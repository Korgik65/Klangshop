using System;
using Klangshop.Models.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Klangshop.Models.ViewModels
{
    public class CharacteristicVM
    {
        public CharacteristicVM()
        {

        }

        public CharacteristicVM(Characteristic row)
        {
            Id = row.Id;
            Name = row.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}