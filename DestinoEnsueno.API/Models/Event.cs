namespace DestinoEnsueno.API.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Organizer { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsFree { get; set; }
        public bool IsPublished { get; set; }

        public int? BusinessId { get; set; }
        public Business? Business { get; set; }
    }
}
