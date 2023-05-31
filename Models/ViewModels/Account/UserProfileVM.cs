using Klangshop.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Klangshop.Models.ViewModels.Account
{
    public class UserProfileVM
    {
        public UserProfileVM()
        {

        }

        public UserProfileVM(Customer row)
        {
            Id = row.Id;
            Name = row.Name;
            LName = row.LName;
            Email = row.Email;
            Phone = row.Phone;
            Address = row.Address;
            Country = row.Country;
            City = row.City;
            Index = row.Index;
            Password = row.Password;
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Введіть Ваше ім'я")]
        [DisplayName("Ім'я")]
        [StringLength(32, MinimumLength = 2, ErrorMessage = "Ім'я повинно бути від 2 до 32 символів")]
        [RegularExpression(@"[A-Za-z]+|[А-Яа-яЇїІіЄє]+", ErrorMessage = "Введіть своє справжнє ім'я")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введіть Ваше прізвище")]
        [DisplayName("Прізвище")]
        [StringLength(32, MinimumLength = 2, ErrorMessage = "Прізвище повинно бути від 2 до 32 символів")]
        [RegularExpression(@"[A-Za-z]+|[А-Яа-яЇїІіЄє]+", ErrorMessage = "Введіть своє справжнє прізвище")]
        public string LName { get; set; }
        [Required(ErrorMessage = "Введіть Вашу електрону пошту")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Некоректна адреса електронної пошти")]
        [DisplayName("Електронна пошта")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Введіть Ваш номер телефону")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Некоректний номер телефону")]
        [DisplayName("Телефон")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Номер повинен складатися тільки із цифр")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Номер повинен містити в собі 10 цифр")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Введіть Вашу адресу")]
        [StringLength(70, MinimumLength = 3, ErrorMessage = "Адреса повинна бути від 3 до 70 символів")]
        [DisplayName("Адреса")]
        public string Address { get; set; }
        [DisplayName("Країна")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Назва країни повинна бути від 2 до 60 символів")]
        [RegularExpression(@"[A-Za-z]+|[А-Яа-яЇїІіЄє]+", ErrorMessage = "Некоректна назва міста")]
        public string Country { get; set; }
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Назва міста повинна бути від 2 до 60 символів")]
        [RegularExpression(@"[A-Za-z]+|[А-Яа-яЇїІіЄє]+", ErrorMessage = "Некоректна назва міста")]
        [DisplayName("Місто")]
        public string City { get; set; }
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Індекс складається із 5 цифр")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Індекс складається тільки із цифр")]
        [DisplayName("Індекс")]
        public string Index { get; set; }
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Пароль повинен бути від 4 до 20 символів")]
        [DataType(DataType.Password)]
        [DisplayName("Пароль")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [DataType(DataType.Password)]
        [DisplayName("Підтвердіть пароль")]
        public string ConfirmPassword { get; set; }
    }
}