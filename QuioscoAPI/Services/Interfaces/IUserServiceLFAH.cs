using QuioscoAPI.DTOs;

namespace QuioscoAPI.Services.Interfaces;

public interface IUserServiceLFAH
{
    Task<IEnumerable<UserDtoLFAH>> GetAll();
    Task<UserDtoLFAH?> GetById(int id);
    Task<UserDtoLFAH> Create(CreateUserDtoLFAH dto);
    Task<UserDtoLFAH?> Update(int id, UpdateUserDtoLFAH dto);
    Task<bool> Delete(int id);
}
