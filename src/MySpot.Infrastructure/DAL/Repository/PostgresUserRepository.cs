using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repository;

internal sealed class PostgresUserRepository(MySpotDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetByIdAsync(UserId id) 
        => await dbContext.Users.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<User?> GetByEmailAsync(Email email)
        => await dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);

    public async Task<User?> GetByUsernameAsync(Username username)
        => await dbContext.Users.SingleOrDefaultAsync(x => x.Username == username);
    
    public async Task AddAsync(User user)
        => await dbContext.Users.AddAsync(user);
}