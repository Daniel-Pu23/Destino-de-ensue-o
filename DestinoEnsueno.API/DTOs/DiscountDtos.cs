using System;
namespace DestinoEnsueno.API.DTOs;
public class DiscountDto {
    public int Id { get; set; } public string Name { get; set; } = null!; public string Description { get; set; } = null!;
    public int Percentage { get; set; } public DateTime StartDate { get; set; } public DateTime EndDate { get; set; }
    public int BusinessId { get; set; } public string BusinessName { get; set; } = null!;
}
public class DiscountCreateDto {
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Percentage { get; set; }
    public string? Conditions { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int BusinessId { get; set; }
    public bool IsPublished { get; set; }
    public string? ImageUrl { get; set; }
}
