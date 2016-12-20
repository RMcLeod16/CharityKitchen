using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    /// <summary>
    /// Class for object to store which User has what Access Level with which Role
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// The ID of the UserRole
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The ID of the User for this UserRole
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// The IF of the Role for this UserRole
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// The Access Level granted ffor this UserRole combo
        /// </summary>
        public int AccessLevel { get; set; }

        // Some string constants defined for use by the Constructor
        private const string USERROLE_ID = "UserRoleID";
        private const string USER_ID = "UserID";
        private const string ROLE_ID = "RoleID";
        private const string ACCESS_LEVEL = "AccessLevel";

        /// <summary>
        /// Default Constructor for UserRole object, required for Web Service to function.
        /// </summary>
        public UserRole() { }

        /// <summary>
        /// Constructor for the UserRole object, populating it wiith data from a specified OleDbDataReader
        /// </summary>
        /// <param name="reader">The OleDbDataReader containing UserRole data</param>
        public UserRole(OleDbDataReader reader)
        {
            ID = (int)reader[USERROLE_ID];
            UserID = (int)reader[USER_ID];
            RoleID = (int)reader[ROLE_ID];
            AccessLevel = (int)reader[ACCESS_LEVEL];
        }
    }
}