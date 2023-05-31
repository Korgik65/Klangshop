using System;
using Klangshop.Models.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klangshop.Models.ViewModels
{
    public class ProdCharacterVM
    {
        public ProdCharacterVM()
        {

        }

        public ProdCharacterVM(ProdCharacter row)
        {
            Id = row.Id;
            CharacteristicId = row.CharacteristicId;
            ProductId = row.ProductId;
            Value = row.Value;
        }

        public int Id { get; set; }
        public int CharacteristicId { get; set; }
        public int ProductId { get; set; }
        public string Value { get; set; }
    }
}