using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using WebStore.Controllers;

namespace WebStore.Domain.ViewModels.Identity
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "Имя пользователя обязательно для заполнения")]
        [MaxLength(256, ErrorMessage = "Максимальная длина поля не должна превышать 256 символов")]
        [Remote("IsNameFree", "Account", ErrorMessage = "Пользователь с таким именем уже существует.")]
        [Display(Name = "Имя пользователя")]
        [RegularExpression(@"[A-Za-z][A-Za-z0-9_]{3,}", ErrorMessage = "Некорректное имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите ввод пароля")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите ввод пароля")]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
