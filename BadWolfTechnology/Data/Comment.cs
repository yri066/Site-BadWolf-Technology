using BadWolfTechnology.Areas.Identity.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BadWolfTechnology.Data
{
    public class Comment
    {
        public long Id { get; set; }
        [DisplayName("Текст")]
        public string Text { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime Created { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Comment? Parent { get; set; }

        public bool HasParent(Comment parent)
        {
            var current = Parent;

            while (current != null)
            {
                if (current == parent)
                    return true;

                current = current.Parent;
            }

            return false;
        }
    }
}
