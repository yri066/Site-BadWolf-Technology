using BadWolfTechnology.Data.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace BadWolfTechnology.Data
{
    public class News : IPublication
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? ImageName { get; set; }
        public string Text { get; set; } = null!;
        public string SearchString { get; set; } = null!;
        [NotMapped]
        public int? CommentCount { get; set; }
        public bool IsView { get; set; } = true;
        public bool IsDelete { get; set; } = false;
        public DateTime Created { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
