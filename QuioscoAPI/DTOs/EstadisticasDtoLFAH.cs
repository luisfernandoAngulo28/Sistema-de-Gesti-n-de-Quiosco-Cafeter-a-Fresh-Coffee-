namespace QuioscoAPI.DTOs;

public class EstadisticasDtoLFAH
{
    public int TotalPedidos { get; set; }
    public int PedidosPendientes { get; set; }
    public int PedidosCompletados { get; set; }
    public double IngresoTotal { get; set; }
    public List<TopProductoDtoLFAH> TopProductos { get; set; } = new();
}

public class TopProductoDtoLFAH
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Imagen { get; set; } = string.Empty;
    public int TotalVendido { get; set; }
}
