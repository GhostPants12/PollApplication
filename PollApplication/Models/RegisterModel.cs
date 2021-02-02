using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PollApplication.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указано имя.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Не указана фамилия.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Не указан email.")]
        [RegularExpression (@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный email.")]
        public string Email { get; set; }
 
        [Required(ErrorMessage = "Не указан пароль.")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Длина пароля должна быть не менее 8 символов.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
 
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}
