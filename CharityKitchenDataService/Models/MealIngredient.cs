using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    /// <summary>
    /// Class for object to store data for which Meal has which Ingredients and how many are required.
    /// </summary>
    public class MealIngredient
    {
        /// <summary>
        /// The ID of the MealIngredient.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The ID of the Meal for this MealIngredient.
        /// </summary>
        public int MealID { get; set; }

        /// <summary>
        /// The ID of the Ingredient for this MealIngredient.
        /// </summary>
        public int IngredientID { get; set; }

        /// <summary>
        /// The Quantity of the Ingredient required for the Meal.
        /// </summary>
        public int RequiredQty { get; set; }

        // Some string constants defined for use by the Constructor
        private const string MEAL_INGREDIENT_ID = "MealIngredientID";
        private const string MEAL_ID = "MealID";
        private const string INGREDIENT_ID = "IngredientID";
        private const string REQUIRED_QTY = "RequiredQty";

        /// <summary>
        /// Default Constructor for MealIngredient object, required for Web Service to function.
        /// </summary>
        public MealIngredient() { }

        /// <summary>
        /// Constructor for the MealIngredient object, populating it wiith data from a specified OleDbDataReader
        /// </summary>
        /// <param name="reader">The OleDbDataReader containing MealIngredient data</param>
        public MealIngredient(OleDbDataReader reader)
        {
            ID = (int)reader[MEAL_INGREDIENT_ID];
            MealID = (int)reader[MEAL_ID];
            IngredientID = (int)reader[INGREDIENT_ID];
            RequiredQty = (int)reader[REQUIRED_QTY];
        }
    }
}