using System.ComponentModel.DataAnnotations;

namespace QuioscoAPI.DTOs;

public class PedidoDtoLFAH
{
    public int Id { get; set; }
    public double Total { get; set; }
    public bool Estado { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserDtoLFAH Usuario { get; set; } = null!;
    public List<PedidoProductoDtoLFAH> Productos { get; set; } = new();
}

// El React envía: { total, productos: [{ id, cantidad }] }
// El userId viene del token JWT
public class CreatePedidoDtoLFAH
{
    [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
    public double Total { get; set; }

    public int? MesaId { get; set; }

    [Required(ErrorMessage = "El pedido debe tener al menos un producto")]
    [MinLength(1, ErrorMessage = "El pedido debe tener al menos un producto")]
    public List<CreatePedidoProductoDtoLFAH> Productos { get; set; } = new();
}

public class UpdatePedidoDtoLFAH
{
    public bool Estado { get; set; }
}
