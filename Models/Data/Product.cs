using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Klangshop.Models.Data
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Available { get; set; }
        public string Maker { get; set; }
        public string MakerSite { get; set; }

        public string Forma { get; set; }
        public string KilStrun { get; set; }
        public string Kolir { get; set; }
        public string Deka { get; set; }
        public string Grif { get; set; }
        public string KilLad { get; set; }
        public string Zvukoznim { get; set; }
        public string Rozmir { get; set; }
        public string Typ { get; set; }
        public string Priznach { get; set; }
        public string Material { get; set; }
        public string Kalibr { get; set; }
        public string Potuzhnist { get; set; }
        public string Efect { get; set; }
        public string AnalogVihod { get; set; }
        public string Vaga { get; set; }


        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}