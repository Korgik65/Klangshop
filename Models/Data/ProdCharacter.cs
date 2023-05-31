using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Klangshop.Models.Data
{
    [Table("ProdCharacters")]
    public class ProdCharacter
    {
        [Key]
        public int Id { get; set; }
        public int CharacteristicId { get; set; }
        public int ProductId { get; set; }
        public string Value { get; set; }

        [ForeignKey("CharacteristicId")]
        public virtual Characteristic Characteristic { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}