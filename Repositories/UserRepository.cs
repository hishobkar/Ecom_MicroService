using Example01.Data;
using Example01.Models;
using MongoDB.Driver;

namespace Example01.Repositories;

public class UserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(MongoDbContext context)
    {
        _users = context.GetCollection<User>("Users");
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
    }

    public async Task AddUserAsync(User user)
    {
        try
        {
            await _users.InsertOneAsync(user);
        } catch(Exception e)
        {
            throw e;
        }
    }

}