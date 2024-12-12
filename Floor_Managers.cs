using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Assignement_2v2.Menu;

namespace Assignement_2v2
{
    public class Floor_Managers :Staff, PasswordChange
    {
        private int Floor_Number { get; set; }

        /// <summary>
        /// A constructor for the FloorManager class.
        /// </summary>
        /// <param name="name">The name of the FloorManager</param>
        /// <param name="age">The age of the FloorManager</param>
        /// <param name="mobile_number">The mobile number of the FloorManager</param>
        /// <param name="email">The email of the FloorManager</param>
        /// <param name="password">The password for login</param>
        /// <param name="staff_id">The staff ID of the FloorManager</param>
        /// <param name="floor_Number">The FloorManager's FloorNumber</param>
        public Floor_Managers(string name, int age, string mobile_number, string email, string password, int staff_id, int floor_Number)
        : base(name, age, mobile_number, email, password, staff_id)
        {
            this.Floor_Number = floor_Number;
        }

        // Encapsulation for FloorNumber
        public int GetFloorNumber()
        {
            return Floor_Number;
        }

        public void SetFloorNumber(int floor_Number)
        {
            this.Floor_Number = floor_Number;
        }
        /// <summary>
        /// AssignRoom Method
        /// Displays the list of unassigned patients and prompts the user to select a patient and assign them to a room.
        /// Removes the selected patient from the unassignedPatients list and moves them to the assignedPatientsRoom list.
        /// The method checks if no patients are registered or if all rooms are occupied.
        /// If no rooms are available or the patient hasn't checked in, they cannot be assigned a room.
        /// </summary>
        /// <param name="unassignedPatients">The list of unassigned patients.</param>
        /// <param name="assignedPatientsRoom">The list of patients who have been assigned rooms.</param>
        /// <param name="patientRoomDirectory">The dictionary for storing the patient's name and their assigned room number.</param>
        public void AssignRoom(List<Patients> unassignedPatients, List<Patients> assignedPatientsRoom, Dictionary<string, int> patientRoomDirectory)
        {
            if (patientRoomDirectory.Count >= 10)
            {
                UI.DisplayError("All rooms on this floor are assigned");
                return;
            }
            if (unassignedPatients.Count == 0 && assignedPatientsRoom.Count == 0)
            {
                UI.DisplayMessage("There are no registered patients.");
                return;

            }

            bool hasCheckedInPatients = false;
            for (int i = 0; i < unassignedPatients.Count; i++)
            {
                if (unassignedPatients[i].IsCheckedIn)
                {   
                    hasCheckedInPatients = true;
                    break;
                }
            }
            if (!hasCheckedInPatients)
            {
                UI.DisplayMessage("There are no checked in patients.");
                return;
            }
            // Display unassigned patients
            DisplayUnassignedPatients(unassignedPatients, patientRoomDirectory);

            // Select a patient
            Patients selectedPatient = SelectPatient(unassignedPatients);

            // Assign room number using the RoomNumber() method
            RoomNumber(unassignedPatients.IndexOf(selectedPatient), unassignedPatients, assignedPatientsRoom, patientRoomDirectory);
        }
        private void DisplayUnassignedPatients(List<Patients> unassignedPatients, Dictionary<string, int> patientRoomDirectory)
        {
            UI.DisplayMessage("Please select your patient:");
            for (int i = 0; i < unassignedPatients.Count; i++)
            {
                if (!patientRoomDirectory.ContainsKey(unassignedPatients[i].GetName()))
                {
                    Console.WriteLine($"{i + 1}. {unassignedPatients[i].GetName()}");
                }
            }
        }
        /// <summary>
        /// Error Handling the logic of selecting the patient 
        /// </summary>
        /// <param name="unassignedPatients"></param>
        /// <returns></returns> return said index in the list of unassignedPatients
        private Patients SelectPatient(List<Patients> unassignedPatients)
        {
            bool IsValidChoice = false;
            int choice_patient;

            do
            {
                UI.DisplayMessage($"Please enter a choice between 1 and {unassignedPatients.Count}:");
                choice_patient = UI.GetInt() - 1;

                if (choice_patient < 0 || choice_patient >= unassignedPatients.Count)
                {
                    UI.DisplayErrorAgain("Supplied value is out of range");
                    IsValidChoice = false; // Loop will continue
                }
                else
                {
                    IsValidChoice = true; // Valid choice, exit the loop
                }
            } while (!IsValidChoice);

            return unassignedPatients[choice_patient]; // Return the valid selection
        }
        /// <summary>
        /// Prompts the user to enter a room number, validates the input, assigns the room to the patient, 
        /// and updates the lists of unassigned and assigned patients, as well as the room directory.
        /// </summary>
        /// <param name="choice_patient">The index of the patient from the unassignedPatients list.</param>
        /// <param name="unassignedPatients">A list of patients who have not yet been assigned a room.</param>
        /// <param name="assignedPatientsRoom">A list of patients who have been assigned a room.</param>
        /// <param name="patientRoomDirectory">A dictionary where the key is the patient's name, and the value is their assigned room number.</param>
        /// <returns>None. The function updates the lists and directory for room assignments.</returns>
        public void RoomNumber(int choice_patient, List<Patients> unassignedPatients, List<Patients> assignedPatientsRoom, Dictionary<string, int> patientRoomDirectory)
        {
            int roomNumber;
            bool isValidRoomNumber = true;
            do
            {
                // Prompt the user to enter the room number
                UI.DisplayMessage("Please enter your room (1 - 10):");
                roomNumber = UI.GetInt();

                // Check if the room number is valid
                if (roomNumber < 1 || roomNumber > 10)
                {
                    UI.DisplayErrorAgain(UI.ChoiceOutRange);
                    isValidRoomNumber = false;
                }
                else if (patientRoomDirectory.ContainsValue(roomNumber))
                {
                    UI.DisplayErrorAgain("Room has been assigned to another patient");
                    isValidRoomNumber = false;
                }
                else
                {
                    isValidRoomNumber = true;
                }
            } while (!isValidRoomNumber);

            // Assign the room to the patient
            unassignedPatients[choice_patient].RoomNumber = roomNumber;
            string patientName = unassignedPatients[choice_patient].GetName();
            UI.DisplayMessage($"Patient {patientName} has been assigned to room number {roomNumber} on floor {this.Floor_Number}.");

            // Remove the patient from the unassigned list and add to assigned list
            Patients selectedPatient = unassignedPatients[choice_patient];
            unassignedPatients.RemoveAt(choice_patient);
            assignedPatientsRoom.Add(selectedPatient);

            // Assign the corresponding Patient to their Room Number
            patientRoomDirectory[patientName] = roomNumber;
        }

