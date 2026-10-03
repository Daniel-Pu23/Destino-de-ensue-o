using System;
namespace DestinoEnsueno.API.DTOs;
public class ReviewDto {
    public int Id { get; set; } public int Rating { get; set; } public string? Comment { get; set; } 
    public DateTime Date { get; set; } public string UserName { get; set; } = null!; public int BusinessId { get; set; }
}
public class ReviewCreateDto {
    public int Rating { get; set; } public string? Comment { get; set; } public int BusinessId { get; set; }
}
