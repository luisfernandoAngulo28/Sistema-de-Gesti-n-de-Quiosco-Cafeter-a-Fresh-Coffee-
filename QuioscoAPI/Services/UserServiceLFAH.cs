using AutoMapper;
using QuioscoAPI.DTOs;
using QuioscoAPI.Models;
using QuioscoAPI.Repositories.Interfaces;
using QuioscoAPI.Services.Interfaces;

namespace QuioscoAPI.Services;

public class UserServiceLFAH : IUserServiceLFAH
{
    private readonly IUserRepositoryLFAH _repository;
    private readonly IMapper _mapper;

    public UserServiceLFAH(IUserRepositoryLFAH repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDtoLFAH>> GetAll()
    {
        var users = await _repository.GetAll();
        return _mapper.Map<IEnumerable<UserDtoLFAH>>(users);
    }

    public async Task<UserDtoLFAH?> GetById(int id)
    {
        var user = await _repository.GetById(id);
        return user is null ? null : _mapper.Map<UserDtoLFAH>(user);
    }

    public async Task<UserDtoLFAH> Create(CreateUserDtoLFAH dto)
    {
        var user = _mapper.Map<User>(dto);
        user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var created = await _repository.Create(user);
        return _mapper.Map<UserDtoLFAH>(created);
    }

    public async Task<UserDtoLFAH?> Update(int id, UpdateUserDtoLFAH dto)
    {
        var user = _mapper.Map<User>(dto);
        var updated = await _repository.Update(id, user);
        return updated is null ? null : _mapper.Map<UserDtoLFAH>(updated);
    }

    public async Task<bool> Delete(int id)
    {
        return await _repository.Delete(id);
    }
}
