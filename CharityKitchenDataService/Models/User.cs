using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    /// <summary>
    /// Class for object to store User data
    /// </summary>
    public class User
    {
        /// <summary>
        /// The ID of the User
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The Username of the User
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The Password of the User
        /// </summary>
        public string Password { get; set; }

        // Some string constants defined for use by the Constructor
        private const string USER_ID = "UserID";
        private const string USERNAME = "Username";
        private const string PASSWORD = "UserPass";

        /// <summary>
        /// Default Constructor for User object, required for Web Service to function.
        /// </summary>
        public User() { }

        /// <summary>
        /// Constructor for the User object, populating it wiith data from a specified OleDbDataReader
        /// </summary>
        /// <param name="reader">The OleDbDataReader containing User data</param>
        public User(OleDbDataReader reader)
        {
            ID = (int)reader[USER_ID];
            Username = (string)reader[USERNAME];
            Password = (string)reader[PASSWORD];
        }
    }
}