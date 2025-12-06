using LoanApi.Data;
using LoanApi.Interfaces;
using LoanApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanApi.Services;

public class UserService : IUserService
{
    private readonly LoanDbContext _db;
    private readonly IJwtService _jwt;

    public UserService(LoanDbContext db, IJwtService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public async Task RegisterAsync(User user, string password)
    {
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == username);

        if (user == null)
            throw new Exception("User not found");

        bool valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        if (!valid)
            throw new Exception("Invalid password");

        return _jwt.GenerateToken(user);
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _db.Users.FindAsync(id);
    }

    public async Task BlockUserAsync(int id, bool blockStatus)
    {
        var user = await _db.Users.FindAsync(id);

        if (user == null)
            throw new Exception("User not found");

        user.IsBlocked = blockStatus;

        await _db.SaveChangesAsync();
    }
}