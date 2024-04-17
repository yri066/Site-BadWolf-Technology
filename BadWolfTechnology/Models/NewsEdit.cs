using System.ComponentModel.DataAnnotations;

namespace BadWolfTechnology.Models
{
    public class NewsEdit
    {
        [Required(ErrorMessage = "Поле '{0}' является обязательным.")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; } = null!;
        [Display(Name = "Изображение")]
        public string? ImageName { get; set; }
        [Display(Name = "Изображение")]
        public string? TempImageName { get; set; }
        [Required(ErrorMessage = "Поле '{0}' является обязательным.")]
        [Display(Name = "Текст")]
        public string Text { get; set; } = null!;
        [Display(Name = "Видимость")]
        public bool IsView { get; set; } = true;
    }
}
