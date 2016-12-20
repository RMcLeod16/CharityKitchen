using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CharityKitchen.CharityKitchenDataService;

namespace CharityKitchen
{
    public partial class SiteMaster : MasterPage
    {
        /// <summary>
        /// Default event for when the page loads.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check to see if user is actually logged in
            // and if they are not, kick them back out to login page
            int userID = 0;
            try
            {
                userID = (int)Session["LoggedInUser_ID"];
            }
            catch
            {
                Response.Redirect("~/Login");
            }

            if (userID == 0)
                Response.Redirect("~/Login");

            // Perform checks to see which pages users can access
            // and hide links to pages they can not access

            // Check for access to Orders
            try
            {
                int ordersAccess = (int)Session["OrdersAccess"];
                if (ordersAccess < 1)
                    hplOrders.Visible = false;
            }
            catch
            {
                hplOrders.Visible = false;
            }

            // Check for access to Meals
            try
            {
                int mealsAccess = (int)Session["MealsAccess"];
                if (mealsAccess < 1)
                    hplMeals.Visible = false;
            }
            catch
            {
                hplMeals.Visible = false;
            }

            // Check for access to Ingredients
            try
            {
                int ingredientsAccess = (int)Session["IngredientsAccess"];
                if (ingredientsAccess < 1)
                    hplIngredients.Visible = false;
            }
            catch
            {
                hplIngredients.Visible = false;
            }

            // Check for access to Users
            try
            {
                int usersAccess = (int)Session["UsersAccess"];
                if (usersAccess < 1)
                    hplUsers.Visible = false;
            }
            catch
            {
                hplUsers.Visible = false;
            }

            // Check for access to Roles
            try
            {
                int rolesAccess = (int)Session["RolesAccess"];
                if (rolesAccess < 1)
                    hplRoles.Visible = false;
            }
            catch
            {
                hplRoles.Visible = false;
            }
        }

        // http://stackoverflow.com/questions/13063231/label-server-side-click
        /// <summary>
        /// Event for when Logout button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtLogout_Click(object sender, EventArgs e)
        {
            // Kill the session data then kick user back to Login page
            Session.Clear();
            Response.Redirect("~/Login");
        }
    }
}