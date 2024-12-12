using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Assignement_2v2.Menu;

namespace Assignement_2v2
{
    public class Patients : User, PasswordChange
    {
        private int roomNumber;
        private Surgeons assignedSurgeon;


        public int RoomNumber { get; set; } = 0; // 0  means no room is assigned

        public void SetSurgeon(Surgeons Surgeon)
        {
            this.assignedSurgeon = Surgeon;
        }

        public Surgeons GetSurgeon()
        {
            return this.assignedSurgeon;
        }

        public string SurgeryDate { get; set; }


        private bool _isCheckedIn = false; // Default value, patient starts as checked out

        /// <summary>
        /// A constructor for the Patient
        /// </summary>
        /// <param name="name">Name of the patient</param>
        /// <param name="age">Age</param>
        /// <param name="mobile_number">Phone number of patients</param>
        /// <param name="email">The email</param>
        /// <param name="password">The password</param>
        /// 
        public Patients(string name, int age, string mobile_number, string email, string password) :
            base(name, age, mobile_number, email, password)
        {
        }

        // Public property to access the check-in status
        public bool CompletedSurgery { get; set; }
        public bool IsCheckedIn
        {
            get { return _isCheckedIn; }
            private set { _isCheckedIn = value; }
        }

        // Method to return check-in or check-out string based on current state
        public string GetCheckStatusString()
        {
            if (IsCheckedIn)
            {
                return "Check out";
            }
            else
            {
                return "Check in";
            }
        }

        // Method to toggle the check-in status
        public void ToggleCheckInStatus()
        {
            {
                // Toggle the value of isCheckedIn
                IsCheckedIn = !IsCheckedIn;
            }
        }
        /// <summary>
        /// Display the check in and check out message in the menu and 
        /// if the patient hasnt had surgery but is checked in then prompt
        /// unable to check in and vice versa then toggle the check in status again 
        /// to keep the status message to check out after printing out an error.
        /// </summary>
        public void DisplayCheckIn()
        {
            ToggleCheckInStatus();
            // Get the current check-in status message
            string statusMessage = this.GetCheckStatusString();

            // Display the appropriate message 
            if (statusMessage == "Check out")
            {

                if (this.CompletedSurgery)
                {
                    UI.DisplayMessage("You are unable to check in at this time.");
                    ToggleCheckInStatus();
                    return;
                }
                else
                {
                    UI.DisplayMessage($"Patient {this.GetName()} has been checked in.");
                }
            }
            else if (statusMessage == "Check in")
            {
                if (!this.CompletedSurgery)
                {
                    UI.DisplayMessage("You are unable to check out at this time.");
                    ToggleCheckInStatus();
                }
                else
                {
                    UI.DisplayMessage($"Patient {this.GetName()} has been checked out.");
                }
            }
        }
        public void SeeRoom(Patients patient, Floor_Managers Floor_Manager)
        {
            int assignedRoom = patient.RoomNumber;
            if (assignedRoom == 0)
            {
                UI.DisplayMessage("You do not have an assigned room.");
            }
            else
            {
                UI.DisplayMessage($"Your room is number {assignedRoom} on floor {Floor_Manager.GetFloorNumber()}. ");
            }
        }
        /// <summary>
        /// Rettrieve the assigned surgeon and checking logic whether there is no surgeon
        /// resulting in a prompt and vice versa
        /// </summary>
        /// <param name="patient"></param>
        public void SeeSurgeon(Patients patient)
        {
            Surgeons assignedSurgeon = patient.GetSurgeon();

            if (assignedSurgeon == null)
            {
                UI.DisplayMessage("You do not have an assigned surgeon.");
            }
            else
            {
                string surgeonName = assignedSurgeon.GetName();

                UI.DisplayMessage($"Your surgeon is {surgeonName}.");
            }
        }
        /// <summary>
        /// Retreive the surgery date then check if the date is either blank or null
        /// resulting in 2 different prompts.
        /// </summary>
        /// <param name="Patient"></param>
        public void Surgerytime(Patients Patient)
        {
            string date = Patient.SurgeryDate;
            if (string.IsNullOrEmpty(date))
            {
                UI.DisplayMessage("You do not have assigned surgery.");
            }
            else
            {
                UI.DisplayMessage($"Your surgery time is {date}.");
            }
        }
    }
}
           
            
        
 

                 
                
            
        

    

