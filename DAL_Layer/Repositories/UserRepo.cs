using DAL;
using Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Repo
{
    public class UserRepo : IUserRepo
    {
        public int AddUser(string name, string email, string password, byte[]? picture)
        {
            const string sql = @"INSERT INTO dbo.Users (Name, Email, Password, Picture) OUTPUT INSERTED.Id VALUES (@Name, @Email, @PasswordHash, @Picture);";

            using (SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 255).Value = name;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = email;
                    cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 255).Value = password;
                    cmd.Parameters.Add("@Picture", SqlDbType.VarBinary, -1).Value = (object?)picture ?? DBNull.Value;

                    object? newIdObj = cmd.ExecuteScalar();
                    return Convert.ToInt32(newIdObj);
                }
            }
        }

        public (int Id, string Name, string Email, string Password, bool Admin)? GetByEmail(string email)
        {
            const string sql = @"SELECT TOP 1 Id, Name, Email, Password, Admin FROM dbo.Users WHERE Email = @Email;";

            using (SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, -1).Value = email;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return null;
                        return (
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetString(reader.GetOrdinal("Name")),
                        reader.GetString(reader.GetOrdinal("Email")),
                        reader.GetString(reader.GetOrdinal("Password")),
                        reader.GetBoolean(reader.GetOrdinal("Admin")));
                    }
                }
            }
        }

        public byte[]? GetPictureById(long id)
        {
            const string sql = @"SELECT TOP 1 Picture FROM dbo.Users WHERE Id = @Id;";

            using (SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    object? obj = cmd.ExecuteScalar();
                    if (obj == null || obj == DBNull.Value) return null;

                    return (byte[])obj;
                }
            }
        }

        public string? GetPasswordHashById(long userId)
        {
            const string sql = @"SELECT TOP 1 Password FROM dbo.Users WHERE Id = @Id;";

            using (SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = userId;

                    object? obj = cmd.ExecuteScalar();
                    if (obj == null || obj == DBNull.Value) return null;
                    return Convert.ToString(obj);
                }
            }
        }

        public void UpdatePasswordHash(long userId, string newPasswordHash)
        {
            const string sql = @"UPDATE dbo.Users SET Password = @PasswordHash WHERE Id = @Id;";

            using (SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 255).Value = newPasswordHash;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
