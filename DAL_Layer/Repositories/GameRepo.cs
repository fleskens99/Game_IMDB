using DAL;
using DTOs;
using Interfaces;
using MySql.Data.MySqlClient;
using System.Data;

namespace Repos
{
    public class GameRepo : IGameRepo
    {
        public List<GameDTO> GetGames()
        {
            List<GameDTO> games = new List<GameDTO>();

            using (MySqlConnection conn = new MySqlConnection(DatabaseConnectionString.ConnectionString))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT Id, Name, Category, Description, Picture FROM game", conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
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
                INSERT INTO game (Name, Description, Category, Picture)
                VALUES (@Name, @Description, @Category, @Picture);
            ";

            await using MySqlConnection conn = new MySqlConnection(DatabaseConnectionString.ConnectionString);
            await conn.OpenAsync(cancellationToken);

            await using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Name", game.Name);
            cmd.Parameters.AddWithValue("@Description", (object?)game.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Category", (object?)game.Category ?? DBNull.Value);
            cmd.Parameters.Add("@Picture", MySqlDbType.LongBlob).Value =
                (object?)game.Picture ?? DBNull.Value;

            await cmd.ExecuteNonQueryAsync(cancellationToken);
            return (int)cmd.LastInsertedId;
        }

        public GameDTO GetGameById(int id)
        {
            GameDTO? game = null;

            using var conn = new MySqlConnection(DatabaseConnectionString.ConnectionString);
            conn.Open();

            const string query = "SELECT Id, Name, Category, Description, Picture FROM game WHERE Id = @Id";

            using var cmd = new MySqlCommand(query, conn);
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

        public byte[] ? GetImageBlob(int id)
        {
            using var connection = new MySqlConnection(DatabaseConnectionString.ConnectionString);
            connection.Open();

            const string query = "SELECT Picture FROM game WHERE Id = @id";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            var result = cmd.ExecuteScalar();

            return result == DBNull.Value ? null : (byte[])result;
        }
    }
}
