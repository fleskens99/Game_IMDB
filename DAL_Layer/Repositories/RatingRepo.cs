using DAL;
using DTOs;
using Interfaces;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;


namespace Repos
{
    public class RatingRepo : IRatingRepo
    {
        public int AddRating(RatingDTO Rating)
        {
            const string sql = @"
                INSERT INTO dbo.Ratings (UserId, GameId, Score, Comment)
                OUTPUT INSERTED.Id
                VALUES (@UserId, @GameId, @Score, @Comment);";
            
            using SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", Rating.UserId);
            cmd.Parameters.AddWithValue("@GameId", Rating.GameId);
            cmd.Parameters.AddWithValue("@Score", Rating.Score);
            cmd.Parameters.AddWithValue("@Comment", Rating.Comment);
            int newId = (int)cmd.ExecuteScalar();
            return newId;
        }

        public List<RatingDTO> GetRatings() 
        {
            List<RatingDTO> ratings = new List<RatingDTO>();
            const string sql = @"SELECT Id, UserId, GameId, Score, Comment From dbo.Ratings";
            using SqlConnection conn = new SqlConnection(DatabaseConnectionString.ConnectionString);
            conn.Open();

            using SqlCommand cmd = new SqlCommand(sql, conn);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read()) 
                {
                    ratings.Add(new RatingDTO
                    {
                        Id = reader.GetInt32("Id"),
                        Score = reader.GetInt32("Score"),
                        Comment = reader.GetString("Comment"),
                        UserId = reader.GetInt32("UserId"),
                        GameId = reader.GetInt32("GameId")
                    });
                }
            }
            return ratings;

        }

    }
}
