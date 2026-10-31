using System;
using System.ComponentModel.DataAnnotations;

namespace DestinoEnsueno.API.DTOs;

public class EventDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public decimal Price { get; set; }
    public bool IsFree { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Organizer { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int? BusinessId { get; set; }
    public string? BusinessName { get; set; }
}

public class EventCreateDto
{
    [Required(ErrorMessage = "El nombre del evento es obligatorio.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "La descripción del evento es obligatoria.")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "La fecha del evento es obligatoria.")]
    public DateTime Date { get; set; }

    public string? Time { get; set; }
    public string? Location { get; set; }
    public string? Organizer { get; set; }
    public bool IsFree { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El precio no puede ser negativo.")]
    public decimal Price { get; set; }

    public int? BusinessId { get; set; }
    public bool IsPublished { get; set; }
    public string? ImageUrl { get; set; }
}
