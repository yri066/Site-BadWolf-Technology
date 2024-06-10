namespace BadWolfTechnology.Data
{
    public class Product : Post
    {
        public bool IsDelete { get; set; } = false;

        //public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
