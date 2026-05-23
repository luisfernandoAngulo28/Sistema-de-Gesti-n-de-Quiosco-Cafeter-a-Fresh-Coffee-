using System.ComponentModel.DataAnnotations;

namespace QuioscoAPI.DTOs;

public class ProductoDtoLFAH
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public double Precio { get; set; }
    public string Imagen { get; set; } = string.Empty;
    public bool Disponible { get; set; }
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
}

public class CreateProductoDtoLFAH
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 150 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Range(0.01, 99999.99, ErrorMessage = "El precio debe ser mayor a 0")]
    public double Precio { get; set; }

    [Required(ErrorMessage = "La imagen es obligatoria")]
    public string Imagen { get; set; } = string.Empty;

    public bool Disponible { get; set; } = true;

    [Range(1, int.MaxValue, ErrorMessage = "La categoría es obligatoria")]
    public int CategoriaId { get; set; }
}

public class UpdateProductoDtoLFAH
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 150 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Range(0.01, 99999.99, ErrorMessage = "El precio debe ser mayor a 0")]
    public double Precio { get; set; }

    [Required(ErrorMessage = "La imagen es obligatoria")]
    public string Imagen { get; set; } = string.Empty;

    public bool Disponible { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La categoría es obligatoria")]
    public int CategoriaId { get; set; }
}
