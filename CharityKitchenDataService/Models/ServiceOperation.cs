using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CharityKitchenDataService.Models
{
    // Use XmlInclude to recognise the object types that it is used for.
    /// <summary>
    /// Class for a generic object to handle and return Data and Errors from the DB to the Application
    /// </summary>
    [XmlInclude(typeof(Order))]
    [XmlInclude(typeof(OrderMeal))]
    [XmlInclude(typeof(Meal))]
    [XmlInclude(typeof(MealIngredient))]
    [XmlInclude(typeof(Ingredient))]
    [XmlInclude(typeof(User))]
    [XmlInclude(typeof(UserRole))]
    [XmlInclude(typeof(UserIDNameCombo))]
    [XmlInclude(typeof(Role))]
    public class ServiceOperation
    {
        /// <summary>
        /// A boolean to represent whther or not the DB query was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// A simple user-friendly message for the result of executing the DB query.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The text message from the Exception, if an exception is thrown when Executing the DB query.
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// The List of objects to store data retrieved datd from DB.
        /// </summary>
        public List<object> Data { get; set; }

        /// <summary>
        /// Default Constructor for the ServiceObject Object.
        /// </summary>
        public ServiceOperation()
        {
            Success = true;
            Message = "Operation successful";
            Exception = "N/A";
            Data = new List<object>();
        }
    }
}