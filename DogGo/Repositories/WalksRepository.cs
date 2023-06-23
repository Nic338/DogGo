using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly IConfiguration _config;

        public WalksRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        public List<Walks> GetWalksByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Walks.Id as WalkId, Walks.Date, Walks.Duration, Walks.DogId, Owner.Id as OwnerId, Owner.[Name] as OwnerName
                        FROM Walks
                        JOIN Dog on Walks.DogId = Dog.Id
                        JOIN Owner on Dog.OwnerId = Owner.Id
                        WHERE WalkerId = @walkerId";

                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walks> walks = new List<Walks>();

                    while (reader.Read())
                    {
                        Walks walk = new Walks
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("WalkId")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = TimeSpan.FromSeconds(reader.GetInt32(reader.GetOrdinal("Duration"))),                          
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Owner = new Owner
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                            }
                        };
                        walks.Add(walk);
                    }
                    reader.Close();

                    return walks;
                }
            }
        }
    }
}
