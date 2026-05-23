namespace QuioscoAPI.Models;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public double Precio { get; set; }
    public string Imagen { get; set; } = string.Empty;
    public bool Disponible { get; set; } = true;
    public int CategoriaId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Categoria Categoria { get; set; } = null!;
    public ICollection<PedidoProducto> PedidoProductos { get; set; } = new List<PedidoProducto>();
}
