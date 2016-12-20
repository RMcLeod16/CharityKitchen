using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    /// <summary>
    /// Class for object to store Order data
    /// </summary>
    public class Order
    {
        /// <summary>
        /// The ID for the Order
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The Name for the Order
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Email for the Order
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The Address for the Order
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The Suburb for the ORder
        /// </summary>
        public string Suburb { get; set; }

        /// <summary>
        /// The Postcode for the Order
        /// </summary>
        public int Postcode { get; set; }

        // Some string constants defined for use by the Constructor
        private const string ORDER_ID = "OrderID";
        private const string ORDER_NAME = "OrderName";
        private const string ORDER_EMAIL = "Email";
        private const string ORDER_ADDRESS = "Address";
        private const string ORDER_SUBURB = "Suburb";
        private const string ORDER_POSTCODE = "Postcode";

        /// <summary>
        /// Default Constructor for Order object, required for Web Service to function.
        /// </summary>
        public Order() { }

        /// <summary>
        /// Constructor for the Order object, populating it wiith data from a specified OleDbDataReader
        /// </summary>
        /// <param name="reader">The OleDbDataReader containing Order data</param>
        public Order(OleDbDataReader reader)
        {
            ID = (int)reader[ORDER_ID];
            Name = (string)reader[ORDER_NAME];
            Email = (string)reader[ORDER_EMAIL];
            Address = (string)reader[ORDER_ADDRESS];
            Suburb = (string)reader[ORDER_SUBURB];
            Postcode = (int)reader[ORDER_POSTCODE];
        }
    }
}