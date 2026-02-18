using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Tests.Unit.Infrastructure;

internal sealed class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public Task<User?> GetByIdAsync(UserId id)
        => Task.FromResult(_users.SingleOrDefault(user => user.Id == id));

    public Task<User?> GetByEmailAsync(Email email)
        => Task.FromResult(_users.SingleOrDefault(user => user.Email == email));

    public Task<User?> GetByUsernameAsync(Username username)
        => Task.FromResult(_users.SingleOrDefault(user => user.Username == username));

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }
}
