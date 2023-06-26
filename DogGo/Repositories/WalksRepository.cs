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
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
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
        public void AddWalks(Walks walk, List<int> SelectedDogIds)
        {
            //Putting a foreach loop in this so you can add a walk object for each dog selected from the drop down
            //Hold Ctrl and click each dog in the select to walk multiple dogs
            foreach (int dogId in SelectedDogIds)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {

                        cmd.CommandText = @"
                        INSERT INTO Walks (Date, Duration, WalkerId, DogId)
                        OUTPUT INSERTED.ID
                        VALUES (@date, @duration, @walkerId, @dogId);
                    ";

                        cmd.Parameters.AddWithValue("@date", walk.Date);
                        cmd.Parameters.AddWithValue("@duration", walk.Duration);
                        cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
                        cmd.Parameters.AddWithValue("@dogId", dogId);

                        int id = (int)cmd.ExecuteScalar();

                        walk.Id = id;
                    }
                }
            }
        }
        public void DeleteWalks(List<int> SelectedWalkIds)
        {
            foreach (int walkId in SelectedWalkIds)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                                DELETE FROM Walks
                                WHERE Id = @walkId
                            ";

                        cmd.Parameters.AddWithValue("@walkId", walkId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
