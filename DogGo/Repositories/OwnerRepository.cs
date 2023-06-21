using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;


namespace DogGo.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;

        public OwnerRepository(IConfiguration config)
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

        public List<Owner> GetAllOwners()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Owner.Id, Owner.Email, Owner.[Name], Owner.[Address], Owner.NeighborhoodId, Neighborhood.Name as NeighborhoodName, Owner.Phone
                        FROM Owner
                        LEFT JOIN Neighborhood on Neighborhood.Id = Owner.NeighborhoodId
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Owner> owners = new List<Owner>();
                    while (reader.Read())
                    {
                        Owner owner = new Owner()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            Neighborhood = new Neighborhood
                            {
                                Name = reader.GetString(reader.GetOrdinal("NeighborhoodName"))
                            }
                        };

                        owners.Add(owner);
                    }
                    reader.Close();

                    return owners;
                }
            }
        }
        public Owner GetOwnerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Owner.Id, Owner.Email, Owner.[Name], Owner.Address, Owner.NeighborhoodId, Neighborhood.Name as NeighborhoodName, Owner.Phone, Dog.Id as DogId, Dog.Name as DogName, Dog.Breed, Dog.Notes, Dog.ImageUrl
                        FROM Owner
                        LEFT JOIN Neighborhood on Neighborhood.Id = Owner.NeighborhoodId
                        LEFT JOIN Dog on Dog.OwnerId = Owner.Id
                        WHERE Owner.Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    Owner owner = null;

                    while (reader.Read())
                    {
                        if (owner == null)
                        {
                            owner = new Owner
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                Neighborhood = new Neighborhood
                                {
                                    Name = reader.GetString(reader.GetOrdinal("NeighborhoodName"))
                                },
                                Dogs = new List<Dog>()
                            };
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("DogId")))
                        {
                            Dog dog = new Dog
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("DogId")),
                                Name = reader.GetString(reader.GetOrdinal("DogName")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                Notes = reader.IsDBNull(reader.GetOrdinal("Notes"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("Notes")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl"))
                                           ? null
                                           : reader.GetString(reader.GetOrdinal("ImageUrl")),
                                OwnerId = owner.Id
                            };

                            owner.Dogs.Add(dog);
                        }
                    }
                    reader.Close();

                    return owner;
                }
            }
        }
    }
}
