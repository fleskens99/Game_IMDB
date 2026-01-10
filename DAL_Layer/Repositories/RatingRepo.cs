using DAL;
using DTOs;
using Interfaces;
using Microsoft.Data.SqlClient;


namespace Repos
{
    public class RatingRepo : IRatingRepo
    {
        public int AddRating(RatingDTO Rating)
        {
            const string sql = @"
                INSERT INTO dbo.Rating (UserId, GameId, Score, Comment)
                OUTPUT INSERTED.Id
                VALUES (@UserId, @GameId, @Score, @Comment);";
            
            using SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@GameId", Rating.GameId);
            cmd.Parameters.AddWithValue("@Score", Rating.Rate);
            cmd.Parameters.AddWithValue("@Comment", Rating.Comment);
            int newId = (int)cmd.ExecuteScalar();
            return newId;
        }

    }
}
