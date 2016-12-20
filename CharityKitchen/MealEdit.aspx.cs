using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CharityKitchen.CharityKitchenDataService;

namespace CharityKitchen
{
    public partial class MealEdit : System.Web.UI.Page
    {
        #region vars

        /// <summary>
        /// List of ListItems for easy lookup of Available Ingredients and their Names
        /// </summary>
        private List<ListItem> ingredients;

        #endregion vars

        /// <summary>
        /// Default event for Page Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the User's Access Level for this page.
            int mealsAccess = 0;

            try
            {
                mealsAccess = (int)Session["MealsAccess"];
            }
            catch
            {
                Response.Redirect("~/Default");
            }

            // If they have No Access to this page, kick them out.
            if (mealsAccess < 1)
                Response.Redirect("~/Default");

            // If they have only Read access, disable all saving controls.
            if (mealsAccess < 2)
            {
                btnNew.Enabled = false;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }

            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();

            // Get and store list of roles on page load.
            if (!PopulateIngredients(svc)) return;
            if (IsPostBack) return;

            // if not a postback, setup DropDownLists and GridView with data from DB.
            BindIngredientsDDL();
            GetMealIngredients(svc); 
        }

        /// <summary>
        /// Event for when a GridView row is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMealIngredients_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected row and populate the page controls with its data.
            GridViewRow row = gvMealIngredients.SelectedRow;

            lblID.Text = row.Cells[1].Text;

            foreach (ListItem ing in ingredients)
            {
                if (ing.Text == row.Cells[3].Text)
                    ddlIngredients.SelectedValue = ing.Value;
            }

            txtReqQty.Text = row.Cells[4].Text;
        }

        /// <summary>
        /// Event for when the "Save" Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Bundle data from page controls into object to send to DB.
            MealIngredient mealIngredient = new MealIngredient();

            try
            {
                int mealID = (int)Session["MealToEdit_ID"];
                mealIngredient.ID = int.Parse(lblID.Text);
                mealIngredient.MealID = mealID;
                mealIngredient.IngredientID = int.Parse(ddlIngredients.SelectedValue);
                mealIngredient.RequiredQty = int.Parse(txtReqQty.Text);
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

            if (mealIngredient.ID == 0)
                operation = svc.AddMealIngredient(mealIngredient);
            else
                operation = svc.UpdateMealIngredient(mealIngredient);

            if (operation.Success)
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;

                // Refresh GridView if data was modified.
                GetMealIngredients(svc);
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
            lblID.Text = "0";
            ddlIngredients.SelectedIndex = 0;
            txtReqQty.Text = "";
        }

        /// <summary>
        /// Event for when the "Delete" Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            // Get currently selected record's ID.
            int mealIngredientID = int.Parse(lblID.Text);
            if (mealIngredientID != 0)
            {
                // Tell DB to delete it.
                CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();
                ServiceOperation operation = svc.DeleteMealIngredient(mealIngredientID);

                if (operation.Success)
                {
                    lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                    lblInfo.Text = operation.Message;

                    // Refresh GridView if data was modified.
                    GetMealIngredients(svc);
                }
                else
                {
                    lblInfo.ForeColor = System.Drawing.Color.Red;
                    lblInfo.Text = operation.Message + Environment.NewLine + operation.Exception;
                }
            }
        }

        #region methods

        /// <summary>
        /// Method to Get all the MealIngredients for a specified Meal from the DB and display in a GridView on the page.
        /// </summary>
        /// <param name="svc">The Instantiated service objedct to use.</param>
        private void GetMealIngredients(CharityKitchenDataServiceSoapClient svc)
        {
            // get MealID of Meal-to-edit stored in Session data by Meals page.
            int mealID = 0; //default

            try
            {
                mealID = (int)Session["MealToEdit_ID"];
            }
            catch
            {
                Response.Redirect("~/Meals");
            }

            // If MealID is an actual ID, continue, else kick user back to Meals page.
            if (mealID != 0)
            {
                // Get data from DB.
                ServiceOperation operation = svc.GetMealIngredients(mealID);

                if (operation.Success)
                {
                    // Display data on page.
                    lblMealName.Text = (string)Session["MealToEdit_Name"];

                    gvMealIngredients.DataSource = operation.Data;
                    gvMealIngredients.DataBind();

                    // Prettyifying things, makes GridView display Ingredient's name for user instead of ID.
                    if (operation.Data.Count > 0)
                    {
                        gvMealIngredients.HeaderRow.Cells[3].Text = "IngredientName";

                        for (int i = 0; i < gvMealIngredients.Rows.Count; i++)
                            foreach (ListItem ing in ingredients)
                                if (ing.Value == gvMealIngredients.Rows[i].Cells[3].Text)
                                    gvMealIngredients.Rows[i].Cells[3].Text = ing.Text;
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
                Response.Redirect("~/Meals");
            }
        }

        /// <summary>
        /// Method to get all Ingredients and their IDs from DB and store them in a List for this page.
        /// </summary>
        /// <param name="svc">The Instantiated service objedct to use.</param>
        /// <returns></returns>
        private bool PopulateIngredients(CharityKitchenDataServiceSoapClient svc)
        {
            ServiceOperation operation = svc.GetIngredients();

            if (operation.Success)
            {
                ingredients = new List<ListItem>();

                foreach (object record in operation.Data)
                {
                    var ing = record as Ingredient;
                    ingredients.Add(new ListItem(ing.Name, ing.ID.ToString()));
                }

                return true;
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Failed to load Ingredients";
                btnSave.Enabled = false;
                return false;
            }

        }

        /// <summary>
        /// Method to Bind data from ingredients List to the respective DropDownList
        /// </summary>
        private void BindIngredientsDDL()
        {
            ddlIngredients.DataSource = ingredients;
            ddlIngredients.DataTextField = "Text";
            ddlIngredients.DataValueField = "Value";
            ddlIngredients.DataBind();
        }

        #endregion methods

        protected void btnHelp_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Help/MealIngredientsHelp");
        }
    }
}