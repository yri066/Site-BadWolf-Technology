using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BadWolfTechnology.Data.Interfaces
{
    public interface IPublication
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? ImageName { get; set; }
        public string Text { get; set; }
        public int? CommentCount { get; set; }
        public bool IsView { get; set; }
        public bool IsDelete { get; set; }
        public DateTime Created { get; set; }
    }
}
