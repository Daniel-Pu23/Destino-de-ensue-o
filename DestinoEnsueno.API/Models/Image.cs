namespace DestinoEnsueno.API.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }

        public int BusinessId { get; set; }
        public Business Business { get; set; } = null!;
    }
}
