using DAL;
using Interfaces;
using MySql.Data.MySqlClient;
namespace Repo
{
    public class UserRepo : IUserRepo
    {
        public int AddUser(string name, string email, string password, byte[]? picture)
        {
            string sql =
                "INSERT INTO `user` (Name, Email, Password, Picture) " +
                "VALUES (@Name, @Email, @Password, @Picture); ";

            using MySqlConnection conn = new MySqlConnection(DatabaseConnectionString.ConnectionString);
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@Picture", (object?)picture ?? DBNull.Value);

            cmd.ExecuteScalar();
            return Convert.ToInt32(cmd.LastInsertedId);
        }

        public (int Id, string Name, string Email, string Password)? GetByEmail(string email)
        {
            string sql = @"SELECT Id, Name, Email, Password FROM user WHERE Email=@Email LIMIT 1;";

            using MySqlConnection conn = new MySqlConnection(DatabaseConnectionString.ConnectionString);

            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Email", email);

            using MySqlDataReader reader = cmd.ExecuteReader();

            if (!reader.Read()) return null;

            return(
                reader.GetInt32("Id"),
                reader.GetString("Name"),
                reader.GetString("Email"),
                reader.GetString("Password")
            );
        }

        public byte[]? GetPictureById(long id)
        {
            string sql = "SELECT Picture FROM `user` WHERE Id=@Id LIMIT 1;";

            using MySqlConnection conn = new MySqlConnection(DatabaseConnectionString.ConnectionString);
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            object obj = cmd.ExecuteScalar();
            if (obj == null || obj == DBNull.Value) return null;

            return (byte[])obj;
        }
        public string? GetPasswordHashById(long userId)
        {
            const string sql =
                "SELECT PasswordHash FROM `user` WHERE Id=@Id LIMIT 1;";

            using var conn = new MySqlConnection(DatabaseConnectionString.ConnectionString);
            conn.Open();

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", userId);

            var obj = cmd.ExecuteScalar();
            if (obj == null || obj == DBNull.Value) return null;

            return Convert.ToString(obj);
        }

        public void UpdatePasswordHash(long userId, string newPasswordHash)
        {
            const string sql =
                "UPDATE `user` SET PasswordHash=@Hash WHERE Id=@Id;";

            using var conn = new MySqlConnection(DatabaseConnectionString.ConnectionString);
            conn.Open();

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", userId);
            cmd.Parameters.AddWithValue("@Hash", newPasswordHash);

            cmd.ExecuteNonQuery();
        }
    }
}
