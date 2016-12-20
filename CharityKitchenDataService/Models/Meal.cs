using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    /// <summary>
    /// Class for object to store Meal data
    /// </summary>
    public class Meal
    {
        /// <summary>
        /// The ID of the Meal.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The Name of the Meal.
        /// </summary>
        public string Name { get; set; }

        // Some string constants defined for use by the Constructor
        private const string MEAL_ID = "MealID";
        private const string MEAL_NAME = "MealName";

        /// <summary>
        /// Default Constructor for Meal object, required for Web Service to function.
        /// </summary>
        public Meal() { }

        /// <summary>
        /// Constructor for the Meal object, populating it wiith data from a specified OleDbDataReader
        /// </summary>
        /// <param name="reader">The OleDbDataReader containing Meal data</param>
        public Meal(OleDbDataReader reader)
        {
            ID = (int)reader[MEAL_ID];
            Name = (string)reader[MEAL_NAME];
        }
    }
}