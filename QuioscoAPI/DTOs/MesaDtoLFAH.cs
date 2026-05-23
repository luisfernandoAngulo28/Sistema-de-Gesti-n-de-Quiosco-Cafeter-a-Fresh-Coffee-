using System.ComponentModel.DataAnnotations;

namespace QuioscoAPI.DTOs;

public class MesaDtoLFAH
{
    public int Id { get; set; }
    public int Numero { get; set; }
    public int Capacidad { get; set; }
    public bool Disponible { get; set; }
    public bool Ocupada { get; set; }
}

public class CreateMesaDtoLFAH
{
    [Range(1, 100, ErrorMessage = "El número de mesa debe estar entre 1 y 100")]
    public int Numero { get; set; }

    [Range(1, 20, ErrorMessage = "La capacidad debe estar entre 1 y 20")]
    public int Capacidad { get; set; }
}

public class UpdateMesaDtoLFAH
{
    [Range(1, 100, ErrorMessage = "El número de mesa debe estar entre 1 y 100")]
    public int Numero { get; set; }

    [Range(1, 20, ErrorMessage = "La capacidad debe estar entre 1 y 20")]
    public int Capacidad { get; set; }

    public bool Disponible { get; set; }
}
