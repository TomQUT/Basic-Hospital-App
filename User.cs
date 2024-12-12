using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignement_2v2
{
    public abstract class User
    {
        private string Name;
        private int Age;
        private string Email;
        private string Mobile_Number;
        private string Password;

        /// <summary>
        /// The Constructor for User class
        /// </summary>
        /// <param name="name"></param>
        /// <param name="age"></param>
        /// <param name="mobile_number"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public User(string name, int age, string mobile_number, string email,string password)
        {
            this.Name = name;
            this.Age = age;
            this.Email = email;
            this.Mobile_Number = mobile_number;
            this.Password = password;
        }

        // Encapsulation via getters and setters
        public string GetName()
        {
            return Name;
        }

        public int GetAge()
        {
            return Age;
        }

        public string GetEmail()
        {
            return Email;
        }

        public string GetMobileNumber()
        {
            return Mobile_Number;
        }
        public string GetPassword()
        {
            return Password;
        }

        public void SetPassword(string newPassword)
        {
            Password = newPassword;
        }
    }

    public abstract class Staff : User
    {
        private int Staff_Id;

        /// <summary>
        /// Constructor For the Staff Class which inherits from the user class but with staff related Data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="age"></param>
        /// <param name="mobile_number"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="staff_id"></param>
        public Staff(string name, int age, string mobile_number, string email, string password, int staff_id)
            : base(name, age, mobile_number, email, password)
        {
            this.Staff_Id = staff_id;
        }

        
        public int GetStaffID()
        {
            return Staff_Id;
        }

        public void SetStaffId(int staff_id)
        {
            this.Staff_Id = staff_id;
        }
    }




}
