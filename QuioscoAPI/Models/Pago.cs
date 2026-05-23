namespace QuioscoAPI.Models;

public class Pago
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public string Metodo { get; set; } = string.Empty;
    public double Monto { get; set; }
    public DateTime FechaPago { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Pedido Pedido { get; set; } = null!;
}
