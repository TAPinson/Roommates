using System;
using System.Collections.Generic;
using System.Text;
using Roommates.Models;
using Microsoft.Data.SqlClient;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

        // Get a list of all Chores in the database

        public List<Chore> GetAll()
        {
            //Prep to open up a connection. We need SqlConnection for this part so we are "using" it
            using (SqlConnection conn = Connection)
            {
                //Actually open the connection
                conn.Open();

                // We will be "using" SqlCommand also
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Prep the SQL
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    //List for Chores retrieved from the db
                    List<Chore> chores = new List<Chore>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");

                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue
                        };

                        chores.Add(chore);
                    }

                    reader.Close();

                    return chores;
                };
            }
        }
    }
}
