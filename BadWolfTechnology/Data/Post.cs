using BadWolfTechnology.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BadWolfTechnology.Data
{
    public class Post : IPublication
    {
        public Guid Id { get; set; }
        [Display(Name = "Заголовок.")]
        public string Title { get; set; } = null!;
        public string? ImageName { get; set; }
        [Display(Name = "Краткая информация.")]
        public string Text { get; set; } = null!;
        [NotMapped]
        public int? CommentCount { get; set; }
        public bool IsView { get; set; } = true;
        public DateTime Created { get; set; }
        [Display(Name = "Уникальное название Url страницы.")]
        public string CodePage { get; set; } = null!;

        public IList<Content> Contents { get; set; } = new List<Content>();
    }

    public enum ContentType
    {
        [Display(Name = "Текст")]
        Text,
        [Display(Name = "Изображение")]
        Image,
        [Display(Name = "Слайдер")]
        Slider,
        [Display(Name = "Слайдер с текстом")]
        SliderWithText,
        [Display(Name = "YouTube")]
        Video
    }

    public class Content
    {
        public ContentType Type { get; set; }
        public IList<ContentItem> Items { get; set; } = new List<ContentItem>();
    }

    public class ContentItem
    {
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? ImageName { get; set; }
    }
}
