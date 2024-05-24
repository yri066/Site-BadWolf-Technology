using System.ComponentModel.DataAnnotations;
using BadWolfTechnology.Data;

namespace BadWolfTechnology.Models
{
    public class PostEdit : NewsEdit
    {
        public Guid? Id { get; set; }
        [Display(Name = "Уникальное название Url страницы")]
        [Required]
        public string CodePage { get; set; } = null!;

        public IList<PostContent> Contents { get; set; } = new List<PostContent>();

        public PostEdit() { }
        public PostEdit(Post post)
        {
            Title = post.Title;
            ImageName = post.ImageName;
            Text = post.Text;
            IsView = post.IsView;

            if(post.Contents.Count == 0)
            {
                return;
            }

            foreach (var content in post.Contents)
            {
                var contentTemp = new PostContent();
                contentTemp.Type = content.Type;

                foreach (var item in content.Items)
                {
                    var itemItemp = new PostContentItem
                    {
                        Title = item.Title,
                        ImageName = item.ImageName,
                        Text = item.Text,
                    };

                    contentTemp.Items.Add(itemItemp);
                }

                Contents.Add(contentTemp);
            }
        }

        public void PostUpdate(Post post)
        {
            post.Title = Title;
            post.ImageName = ImageName;
            post.Text = Text;
            post.IsView = IsView;

            post.Contents.Clear();

            foreach (var content in Contents)
            {
                if(content.CheckIsNull())
                {
                    continue;
                }

                var contentTemp = new Content();
                contentTemp.Type = content.Type;

                foreach (var item in content.Items)
                {
                    if (item.CheckIsNull())
                    {
                        continue;
                    }

                    var itemItemp = new ContentItem
                    {
                        Title = item.Title,
                        ImageName = item.ImageName,
                        Text = item.Text,
                    };

                    contentTemp.Items.Add(itemItemp);
                }

                post.Contents.Add(contentTemp);
            }
        }
    }

    public class PostContent
    {
        public ContentType Type { get; set; }
        public IList<PostContentItem> Items { get; set; } = new List<PostContentItem>();

        public bool CheckIsNull()
        {
            return Items.All(x => x.CheckIsNull());
        }
    }

    public class PostContentItem
    {
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? ImageName { get; set; }
        public string? TempImageName { get; set; }
        public IFormFile? File { get; set; }

        public bool CheckIsNull()
        {
            if(Title is null &&
               Text is null &&
               ImageName is null &&
               TempImageName is null &&
               File is null)
            {
                return true;
            }

            return false;
        }
    }
}
