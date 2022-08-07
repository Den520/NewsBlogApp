using System.ComponentModel.DataAnnotations;

namespace NewsBlogApp.Models
{
    public class CityModel
    {
        [Required(ErrorMessage = "Вы не ввели название города.")]
        [Display(Name = "Название города")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "Вы не ввели численность населения.")]
        [Display(Name = "Численность населения")]  //для админа (возможно, для просмотра и идентификации)
        public int PopulationSize { get; set; }
    }
}