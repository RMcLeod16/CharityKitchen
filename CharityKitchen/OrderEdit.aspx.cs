using CharityKitchen.CharityKitchenDataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen
{
    public partial class OrderEdit : System.Web.UI.Page
    {
        #region vars

        /// <summary>
        /// List of ListItems for easy lookup of Available Meals and their names.
        /// </summary>
        private List<ListItem> meals;

        #endregion vars

        #region events

        /// <summary>
        /// Default event for Page Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the User's Access Level for this page.
            int ordersAccess = 0;

            try
            {
                ordersAccess = (int)Session["OrdersAccess"];
            }
            catch
            {
                Response.Redirect("~/Default");
            }

            // If they have No Access to this page, kick them out.
            if (ordersAccess < 1)
                Response.Redirect("~/Default");

            // If they have only Read access, disable all saving controls.
            if (ordersAccess < 2)
            {
                btnNew.Enabled = false;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }

            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();

            // Get and store list of Meals on page load.
            if (!PopulateMeals(svc)) return;
            if (IsPostBack) return;

            // if not a postback, setup DropDownLists and GridView with data from DB.
            BindMealsDDL();
            GetOrderMeals(svc); 
        }

        /// <summary>
        /// Event for when a GridView row is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOrderMeals_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected row and populate the page controls with its data.
            GridViewRow row = gvOrderMeals.SelectedRow;

            lblOrderMealID.Text = row.Cells[1].Text;

            foreach (ListItem ml in meals)
            {
                if (ml.Text == row.Cells[3].Text)
                    ddlMeals.SelectedValue = ml.Value;
            }

            txtOrdQty.Text = row.Cells[4].Text;
        }

        /// <summary>
        /// Event for when the "Save" Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Bundle data from page controls into object to send to DB.
            OrderMeal orderMeal = new OrderMeal();

            try
            {
                int orderID = (int)Session["OrderToEdit_ID"];
                orderMeal.ID = int.Parse(lblOrderMealID.Text);
                orderMeal.OrderID = orderID;
                orderMeal.MealID = int.Parse(ddlMeals.SelectedValue);
                orderMeal.OrderedQty = int.Parse(txtOrdQty.Text);
            }
            catch (Exception ex)
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = ex.Message;
                return;
            }

            // Send data to DB.
            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();
            ServiceOperation operation = new ServiceOperation();

            if (orderMeal.ID == 0)
                operation = svc.AddOrderMeal(orderMeal);
            else
                operation = svc.UpdateOrderMeal(orderMeal);

            if (operation.Success)
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;

                // Refresh GridView if data was modified.
                GetOrderMeals(svc);
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
            }
        }

        /// <summary>
        /// Event for when the "New" Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, EventArgs e)
        {
            // Set page controls' values to default.
            lblOrderMealID.Text = "0";
            ddlMeals.SelectedIndex = 0;
            txtOrdQty.Text = "";
        }

        /// <summary>
        /// Event for when the "Delete" Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            // Get currently selected record's ID.
            int orderMealID = int.Parse(lblOrderMealID.Text);
            if (orderMealID != 0)
            {
                // Tell DB to delete it.
                CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();
                ServiceOperation operation = svc.DeleteMealIngredient(orderMealID);

                if (operation.Success)
                {
                    lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                    lblInfo.Text = operation.Message;

                    // Refresh GridView if data was modified.
                    GetOrderMeals(svc);
                }
                else
                {
                    lblInfo.ForeColor = System.Drawing.Color.Red;
                    lblInfo.Text = operation.Message + Environment.NewLine + operation.Exception;
                }
            }
        }

        /// <summary>
        /// Event for when the "How To Use" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnHelp_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Help/OrderMealsHelp");
        }

        #endregion events

        #region methods

        /// <summary>
        /// Method to Get all the OrderMeals for a specified Oredr from the DB and display in a GridView on the page.
        /// </summary>
        /// <param name="svc">The Instantiated service objedct to use.</param>
        private void GetOrderMeals(CharityKitchenDataServiceSoapClient svc)
        {
            // get OrderID of Order-to-edit stored in Session data by Orders page.
            int orderID = 0; //default

            try
            {
                orderID = (int)Session["OrderToEdit_ID"];
            }
            catch
            {
                Response.Redirect("~/Orders");
            }

            // If OrderID is an actual ID, continue, else kick user back to Orders page.
            if (orderID != 0)
            {
                // Get data from DB.
                ServiceOperation operation = svc.GetOrderMeals(orderID);

                if (operation.Success)
                {
                    // Display data on page.
                    gvOrderMeals.DataSource = operation.Data;
                    gvOrderMeals.DataBind();

                    lblOrderIDName.Text = orderID.ToString() + " - " + (string)Session["OrderToEdit_Name"];
                    // Prettyifying things, makes GridView display Order's name for user instead of ID.
                    if (operation.Data.Count > 0)
                    {
                        gvOrderMeals.HeaderRow.Cells[3].Text = "MealName";



                        for (int i = 0; i < gvOrderMeals.Rows.Count; i++)
                            foreach (ListItem ml in meals)
                                if (ml.Value == gvOrderMeals.Rows[i].Cells[3].Text)
                                    gvOrderMeals.Rows[i].Cells[3].Text = ml.Text;
                    }
                }
                else
                {
                    lblInfo.ForeColor = System.Drawing.Color.Red;
                    lblInfo.Text = operation.Message + Environment.NewLine + operation.Exception;
                }
            }
            else
            {
                Response.Redirect("~/Orders");
            }
        }

        /// <summary>
        /// Method to get all Meals and their IDs from DB and store them in a List for this page.
        /// </summary>
        /// <param name="svc">The Instantiated service objedct to use.</param>
        /// <returns></returns>
        private bool PopulateMeals(CharityKitchenDataServiceSoapClient svc)
        {
            ServiceOperation operation = svc.GetMeals();

            if (operation.Success)
            {
                meals = new List<ListItem>();

                foreach (object record in operation.Data)
                {
                    var ml = record as Meal;
                    meals.Add(new ListItem(ml.Name, ml.ID.ToString()));
                }
                return true;
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Failed to load Meals";
                btnSave.Enabled = false;
                return false;
            }
        }

        /// <summary>
        /// Method to Bind data from meals List to the respective DropDownList
        /// </summary>
        private void BindMealsDDL()
        {
            ddlMeals.DataSource = meals;
            ddlMeals.DataTextField = "Text";
            ddlMeals.DataValueField = "Value";
            ddlMeals.DataBind();
        }

        #endregion methods


    }
}