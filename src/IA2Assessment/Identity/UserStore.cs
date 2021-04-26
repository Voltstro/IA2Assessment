using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using IA2Assessment.Models;

namespace IA2Assessment.Identity
{
    /// <summary>
    ///     <see cref="IUserStore{TUser}"/> for <see cref="User"/>
    /// </summary>
    public sealed class UserStore : IUserPasswordStore<User>
    {
        public UserStore(TuckshopDbContext context)
        {
            this.context = context;
        }

        ~UserStore()
        {
            Dispose(true);
        }
        
        private readonly TuckshopDbContext context;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                context?.Dispose();
            }
        }

        /// <summary>
        ///     Gets the id from <see cref="User"/>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserId.ToString());
        }
 
        /// <summary>
        ///     Gets the username from <see cref="User"/>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
 
        /// <summary>
        ///     Creates a new <see cref="User"/>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
 
        /// <summary>
        ///     Deletes a <see cref="User"/>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            context.Remove(user);
             
            int i = await context.SaveChangesAsync(cancellationToken);
 
            return await Task.FromResult(i == 1 ? IdentityResult.Success : IdentityResult.Failed());
        }
 
        /// <summary>
        ///     Finds a <see cref="User"/> from an ID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
 
        /// <summary>
        ///     Gets a <see cref="User"/> from a user name
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<User> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            return await context.Users
                           .AsAsyncEnumerable()
                           .SingleOrDefaultAsync(p => p.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }
 
        /// <summary>
        ///     Sets a <see cref="User"/> password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.UserPasswordHash = passwordHash;
 
            return Task.FromResult((object) null);
        }
 
        /// <summary>
        ///     Gets a password hash from a <see cref="User"/>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserPasswordHash);
        }
 
        /// <summary>
        ///     Checks if a <see cref="User"/> has a password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.UserPasswordHash));
        }
    }
}