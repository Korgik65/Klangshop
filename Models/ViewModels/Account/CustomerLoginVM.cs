using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klangshop.Models.ViewModels.Account
{
    public class CustomerLoginVM
    {
        [Required(ErrorMessage = "Введіть Вашу електрону пошту")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Електрона пошта")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Введіть Ваш пароль")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Пароль повинен бути від 4 до 20 символів")]
        [DataType(DataType.Password)]
        [DisplayName("Пароль")]
        public string Password { get; set; }
        [DisplayName("Запам'ятати мене")]
        public bool RememberMe { get; set; }
    }
}