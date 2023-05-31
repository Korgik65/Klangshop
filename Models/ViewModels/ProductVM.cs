using Klangshop.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Klangshop.Models.ViewModels
{
    public class ProductVM
    {

        public ProductVM()
        {

        }

        public ProductVM(Product row)
        {
            Id = row.Id;
            CategoryId = row.CategoryId;
            Name = row.Name;
            Image = row.Image;
            Description = row.Description;
            Price = row.Price;
            Available = row.Available;
            Maker = row.Maker;
            MakerSite = row.MakerSite;
            Forma = row.Forma;
            KilStrun = row.KilStrun;
            Kolir = row.Kolir;
            Deka = row.Deka;
            Grif = row.Grif;
            KilLad = row.KilLad;
            Zvukoznim = row.Zvukoznim;
            Rozmir = row.Rozmir;
            Typ = row.Typ;
            Priznach = row.Priznach;
            Material = row.Material;
            Kalibr = row.Kalibr;
            Potuzhnist = row.Potuzhnist;
            Efect = row.Efect;
            AnalogVihod = row.AnalogVihod;
            Vaga = row.Vaga;
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Виберіть категорію товару")]
        [DisplayName("Категорія товару")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Введіть назву товару")]
        [StringLength(50, MinimumLength = 3)]
        [DisplayName("Назва товару")]
        public string Name { get; set; }
        [DisplayName("Зображення товару")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Введіть опис товару")]
        [DisplayName("Опис товару")]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Введіть ціну товару")]
        [DisplayName("Ціна товару")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Виберіть наявність товару")]
        [DisplayName("Наявність товару")]
        public int Available { get; set; }
        [Required(ErrorMessage = "Введіть виробника товару")]
        [DisplayName("Виробник товару")]
        public string Maker { get; set; }
        [DisplayName("Посилання на виробника товару")]
        public string MakerSite { get; set; }
        [DisplayName("Форма корпусу")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string Forma { get; set; }
        [DisplayName("Кількість струн")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string KilStrun { get; set; }
        [DisplayName("Колір")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string Kolir { get; set; }
        [DisplayName("Дека")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string Deka { get; set; }
        [DisplayName("Гриф")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string Grif { get; set; }
        [DisplayName("Кількість ладів")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string KilLad { get; set; }
        [DisplayName("Звукознімачі")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string Zvukoznim { get; set; }
        [DisplayName("Розмір")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string Rozmir { get; set; }
        [DisplayName("Тип")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string Typ { get; set; }
        [DisplayName("Призначення")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string Priznach { get; set; }
        [DisplayName("Матеріал")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string Material { get; set; }
        [DisplayName("Калібр")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string Kalibr { get; set; }
        [DisplayName("Рейтинг потужності")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string Potuzhnist { get; set; }
        [DisplayName("Ефекти")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string Efect { get; set; }
        [DisplayName("Аналогові виходи")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string AnalogVihod { get; set; }
        [DisplayName("Вага")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Поле повинно містити від 1 до 60 символів")]
        public string Vaga { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        //public IEnumerable<SelectListItem> ProdCharacters { get; set; }
        //public IEnumerable<SelectListItem> Characteristics { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}