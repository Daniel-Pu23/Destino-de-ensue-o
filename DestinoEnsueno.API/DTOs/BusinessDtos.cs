using System.ComponentModel.DataAnnotations;

namespace DestinoEnsueno.API.DTOs;

public class BusinessDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsPublished { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}

public class BusinessCreateDto
{
    [Required(ErrorMessage = "El nombre del negocio es obligatorio.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    public string Address { get; set; } = null!;

    public string? Phone { get; set; }
    public string? Email { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsPublished { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
    public int CategoryId { get; set; }
}
