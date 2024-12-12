using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Assignement_2v2.Menu;

namespace Assignement_2v2
{
    public class Surgeons : Staff, PasswordChange
    {
        private string specialty;

        /// <summary>
        /// A constructor for the Surgeon class.
        /// </summary>
        /// <param name="name">The name of the surgeon</param>
        /// <param name="age">The age of the surgeon</param>
        /// <param name="mobile_number">The mobile number of the surgeon</param>
        /// <param name="email">The email of the surgeon</param>
        /// <param name="password">The password for login</param>
        /// <param name="staff_id">The staff ID of the surgeon</param>
        /// <param name="specialty">The surgeon's speciality</param>
        public Surgeons(string name, int age, string mobile_number, string email, string password, int staff_id, string specialty)
            : base(name, age, mobile_number, email, password, staff_id)
        {
            this.specialty = specialty;
        }

        // Encapsulation for Specialty
        public string GetSpecialty()
        {
            return specialty;
        }

        public void SetSpecialty(string specialty)
        {
            this.specialty = specialty;
        }

        

        private List<Patients> assignedPatients = new List<Patients>(); // List to store multiple patients

        // Add a patient to the list of patients for this surgeon
        public void AddPatient(Patients patient)
        {
            assignedPatients.Add(patient);
        }

        // Retrieve the list of patients assigned to this surgeon
        public List<Patients> GetPatients()
        {
            return assignedPatients;
        }
        /// <summary>
        /// Displays the list of patients assigned to the specified surgeon for surgery. 
        /// If no patients are assigned, an error message is displayed and the operation is terminated.
        /// else,the names of all assigned patients are printed in a numbered list.
        /// </summary>
        /// <param name="Surgeon">The surgeon whose list of assigned patients will be displayed.</param>

        public void SeePatients(Surgeons Surgeon)
        {
            List<Patients> assignedPatients = Surgeon.GetPatients();
            UI.DisplayMessage("Your Patients.");
            if (assignedPatients.Count == 0)
            {
                UI.DisplayMessage(UI.NoPatientsAssigned);
                return;
            }
            else
                for (int i = 0; i < assignedPatients.Count; i++)
                {
                    Console.WriteLine($"{i + 1}.{assignedPatients[i].GetName()}");
                }
        }
        /// <summary>
        /// Displays the list of patients assigned to surgery for a given surgeon
        /// If no patients are assigned, print an error message and terminate the logic then
        /// For each patient with a surgery date, their surgery details are displayed in a formatted message
        /// If a patient does not have a surgery date, print out an error message
        /// </summary>
        /// <param name="Surgeon">The surgeon whose assigned patients' surgery schedule will be displayed.</param>

        public void SeeSchedule(Surgeons Surgeon)
        {
            // Get the list of assigned patients for the surgeon
            List<Patients> assignedPatients = Surgeon.GetPatients();
            UI.DisplayMessage("Your schedule.");
            // If there are no patients assigned, display an appropriate message
            if (assignedPatients.Count == 0)
            {
                UI.DisplayMessage(UI.NoPatientsAssigned);
                return;
            }
            // Go through each assigned patient and display their surgery details
            assignedPatients.Sort(CompareSurgeryDate);
            foreach (Patients patient in assignedPatients)
            {
                string dateSTR = DateTime.Parse(patient.SurgeryDate).ToString("HH:mm dd/MM/yyyy"); // Format the date for display

                // Ensure that the surgery date is not null or empty before displaying
                if (!string.IsNullOrEmpty(dateSTR))
                {
                    string patientName = patient.GetName();
                    UI.DisplayMessage($"Performing surgery on patient {patientName} on {dateSTR}");
                }
                else
                {
                    // If there's no surgery date assigned to the patient, display a message
                    UI.DisplayMessage($"No surgery date assigned for patient {patient.GetName()}.");
                }
            }
        }
        /// <summary>
        /// Performing surgery on a selected patient for a given surgeon.
        /// The method prompts the user to select a patient from the list of assigned patients for surgery
        /// After selecting a patient, the method updates the surgery records, moves the patient from the surgery list
        /// to the assigned rooms list, and adjusts the patient's position in the assigned rooms list as necessary
        /// </summary>
        /// <param name="Surgeon">The surgeon performing the surgery on the selected patient.</param>
        /// <param name="assignedPatientsSurgery">A list of patients scheduled for surgery.</param>
        /// <param name="assignedPatientsRoom">A list of patients currently assigned to rooms.</param>
        public void PerformSurgery(Surgeons Surgeon, List<Patients> assignedPatientsSurgery, List<Patients> assignedPatientsRoom)
        {
            List<Patients> assignedPatients = Surgeon.GetPatients();
            UI.DisplayMessage("Please select your patient:");
            for (int i = 0; i < assignedPatientsSurgery.Count; i++)
            {
                Console.WriteLine($"{i + 1}.{assignedPatientsSurgery[i].GetName()}");
            }
            int choice_patient = UI.GetValidChoice(assignedPatientsSurgery.Count);
            // Ensure the chosen index is within bounds
            if (choice_patient < 0 || choice_patient >= assignedPatientsSurgery.Count)
            {
                UI.DisplayErrorAgain(UI.ChoiceOutRange);
                return;
            }
            int patientIndex = choice_patient;
            UI.DisplayMessage($"Surgery performed on {assignedPatientsSurgery[choice_patient].GetName()} by {Surgeon.GetName()}.");
            Patients selectedPatient = assignedPatientsSurgery[choice_patient];
            if (patientIndex >= assignedPatientsRoom.Count)
            {
                // If the index is out of bounds, add the patient to the end of the list
                assignedPatientsRoom.Add(selectedPatient);
            }
            else
            {
                // Otherwise, insert the patient at the original index
                assignedPatientsRoom.Insert(patientIndex, selectedPatient);
            }
            assignedPatientsSurgery.RemoveAt(choice_patient);
            // Continue with existing logic
        }
        //Method to compare Surgery Date
        private int CompareSurgeryDate(Patients p1, Patients p2)
        {
            DateTime date1 = DateTime.Parse(p1.SurgeryDate); // Convert p1's surgery date to DateTime
            DateTime date2 = DateTime.Parse(p2.SurgeryDate); // Convert p2's surgery date to DateTime
            return date1.CompareTo(date2); // Compare the dates
        }
    }
}