        /// <summary>
        /// Assign Surgery Method
        /// Displaying a list of patients that are asssigned a room then prompts the user to select which patient, select which surgeon
        /// and assigning them to each other along with entering a valid time and date then finally report success to the user 
        /// that the surgery has been assigned
        /// </summary>
        /// <param name="Floor_Manager"></param> the instance of the class
        /// <param name="registeredPatients"></param> the list of registeredPatients
        /// <param name="assignedPatientsRoom"></param> the list of Patients that are assigned a room
        /// <param name="patientRoomDirectory"></param> the dictionary which holds the name and roomnumber of the patient
        /// <param name="registeredSurgeons"></param> the list of all the registered surgeons
        /// <param name="assignedPatientsSurgery"></param> the list of patients asigned to a surgery

        public void Assign_Surgery(Floor_Managers Floor_Manager, List<Patients> registeredPatients, List<Patients> assignedPatientsRoom, Dictionary<string, int> patientRoomDirectory, List<Surgeons> registeredSurgeons , List<Patients> assignedPatientsSurgery)
        {
            if (!CheckRegisteredPatients(Floor_Manager, registeredPatients))
            {
                return;
            }

            if (!CheckAssignedPatientsSurgery(Floor_Manager, assignedPatientsRoom))
            {
                return;
            }

            DisplayPatientsRoom(assignedPatientsRoom);

            int choice_patient = UI.GetValidChoice(assignedPatientsRoom.Count);

            DisplaySurgeons(registeredSurgeons);

            int choice_Surgeon = UI.GetValidChoice(registeredSurgeons.Count);

            AssignPatientSurgeon(choice_patient, choice_Surgeon, assignedPatientsRoom , registeredSurgeons);

            AssignSurgeryDate(choice_patient, choice_Surgeon, assignedPatientsRoom, registeredSurgeons);

            MarkPatientAssignedSurgery(choice_patient, assignedPatientsRoom, registeredSurgeons, assignedPatientsSurgery);

        }
        //Check to see if there are any patients registered
        //If not returns false and the logic terminates
        private bool CheckRegisteredPatients(Floor_Managers Floor_Manager, List<Patients> registeredPatients)
        {
            if (registeredPatients.Count == 0)
            {
                UI.DisplayMessage("There are no registered patients.");
                return false;
            }
            return true;
        }
        //Check to see if there are any patients assigned to a room
        //If not returns false and the logic terminates
        private bool CheckAssignedPatientsSurgery(Floor_Managers Floor_Manager, List<Patients> assignedPatientsRoom)
        {
            if (assignedPatientsRoom.Count == 0)
            {
                UI.DisplayMessage("There are no patients ready for surgery.");
                return false;
            }
            return true;
        }
        //Display the list of patients that are assigned a Room
        private void DisplayPatientsRoom(List<Patients> assignedPatientsRoom)
        {
            UI.DisplayMessage("Please select your patient:");
            for (int i = 0; i < assignedPatientsRoom.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {assignedPatientsRoom[i].GetName()}");
            }
        }
        //Display the list of Surgeons
        private void DisplaySurgeons(List<Surgeons> registeredSurgeons)
        {
            UI.DisplayMessage("Please select your surgeon:");
            for (int i = 0; i < registeredSurgeons.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {registeredSurgeons[i].GetName()}");
            }
        }
        //Assign the patient to the surgeon and adding the patient to a list of patient stored in the Surgeon class
        private void AssignPatientSurgeon(int choice_patient, int choice_Surgeon , List<Patients> assignedPatientsRoom, List<Surgeons> registeredSurgeons)
        {
            assignedPatientsRoom[choice_patient].SetSurgeon(registeredSurgeons[choice_Surgeon]);
            registeredSurgeons[choice_Surgeon].AddPatient(assignedPatientsRoom[choice_patient]);
        }

