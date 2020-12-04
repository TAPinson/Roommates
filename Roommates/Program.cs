using System;
using System.Collections.Generic;
using System.Linq;
using Roommates.Repositories;
using Roommates.Models;


namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        showAllRooms(roomRepo);
                        break;
                    case ("Search for room"):
                        searchForRoom(roomRepo);
                        break;
                    case ("Add a room"):
                        addARoom(roomRepo);
                        break;
                    case ("Update a room"):
                        updateARoom(roomRepo);
                        break;
                    case ("Delete a room"):
                        deleteRoom(roomRepo);
                        break;
                    case ("Show all chores"):
                        showAllChores(choreRepo);
                        break;
                    case ("Show unassigned chores"):
                        showUnassignedChores(choreRepo);
                        break;
                    case ("Search for chore"):
                        searchForChore(choreRepo);
                        break;
                    case ("Add a chore"):
                        addAChore(choreRepo);
                        break;
                    case ("Update a chore"):
                        updateChore(choreRepo);
                        break;
                    case ("Search for a roommate"):
                        searchForRoommate(roommateRepo);
                        break;
                    case ("Assign chore to roommate"):
                        assignChore(choreRepo, roommateRepo);
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

            static void updateChore(ChoreRepository choreRepo)
            {
                List<Chore> choresToUpdate = choreRepo.GetAll();
                foreach (Chore c in choresToUpdate)
                {
                    Console.WriteLine($"{c.Id} - {c.Name}");
                }
                Console.Write("Select a chore to update ");
                int choreUpdateId = int.Parse(Console.ReadLine());
                Console.Write("Enter the new name: ");
                string choreUpdateName = Console.ReadLine();
                choreRepo.Update(choreUpdateId, choreUpdateName);
                Console.Write("Press any key to continue");
                Console.ReadKey();
            }

            static void deleteRoom(RoomRepository roomRepo)
            {
                List<Room> roomsAtRisk = roomRepo.GetAll();
                foreach (Room r in roomsAtRisk)
                {
                    Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                }
                Console.Write("Select a room to delete: ");
                int roomToDelete = int.Parse(Console.ReadLine());
                roomRepo.Delete(roomToDelete);
                Console.Write("Press any key to continue");
                Console.ReadKey();
            }

            static void assignChore(ChoreRepository choreRepo, RoommateRepository roommateRepo)
            {
                List<Chore> proposedChores = choreRepo.GetAll();
                foreach (Chore c in proposedChores)
                {
                    Console.WriteLine($"{c.Id} - {c.Name}");
                }
                Console.Write("Select the Id of the chore you want ");
                int chosenChore = int.Parse(Console.ReadLine());
                Console.WriteLine("-------------------------");
                List<Roommate> proposedRoommates = roommateRepo.GetAll();
                foreach (Roommate r in proposedRoommates)
                {
                    Console.WriteLine($"{r.Id}) {r.Firstname} {r.Lastname}");
                }
                Console.Write("Select the Id of the roommate to assign ");
                int chosenRoommate = int.Parse(Console.ReadLine());
                choreRepo.AssignChore(chosenRoommate, chosenChore);
            }

            static void searchForRoommate(RoommateRepository roommateRepo)
            {
                Console.Write("Roommate Id: ");
                int roommateId = int.Parse(Console.ReadLine());
                Roommate roommate = roommateRepo.GetById(roommateId);
                Console.WriteLine($"{roommate.Firstname} - Rent Portion: {roommate.RentPortion}% - {roommate.Room.Name}");
                Console.Write("Press any key to continue");
                Console.ReadKey();
            }

            static void addAChore(ChoreRepository choreRepo)
            {
                Console.Write("Chore name: ");
                string choreName = Console.ReadLine();
                Chore choreToAdd = new Chore()
                {
                    Name = choreName
                };
                choreRepo.Insert(choreToAdd);
                Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                Console.Write("Press any key to continue");
                Console.ReadKey();
            }

            static void searchForChore(ChoreRepository choreRepo)
            {
                Console.Write("Chore Id: ");
                int choreId = int.Parse(Console.ReadLine());
                Chore chore = choreRepo.GetById(choreId);
                Console.WriteLine($"{chore.Name} - {chore.Id}");
                Console.Write("Press any key to continue");
                Console.ReadKey();
            }

            static void showUnassignedChores(ChoreRepository choreRepo)
            {
                List<Chore> unassignedChores = choreRepo.GetUnassignedChores();
                foreach (Chore c in unassignedChores)
                {
                    Console.WriteLine($"{c.Id} - {c.Name}");
                }
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }

            static void showAllChores(ChoreRepository choreRepo)
            {
                List<Chore> chores = choreRepo.GetAll();
                foreach (Chore c in chores)
                {
                    Console.WriteLine($"{c.Id} - {c.Name}");
                }
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }

            static void updateARoom(RoomRepository roomRepo)
            {
                List<Room> roomOptions = roomRepo.GetAll();
                foreach (Room r in roomOptions)
                {
                    Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                }

                Console.Write("Which room would you like to update? ");
                int selectedRoomId = int.Parse(Console.ReadLine());
                Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);
                Console.Write("New Name: ");
                selectedRoom.Name = Console.ReadLine();
                Console.Write("New Max Occupancy: ");
                selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());
                roomRepo.Update(selectedRoom);
                Console.WriteLine($"Room has been successfully updated");
                Console.Write("Press any key to continue");
                Console.ReadKey();
            }

            static void addARoom(RoomRepository roomRepo)
            {
                Console.Write("Room name: ");
                string name = Console.ReadLine();
                Console.Write("Max occupancy: ");
                int max = int.Parse(Console.ReadLine());
                Room roomToAdd = new Room()
                {
                    Name = name,
                    MaxOccupancy = max
                };
                roomRepo.Insert(roomToAdd);
                Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                Console.Write("Press any key to continue");
                Console.ReadKey();
            }

            static void searchForRoom(RoomRepository roomRepo)
            {
                Console.Write("Room Id: ");
                int id = int.Parse(Console.ReadLine());
                Room room = roomRepo.GetById(id);
                Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                Console.Write("Press any key to continue");
                Console.ReadKey();
            }

            static void showAllRooms(RoomRepository roomRepo)
            {
                List<Room> rooms = roomRepo.GetAll();
                foreach (Room r in rooms)
                {
                    Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                }
                Console.Write("Press any key to continue");
                Console.ReadKey();
            }
        }
        static string GetMenuSelection()
        {
            Console.Clear();
            List<string> options = new List<string>()
        {
            "Show all rooms",
            "Search for room",
            "Update a room",
            "Add a room",
            "Delete a room",
            "Show all chores",
            "Show unassigned chores",
            "Search for chore",
            "Add a chore",
            "Update a chore",
            "Search for a roommate",
            "Assign chore to roommate",
            "Exit"
        };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");
                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}
