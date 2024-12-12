using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assignement_2v2
{
    internal class UI
    {
        /// <summary>
        /// Reads in a string from console and returns it
        /// </summary>
        /// <returns>A string representation of the users input</returns>
        public static string GetString()
        {
            string input = Console.ReadLine();
            return input;
        }
        /// <summary>
        /// Reads in a string from console, converts it to a Int32
        /// and returns the converted value
        /// </summary>
        /// <returns>A Int32 representation of the users input</returns>
        public static int GetInt()
        {
            string input = Console.ReadLine();
            int i = int.Parse(input);
            return i;
        }

        /// <summary>
        /// Displays a blank line to the screen.
        /// </summary>
        public static void DisplayMessage()
        {
            Console.WriteLine();
        }
        /// <summary>
        /// Displays a message to the screen.
        /// </summary>
        /// <param name="message">The message to display</param>
        public static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Reads in a string from console and returns it
        /// </summary>
        /// <returns>A string representation of the users input</returns>
        public static string Getstring()
        {
            string input = Console.ReadLine();
            return input;
        }

        /// <summary>
        /// Reads in a string from console, converts it to a Int32
        /// and returns the converted value
        /// </summary>
        /// <returns>A Int32 representation of the users input</returns>
        public static int Getint()
        {
            string input = Console.ReadLine();
            int i = int.Parse(input);
            return i;
        }

        /// <summary>
        /// Displays a menu, with the options numbered from 1 to options.Length,
        /// the gets a validated integer in the range 1..options.Length. 
        /// Subtracts 1, then returns the result. If the supplied list of options 
        /// is empty, returns an error value (-1).
        /// </summary>
        /// <param name="title">A heading to display before the menu is listed.</param>
        /// <param name="options">The list of objects to be displayed.</param>
        /// <returns>Return value is either -1 (if no options are provided) or a 
        /// value in 0 .. (options.Length-1).</returns>
        public static int GetOption(string title, string titles, params string[] options)
        {
            if (options.Length == 0) return -1;

            Console.WriteLine(title);
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
            Console.WriteLine(titles);
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > options.Length)
            {
             //If its the original code my default for the switch statements wont work so if the input is invalid return -1 so it triggers the default case
                return -1;

            }
            return choice - 1;

        }
        /// <summary>
        /// Displays an error message to the screen.
        /// </summary>
        /// <param name="msg">The message to display</param
        public static void DisplayError(string msg)
        {
            UI.DisplayMessage($"#####");
            UI.DisplayMessage($"#Error - {msg}.");
            UI.DisplayMessage($"#####");
        }
        /// <summary>
        /// Displays an error message and asks the user to try again.
        /// </summary>
        /// <param name="msg">The message to display</param>
        public static void DisplayErrorAgain(string msg)
        {
            UI.DisplayMessage($"#####");
            UI.DisplayMessage($"#Error - {msg}, please try again.");
            UI.DisplayMessage($"#####");
        }

        /// <summary>
        /// Get Name method
        /// reads the string input as name and validate if the string is either empty or have special characters
        /// </summary>
        /// <returns></returns> return name
        const string InValidName = "Supplied name is invalid";
        public static string GetName()
        {
            string name;

            do
            {
                UI.DisplayMessage("Please enter in your name:");
                name = UI.GetString();

                // Check if the input is invalid (blank or contains non-letter characters)
                if (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, @"^[A-Za-z ]+$"))
                {
                    DisplayErrorAgain(InValidName);
                }

            } while (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, @"^[A-Za-z ]+$"));

            return name;
        }


        /// <summary>
        /// Get Age method
        /// </summary>
        /// First get the age and return it as a string to validate if its blank or the user entered nothing
        /// Then Use tryparse to validate if this is an interger
        /// Then crosscheck the age using the parameter set as a string
        /// <returns></returns> return age value if passed all validation cases
        const string InvalidAge = "Supplied age is invalid";
        const string InvalidInput = "Supplied value is not an integer";
        public static int GetAge(string role)
        {
            int age = 0;  // Initialize age
            bool isValidNumber = false;
            string input;

            do
            {
                UI.DisplayMessage("Please enter in your age:");
                input = UI.GetString();  // Get user input as a string

                // Check if the input is blank or contains only whitespace
                if (string.IsNullOrWhiteSpace(input))
                {
                    DisplayErrorAgain(InvalidInput);
                }
                else if (!int.TryParse(input, out age))  // Check if the input is not a valid number
                {
                    DisplayErrorAgain(InvalidInput);
                }
                else // Check if the age is outside the allowed range based on the role
                {
                    if (role == "Patient")
                    {
                        if (age >= 0 && age <= 100)
                        {
                            isValidNumber = true;
                        }
                        else
                        {
                            DisplayErrorAgain(InvalidAge);
                        }
                    }
                    else if (role == "Floor_Manager")
                    {
                        if (age >= 21 && age <= 70)
                        {
                            isValidNumber = true;
                        }
                        else
                        {
                            DisplayErrorAgain(InvalidAge);
                        }
                    }
                    else if (role == "Surgeon")
                    {
                        if (age >= 30 && age <= 75)
                        {
                            isValidNumber = true;
                        }
                        else
                        {
                            DisplayErrorAgain(InvalidAge);
                        }
                    }
                }
            } while (!isValidNumber);  // Keep looping until a valid age is entered
            return age;
        }

        /// <summary>
        /// GetMobileNumber Method 
        /// Prompts the user to enter in their phone number and check whether
        /// The phone Number is either blank or has characters instead of digits
        /// Then its checked for if the length exceeds 10 or lower than  10
        /// Finally checks if the phonenumber starts with a 0
        /// </summary>
        /// <returns></returns> returns the phone number if the input value passed all Validation test cases
        const string InvalidPhoneNumber = "Supplied mobile number is invalid";
        public static string GetMobileNumber()
        {
            string mobile_number;
            do
            {
                // Prompt the user to enter their mobile number
                UI.DisplayMessage("Please enter in your mobile number:");
                mobile_number = UI.GetString();

                // Test Case 1: Check if the input is empty or just spaces
                if (string.IsNullOrWhiteSpace(mobile_number))
                {
                    DisplayErrorAgain(InvalidPhoneNumber);
                }
                // Test Case 2: Check if the mobile number is too short or too long (less or more than 10 digits)
                else if (mobile_number.Length < 10 || mobile_number.Length > 10)
                {
                    DisplayErrorAgain(InvalidPhoneNumber);
                }
                // Test Case 3: Check if the mobile number starts with a leading zero
                else if (!mobile_number.StartsWith("0"))
                {
                    DisplayErrorAgain(InvalidPhoneNumber);
                }
                // Test Case 4: Check if the mobile number contains only digits
                else if (!Regex.IsMatch(mobile_number, @"^\d+$"))
                {
                    DisplayErrorAgain(InvalidPhoneNumber);
                }

            } while (string.IsNullOrWhiteSpace(mobile_number) || mobile_number.Length != 10 || !mobile_number.StartsWith("0") || !Regex.IsMatch(mobile_number, @"^\d+$"));

            // Return the valid mobile number
            return mobile_number;
        }

        /// <summary>
        /// GetEmail Method
        /// Prompts the user to enter their email and checks whether the email is valid.
        /// The email is considered invalid if it is blank, missing the "@" symbol, missing the "." symbol, 
        /// or if either of these symbols is at the start or end of the email.
        /// Finally, if the email is invalid, the user will be prompted to enter the email again until it passes all checks.
        /// </summary>
        /// <returns>Returns the email once it passes all validation test cases.</returns>
        const string InvalidEmail = "Supplied email is invalid";
        public static string GetEmail()
        {
            string email;

            do
            {
                UI.DisplayMessage("Please enter in your email:");
                email = UI.GetString();

                // Check if the input is invalid (blank or contains non-letter characters)
                if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || email.IndexOf("@") == 0 || email.IndexOf("@") == email.Length - 1)
                {
                    DisplayErrorAgain(InvalidEmail);
                }

            } while (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || email.IndexOf("@") == 0 || email.IndexOf("@") == email.Length - 1);

            return email;
        }
        const string InvalidPassword = "Supplied password is invalid";
        /// <summary>
        /// GetPassword Method
        /// Prompts the user to enter their password and checks whether the password is valid.
        /// The password is considered valid if it is at least 8 characters long, contains at least one 
        /// uppercase letter, one lowercase letter, and one digit.
        /// If the password does not meet these requirements or is blank, the user will be prompted to enter 
        /// the password again until it passes all validation checks.
        /// </summary>
        /// <returns>Returns the password once it passes all validation test cases.</returns>
        public static string GetPassword()
        {
            string password;
            bool isvalidPassword = false;
            do
            {
                UI.DisplayMessage("Please enter in your password:");
                password = UI.GetString();
                if (!string.IsNullOrWhiteSpace(password) && (password.Length >= 8) && 
                    password.Length >= 8 &&
                    password.Any(char.IsUpper) &&
                    password.Any(char.IsLower) &&
                    password.Any(char.IsDigit))
                {
                    isvalidPassword = true;
                }
                else
                {
                    DisplayErrorAgain(InvalidPassword);
                }
            } while (!isvalidPassword);
            return password;
        }

        const string InvalidStaffID = "Supplied staff identification number is invalid";
        /// <summary>
        /// GetStaffID Method
        /// Prompts the user to enter their staff ID and validates the input.
        /// The staff ID is considered valid if it is an integer between 100 and 999.
        /// If the input is blank, contains only whitespace, or the value is outside the allowed range, 
        /// the user will be prompted to enter the staff ID again until it meets the validation criteria.
        /// </summary>
        /// <returns>Returns the valid staff ID once all validation checks are passed.</returns>
        public static int GetStaffID()
        {
            int staff_id;
            string input;
            bool isValid_ID;
            do
            {
                // Prompt the user to enter their age
                UI.DisplayMessage("Please enter in your staff ID:");

                // Get the user input as a string
                input = UI.GetString();
                isValid_ID = int.TryParse(input, out staff_id);
                // Check if the input is blank or contains only whitespace
                if (string.IsNullOrWhiteSpace(input))
                {
                    DisplayErrorAgain(InvalidStaffID);
                }
                // Check if the age is outside the allowed range (0 to 100)
                else if (staff_id < 100 || staff_id > 999)
                {
                    DisplayErrorAgain(InvalidStaffID);
                }

            } while (string.IsNullOrWhiteSpace(input) ||!isValid_ID || staff_id <100 || staff_id > 999);

            // Return the valid age
            return staff_id;
        }
        const string InValidFloorNumber = "Supplied floor is invalid";
        /// <summary>
        /// GetFloor_numb Method
        /// Prompts the user to enter their floor number and validates the input.
        /// The floor number is considered valid if it is an integer between 1 and 6.
        /// If the input is blank, contains only whitespace, or if the value is outside the allowed range, 
        /// the user will be prompted to enter the floor number again until it meets the validation criteria.
        /// </summary>
        /// <returns>Returns the valid floor number once all validation checks are passed.</returns>
        public static int GetFloor_numb()
        {
            int floor_numb;
            string input;
            bool isvalid_floor;
            do
            {
                // Prompt the user to enter their age
                UI.DisplayMessage("Please enter in your floor number:");

                // Get the user input as a string
                input = UI.GetString();
                isvalid_floor = int.TryParse(input, out floor_numb);
                // Check if the input is blank or contains only whitespace
                if (string.IsNullOrWhiteSpace(input))
                {
                    DisplayErrorAgain(InValidFloorNumber);
                }
                
                else if (floor_numb < 1 || floor_numb > 6)
                {
                    DisplayErrorAgain(InValidFloorNumber);
                }

            } while (string.IsNullOrWhiteSpace(input) || !isvalid_floor || floor_numb < 1 || floor_numb > 6);

            // Return the valid age
            return floor_numb;
        }

        /// <summary>
        /// Reads in a string and converts it to a DateTime using the QUT consistent HH:mm dd/MM/yyyy format
        /// All dates must be in this format so 1/2/2000 should be 01/02/2000 or you will ge an error
        /// https://learn.microsoft.com/en-us/dotnet/api/system.datetime.tryparseexact?view=net-8.0
        /// </summary>
        /// <returns></returns>
        public static string GetDateTime() 
        {
            DateTime datetime;
            string input = Console.ReadLine();
            DateTime result;
            string format = Datetime.DATETIMEFORMAT; // Expected format is "HH:mm dd/MM/yyyy"
            bool dtWorked = DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            if (!dtWorked)
            {
                DisplayError("Supplied value is not a valid DateTime");
                Console.WriteLine("Please enter a date and time (e.g. 14:30 31/01/2024).");
                return GetDateTime();
            }
            string date = result.ToString("HH:mm dd/MM/yyyy");
            return date;
        }
        public const string ChoiceOutRange = "Supplied value is out of range";
        public const string StaffID_Alr_Exist = "Staff ID is already registered";
        public const string NoPatientsAssigned = "You do not have any patients assigned.";

        /// <summary>
        /// GetValidChoice Method
        /// Prompts the user to enter a choice and checks whether the choice is valid.
        /// The choice is invalid if it is less than 1 or greater than or equal to the provided count.
        /// If the choice is invalid, the user will be prompted to enter the choice again until a valid selection is made.
        /// </summary>
        /// <param name="count">The upper limit of the choices available to the user.</param>
        /// <returns>Returns the valid choice after it passes all validation test cases.</returns>
        public static int GetValidChoice(int count)
        {
            int choice;
            bool isValidChoice = false;

            do
            {
                UI.DisplayMessage($"Please enter a choice between 1 and {count}.");
                choice = UI.GetInt() - 1;
                if (choice < 0 || choice >= count)
                {
                    UI.DisplayErrorAgain(UI.ChoiceOutRange);
                }
                else
                {
                    isValidChoice = true;
                }
            } while (!isValidChoice);

            return choice;
        }
    }

}
    


