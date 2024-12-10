namespace hello.net.Models.Entity
{
    public class Article
    {
        public required ulong ID { get; set; }
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public required string Content { get; set; }
        public required DateTime PublishedAt { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
