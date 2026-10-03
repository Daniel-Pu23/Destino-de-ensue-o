namespace DestinoEnsueno.API.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; } // 1 a 5
        public string? Comment { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public int BusinessId { get; set; }
        public Business Business { get; set; } = null!;
    }
}
