using DAL;
using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Repos
{
    public class GameRepo : IGameRepo
    {
        public int AddGame(GameDTO game)
        {
            const string sql = @"INSERT INTO dbo.Game (Name, Description, Category, Picture, CreatedByUserId) OUTPUT INSERTED.Id VALUES (@Name, @Description, @Category, @Picture, @CreatedByUserId); ";

            using (SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = game.Name;
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = game.Description;
                    cmd.Parameters.Add("@Category", SqlDbType.NVarChar, 100).Value = game.Category;
                    cmd.Parameters.Add("@Picture", SqlDbType.VarBinary, -1).Value = game.Picture;
                    cmd.Parameters.Add("@CreatedByUserId", SqlDbType.Int).Value = game.CreatedByUserId;

                    object? newIdObj = cmd.ExecuteScalar();
                    return Convert.ToInt32(newIdObj);
                }
            }
        }

        public List<GameDTO> GetGames()
        {
            List<GameDTO> games = new List<GameDTO>();

            const string sql = "SELECT Id, Name, Category, Description, Picture FROM dbo.Game";
            
            using (SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            games.Add
                            (new GameDTO
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? string.Empty : reader.GetString(reader.GetOrdinal("Category")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? string.Empty : reader.GetString(reader.GetOrdinal("Description")),
                                Picture = reader.IsDBNull(reader.GetOrdinal("Picture")) ? null : (byte[])reader["Picture"]
                            });
                        }
                    }
                }
                return games;
            }
        }

        public void EditGame(GameDTO game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));

            const string sql = @"UPDATE Game SET Name = @Name, Category = @Category, Description = @Description, Picture = @Picture WHERE Id = @Id; ";

            using (SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString))
            {
                conn.Open();

                using(SqlCommand cmd = new SqlCommand(sql,conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = game.Id;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = game.Name;
                    cmd.Parameters.Add("@Category", SqlDbType.NVarChar, 100).Value = game.Category;
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = game.Description;
                    cmd.Parameters.Add("@Picture", SqlDbType.VarBinary, -1).Value = game.Picture;
                    cmd.ExecuteNonQuery();
                }

            }
        }


        public void DeleteGame(int id)
        {
            const string sql = @"DELETE FROM Game WHERE Id = @Id;";

            using (SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public GameDTO? GetGameById(int id)
        {
            const string sql = "SELECT Id, Name, Category, Description, Picture, CreatedByUserId FROM dbo.Game WHERE Id = @Id";

            using var conn = new SqlConnection(DatabaseConnectionString.ConnectionString);
            conn.Open();

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new GameDTO
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? string.Empty : reader.GetString(reader.GetOrdinal("Category")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? string.Empty : reader.GetString(reader.GetOrdinal("Description")),
                Picture = reader.IsDBNull(reader.GetOrdinal("Picture")) ? null : (byte[])reader["Picture"],
                CreatedByUserId = reader.GetInt32(reader.GetOrdinal("CreatedByUserId")),
            };
        }

        public byte[]? GetImageBlob(int id)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnectionString.ConnectionString))
            {
                connection.Open();

                const string query = "SELECT Picture FROM dbo.Game WHERE Id = @id";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value) return null;

                    return (byte[])result;
                }
            }
        }

    }

}
