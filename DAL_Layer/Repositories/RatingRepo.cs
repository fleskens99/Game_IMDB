using DAL;
using DTOs;
using Entities;
using Interfaces;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;


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
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Rating.UserId;
            cmd.Parameters.Add("@GameId", SqlDbType.Int).Value = Rating.GameId;
            cmd.Parameters.Add("@Score", SqlDbType.Int).Value = Rating.Score;
            cmd.Parameters.Add("@Comment", SqlDbType.NVarChar, 500).Value = Rating.Comment;

            int newId = (int)cmd.ExecuteScalar();
            return newId;
        }

        public List<RatingDTO> GetRatingsByGame(int gameId)
        {
            List<RatingDTO> ratings = new();

            const string sql = @"
        SELECT Id, UserId, GameId, Score, Comment
        FROM dbo.Rating
        WHERE GameId = @GameId";

            using SqlConnection conn = new(DatabaseConnectionString.ConnectionString);
            conn.Open();

            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.Add("@GameId", SqlDbType.Int).Value = gameId;

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ratings.Add(new RatingDTO
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader.GetInt32("UserId"),
                    GameId = reader.GetInt32("GameId"),
                    Score = reader.GetDecimal("Score"),
                    Comment = reader.GetString("Comment")
                });
            }

            return ratings;
        }

        public bool UserHasRated(int userId, int gameId)
        {
            const string sql = @" SELECT COUNT(1) FROM dbo.Rating WHERE UserId = @UserId AND GameId = @GameId";

            using SqlConnection conn = new(DatabaseConnectionString.ConnectionString);
            conn.Open();

            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            cmd.Parameters.Add("@GameId", SqlDbType.Int).Value = gameId;

            return (int)cmd.ExecuteScalar() > 0;
        }

        public double GetAverageRatingForGame(int gameId)
        {
            const string sql = @"SELECT AVG(CAST(Score AS FLOAT)) FROM dbo.Rating WHERE GameId = @GameId";

            using SqlConnection conn = new(DatabaseConnectionString.ConnectionString);
            conn.Open();

            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.Add("@GameId", SqlDbType.Int).Value = gameId;

            object result = cmd.ExecuteScalar();

            return result == DBNull.Value ? 0 : Convert.ToDouble(result);
        }


    }
}