        //Assign the surgery date and reporting success to the user
        private void AssignSurgeryDate(int choice_patient, int choice_Surgeon, List<Patients> assignedPatientsRoom, List<Surgeons> registeredSurgeons)
        {
            UI.DisplayMessage("Please enter a date and time (e.g. 14:30 31/01/2024).");
            string date = UI.GetDateTime();
            //Assign the surgery Date to the patient
            assignedPatientsRoom[choice_patient].SurgeryDate = date;
            UI.DisplayMessage($"Surgeon {registeredSurgeons[choice_Surgeon].GetName()}has been assigned to patient {assignedPatientsRoom[choice_patient].GetName()}.");
            UI.DisplayMessage($"Surgery will take place on {date}.");
            //Altering the list
        }
        /// <summary>
        /// Setting the boolean value to true meaning the patient has had surgery for Check IN Check out logic
        /// Remove the said index from the patients that are assigned a room and adding to now patients that are assigned surgery
        /// </summary>
        /// <param name="choice_patient"></param>
        /// <param name="assignedPatientsRoom"></param>
        /// <param name="registeredSurgeons"></param>
        /// <param name="assignedPatientsSurgery"></param>
        private void MarkPatientAssignedSurgery(int choice_patient , List<Patients> assignedPatientsRoom, List<Surgeons> registeredSurgeons, List<Patients> assignedPatientsSurgery)
        {
            assignedPatientsRoom[choice_patient].CompletedSurgery = true;
            Patients selectedPatient = assignedPatientsRoom[choice_patient];
            assignedPatientsRoom.RemoveAt(choice_patient);
            assignedPatientsSurgery.Add(selectedPatient);
        }

