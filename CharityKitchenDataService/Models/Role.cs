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
    public class Role
    {
        /// <summary>
        /// The ID of the Role
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The Description of the Role
        /// </summary>
        public string Description { get; set; }

        // Some string constants defined for use by the Constructor
        private const string ROLE_ID = "RoleID";
        private const string ROLE_DESCRIPTION = "RoleDescription";

        /// <summary>
        /// Default Constructor for User object, required for Web Service to function.
        /// </summary>
        public Role() { }

        /// <summary>
        /// Constructor for the Role object, populating it wiith data from a specified OleDbDataReader
        /// </summary>
        /// <param name="reader">The OleDbDataReader containing Role data</param>
        public Role(OleDbDataReader reader)
        {
            ID = (int)reader[ROLE_ID];
            Description = (string)reader[ROLE_DESCRIPTION];
        }
    }
}