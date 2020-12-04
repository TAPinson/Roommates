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

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName, RentPortion, MoveInDate, RoomId FROM Roommate";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Roommate> roommates = new List<Roommate>();
                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosition);

                        int lastNameColumnPosition = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);

                        int rentPortionPosition = reader.GetOrdinal("RentPortion");
                        int rentPortionValue = reader.GetInt32(rentPortionPosition);

                        int moveInDatePosition = reader.GetOrdinal("MoveInDate");
                        DateTime moveInDataValue = reader.GetDateTime(moveInDatePosition);

                        int roomIdPosition = reader.GetOrdinal("RoomId");
                        int roomIdValue = reader.GetInt32(roomIdPosition);
                        
                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            Firstname = firstNameValue,
                            Lastname = lastNameValue,
                            RentPortion = rentPortionValue,
                            MovedInDate = moveInDataValue,
                            Room = new Room()
                            {
                                Id = roomIdValue
                            }
                        };
                        roommates.Add(roommate);
                    }
                    reader.Close();
                    return roommates;
                }
            }    
        }
    }
}
