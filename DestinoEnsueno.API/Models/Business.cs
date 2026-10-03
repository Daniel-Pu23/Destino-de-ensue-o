namespace DestinoEnsueno.API.Models
{
    public class Business
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string SocialMedia { get; set; } = string.Empty;
        
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Discount> Discounts { get; set; } = new List<Discount>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
