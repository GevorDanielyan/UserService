using System;
using System.Text;
using System.Data;
using Domain.Models;
using Domain.Services;
using DataLayer.Entities;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace DataLayer.Services
{
    public class UserService : IUserService
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public UserService(string connectionString, ILogger<UserService> logger = null)
        {
            _connectionString = connectionString;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> CreateUserAsync(string email, string name, string surname, string phoneNumber)
        {
            string applicationId = string.Empty;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_CreateUser]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Email", email));
                        command.Parameters.Add(new SqlParameter("@Name", name));
                        command.Parameters.Add(new SqlParameter("@Surname", surname));
                        command.Parameters.Add(new SqlParameter("@PhoneNumber", phoneNumber));
                        command.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
                        command.Parameters["@ID"].Direction = ParameterDirection.Output;

                        await command.ExecuteNonQueryAsync();

                        var id = command.Parameters["@ID"].Value as Guid?;
                        if (id.HasValue)
                        {
                            applicationId = id.ToString();
                        }
                    }
                }
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Exception when trying to create new user");
                throw;
            }

            return applicationId;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var cmd = new SqlCommand("[dbo].[sp_DeleteUserById]", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", id));

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Exception when trying to delete user");
                throw;
            }
        }

        public async Task<IUser> GetUserByIdAsync(Guid id)
        {
            IUser result = null;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var cmd = new SqlCommand("[dbo].[sp_GetUserById]", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Id", id));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                Guid Id =  reader.GetGuid("Id");

                                string email = await reader.IsDBNullAsync("Email")
                                    ? null : reader.GetString("Email");

                                string name = await reader.IsDBNullAsync("Name")
                                    ? null : reader.GetString("Name");

                                string surName = await reader.IsDBNullAsync("SurName")
                                    ? null : reader.GetString("SurName");

                                string phoneNumber = await reader.IsDBNullAsync("PhoneNumber")
                                    ? null : reader.GetString("PhoneNumber");

                                DateTime? createdDateTime = await reader.IsDBNullAsync("CreatedDateTimeUtc")
                                    ? null : reader.GetDateTime("CreatedDateTimeUtc");

                                result = new User
                                {
                                    Id = Id,
                                    Email = email,
                                    Name = name,
                                    Surname = surName,
                                    PhoneNumber = phoneNumber,
                                    CreatedDateTimeUtc = createdDateTime
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
                _logger.LogError(error, $"Error while reading account with ID {id}");
                throw;
            }

            return result;
        }

        public async Task<IEnumerable<IUser>> GetUsersAsync()
        {
            List<User> result = new();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var cmd = new SqlCommand("[dbo].[sp_GetUsersList]", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Guid id = reader.GetGuid("Id");
                                string email = await reader.IsDBNullAsync("Email")
                                    ? null : reader.GetString("Email");

                                string name = await reader.IsDBNullAsync("Name")
                                    ? null : reader.GetString("Name");

                                string surName = await reader.IsDBNullAsync("SurName")
                                    ? null : reader.GetString("SurName");

                                string phoneNumber = await reader.IsDBNullAsync("PhoneNumber")
                                    ? null : reader.GetString("PhoneNumber");

                                DateTime? createdDateTime = await reader.IsDBNullAsync("CreatedDateTimeUtc")
                                    ? null : reader.GetDateTime("CreatedDateTimeUtc");

                                result.Add(new User
                                {
                                    Id = id,
                                    Email = email,
                                    Name = name,
                                    Surname = surName,
                                    PhoneNumber = phoneNumber,
                                    CreatedDateTimeUtc = createdDateTime
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Exception when trying to get Users list");
                throw;
            }

            return result;
        }

        public async Task<string> UpdateUserAsync(Guid Id, string email, string name, string surname, string phoneNumber)
        {
            string result = string.Empty;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("[dbo].[sp_UpdateUser]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Id", Id));
                        command.Parameters["@Id"].SqlDbType = SqlDbType.UniqueIdentifier;
                        //command.Parameters["@Id"].Direction = ParameterDirection.Output;

                        command.Parameters.Add(new SqlParameter("@Email", email));
                        command.Parameters.Add(new SqlParameter("@Name", name));
                        command.Parameters.Add(new SqlParameter("@Surname", surname));
                        command.Parameters.Add(new SqlParameter("@PhoneNumber", phoneNumber));
                        command.Parameters.Add(new SqlParameter("@Result", SqlDbType.NVarChar, 200));
                        command.Parameters["@Result"].Direction = ParameterDirection.Output;

                        await command.ExecuteNonQueryAsync();

                        var id = command.Parameters["@Id"].Value as Guid?;
                        if (id.HasValue)
                        {
                            result = id.ToString();
                        }
                    }
                }
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Exception when trying to update user");
                throw;
            }

            return result;
        }
    }
}
