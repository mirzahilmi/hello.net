namespace hello.net.Models.Entities
{
    public class Article
    {
        public ulong ID { get; set; }
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public required string Content { get; set; }
        public required DateTime PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
