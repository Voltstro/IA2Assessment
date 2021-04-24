using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using IA2Assessment.Models;

namespace IA2Assessment.Identity
{
    public class UserStore : IUserStore<User>, IUserPasswordStore<User>
    {
        public UserStore(TuckshopDbContext context)
        {
            this.context = context;
        }
        
        private readonly TuckshopDbContext context;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                context?.Dispose();
            }
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserId.ToString());
        }
 
        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }
 
        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(nameof(SetUserNameAsync));
        }
 
        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(nameof(GetNormalizedUserNameAsync));
        }
 
        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult((object) null);
        }
 
        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            context.Add(user);
 
            await context.SaveChangesAsync(cancellationToken);
 
            return await Task.FromResult(IdentityResult.Success);
        }
 
        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(nameof(UpdateAsync));
        }
 
        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            context.Remove(user);
             
            int i = await context.SaveChangesAsync(cancellationToken);
 
            return await Task.FromResult(i == 1 ? IdentityResult.Success : IdentityResult.Failed());
        }
 
        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (int.TryParse(userId, out int id))
            {
                return await context.Users.FindAsync(id);
            }
            else
            {
                return await Task.FromResult((User) null);
            }
        }
 
        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await context.Users
                           .AsAsyncEnumerable()
                           .SingleOrDefaultAsync(p => p.UserName.Equals(normalizedUserName, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }
 
        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.UserPasswordHash = passwordHash;
 
            return Task.FromResult((object) null);
        }
 
        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserPasswordHash);
        }
 
        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.UserPasswordHash));
        }
    }
}