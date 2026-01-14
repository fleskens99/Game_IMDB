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
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PasswordHash", password);
                    cmd.Parameters.Add("@Picture", SqlDbType.VarBinary, -1).Value = (object?)picture ?? DBNull.Value;

                    object? newIdObj = cmd.ExecuteScalar();
                    return Convert.ToInt32(newIdObj);
                }
            }
        }

        public (int Id, string Name, string Email, string Password)? GetByEmail(string email)
        {
            const string sql = @"SELECT TOP 1 Id, Name, Email, Password FROM dbo.Users WHERE Email = @Email;";

            using (SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return null;
                        return (
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetString(reader.GetOrdinal("Name")),
                        reader.GetString(reader.GetOrdinal("Email")),
                        reader.GetString(reader.GetOrdinal("Password")));
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
                    cmd.Parameters.AddWithValue("@Id", id);

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
                    cmd.Parameters.AddWithValue("@Id", userId);

                    object? obj = cmd.ExecuteScalar();
                    if (obj == null || obj == DBNull.Value) return null;
                    return Convert.ToString(obj);
                }
            }
        }

        public void UpdatePasswordHash(long userId, string newPasswordHash)
        {
            const string sql = @"UPDATE dbo.Users SET Password = @Hash WHERE Id = @Id;";

            using (SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", userId);
                    cmd.Parameters.AddWithValue("@Hash", newPasswordHash);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
