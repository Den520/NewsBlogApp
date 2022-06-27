using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsBlogApp.Models
{
    public class NewsModel
    {
        [Display(Name = "ID")]  //для админа (возможно, для просмотра и идентификации)
        public int Id { get; set; }

        [Required(ErrorMessage = "Вы не ввели заголовок.")]
        [StringLength(200, ErrorMessage = "Размер заголовка должен быть не более 200 символов.")]
        [Display(Name = "Заголовок")]
        public string Article { get; set; }

        [Required(ErrorMessage = "Вы не ввели содержание.")]
        [StringLength(5000, ErrorMessage = "Размер статьи должен быть не более 5000 символов.")]
        [Display(Name = "Содержание")]
        public string Content { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Дата публикации")]
        public string DateOfPublication { get; set; }
    }
}