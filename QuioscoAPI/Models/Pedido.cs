namespace QuioscoAPI.Models;

public class Pedido
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public double Total { get; set; }
    public bool Estado { get; set; } = false;
    public int? MesaId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Mesa? Mesa { get; set; }
    public Pago? Pago { get; set; }
    public ICollection<PedidoProducto> PedidoProductos { get; set; } = new List<PedidoProducto>();
}
