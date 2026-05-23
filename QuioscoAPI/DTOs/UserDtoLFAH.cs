namespace QuioscoAPI.DTOs;

public class UserDtoLFAH
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Admin { get; set; }
}

public class CreateUserDtoLFAH
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool Admin { get; set; } = false;
}

public class UpdateUserDtoLFAH
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Admin { get; set; }
}
