namespace QuioscoAPI.DTOs;

// Formato exacto que espera el React (igual que Laravel resource)
public class PedidoResponseDtoLFAH
{
    public int Id { get; set; }
    public double Total { get; set; }
    public bool Estado { get; set; }
    public MesaDtoLFAH? Mesa { get; set; }
    public PagoDtoLFAH? Pago { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserBriefDtoLFAH User { get; set; } = null!;
    public List<ProductoPivotDtoLFAH> Productos { get; set; } = new();
}

public class UserBriefDtoLFAH
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class ProductoPivotDtoLFAH
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public double Precio { get; set; }
    public string Imagen { get; set; } = string.Empty;
    public PivotDtoLFAH Pivot { get; set; } = null!;
}

public class PivotDtoLFAH
{
    public int Cantidad { get; set; }
}
