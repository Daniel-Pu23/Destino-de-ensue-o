using System;
using System.ComponentModel.DataAnnotations;

namespace DestinoEnsueno.API.DTOs;

public class DiscountDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Percentage { get; set; }
    public string Conditions { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsPublished { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int BusinessId { get; set; }
    public string BusinessName { get; set; } = null!;
}

public class DiscountCreateDto
{
    [Required(ErrorMessage = "El nombre del descuento es obligatorio.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "La descripción del descuento es obligatoria.")]
    public string Description { get; set; } = null!;

    [Range(1, 100, ErrorMessage = "El porcentaje debe estar entre 1 y 100.")]
    public int Percentage { get; set; }

    public string? Conditions { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
    public DateTime EndDate { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un negocio válido.")]
    public int BusinessId { get; set; }

    public bool IsPublished { get; set; }
    public string? ImageUrl { get; set; }
}
