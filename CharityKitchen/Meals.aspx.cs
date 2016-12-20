using CharityKitchen.CharityKitchenDataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen
{
    public partial class Meals : System.Web.UI.Page
    {
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
            }

            // Get Meals data from DB.
            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();
            ServiceOperation operation = svc.GetMeals();

            // Bind data to GridView on page.
            if (operation.Success)
            {
                gvMeals.DataSource = operation.Data;
                gvMeals.DataBind();
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
            }

            // If no Meal is selected, disable "View/Edit Meal Contents" button.
            if (int.Parse(lblMealID.Text) == 0)
                btnMealIngredientsEdit.Enabled = false;
        }

        /// <summary>
        /// Event for when a GridView row is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMeals_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Populate controls with data from Gridview.
            lblMealID.Text = gvMeals.SelectedRow.Cells[1].Text;
            txtMealName.Text = gvMeals.SelectedRow.Cells[2].Text;

            // If an existing Meal's ID is present , Enable the "View/Edit Meal Contents" button.
            if (int.Parse(lblMealID.Text) != 0)
                btnMealIngredientsEdit.Enabled = true;
        }

        /// <summary>
        /// Event for when the "Save" Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Some sanity checks for TextBox contents.
            if (txtMealName.Text == "")
            {
                    lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                    lblInfo.Text = "Please enter a name for the Meal.";
                    return;
            }

            if (!txtMealName.Text.IsAlphaNumeric())
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Meal Name is not allowed. Needs to be AlphaNumeric (Letters and numbers only). Please enter a valid Meal Name.";
                return;
            }

            // Bundle data from page controls into object to send to DB.
            Meal meal = new Meal();

            try
            {
                meal.ID = int.Parse(lblMealID.Text);
                meal.Name = txtMealName.Text;
            }
            catch (Exception ex)
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = ex.Message;
                return;
            }

            // Send data to DB.
            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();
            ServiceOperation operation;

            if (meal.ID == 0)
                operation = svc.AddMeal(meal);
            else
                operation = svc.UpdateMeal(meal);

            if (operation.Success)
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;

                // Refresh GridView if data was modified.
                gvMeals.DataSource = svc.GetMeals().Data;
                gvMeals.DataBind();
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message + Environment.NewLine + operation.Exception;
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
            lblMealID.Text = "0";
            txtMealName.Text = "";
            btnMealIngredientsEdit.Enabled = false;
        }

        /// <summary>
        /// Event for when the "View/Edit Meal Contents" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMealIngredientsEdit_Click(object sender, EventArgs e)
        {
            // Grab selected meals's data and hold in session, then go to MealEdit page.
            Session["MealToEdit_ID"] = int.Parse(gvMeals.SelectedRow.Cells[1].Text);
            Session["MealToEdit_Name"] = gvMeals.SelectedRow.Cells[2].Text;
            Response.Redirect("~/MealEdit");
        }

        /// <summary>
        /// Event for when the "How To Use" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnHelp_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Help/MealsHelp");
        }
    }
}