using System.Collections.Generic;
namespace DestinoEnsueno.API.DTOs;
public class BusinessDto { 
    public int Id { get; set; } public string Name { get; set; } = null!; public string Description { get; set; } = null!; 
    public string Address { get; set; } = null!; public string Phone { get; set; } = null!; public string CategoryName { get; set; } = null!;
    public double Latitude { get; set; } public double Longitude { get; set; } public bool IsPublished { get; set; }
    public double AverageRating { get; set; } public int ReviewCount { get; set; }
}
public class BusinessCreateDto {
    public string Name { get; set; } = null!; public string Description { get; set; } = null!; public string Address { get; set; } = null!;
    public string Phone { get; set; } = null!; public string Email { get; set; } = null!; public double Latitude { get; set; } public double Longitude { get; set; }
    public bool IsPublished { get; set; } public int CategoryId { get; set; }
}
