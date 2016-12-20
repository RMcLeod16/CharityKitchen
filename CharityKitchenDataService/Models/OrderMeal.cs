using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CharityKitchenDataService.Models
{
    /// <summary>
    /// Class for object to store data for which Order has which Meals and how many.
    /// </summary>
    public class OrderMeal
    {
        /// <summary>
        /// The ID of the OrderMeal.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The ID of the Order for this OrderMeal.
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// The ID of the Meal for this OrderMeal.
        /// </summary>
        public int MealID { get; set; }

        /// <summary>
        /// The Quantity of the Meal in that Order.
        /// </summary>
        public int OrderedQty { get; set; }

        // Some string constants defined for use by the Constructor
        private const string ORDERMEAL_ID = "OrderMealID";
        private const string ORDER_ID = "OrderID";
        private const string MEAL_ID = "MealID";
        private const string ORDERED_QTY = "OrderedQty";

        /// <summary>
        /// Default Constructor for OrderMeal object, required for Web Service to function.
        /// </summary>
        public OrderMeal() { }

        /// <summary>
        /// Constructor for the OrderMeal object, populating it wiith data from a specified OleDbDataReader
        /// </summary>
        /// <param name="reader">The OleDbDataReader containing OrderMeal data</param>
        public OrderMeal(OleDbDataReader reader)
        {
            ID = (int)reader[ORDERMEAL_ID];
            OrderID = (int)reader[ORDER_ID];
            MealID = (int)reader[MEAL_ID];
            OrderedQty = (int)reader[ORDERED_QTY];
        }
    }
}