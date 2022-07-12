using System.ComponentModel.DataAnnotations;

namespace NewsBlogApp.Models
{
    public class LogInModel  //модель логина
    {
        [Required(ErrorMessage = "Вы не ввели логин.")]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Вы не ввели пароль.")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel  //модель регистрации
    {
        [Required(ErrorMessage = "Вы не ввели логин.")]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Вы не ввели пароль.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Вы не ввели электронную почту.")]
        [StringLength(100, ErrorMessage = "Длина пароля должна быть не менее 5 символов.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}