using System;
namespace DestinoEnsueno.API.DTOs;
public class EventDto {
    public int Id { get; set; } public string Name { get; set; } = null!; public string Description { get; set; } = null!;
    public DateTime Date { get; set; } public TimeSpan Time { get; set; } public decimal Price { get; set; } public bool IsFree { get; set; }
    public string Location { get; set; } = null!; public int? BusinessId { get; set; } public string? BusinessName { get; set; }
}
public class EventCreateDto {
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Date { get; set; }
    public string? Time { get; set; }
    public string? Location { get; set; }
    public string? Organizer { get; set; }
    public bool IsFree { get; set; }
    public decimal Price { get; set; }
    public int? BusinessId { get; set; }
    public bool IsPublished { get; set; }
    public string? ImageUrl { get; set; }
}
