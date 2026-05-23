namespace QuioscoAPI.DTOs;

public class CategoriaDtoLFAH
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
}

public class CreateCategoriaDtoLFAH
{
    public string Nombre { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
}

public class UpdateCategoriaDtoLFAH
{
    public string Nombre { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
}
