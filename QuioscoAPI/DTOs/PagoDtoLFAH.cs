using System.ComponentModel.DataAnnotations;

namespace QuioscoAPI.DTOs;

public class PagoDtoLFAH
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public string Metodo { get; set; } = string.Empty;
    public double Monto { get; set; }
    public DateTime FechaPago { get; set; }
}

public class CreatePagoDtoLFAH
{
    [Required(ErrorMessage = "El pedido es requerido")]
    public int PedidoId { get; set; }

    [Required(ErrorMessage = "El método de pago es requerido")]
    [RegularExpression("efectivo|tarjeta|transferencia",
        ErrorMessage = "El método debe ser: efectivo, tarjeta o transferencia")]
    public string Metodo { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    public double Monto { get; set; }
}
