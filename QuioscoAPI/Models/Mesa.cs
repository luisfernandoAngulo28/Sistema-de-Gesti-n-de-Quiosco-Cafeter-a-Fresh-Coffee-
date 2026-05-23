namespace QuioscoAPI.Models;

public class Mesa
{
    public int Id { get; set; }
    public int Numero { get; set; }
    public int Capacidad { get; set; }
    public bool Disponible { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
