using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    public class Ingredient
    {
        /// <summary>
        /// The ID of the Ingredient.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The Name of the Ingredient.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Available Quantity for the Ingredient.
        /// </summary>
        public int AvailableQty { get; set; }

        /// <summary>
        /// The cost per unit for the Ingredient.
        /// </summary>
        public decimal CostPerUnit { get; set; }

        // Some string constants defined for use by the Constructor.
        private const string INGREDIENT_ID = "IngredientID";
        private const string INGREDIENT_NAME = "IngredientName";
        private const string INGREDIENT_QTY = "AvailableQty";
        private const string INGREDIENT_COST = "CostPerUnit";

        /// <summary>
        /// Default Constructor for Ingredient object, required for Web Service to function.
        /// </summary>
        public Ingredient() { }

        /// <summary>
        /// Constructor for the Ingredient object, populating it wiith data from a specified OleDbDataReader.
        /// </summary>
        /// <param name="reader">The OleDbDataReader containing Ingredient data</param>
        public Ingredient(OleDbDataReader reader)
        {
            ID = (int)reader[INGREDIENT_ID];
            Name = (string)reader[INGREDIENT_NAME];
            AvailableQty = (int)reader[INGREDIENT_QTY];
            CostPerUnit = (decimal)reader[INGREDIENT_COST];
        }
    }
}