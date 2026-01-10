using DAL;
using DTOs;
using Interfaces;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;

namespace Repos
{
    public class GameRepo : IGameRepo
    {
        public List<GameDTO> GetGames()
        {
            List<GameDTO> games = new List<GameDTO>();

            using (SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name, Category, Description, Picture FROM dbo.Game", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            games.Add
                            (new GameDTO
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name"),
                                Category = reader.IsDBNull(reader.GetOrdinal("Category"))
                                    ? string.Empty
                                    : reader.GetString("Category"),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                                    ? string.Empty
                                    : reader.GetString("Description"),
                                Picture = reader.IsDBNull(reader.GetOrdinal("Picture"))
                                    ? null
                                    : (byte[])reader["Picture"]
                            });
                        }
                    }
                }
            }

            return games;
        }

        public async Task<int> AddGame(GameDTO game, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO dbo.Game (Name, Description, Category, Picture)
                OUTPUT INSERTED.Id
                VALUES (@Name, @Description, @Category, @Picture);
            ";

            using SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString);
            await conn.OpenAsync(cancellationToken).ConfigureAwait(false);

            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Name", game.Name);
            cmd.Parameters.AddWithValue("@Description", (object?)game.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Category", (object?)game.Category ?? DBNull.Value);

            cmd.Parameters.Add("@Picture", SqlDbType.VarBinary, -1).Value =
                (object?)game.Picture ?? DBNull.Value;

            object? newIdObj = await cmd.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            return Convert.ToInt32(newIdObj);
        }

        public GameDTO GetGameById(int id)
        {
            GameDTO? game = null;

            using var conn = new SqlConnection(DatabaseConnectionString.ConnectionString);
            conn.Open();

            const string query = "SELECT Id, Name, Category, Description, Picture FROM dbo.Game WHERE Id = @Id";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                game = new GameDTO
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Category = reader.IsDBNull(reader.GetOrdinal("Category"))
                        ? string.Empty
                        : reader.GetString("Category"),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                        ? string.Empty
                        : reader.GetString("Description"),
                    Picture = reader.IsDBNull(reader.GetOrdinal("Picture"))
                        ? null
                        : (byte[])reader["Picture"]
                };
            }

            return game!;
        }

        public byte[]? GetImageBlob(int id)
        {
            using var connection = new SqlConnection(DatabaseConnectionString.ConnectionString);
            connection.Open();

            const string query = "SELECT Picture FROM dbo.Game WHERE Id = @id";

            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            var result = cmd.ExecuteScalar();

            if (result == null || result == DBNull.Value) return null;
            return (byte[])result;
        }
    }

    internal static class SqlDataReaderExtensions
    {
        public static int GetInt32(this SqlDataReader reader, string name) =>
            reader.GetInt32(reader.GetOrdinal(name));

        public static string GetString(this SqlDataReader reader, string name) =>
            reader.GetString(reader.GetOrdinal(name));
    }
}
