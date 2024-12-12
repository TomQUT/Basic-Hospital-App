using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assignement_2v2
{
    internal class Menu
    {
        private bool assigned;
        public Menu()
        {
            assigned = false;
        }
        public void Run()
        {
            DisplayHeader();
            bool keepGoing = true;
            // Keep running the menu until the user chooses to exit the program
            // This will be when the user selects exit in the DisplayMainMenu method
            while (keepGoing)
            {
                keepGoing = Opening_Menu();
            }

        }
        private void DisplayHeader()
        {
            Console.WriteLine("=================================");
            Console.WriteLine("Welcome to Gardens Point Hospital");
            Console.WriteLine("=================================");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

        }
        /// <summary>
        /// Displays the main menu to the screen
        /// </summary>
        /// <returns>Returns FALSE is the user wants to exit the program,  
        /// otherwise returns TRUE
        /// </returns>
        public bool Opening_Menu()
        {
            // Main Menu strings
            const string PleaseChoose_STR = "Please choose from the menu below:";// Header
            const string Login_STR = "Login as a registered user";// Option 1
            const string Register_STR = "Register as a new user";// Option 2
            const string Exit_STR = "Exit";// Option 3
            const string PleaseEnter_STR = "Please enter a choice between 1 and 3.";// Prompt

            // Array of options
            string[] options = { Login_STR, Register_STR, Exit_STR };
            bool validInput = false;
            while (!validInput)
            {
                // Display the menu and get the user's choice
                int choice = UI.GetOption(PleaseChoose_STR, PleaseEnter_STR, options);

                // Handle the user's choice
                switch (choice + 1) // Adding 1 to match the defined constants
                {
                    case 1:
                        // Code to handle login
                        LoginMenu();
                        validInput = true;
                        break; // Return a value
                    case 2:
                        // Code to handle registration
                        RegisterMenu();
                        validInput = true;
                        break;
                    case 3:
                        // Code to handle exit
                        Console.WriteLine("Goodbye. Please stay safe.");
                        validInput = true;
                        break;
                    default:
                        // Invalid choice, redirect back to the menu
                        UI.DisplayErrorAgain("Invalid Menu Option");
                        // The loop continues, and the menu is redisplayed
                        break;
                }
            }
            return false;
        }
        /// <summary>
        /// Displays a registration menu allowing the user to choose between registering
        /// as a patient, staff, or returning to the main menu.
        /// </summary>
        /// <returns>Returns FALSE if the user selects to return to the main menu.
        /// Otherwise, proceeds with the selected registration option or displays an error for invalid input.</returns>
        private bool RegisterMenu()
        {
            // Main Menu strings
            const string User_Type_STR = "Register as which type of user:"; // Header
            const string Patient_STR = "Patient"; // Option 1
            const string Staff_STR = "Staff"; // Option 2
            const string Exit_STR = "Return to the first menu"; // Option 3
            const string PleaseEnter_STR = "Please enter a choice between 1 and 3."; // Prompt

            // Array of options
            string[] options = { Patient_STR, Staff_STR, Exit_STR };

            // Display the menu and get the user's choice
            int choice = UI.GetOption(User_Type_STR, PleaseEnter_STR, options);

            // Handle the user's choice
            switch (choice + 1) // Adding 1 to match the defined constants
            {
                case 1:
                    CreatePatientMenu();
                    break;
                case 2:
                    StaffRegister();
                    break;
                case 3:
                    return Opening_Menu();
                default:
                    UI.DisplayErrorAgain("Invalid Menu Option");
                    Opening_Menu();
                    break;
            }
            return false;

        }
        /// <summary>
        /// Displays a staff registration menu, allowing the user to choose between registering 
        /// as a floor manager, surgeon, or returning to the main menu.
        /// </summary>
        /// <returns>Returns FALSE if the user selects to return to the main menu.
        /// Otherwise, proceeds with the selected staff registration option or displays an error for invalid input.</returns>
        private bool StaffRegister()
        {
            const string Staff_Type_STR = "Register as which type of staff:"; // Header
            const string FloorManager_STR = "Floor manager"; // Option 1
            const string Surgeon_STR = "Surgeon"; // Option 2
            const string Exit_STR = "Return to the first menu"; // Option 3
            const string PleaseEnter_STR = "Please enter a choice between 1 and 3."; // Prompt

            string[] options = { FloorManager_STR, Surgeon_STR, Exit_STR };

            int choice = UI.GetOption(Staff_Type_STR, PleaseEnter_STR, options);

            switch (choice + 1) // Adding 1 to match the defined constants
            {
                case 1://Floor_Manager
                    CreateFloor_ManagerMenu();
                    break;
                case 2://Surgeon
                    CreateSurgeonMenu();
                    break;
                case 3://Exit
                    Opening_Menu();
                    break;
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// Initilazing 5 lists for functionality 
        /// </summary>
        private List<User> registeredUsers = new List<User>();
        private Patients Patient;
        // A list to store registered patients
        List<Patients> registeredPatients = new List<Patients>();
        List<Patients> unassignedPatients = new List<Patients>();
        List<Patients> assignedPatientsRoom = new List<Patients>();
        List<Patients> assignedPatientsSurgery = new List<Patients>();

        /// <summary>
        /// Adding instances of patients to 2 list only if each of them does not already 
        /// have the instance to resolve duplication of instances of patients.
        /// </summary>
        public void CreatePatientList()
        {
            foreach (var user in registeredUsers)
            {
                if (user is Patients patient)
                {
                    if (!registeredPatients.Contains(patient))
                    {
                        registeredPatients.Add(patient);
                    }
                    if (!unassignedPatients.Contains(patient))
                    {
                        unassignedPatients.Add(patient);
                    }
                }
            }
        }
        /// <summary>
        /// Displays the menu for registering a new patient such as name, age, etc...
        /// Prompts the User to enter their credentials with logic checking validation usiing methods in the UI class
        /// Adding the new patient in the registeredUser's list 
        /// Then from that list proceed to add that same newly created instance to 2 other lists mentioned above.
        /// </summary>
        private void CreatePatientMenu()
        {
            Console.WriteLine("Registering as a patient.");
            string name = UI.GetName();
            // Prompt the user to enter their age
            int age = UI.GetAge("Patient");
            // Prompt the user to enter their mobile number
            string mobile_number = UI.GetMobileNumber();
            //Prompt the user to enter their emalil
            string email;
            bool emailExist;
            do
            {
                // Prompt the user to enter their email
                email = UI.GetEmail();

                // Check if the email is already registered
                emailExist = false;
                foreach (Patients patient in registeredPatients)
                {
                    if (patient.GetEmail() == email)
                    {
                        UI.DisplayErrorAgain("Email is already registered");
                        emailExist = true;
                        break;
                    }
                }
            } while (emailExist);
            // Prompt Password
            string password = UI.GetPassword();
            // Adding to the new List of patients and report success 
            Patient = new Patients(name, age, mobile_number, email, password);
            UI.DisplayMessage($"{name} is registered as a patient.");
            Patients newPatient = new Patients(name, age, mobile_number, email, password);
            //Add to a unified list for easier Login logic
            registeredUsers.Add(newPatient);
            //Create a patients list.
            CreatePatientList();
            assigned = true;
            Opening_Menu();
        }



        //Initializing The list for registered Floor Managers
        private Floor_Managers Floor_Manager;
        private List<Floor_Managers> registeredFloor_Managers = new List<Floor_Managers>();

        /// <summary>
        /// Method of adding instances of Floor_Managers to a list and with logic for handling duplicates instances
        /// </summary>
        public void CreateFloor_ManagerList()
        {
            foreach (var user in registeredUsers)
            {
                // If the user is a Surgeon, add them to the registeredSurgeons list
                if (user is Floor_Managers Floor_Manager)
                {
                    if (!registeredFloor_Managers.Contains(Floor_Manager))
                    {
                        registeredFloor_Managers.Add(Floor_Manager);
                    }
                }
            }
        }
        
        /// <summary>
        /// Method of checking whether All Floors are Assigned
        /// </summary>
        /// <returns></returns> returns the value true if there are still floors left to assign
        public bool IsAllFloorAssigned()
        {
            if (registeredFloor_Managers.Count >= 6)
            {
                UI.DisplayError("All floors are assigned");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Displays the menu for registering a new Floor_Manger such as name, age, etc...
        /// then creating a new instance of Floor Manager and adding them to the unified list RegisteredUsers
        /// then using that list to add to the 1 other list mentioned above
        /// <remarks> if all floors are asisnged then return to the opening menu if not continue with the logic
        /// </summary>
        private void CreateFloor_ManagerMenu()
        {
            if (IsAllFloorAssigned() == false)
            {
                Opening_Menu();
                return;
            }
            Console.WriteLine("Registering as a floor manager.");
            // Get the information from the user 

            string name = UI.GetName();

            int age = UI.GetAge("Floor_Manager");

            string mobile_number = UI.GetMobileNumber();

            string email = UI.GetEmail();

            string password = UI.GetPassword();

            int staff_id = checkValidStaffID();

            int floor_numb = checkFloorNumber();

            // Create the new manager and report success 
            Floor_Manager = new Floor_Managers(name, age, mobile_number, email, password, staff_id, floor_numb);
            UI.DisplayMessage($"{name} is registered as a floor manager.");
            Floor_Managers newFloor_Manager = new Floor_Managers(name, age, mobile_number, email, password, staff_id, floor_numb);
            registeredUsers.Add(newFloor_Manager);
            CreateFloor_ManagerList();
            assigned = true;
            Opening_Menu();
        }
        //Method to check whether the staffID has already existed when registering
        private int checkValidStaffID()
        {
            int staff_id;
            bool validStaffID = true;
            do
            {
                staff_id = UI.GetStaffID();
                for (int i = 0; i < registeredFloor_Managers.Count; i++)
                {
                    if ((registeredFloor_Managers[i].GetStaffID() == staff_id))
                    {
                        UI.DisplayErrorAgain(UI.StaffID_Alr_Exist);
                        validStaffID = false; // The loop keeps prompting the user until a valid input
                    }
                    else
                    {
                        validStaffID = true; // Valid Input then breaks the loop and return the value staffID
                    }
                }
                for (int i = 0; i < registeredSurgeons.Count; i++)
                {
                    if ((registeredSurgeons[i].GetStaffID() == staff_id))
                    {
                        UI.DisplayErrorAgain(UI.StaffID_Alr_Exist);
                        validStaffID = false;
                    }
                    else
                    {
                        validStaffID = true;
                    }
                }
            } while (!validStaffID);
            return staff_id;
        }
        //Method to check whether the FloorNumber has been previously assigned to other staff when registering
        private int checkFloorNumber()
        {
            int floor_Number;
            bool isValidfloorNumber = true;
            do
            {
                floor_Number = UI.GetFloor_numb();
                for (int i = 0; i < registeredFloor_Managers.Count; i++)
                {
                    if ((registeredFloor_Managers[i].GetFloorNumber() == floor_Number))
                    {
                        UI.DisplayErrorAgain("Floor has been assigned to another floor manager");
                        isValidfloorNumber = false;
                    }
                    else
                    {
                        isValidfloorNumber = true;
                    }
                }
            } while (!isValidfloorNumber);
            return floor_Number;
        }

        //Initialzing Surgeon list
        private Surgeons surgeon;
        private List<Surgeons> registeredSurgeons = new List<Surgeons>();

        /// <summary>
        /// Method of adding instances of Surgeon to a list and with logic for handling duplicates instances
        /// </summary>
        public void CreateSurgeonList()
        {
            foreach (var user in registeredUsers)
            {
                // If the user is a Surgeon, add them to the registeredSurgeons list
                if (user is Surgeons Surgeon)
                {
                    registeredSurgeons.Add(Surgeon);
                }
            }
        }
        /// <summary>
        /// Displays the menu for registering a new surgeon such as name, age, etc...
        /// then creating a new instance of a Surgeon and adding them to the unified list RegisteredUsers
        /// then using that list to add to the 1 other list mentioned above
        /// </summary>
        private void CreateSurgeonMenu()
        {
            Console.WriteLine("Registering as a surgeon.");

            // Get the information from the user

            string name = UI.GetName();

            int age = UI.GetAge("Surgeon");

            string mobile_number = UI.GetMobileNumber();

            string email = UI.GetEmail();

            string password = UI.GetPassword();

            int staff_id = checkValidStaffID();

            // Ask the user for their specialty
            bool isvalidSpecialty = false;
            string speciality = "";
            do {
                UI.DisplayMessage("Please choose your speciality:");
                UI.DisplayMessage("1. General Surgeon");
                UI.DisplayMessage("2. Orthopaedic Surgeon");
                UI.DisplayMessage("3. Cardiothoracic Surgeon");
                UI.DisplayMessage("4. Neurosurgeon");
                UI.DisplayMessage("Please enter a choice between 1 and 4.");
                int specialty_choice = UI.GetInt();
                //Switch statement for selecting the speciality

                switch (specialty_choice)
                {
                    case 1:
                        speciality = "General Surgeon";
                        isvalidSpecialty = true;
                        break;
                    case 2:
                        speciality = "Orthopaedic Surgeon";
                        isvalidSpecialty = true;
                        break;
                    case 3:
                        speciality = "Cardiothoracic Surgeon";
                        isvalidSpecialty = true;
                        break;
                    case 4:
                        speciality = "Neurosurgeon";
                        isvalidSpecialty = true; // Breaks the loop and  return the Specialty
                        break;
                    default:
                        UI.DisplayErrorAgain("Non-valid speciality type");
                        isvalidSpecialty = false; // If the user didnt select a specialty or choose an out of range choice => Keep Looping
                        break;
                }
            } while (!isvalidSpecialty);
            // Create the new surgeon and report success
            surgeon = new Surgeons(name, age, mobile_number, email, password, staff_id, speciality);
            UI.DisplayMessage($"{name} is registered as a surgeon.");
            Surgeons newSurgeon = new Surgeons(name, age, mobile_number, email, password, staff_id, speciality);
            registeredUsers.Add(newSurgeon);
            CreateSurgeonList();
            assigned = true;
            Opening_Menu();  // Return to the main menu after registration
        }

        // Method to display the logged-in patient's menu
        public bool ShowPatientMenu(Patients patient)
        {
            //Check in and check out logic 
            string CheckSTR = patient.GetCheckStatusString();
            // Menu strings
            const string Patient_Menu_STR = "Patient Menu.";
            const string PleaseChoose_STR = "Please choose from the menu below:";  // Header
            const string DisplayDetails_STR = "Display my details";  // Option 1
            const string ChangePassword_STR = "Change password";     // Option 2
            const string SeeRoom_STR = "See room";                   // Option 4
            const string SeeSurgeon_STR = "See surgeon";             // Option 5
            const string SeeSurgery_STR = "See surgery date and time";  // Option 6
            const string Logout_STR = "Log out";                     // Option 7
            const string PleaseEnter_STR = "Please enter a choice between 1 and 7.";  // Prompt
            // Array of options
            string[] options = { DisplayDetails_STR, ChangePassword_STR, CheckSTR, SeeRoom_STR, SeeSurgeon_STR, SeeSurgery_STR, Logout_STR };

            // Display the menu and get the user's choice
            UI.DisplayMessage(Patient_Menu_STR);
            int choice = UI.GetOption(PleaseChoose_STR, PleaseEnter_STR, options);

            // Handle the user's choice
            switch (choice + 1)  // Adding 1 to match the defined constants
            {
                case 1:
                    // Code to display patient details
                    DisplayPatientDetails(patient);
                    break;

                case 2:
                    // Code to change password
                    ChangePassword(patient);
                    ShowPatientMenu(patient);
                    break;

                case 3:
                    patient.DisplayCheckIn();
                    ShowPatientMenu(patient);
                    break;

                case 4:
                    // Code to see room
                    patient.SeeRoom(patient, Floor_Manager);
                    ShowPatientMenu(patient);
                    break;

                case 5:
                    // Code to see surgeon
                    patient.SeeSurgeon(patient);
                    ShowPatientMenu(patient);
                    break;

                case 6:
                    patient.Surgerytime(patient);
                    ShowPatientMenu(patient);
                    break;

                case 7:
                    // Code to log out
                    UI.DisplayMessage($"Patient {patient.GetName()} has logged out.");
                    Opening_Menu();
                    break;  // Return false to signal logging out

                default:
                    break;
            }

            // Returning true to keep the patient menu running
            return false;
        }
        /// <summary>
        /// Displays the floor manager's menu and processes the user's choice.
        /// Based on the selection, it triggers the appropriate action (e.g., display details, change password, assign room).
        /// </summary>
        public bool ShowFloorManagersMenu(Floor_Managers Floor_Manager)
        {
            // Menu strings
            const string PleaseChoose_STR = "Please choose from the menu below:";  // Header
            const string DisplayDetails_STR = "Display my details";                // Option 1
            const string ChangePassword_STR = "Change password";                   // Option 2
            const string AssignRoom_STR = "Assign room to patient";                // Option 3
            const string AssignSurgery_STR = "Assign surgery";                     // Option 4
            const string UnassignRoom_STR = "Unassign room";                       // Option 5
            const string Logout_STR = "Log out";                                   // Option 6
            const string PleaseEnter_STR = "Please enter a choice between 1 and 6.";  // Prompt

            // Array of options
            string[] options = { DisplayDetails_STR, ChangePassword_STR, AssignRoom_STR, AssignSurgery_STR, UnassignRoom_STR, Logout_STR };

            // Display the menu and get the user's choice
            UI.DisplayMessage("Floor Manager Menu.");
            int choice = UI.GetOption(PleaseChoose_STR, PleaseEnter_STR, options);

            // Handle the user's choice
            switch (choice + 1)  // Adding 1 to match the defined constants
            {
                case 1:
                    // Code to display floor manager details
                    DisplayFloor_ManagerDetails(Floor_Manager);
                    break;

                case 2:
                    // Code to change password
                    ChangePassword(Floor_Manager);
                    ShowFloorManagersMenu(Floor_Manager); // return to the FloorManagersMenu after done
                    break;

                case 3:
                    // Code to assign room to patient
                    Floor_Manager.AssignRoom(unassignedPatients, assignedPatientsRoom, patientRoomDirectory);
                    ShowFloorManagersMenu(Floor_Manager);
                    break;

                case 4:
                    // Code to assign surgery
                    Floor_Manager.Assign_Surgery( Floor_Manager,registeredPatients,assignedPatientsRoom,patientRoomDirectory,registeredSurgeons,assignedPatientsSurgery);
                    ShowFloorManagersMenu(Floor_Manager);
                    break;

                case 5:
                    // Code to unassign room
                    Floor_Manager.UnAssignRoom(Floor_Manager,assignedPatientsRoom,registeredSurgeons,assignedPatientsSurgery,patientRoomDirectory);
                    ShowFloorManagersMenu(Floor_Manager);
                    break;

                case 6:
                    // Code to log out
                    UI.DisplayMessage($"Floor manager {Floor_Manager.GetName()} has logged out.");
                    Opening_Menu();
                    break;

                default:
                    break;
            }
            return false;
        }
        public bool ShowSurgeonMenu(Surgeons Surgeon)
        {
            // Menu strings
            const string Surgeon_Menu_STR = "Surgeon Menu.";
            const string PleaseChoose_STR = "Please choose from the menu below:";  // Header
            const string DisplayDetails_STR = "Display my details";                // Option 1
            const string ChangePassword_STR = "Change password";                   // Option 2
            const string SeePatients_STR = "See your list of patients";            // Option 3
            const string SeeSchedule_STR = "See your schedule";                    // Option 4
            const string PerformSurgery_STR = "Perform surgery";                   // Option 5
            const string Logout_STR = "Log out";                                   // Option 6
            const string PleaseEnter_STR = "Please enter a choice between 1 and 6."; // Prompt

            // Array of options
            string[] options = { DisplayDetails_STR, ChangePassword_STR, SeePatients_STR, SeeSchedule_STR, PerformSurgery_STR, Logout_STR };

            // Display the menu and get the user's choice
            UI.DisplayMessage(Surgeon_Menu_STR);
            int choice = UI.GetOption(PleaseChoose_STR, PleaseEnter_STR, options);

            // Handle the user's choice
            switch (choice + 1)  // Adding 1 to match the defined constants
            {
                case 1:
                    // Code to display surgeon details
                    DisplaySurgeonDetails(Surgeon);
                    break;

                case 2:
                    // Code to change password
                    ChangePassword(Surgeon);  
                    ShowSurgeonMenu(Surgeon); // Return to the surgeonMenu after done
                    break;

                case 3:
                    // Code to see list of patients
                    Surgeon.SeePatients(Surgeon);
                    ShowSurgeonMenu(Surgeon);
                    break;

                case 4:
                    // Code to see schedule
                    Surgeon.SeeSchedule(Surgeon);
                    ShowSurgeonMenu(Surgeon);
                    break;

                case 5:
                    // Code to perform surgery
                    Surgeon.PerformSurgery(Surgeon,assignedPatientsSurgery, assignedPatientsRoom);
                    ShowSurgeonMenu(Surgeon);
                    break;

                case 6:
                    // Code to log out
                    UI.DisplayMessage($"Surgeon {Surgeon.GetName()} has logged out.");
                    Opening_Menu();
                    break;

                default:
                    break;
            }
            return false;
        }

        //Displays the Surgeon Details.
        private void DisplaySurgeonDetails(Surgeons Surgeon)
        {
            UI.DisplayMessage("Your details.");
            UI.DisplayMessage($"Name: {Surgeon.GetName()}");
            UI.DisplayMessage($"Age: {Surgeon.GetAge()}");
            UI.DisplayMessage($"Mobile phone: {Surgeon.GetMobileNumber()}");
            UI.DisplayMessage($"Email: {Surgeon.GetEmail()}");
            UI.DisplayMessage($"Staff ID:{Surgeon.GetStaffID()}");
            UI.DisplayMessage($"Speciality:{Surgeon.GetSpecialty()}");
            ShowSurgeonMenu(Surgeon);
        }



        /// <summary>
        /// Display Floor Manager's Details
        /// </summary>
        /// <param name="Floor_Manager"></param>
        private void DisplayFloor_ManagerDetails(Floor_Managers Floor_Manager)
        {
            UI.DisplayMessage("Your details.");
            UI.DisplayMessage($"Name: {Floor_Manager.GetName()}");
            UI.DisplayMessage($"Age: {Floor_Manager.GetAge()}");
            UI.DisplayMessage($"Mobile phone: {Floor_Manager.GetMobileNumber()}");
            UI.DisplayMessage($"Email: {Floor_Manager.GetEmail()}");
            UI.DisplayMessage($"Staff ID:{Floor_Manager.GetStaffID()}");
            UI.DisplayMessage($"Floor:{Floor_Manager.GetFloorNumber()}.");
            ShowFloorManagersMenu(Floor_Manager);
        }


        // Method to display patient details
        private void DisplayPatientDetails(Patients patient)
        {
            UI.DisplayMessage("Your details.");
            UI.DisplayMessage($"Name: {patient.GetName()}");
            UI.DisplayMessage($"Age: {patient.GetAge()}");
            UI.DisplayMessage($"Mobile phone: {patient.GetMobileNumber()}");
            UI.DisplayMessage($"Email: {patient.GetEmail()}");
            ShowPatientMenu(patient);
        }

        // Method to change the password
        private void ChangePassword(PasswordChange user)
        {
            UI.DisplayMessage("Enter new password:");
            string newPassword = UI.GetString();
            user.SetPassword(newPassword);
            UI.DisplayMessage("Password has been changed.");
        }
        /// <summary>
        /// Logic for the login menu
        /// Crosscheck the database of users with the entered email and password to 
        /// know whether the user is a patient, surgeon, FloorManager or if the user entered
        /// the wrong password or email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns> returns null if no users found 
        private User FindUser(string email, string password)
        {
            foreach (var user in registeredUsers)
            {
                if (user.GetEmail().Trim() == email.Trim() && user.GetPassword().Trim() == password.Trim())
                {
                    return user; // Return the user if found
                }
            }
            foreach (var user in registeredUsers)
            {
                if (user.GetEmail().Trim() != email.Trim() && user.GetPassword().Trim() == password.Trim())
                {
                    UI.DisplayError("Email is not registered");
                    return null;// Return Null if no users Found
                }
            }
            foreach (var user in registeredUsers)
            {
                if (user.GetPassword().Trim() != password.Trim() && user.GetEmail().Trim() == email.Trim())
                {
                    UI.DisplayError("Wrong Password");
                    return null;
                }
            } return null;
        }
        /// <summary>
        /// Checking if the email either matches with the database or it simply doesnt exist.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private User FindEmail(string email)
        {
            foreach (var user in registeredPatients)
            {
                if (user.GetEmail().Trim() == email.Trim())
                {
                    return user; // return if the email belongs to a Patient
                }  
            }
            foreach (var user in registeredFloor_Managers)
            {
                if (user.GetEmail().Trim() == email.Trim())
                {
                    return user;// return if the email belongs to a Floor Manager
                }
            }
            foreach (var user in registeredSurgeons)
            {
                if (user.GetEmail().Trim() == email.Trim())
                {
                    return user;// return if the email belongs to a Surgeon
                }
            }
            UI.DisplayError("Email is not registered");
            return null;// return Null if no emails Found
        }


        /// <summary>
        /// Displays the menu for logging in and and reporting success to the user
        /// first Check to see if there are any user's registered if not then 
        /// return to the opening menu and terminate the login logic
        /// then check if the user entered the email wrong or it doesnt exist if not then 
        /// return to the opening menu and terminate the login logic
        /// </summary>
        /// <param name="email1"></param>
        /// <param name="email2"></param>
        /// <returns></returns>
        //Log in Menu
        public void LoginMenu()
        {
            bool loginSuccessful = false;
            UI.DisplayMessage("Login Menu.");
            if (registeredUsers == null || registeredUsers.Count == 0)
            {
                UI.DisplayError("There are no people registered");
                Opening_Menu();
                return;
            }
            while (!loginSuccessful)
            {
                // Prompt for email and password
                UI.DisplayMessage("Please enter in your email:");
                string enteredEmail = UI.GetString().Trim();
                if (FindEmail(enteredEmail) == null)
                {
                    Opening_Menu();
                    return ;
                }
                UI.DisplayMessage("Please enter in your password:");
                string enteredPassword = UI.GetString().Trim();

                // Try to find the user in the registered users list
                User user = FindUser(enteredEmail, enteredPassword);

                if (user != null)
                {
                    // Check if the user is a Patient
                    if (user is Patients patient)
                    {
                        UI.DisplayMessage($"Hello {patient.GetName()} welcome back.");
                        ShowPatientMenu(patient);
                        loginSuccessful = true;
                    }
                    // Check if the user is a Floor Manager
                    else if (user is Floor_Managers Floor_Manager)
                    {
                        UI.DisplayMessage($"Hello {Floor_Manager.GetName()} welcome back.");
                        ShowFloorManagersMenu(Floor_Manager);
                        loginSuccessful = true;
                    }
                    // Check if the user is a Surgeon
                    else if (user is Surgeons Surgeon)
                    {
                        UI.DisplayMessage($"Hello {Surgeon.GetName()} welcome back.");
                        ShowSurgeonMenu(Surgeon);
                        loginSuccessful = true;
                    }
                }
                else
                {
                    Opening_Menu();
                    return;
                }
            }
        }
      // Initialzing the dictionary for use with the Name as the KeyValue and the roomnumber as Value
        Dictionary<string, int> patientRoomDirectory = new Dictionary<string, int>();
        //Creating an interface for universal password changing.
        public interface PasswordChange
        {
            void SetPassword(string newPassword);
        }
    }
}
