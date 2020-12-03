using System;
using System.Collections.Generic;
using System.Text;
using Roommates.Models;
using Microsoft.Data.SqlClient;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Roommate.FirstName, Roommate.RentPortion, Room.Name AS OccupiedRoom, Room.MaxOccupancy AS RoomOccupancy FROM Roommate JOIN Room ON Roommate.RoomId = Room.Id WHERE Roommate.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate()
                        {
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("FirstName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            // RoomString = reader.GetString(reader.GetOrdinal("RoomName"))
                            Room = new Room()
                            {
                                Name = reader.GetString(reader.GetOrdinal("OccupiedRoom")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("RoomOccupancy"))
                            }
                        };
                    }
                    reader.Close();
                    return roommate;
                }

            }
            
        }
    }
}
