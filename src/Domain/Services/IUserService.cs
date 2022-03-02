using System;
using Domain.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Get all users
        /// </summary>
        Task<IEnumerable<IUser>> GetUsersAsync();

        /// <summary>
        /// Get user by provided user id
        /// </summary>
        /// <param name="id"> user GUID</param>
        Task<IUser> GetUserByIdAsync(Guid id);

        /// <summary>
        /// Creating user
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="name">User name</param>
        /// <param name="surname">User surname</param>
        /// <param name="phoneNumber">User phone number</param>
        Task<string> CreateUserAsync(string email, string name, string surname, string phoneNumber);

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="name">User name</param>
        /// <param name="surname">User surname</param>
        /// <param name="phoneNumber">User phone number</param>
        /// <returns></returns>
        Task<string> UpdateUserAsync(Guid Id, string email, string name, string surname, string phoneNumber);

        /// <summary>
        /// Deleting user by user id
        /// </summary>
        /// <param name="id">User GUID</param>
        Task DeleteUserAsync(Guid id);
    }
}
