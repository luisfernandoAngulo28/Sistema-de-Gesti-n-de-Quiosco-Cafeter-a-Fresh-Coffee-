using Microsoft.EntityFrameworkCore;
using QuioscoAPI.Data;
using QuioscoAPI.Models;
using QuioscoAPI.Repositories.Interfaces;

namespace QuioscoAPI.Repositories;

public class UserRepositoryLFAH : IUserRepositoryLFAH
{
    private readonly QuioscoDbContext _context;

    public UserRepositoryLFAH(QuioscoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.OrderBy(u => u.Name).ToListAsync();
    }

    public async Task<User?> GetById(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> Create(User user)
    {
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> Update(int id, User user)
    {
        var existing = await _context.Users.FindAsync(id);
        if (existing is null) return null;

        existing.Name = user.Name;
        existing.Email = user.Email;
        existing.Admin = user.Admin;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
