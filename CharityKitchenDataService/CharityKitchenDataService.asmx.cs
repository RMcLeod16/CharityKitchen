using CharityKitchenDataService.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CharityKitchenDataService
{
    /// <summary>
    /// Summary description for CharityKitchenDataService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CharityKitchenDataService : System.Web.Services.WebService
    {
        /// <summary>
        /// The Connection String used to connect to the DB.
        /// </summary>
        const string CONNECTION_STRING = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|CharityKitchen.accdb";

        #region Orders

        /// <summary>
        /// Method to get all Orders from the DB.
        /// </summary>
        /// <returns>A ServiceOperation Object that contains either the retrieved Order Data if successful, or the Exception messages if faulure.</returns>
        [WebMethod]
        public ServiceOperation GetOrders()
        {
            // Setup DB Connection and ServiceOperation object.
            ServiceOperation operation = new ServiceOperation();
            OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);

            try
            {
                // Setup Query and send it to DB.
                OleDbCommand dbCmd = new OleDbCommand("SELECT * FROM tblOrders", dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    // Add retrieved data to ServiceOperation object.
                    operation.Data.Add(new Order(reader));
                }
            }

            // If something fails, get Exception message and add it to ServiceOperation object.
            catch (FormatException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Invalid data supplied";
            }

            catch (OleDbException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Database error";
            }

            catch (Exception ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Unspecified error";
            }

            finally
            {
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    // Close any DB connections when done.
                    dbConn.Close();
                }
            }

            // Return the finished ServiceOperation object.
            return operation;
        }

        /// <summary>
        /// A wrapper method that sets up a query to insert new Orders and sends it to DB.
        /// </summary>
        /// <param name="order">The Order object to add to the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation AddOrder(Order order)
        {
            string query = "INSERT INTO tblOrders (OrderName, Email, Address, Suburb, Postcode) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4})";
            return SendNonQuery(String.Format(query, order.Name, order.Email, order.Address, order.Suburb, order.Postcode));
        }

        /// <summary>
        /// A wrapper method that sets up a query to update an existing Order and sends it to the DB.
        /// </summary>
        /// <param name="order">The Order object to update in the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation UpdateOrder(Order order)
        {
            string query = "UPDATE tblOrders SET OrderName=\"{0}\", Email=\"{1}\", Address=\"{2}\", Suburb=\"{3}\", Postcode={4} WHERE OrderID={5}";
            return SendNonQuery(String.Format(query, order.Name, order.Email, order.Address, order.Suburb, order.Postcode, order.ID));
        }

        #endregion Orders

        #region OrderMeals

        /// <summary>
        /// Method to get all OrderMeals for a given Order from the DB.
        /// </summary>
        /// <returns>A ServiceOperation Object that contains either the retrieved OrderMeal Data if successful, or the Exception messages if failure.</returns>
        [WebMethod]
        public ServiceOperation GetOrderMeals(int orderID)
        {
            // Setup DB Connection and ServiceOperation object.
            ServiceOperation operation = new ServiceOperation();
            OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);

            try
            {
                // Setup Query and send it to DB.
                OleDbCommand dbCmd = new OleDbCommand("SELECT * FROM tblOrderMeals WHERE OrderID=" + orderID, dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    // Add retrieved data to ServiceOperation object.
                    operation.Data.Add(new OrderMeal(reader));
                }
            }

            // If something fails, get Exception message and add it to ServiceOperation object.
            catch (FormatException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Invalid data supplied";
            }

            catch (OleDbException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Database error";
            }

            catch (Exception ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Unspecified error";
            }

            finally
            {
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    // Close any DB connections when done.
                    dbConn.Close();
                }
            }

            // Return the finished ServiceOperation object.
            return operation;
        }

        /// <summary>
        /// A wrapper method that sets up a query to insert new OrderMeals and sends it to DB.
        /// </summary>
        /// <param name="orderMeal">The OrderMeal object to add to the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation AddOrderMeal(OrderMeal orderMeal)
        {
            // If it doesn't exist, then build query and send to DB and return result.
            if (!AlreadyExists(orderMeal))
            {
                string query = "INSERT INTO tblOrderMeals (OrderID, MealID, OrderedQty) VALUES ({0}, {1}, {2})";
                return SendNonQuery(String.Format(query, orderMeal.OrderID, orderMeal.MealID, orderMeal.OrderedQty));
            }

            // If already exists, make new ServiceOperation reflecting that fact, and return it.
            ServiceOperation operation = new ServiceOperation();
            operation.Success = false;
            operation.Message = "Already Exists Error";
            operation.Exception = "Either that Meal already exists in this Order, or some other unspecified error.";

            return operation;
        }

        /// <summary>
        /// A wrapper method that sets up a query to update an existing OrderMeal and sends it to DB.
        /// </summary>
        /// <param name="orderMeal">The OrderMeal object to update in the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation UpdateOrderMeal(OrderMeal orderMeal)
        {
            string query = "UPDATE tblOrderMeals SET OrderID={0}, MealID={1}, OrderedQty={2} WHERE OrderMealID={3}";
            return SendNonQuery(String.Format(query, orderMeal.OrderID, orderMeal.MealID, orderMeal.OrderedQty, orderMeal.ID));
        }

        /// <summary>
        /// A wrapper method that sets up a query to delete an existing OrderMeal and sends it to DB.
        /// </summary>
        /// <param name="orderMealID">the ID of the OrderMeal to delete.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation DeleteOrderMeal(int orderMealID)
        {
            string query = "DELETE FROM tblOrderMeals WHERE OrderMealID={0}";
            return SendNonQuery(String.Format(query, orderMealID));
        }

        /// <summary>
        /// A method that checks the DB to see if the given OrderMeals already exists or not.
        /// </summary>
        /// <param name="orderMeal">The OrderMeal to check for.</param>
        /// <returns>True if it already exists, false if not.</returns>
        [WebMethod]
        private bool AlreadyExists(OrderMeal orderMeal)
        {
            ServiceOperation operation = CheckIfExists("tblOrderMeals", String.Format("OrderID = {0} AND MealID = {1}", orderMeal.OrderID, orderMeal.MealID));
            bool exists = true;
            if (operation.Success)
            {
                if (operation.Data.Count == 0)
                    exists = false;
            }

            return exists;
        }

        #endregion OrderMeals

        #region Meals

        /// <summary>
        /// Method to get all Meals from the DB.
        /// </summary>
        /// <returns>A ServiceOperation Object that contains either the retrieved Meal Data if successful, or the Exception messages if failure.</returns>
        [WebMethod]
        public ServiceOperation GetMeals()
        {
            // Setup DB Connection and ServiceOperation object.
            ServiceOperation operation = new ServiceOperation();
            OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);

            try
            {
                // Setup Query and send it to DB.
                OleDbCommand dbCmd = new OleDbCommand("SELECT * FROM tblMeals", dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    // Add retrieved data to ServiceOperation object.
                    operation.Data.Add(new Meal(reader));
                }
            }

            // If something fails, get Exception message and add it to ServiceOperation object.
            catch (FormatException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Invalid data supplied";
            }

            catch (OleDbException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Database error";
            }

            catch (Exception ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Unspecified error";
            }

            finally
            {
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    // Close any DB connections when done.
                    dbConn.Close();
                }
            }

            // Return the finished ServiceOperation object.
            return operation;
        }

        /// <summary>
        /// A wrapper method that sets up a query to insert new Meal and sends it to DB.
        /// </summary>
        /// <param name="meal">The Meal object to add to the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation AddMeal(Meal meal)
        {
            // Check if Meal already exists before trying to add new one.
            if (!AlreadyExists(meal))
            {
                // If it doesn't exist, then build query and send to DB and return result.
                string query = "INSERT INTO tblMeals (MealName) VALUES (\"{0}\")";
                return SendNonQuery(String.Format(query, meal.Name));
            }

            // If already exists, make new ServiceOperation reflecting that fact, and return it.
            ServiceOperation operation = new ServiceOperation();
            operation.Success = false;
            operation.Message = "Already Exists Error";
            operation.Exception = "Either that Meal already exists, or some other unspecified error.";

            return operation;
        }

        /// <summary>
        /// A wrapper method that sets up a query to update an existing Meal and sends it to DB.
        /// </summary>
        /// <param name="meal">The Meal object to update in the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation UpdateMeal(Meal meal)
        {
            string query = "UPDATE tblMeals SET MealName=\"{0}\" WHERE MealID={1}";
            return SendNonQuery(String.Format(query, meal.Name, meal.ID));
        }

        /// <summary>
        /// A method that checks the DB to see if the given Meal already exists or not.
        /// </summary>
        /// <param name="meal">The Meal to check for.</param>
        /// <returns>True if it already exists, false if not.</returns>
        [WebMethod]
        private bool AlreadyExists(Meal meal)
        {
            ServiceOperation operation = CheckIfExists("tblMeals", String.Format("MealName = \"{0}\"", meal.Name));
            bool exists = true;
            if (operation.Success)
            {
                if (operation.Data.Count == 0)
                    exists = false;
            }
            return exists;
        }

        #endregion Meals

        #region MealIngredients

        /// <summary>
        /// Method to get all MealIngredients for a given Meal from the DB.
        /// </summary>
        /// <returns>A ServiceOperation Object that contains either the retrieved OrderMeal Data if successful, or the Exception messages if failure.</returns>
        [WebMethod]
        public ServiceOperation GetMealIngredients(int mealID)
        {
            // Setup DB Connection and ServiceOperation object.
            ServiceOperation operation = new ServiceOperation();
            OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);

            try
            {
                // Setup Query and send it to DB.
                OleDbCommand dbCmd = new OleDbCommand("SELECT * FROM tblMealIngredients WHERE MealID=" + mealID, dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    // Add retrieved data to ServiceOperation object.
                    operation.Data.Add(new MealIngredient(reader));
                }
            }

            // If something fails, get Exception message and add it to ServiceOperation object.
            catch (FormatException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Invalid data supplied";
            }

            catch (OleDbException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Database error";
            }

            catch (Exception ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Unspecified error";
            }

            finally
            {
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    // Close any DB connections when done.
                    dbConn.Close();
                }
            }

            // Return the finished ServiceOperation object.
            return operation;
        }

        /// <summary>
        /// A wrapper method that sets up a query to insert new MealIngredients and sends it to DB.
        /// </summary>
        /// <param name="mealIngredient">The MealIngredient object to add to the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation AddMealIngredient(MealIngredient mealIngredient)
        {
            // If it doesn't exist, then build query and send to DB and return result.
            if (!AlreadyExists(mealIngredient))
            {
                string query = "INSERT INTO tblMealIngredients (MealID, IngredientID, RequiredQty) VALUES ({0}, {1}, {2})";
                return SendNonQuery(String.Format(query, mealIngredient.MealID, mealIngredient.IngredientID, mealIngredient.RequiredQty));
            }

            // If already exists, make new ServiceOperation reflecting that fact, and return it.
            ServiceOperation operation = new ServiceOperation();
            operation.Success = false;
            operation.Message = "Already Exists Error";
            operation.Exception = "Either that Ingredient already exists in this Meal, or some other unspecified error.";

            return operation;
        }

        /// <summary>
        /// A wrapper method that sets up a query to update an existing MealIngredient and sends it to DB.
        /// </summary>
        /// <param name="mealIngredient">The MealIngredient object to update in the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation UpdateMealIngredient(MealIngredient mealIngredient)
        {
            string query = "UPDATE tblMealIngredients SET MealID={0}, IngredientID={1}, RequiredQty={2} WHERE MealIngredientID={3}";
            return SendNonQuery(String.Format(query, mealIngredient.MealID, mealIngredient.IngredientID, mealIngredient.RequiredQty, mealIngredient.ID));
        }

        /// <summary>
        /// A wrapper method that sets up a query to delete an existing MealIngredient and sends it to DB.
        /// </summary>
        /// <param name="mealIngredient">The MealIngredient object to delete from the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation DeleteMealIngredient(int mealIngredientID)
        {
            string query = "DELETE FROM tblMealIngredients WHERE MealIngredientID={0}";
            return SendNonQuery(String.Format(query, mealIngredientID));
        }

        /// <summary>
        /// A method that checks the DB to see if the given MealIngredient already exists or not.
        /// </summary>
        /// <param name="mealIngredient">The MealIngredient to check for.</param>
        /// <returns>True if it already exists, false if not.</returns>
        [WebMethod]
        private bool AlreadyExists(MealIngredient mealIngredient)
        {
            ServiceOperation operation = CheckIfExists("tblMealIngredients", String.Format("MealID = {0} AND IngredientID = {1}", mealIngredient.MealID, mealIngredient.IngredientID));
            bool exists = true;
            if (operation.Success)
            {
                if (operation.Data.Count == 0)
                    exists = false;
            }

            return exists;
        }

        #endregion MealIngredients

        #region Ingredients

        /// <summary>
        /// Method to get all Ingredients from the DB.
        /// </summary>
        /// <returns>A ServiceOperation Object that contains either the retrieved Ingredient Data if successful, or the Exception messages if failure.</returns>
        [WebMethod]
        public ServiceOperation GetIngredients()
        {
            // Setup DB Connection and ServiceOperation object.
            ServiceOperation operation = new ServiceOperation();
            OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);

            try
            {
                // Setup Query and send it to DB.
                OleDbCommand dbCmd = new OleDbCommand("SELECT * FROM tblIngredients", dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    // Add retrieved data to ServiceOperation object.
                    operation.Data.Add(new Ingredient(reader));
                }
            }

            // If something fails, get Exception message and add it to ServiceOperation object.
            catch (FormatException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Invalid data supplied";
            }

            catch (OleDbException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Database error";
            }

            catch (Exception ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Unspecified error";
            }

            finally
            {
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    // Close any DB connections when done.
                    dbConn.Close();
                }
            }

            return operation;
        }

        /// <summary>
        /// A wrapper method that sets up a query to insert new Ingredient and sends it to DB.
        /// </summary>
        /// <param name="ingredient">The Ingredient object to add to the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation AddIngredient(Ingredient ingredient)
        {
            // If it doesn't exist, then build query and send to DB and return result.
            if (!AlreadyExists(ingredient))
            {
                string query = "INSERT INTO tblIngredients (IngredientName, AvailableQty, CostPerUnit) VALUES (\"{0}\", {1}, {2} )";
                return SendNonQuery(String.Format(query, ingredient.Name, ingredient.AvailableQty, ingredient.CostPerUnit));
            }

            // If already exists, make new ServiceOperation reflecting that fact, and return it.
            ServiceOperation operation = new ServiceOperation();
            operation.Success = false;
            operation.Message = "Already Exists Error";
            operation.Exception = "Either that Ingredient already exists, or some other unspecified error.";

            return operation;
        }

        /// <summary>
        /// A wrapper method that sets up a query to update an existing Ingredient and sends it to DB.
        /// </summary>
        /// <param name="ingredient">The Ingredient object to update in the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation UpdateIngredient(Ingredient ingredient)
        {
            string query = "UPDATE tblIngredients SET IngredientName=\"{0}\", AvailableQty={1}, CostPerUnit={2} WHERE IngredientID={3}";
            return SendNonQuery(String.Format(query, ingredient.Name, ingredient.AvailableQty, ingredient.CostPerUnit, ingredient.ID));
        }

        /// <summary>
        /// A method that checks the DB to see if the given Ingredient already exists or not.
        /// </summary>
        /// <param name="ingredient">The Ingredient to check for.</param>
        /// <returns>True if it already exists, false if not.</returns>
        [WebMethod]
        private bool AlreadyExists(Ingredient ingredient)
        {
            ServiceOperation operation = CheckIfExists("tblIngredients", String.Format("IngredientName = \"{0}\"", ingredient.Name));
            bool exists = true;
            if (operation.Success)
            {
                if (operation.Data.Count == 0)
                    exists = false;
            }

            return exists;
        }

        #endregion Ingredients

        #region Users

        /// <summary>
        /// Method that gets user data from the databse based on a given Username and Password combo, to verify a User's credentials
        /// </summary>
        /// <param name="username">The Username of the User</param>
        /// <param name="password">The Password of the User</param>
        /// <returns>A ServiceOperation Object that contains either the retrieved User Data if successful, or the Exception messages if failure.</returns>
        [WebMethod]
        public ServiceOperation UserLogin(string username, string password)
        {
            // Setup DB Connection and ServiceOperation object.
            ServiceOperation operation = new ServiceOperation();
            OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);

            // Setup Query and send it to DB.
            string query = String.Format("SELECT * FROM tblUsers WHERE Username=\"{0}\" AND UserPass=\"{1}\"", username, password);
            try
            {
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();

                OleDbDataReader reader = dbCmd.ExecuteReader();

                while (reader.Read())
                {
                    // Add retrieved data to ServiceOperation object.
                    operation.Data.Add(new User(reader));
                }
            }

            // If something fails, get Exception message and add it to ServiceOperation object.
            catch (FormatException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Invalid data supplied";
            }

            catch (OleDbException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Database error";
            }

            catch (Exception ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Unspecified error";
            }

            finally
            {
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    // Close any DB connections when done.
                    dbConn.Close();
                }
            }

            return operation;
        }

        /// <summary>
        /// Method to get all UserIDNameCombos (UserIDs and Usernames, but not Passwords) from the DB.
        /// </summary>
        /// <returns>A ServiceOperation Object that contains either the retrieved UserIDNameCombo Data if successful, or the Exception messages if failure.</returns>
        [WebMethod]
        public ServiceOperation GetUserIDNameCombos()
        {
            // Setup DB Connection and ServiceOperation object.
            ServiceOperation operation = new ServiceOperation();
            OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);

            // Setup Query and send it to DB.
            string query = "SELECT UserID,Username FROM tblUsers";
            try
            {
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();

                OleDbDataReader reader = dbCmd.ExecuteReader();

                while (reader.Read())
                {
                    // Add retrieved data to ServiceOperation object.
                    operation.Data.Add(new UserIDNameCombo(reader));
                }
            }

            // If something fails, get Exception message and add it to ServiceOperation object.
            catch (FormatException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Invalid data supplied";
            }

            catch (OleDbException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Database error";
            }

            catch (Exception ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Unspecified error";
            }

            finally
            {
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    // Close any DB connections when done.
                    dbConn.Close();
                }
            }

            // Return the finished ServiceOperation object.
            return operation;
        }

        /// <summary>
        /// A wrapper method that sets up a query to insert new User and sends it to DB.
        /// </summary>
        /// <param name="user">The User object to add to the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation AddNewUser(User user)
        {
            // If it doesn't exist, then build query and send to DB and return result.
            if (!AlreadyExists(user))
            {
                string query = "INSERT INTO tblUsers (Username, UserPass) VALUES (\"{0}\", \"{1}\" )";
                return SendNonQuery(String.Format(query, user.Username, user.Password));
            }

            // If already exists, make new ServiceOperation reflecting that fact, and return it.
            ServiceOperation operation = new ServiceOperation();
            operation.Success = false;
            operation.Message = "Already Exists Error";
            operation.Exception = "Either that User already exists, or some other unspecified error.";

            return operation;
        }

        /// <summary>
        /// A method that checks the DB to see if the given User already exists or not.
        /// </summary>
        /// <param name="user">The User to check for.</param>
        /// <returns>True if it already exists, false if not.</returns>
        [WebMethod]
        private bool AlreadyExists(User user)
        {
            ServiceOperation operation = CheckIfExists("tblUsers", String.Format("Username = \"{0}\"", user.Username));
            bool exists = true;
            if (operation.Success)
            {
                if (operation.Data.Count == 0)
                    exists = false;
            }

            return exists;
        }

        /// <summary>
        /// A wrapper method that sets up a query to update an existing User and sends it to DB.
        /// </summary>
        /// <param name="userCombo">The UserIDNameCombo object to use to update the User in the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation UpdateUser(UserIDNameCombo userCombo)
        {
            string query = "UPDATE tblUsers SET Username=\"{0}\" WHERE UserID={1}";
            return SendNonQuery(String.Format(query, userCombo.Username, userCombo.ID));
        }

        /// <summary>
        /// Method to reset an existing User's Password.
        /// </summary>
        /// <param name="user">The User object with new password to reset.</param>
        /// <returns></returns>
        [WebMethod]
        public ServiceOperation ResetUserPass(User user)
        {
            string query = "UPDATE tblUsers SET UserPass=\"{0}\" WHERE UserID={1}";
            return SendNonQuery(String.Format(query, user.Password, user.ID));
        }

        #endregion Users

        #region UserRoles

        /// <summary>
        /// Method to get all UserRoles for a given User from the DB.
        /// </summary>
        /// <returns>A ServiceOperation Object that contains either the retrieved OrderMeal Data if successful, or the Exception messages if failure.</returns>
        [WebMethod]
        public ServiceOperation GetUserRoles(int userID)
        {
            // Setup DB Connection and ServiceOperation object.
            ServiceOperation operation = new ServiceOperation();
            OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);

            try
            {
                // Setup Query and send it to DB.
                OleDbCommand dbCmd = new OleDbCommand("SELECT * FROM tblUserRoles WHERE UserID=" + userID, dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    // Add retrieved data to ServiceOperation object.
                    operation.Data.Add(new UserRole(reader));
                }
            }

            // If something fails, get Exception message and add it to ServiceOperation object.
            catch (FormatException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Invalid data supplied";
            }

            catch (OleDbException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Database error";
            }

            catch (Exception ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Unspecified error";
            }

            finally
            {
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    // Close any DB connections when done.
                    dbConn.Close();
                }
            }

            return operation;
        }

        /// <summary>
        /// A wrapper method that sets up a query to insert new UserRole and sends it to DB.
        /// </summary>
        /// <param name="userRole">The UserRole object to add to the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation AddUserRole(UserRole userRole)
        {
            // If it doesn't exist, then build query and send to DB and return result.
            if (!AlreadyExists(userRole))
            {
                string query = "INSERT INTO tblUserRoles (UserID, RoleID, AccessLevel) VALUES ({0}, {1}, {2})";
                return SendNonQuery(String.Format(query, userRole.UserID, userRole.RoleID, userRole.AccessLevel));
            }

            // If already exists, make new ServiceOperation reflecting that fact, and return it.
            ServiceOperation operation = new ServiceOperation();
            operation.Success = false;
            operation.Message = "Already Exists Error";
            operation.Exception = "Either that Role already exists for this User, or some other unspecified error.";

            return operation;
        }

        /// <summary>
        /// A wrapper method that sets up a query to update an existing UserRole and sends it to DB.
        /// </summary>
        /// <param name="userRole">The UserRole object to update in the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation UpdateUserRole(UserRole userRole)
        {
            string query = "UPDATE tblUserRoles SET UserID={0}, RoleID={1}, AccessLevel={2} WHERE UserRoleID={3}";
            return SendNonQuery(String.Format(query, userRole.UserID, userRole.RoleID, userRole.AccessLevel));
        }

        /// <summary>
        /// A method that checks the DB to see if the given UserRole already exists or not.
        /// </summary>
        /// <param name="userRole">The UserRole to check for.</param>
        /// <returns>True if it already exists, false if not.</returns>
        [WebMethod]
        private bool AlreadyExists(UserRole userRole)
        {
            ServiceOperation operation = CheckIfExists("tblUserRoles", String.Format("UserID = {0} AND RoleID = {1}", userRole.UserID, userRole.RoleID));
            bool exists = true;
            if (operation.Success)
            {
                if (operation.Data.Count == 0)
                    exists = false;
            }

            return exists;
        }

        #endregion UserRoles

        #region Roles

        /// <summary>
        /// Method to get all Roles from the DB.
        /// </summary>
        /// <returns>A ServiceOperation Object that contains either the retrieved OrderMeal Data if successful, or the Exception messages if failure.</returns>
        [WebMethod]
        public ServiceOperation GetRoles()
        {
            // Setup DB Connection and ServiceOperation object.
            ServiceOperation operation = new ServiceOperation();
            OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);

            try
            {
                // Setup Query and send it to DB.
                OleDbCommand dbCmd = new OleDbCommand("SELECT * FROM tblRoles", dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    // Add retrieved data to ServiceOperation object.
                    operation.Data.Add(new Role(reader));
                }
            }

            // If something fails, get Exception message and add it to ServiceOperation object.
            catch (FormatException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Invalid data supplied";
            }

            catch (OleDbException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Database error";
            }

            catch (Exception ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Unspecified error";
            }

            finally
            {
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    // Close any DB connections when done.
                    dbConn.Close();
                }
            }

            // Return the finished ServiceOperation object.
            return operation;
        }

        /// <summary>
        /// A wrapper method that sets up a query to insert new Roles and sends it to DB.
        /// </summary>
        /// <param name="role">The Role object to add to the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation AddRole(Role role)
        {
            // If it doesn't exist, then build query and send to DB and return result.
            if (!AlreadyExists(role))
            {
                string query = "INSERT INTO tblRoles (RoleDescription) VALUES (\"{0}\")";
                return SendNonQuery(String.Format(query, role.Description));
            }

            // If already exists, make new ServiceOperation reflecting that fact, and return it.
            ServiceOperation operation = new ServiceOperation();
            operation.Success = false;
            operation.Message = "Already Exists Error";
            operation.Exception = "Either that Role already exists, or some other unspecified error.";

            return operation;
        }

        /// <summary>
        /// A wrapper method that sets up a query to update an existing Role and sends it to DB.
        /// </summary>
        /// <param name="role">The Role object to update in the DB.</param>
        /// <returns>A ServiceOperation object representing the success result of the DB operation.</returns>
        [WebMethod]
        public ServiceOperation UpdateRole(Role role)
        {
            string query = "UPDATE tblRoles SET RoleDescription=\"{0}\" WHERE RoleID={1}";
            return SendNonQuery(String.Format(query, role.Description, role.ID));
        }

        /// <summary>
        /// A method that checks the DB to see if the given Role already exists or not.
        /// </summary>
        /// <param name="role">The Role to check for.</param>
        /// <returns>True if it already exists, false if not.</returns>
        [WebMethod]
        private bool AlreadyExists(Role role)
        {
            ServiceOperation operation = CheckIfExists("tblRoles", String.Format("RoleDescription = \"{0}\"", role.Description));
            bool exists = true;
            if (operation.Success)
            {
                if (operation.Data.Count == 0)
                    exists = false;
            }

            return exists;
        }

        #endregion Roles

        #region BASE

        /// <summary>
        /// Method to send a Non-Query to the DB.
        /// </summary>
        /// <param name="query">Non-Query Query to send to the DB.</param>
        /// <returns></returns>
        [WebMethod]
        private ServiceOperation SendNonQuery(string query)
        {
            // Setup DB Connection and ServiceOperation object.
            ServiceOperation operation = new ServiceOperation();
            OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);

            try
            {
                // Setup Query and send it to DB.
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
            }

            // If something fails, get Exception message and add it to ServiceOperation object.
            catch (FormatException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Invalid data supplied";
            }
            catch (OleDbException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Database error";
            }
            catch (Exception ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Unspecified error";
            }
            finally
            {
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    // Close any DB connections when done.
                    dbConn.Close();
                }
            }

            // Return the finished ServiceOperation object.
            return operation;
        }

        /// <summary>
        /// Basic method that tries to get data based on a given SELECT query, just to see if said data exists.
        /// </summary>
        /// <param name="table">The name of the DB Table to SELECT</param>
        /// <param name="conditions">Parameters/Conditions to match when SELECTing data from Table. (e.g. UserID={number})</param>
        /// <returns></returns>
        [WebMethod]
        private ServiceOperation CheckIfExists(string table, string conditions)
        {
            // Setup DB Connection and ServiceOperation object.
            ServiceOperation operation = new ServiceOperation();
            OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);

            // Setup Query and send it to DB.
            string query = String.Format("SELECT 1 FROM {0} WHERE {1}", table, conditions);
            try
            {
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();

                while (reader.Read())
                {
                    // Add retrieved data to ServiceOperation object.
                    operation.Data.Add(reader[0]);
                }
            }

            // If something fails, get Exception message and add it to ServiceOperation object.
            catch (FormatException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Invalid data supplied";
            }

            catch (OleDbException ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Database error";
            }

            catch (Exception ex)
            {
                operation.Success = false;
                operation.Exception = ex.Message;
                operation.Message = "Unspecified error";
            }

            finally
            {
                if (dbConn.State == System.Data.ConnectionState.Open)
                {
                    // Close any DB connections when done.
                    dbConn.Close();
                }
            }

            // Return the finished ServiceOperation object.
            return operation;
        }

        #endregion BASE
    }
}
