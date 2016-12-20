using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CharityKitchen.CharityKitchenDataService;

namespace CharityKitchen
{
    public partial class Ingredients : System.Web.UI.Page
    {
        /// <summary>
        /// Default event for Page Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the User's Access Level for this page.
            int ingredientsAccess = 0;

            try
            {
                ingredientsAccess = (int)Session["IngredientsAccess"];
            }
            catch
            {
                Response.Redirect("~/Default");
            }

            // If they have No Access to this page, kick them out.
            if (ingredientsAccess < 1)
                Response.Redirect("~/Default");

            // If they have only Read access, disable all saving controls.
            if (ingredientsAccess < 2)
            {
                btnNew.Enabled = false;
                btnSave.Enabled = false;
            }

            // Get Ingredients data from DB.
            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();
            ServiceOperation operation = svc.GetIngredients();

            // Bind data to GridView on page.
            if (operation.Success)
            {
                gvIngredients.DataSource = operation.Data;
                gvIngredients.DataBind();
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
            }
        }

        /// <summary>
        /// Event for when a GridView row is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvIngredients_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected row and populate the page controls with its data.
            GridViewRow row = gvIngredients.SelectedRow;
            lblIngredientID.Text = row.Cells[1].Text;
            txtIngredientName.Text = row.Cells[2].Text;
            txtAvailableQty.Text = row.Cells[3].Text;
            txtCostPerUnit.Text = row.Cells[4].Text;
        }

        /// <summary>
        /// Event for when the "Save" Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Some sanity checks for TextBox contents.
            if (txtIngredientName.Text == "")
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Please enter a Name for the Ingredient";
                return;
            }

            if (!txtIngredientName.Text.IsAlphaNumeric())
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Ingredient Name is not allowed. Needs to be AlphaNumeric (Letters and numbers only). Please enter a valid Ingredient Name.";
                return;
            }

            // Bundle data from page controls into object to send to DB.
            Ingredient ingredient = new Ingredient();

            try
            {
                ingredient.ID = int.Parse(lblIngredientID.Text);
                ingredient.Name = txtIngredientName.Text;
                ingredient.AvailableQty = int.Parse(txtAvailableQty.Text);
                ingredient.CostPerUnit = decimal.Parse(txtCostPerUnit.Text);
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

            if (ingredient.ID == 0)
                operation = svc.AddIngredient(ingredient);
            else
                operation = svc.UpdateIngredient(ingredient);

            if (operation.Success)
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;

                // Refresh GridView if data was modified.
                gvIngredients.DataSource = svc.GetIngredients().Data;
                gvIngredients.DataBind();
                
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
            lblIngredientID.Text = "0";
            txtIngredientName.Text = "";
            txtAvailableQty.Text = "";
            txtCostPerUnit.Text = "";
        }

        /// <summary>
        /// Event for when the "How To Use" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnHelp_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Help/IngredientsHelp");
        }
    }
}