using System.ComponentModel.DataAnnotations;

namespace QuioscoAPI.DTOs;

public class PedidoProductoDtoLFAH
{
    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;
    public double Precio { get; set; }
    public int Cantidad { get; set; }
    public double Subtotal => Precio * Cantidad;
}

// El React envía "id" no "productoId"
public class CreatePedidoProductoDtoLFAH
{
    [Range(1, int.MaxValue, ErrorMessage = "El id del producto debe ser válido")]
    public int Id { get; set; }

    [Range(1, 100, ErrorMessage = "La cantidad debe ser entre 1 y 100")]
    public int Cantidad { get; set; }
}
