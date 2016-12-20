using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    /// <summary>
    /// Class to get a UserID and Username for a User, but without their password.
    /// </summary>
    public class UserIDNameCombo
    {
        /// <summary>
        /// The ID of the User
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The Username of the User
        /// </summary>
        public string Username { get; set; }

        // Some string constants defined for use by the Constructor
        private const string USER_ID = "UserID";
        private const string USERNAME = "Username";

        /// <summary>
        /// Default Constructor for UserIDNameCombo object, required for Web Service to function.
        /// </summary>
        public UserIDNameCombo() { }

        /// <summary>
        /// Constructor for the UserIDNameCombo object, populating it wiith data from a specified OleDbDataReader
        /// </summary>
        /// <param name="reader">The OleDbDataReader containing UserIDNameCombo data</param>
        public UserIDNameCombo(OleDbDataReader reader)
        {
            ID = (int)reader[USER_ID];
            Username = (string)reader[USERNAME];
        }
    }
}