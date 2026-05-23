namespace QuioscoAPI.Models;

public class PedidoProducto
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Pedido Pedido { get; set; } = null!;
    public Producto Producto { get; set; } = null!;
}