        /// <summary>
        /// Unassign a room method
        /// Displays a list of Patients that are assigned a room then select which one, then that selection 
        /// is passed through the GetPatientName() method to obtian the KeyValue or pattienName to the dictionary
        /// then using the Key to get the value or Roomnumber and report success to the user
        /// </summary>
        /// <param name="Floor_Manager"></param>
        /// <param name="assignedPatientsRoom"></param>
        /// <param name="registeredSurgeons"></param>
        /// <param name="assignedPatientsSurgery"></param>
        /// <param name="patientRoomDirectory"></param>
        public void UnAssignRoom(Floor_Managers Floor_Manager, List<Patients> assignedPatientsRoom, List<Surgeons> registeredSurgeons, List<Patients> assignedPatientsSurgery , Dictionary<string, int> patientRoomDirectory)
        {
            if (!CheckUnassginPatientsRoom(Floor_Manager, assignedPatientsRoom))
            {
                return;
            }
            DisplayAssignedPatients(assignedPatientsRoom);

            int choice_patient = UI.Getint() - 1;

            string patientName = GetPatientName(choice_patient,  assignedPatientsRoom);

            UnassignPatientRoom(patientName, choice_patient , Floor_Manager, patientRoomDirectory , assignedPatientsRoom);

        }
        /// <summary>
        /// Displays a list of patients who have been assigned rooms and prompts the user to select a patient.
        /// </summary>
        /// <param name="assignedPatientsRoom">A list of patients that have already been assigned to rooms.</param>
        private void DisplayAssignedPatients(List<Patients> assignedPatientsRoom)
        {
            UI.DisplayMessage("Please select your patient:");
            for (int i = 0; i < assignedPatientsRoom.Count; i++)
            {
                UI.DisplayMessage($"{i + 1}. {assignedPatientsRoom[i].GetName()}");
            }
            UI.DisplayMessage($"Please enter a choice between 1 and {assignedPatientsRoom.Count}.");
        }
        /// <summary>
        /// Checks if there are any patients who have been assigned rooms and are eligible to have their rooms unassigned.
        /// </summary>
        /// <param name="Floor_Manager">The current floor manager handling the operation.</param>
        /// <param name="assignedPatientsRoom">A list of patients assigned to rooms.</param>
        /// <returns>Returns TRUE if there are patients with assigned rooms, otherwise FALSE if no patients are available for unassigning.</returns>
        private bool CheckUnassginPatientsRoom(Floor_Managers Floor_Manager, List<Patients> assignedPatientsRoom)
        {
            if (assignedPatientsRoom.Count == 0)
            {
                UI.DisplayMessage("There are no patients ready to have their rooms unassigned.");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Retrieves the name of the patient based on the selected index from the list of assigned patients.
        /// </summary>
        /// <param name="choice_patient">The index of the patient selected by the user.</param>
        /// <param name="assignedPatientsRoom">A list of patients assigned to rooms.</param>
        /// <returns>Returns the name of the selected patient.</returns>

        private string GetPatientName(int choice_patient , List<Patients> assignedPatientsRoom)
        {
            Patients selectedPatient = assignedPatientsRoom[choice_patient];
            string patientName = selectedPatient.GetName();
            return patientName;
        }

        /// <summary>
        /// Unassigns a patient from a room and updates the room directory and assigned patient list accordingly.
        /// The method takes the patient name and index of the selected patient to identify which patient’s room 
        /// should be unassigned.
        /// It then updates the room directory and moves the patient from the assigned patients list to reflect this change.
        /// </summary>
        /// <param name="patientName">The name of the patient whose room is being unassigned.</param>
        /// <param name="choice_patient">The index of the selected patient from the assigned list.</param>
        /// <param name="Floor_Manager">The floor manager responsible for handling the unassignment.</param>
        /// <param name="patientRoomDirectory">A dictionary that maps the patient’s name to their assigned room number.</param>
        /// <param name="assignedPatientsRoom">A list of patients who are currently assigned to rooms.</param>
        private void UnassignPatientRoom(string patientName, int choice_patient , Floor_Managers Floor_Manager , Dictionary<string, int> patientRoomDirectory , List<Patients> assignedPatientsRoom)
        {
            if (patientRoomDirectory.ContainsKey(patientName))
            {
                int roomNumber = patientRoomDirectory[patientName];
                UI.DisplayMessage($"Room number {roomNumber} on floor {Floor_Manager.GetFloorNumber()} has been unassigned.");
            }
            assignedPatientsRoom.RemoveAt(choice_patient);
        }



    }
}
        
    

        

