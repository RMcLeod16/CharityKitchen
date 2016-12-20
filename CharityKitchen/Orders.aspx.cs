using CharityKitchen.CharityKitchenDataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen
{
    public partial class Orders : System.Web.UI.Page
    {
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
            }
            // Get Meals data from DB.
            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();
            ServiceOperation operation = svc.GetOrders();

            // Bind data to GridView on page.
            if (operation.Success)
            {
                gvOrders.DataSource = operation.Data;
                gvOrders.DataBind();
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
            }

            // Refresh the state of the "View/Edit Order Contents" button
            RefreshOrderMealEditBtn();
        }

        /// <summary>
        /// Event for when a GridView row is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected row and populate the page controls with its data.
            lblOrderID.Text = gvOrders.SelectedRow.Cells[1].Text;
            txtOrderName.Text = gvOrders.SelectedRow.Cells[2].Text;
            txtEmail.Text = gvOrders.SelectedRow.Cells[3].Text;
            txtAddress.Text = gvOrders.SelectedRow.Cells[4].Text;
            txtSuburb.Text = gvOrders.SelectedRow.Cells[5].Text;
            txtPostcode.Text = gvOrders.SelectedRow.Cells[6].Text;

            // Refresh ""View/Edit Order Contents" button.
            RefreshOrderMealEditBtn();
        }

        /// <summary>
        /// Event for when the "Save" Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Some sanity checks for TextBox contents.
            // Order Name
            if (txtOrderName.Text == "")
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Please enter a name for the Order.";
                return;
            }

            if (!txtOrderName.Text.IsAlphaNumeric())
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Order Name is not allowed. Needs to be AlphaNumeric (Letters and numbers only). Please enter a valid Order Name.";
                return;
            }

            // Email Address
            if (txtEmail.Text == "")
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Please enter an Email Address.";
                return;
            }

            if (!txtEmail.Text.IsValidEmailAddress())
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Email address is not valid. Please enter a valid Email Address.";
                return;
            }

            // Address
            if (txtAddress.Text == "")
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Please enter an Address.";
                return;
            }

            if (!txtAddress.Text.IsAlphaNumeric())
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Address is not allowed. Needs to be AlphaNumeric (Letters and numbers only). Please enter a valid Address.";
                return;
            }

            // Suburb
            if (txtAddress.Text == "")
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Please enter a Suburb.";
                return;
            }

            if (!txtAddress.Text.IsAlphaNumeric())
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Suburb is not allowed. Needs to be AlphaNumeric (Letters and numbers only). Please enter a valid Suburb.";
                return;
            }

            // Bundle data from page controls into object to send to DB.
            Order order = new Order();

            try
            {
                order.ID = int.Parse(lblOrderID.Text);
                order.Name = txtOrderName.Text;
                order.Email = txtEmail.Text;
                order.Address = txtAddress.Text;
                order.Suburb = txtSuburb.Text;
                order.Postcode = int.Parse(txtPostcode.Text);
            }
            catch (Exception ex)
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = ex.Message;
                return;
            }

            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();
            ServiceOperation operation;

            if (order.ID == 0)
                operation = svc.AddOrder(order);
            else
                operation = svc.UpdateOrder(order);

            if (operation.Success)
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;

                // Refresh GridView if data was modified.
                gvOrders.DataSource = svc.GetOrders().Data;
                gvOrders.DataBind();
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
            lblOrderID.Text = "0";
            txtOrderName.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            txtSuburb.Text = "";
            txtPostcode.Text = "";

            // Refresh the "View/Edit Order Contents" button.
            RefreshOrderMealEditBtn();
        }

        /// <summary>
        /// Event for when the "View/Edit Order Contents" buttton is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOrderMealEdit_Click(object sender, EventArgs e)
        {
            // Grab selected Order's data and hold in session, then go to OrderEdit page.
            Session["OrderToEdit_ID"] = int.Parse(gvOrders.SelectedRow.Cells[1].Text);
            Session["OrderToEdit_Name"] = gvOrders.SelectedRow.Cells[2].Text;
            Response.Redirect("~/OrderEdit");
        }

        /// <summary>
        /// Event for when the "How To Use" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnHelp_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Help/OrdersHelp");
        }

        #endregion events

        #region methods

        /// <summary>
        /// Method that either enables or disables the "View/Edit Order Contents" button based on whether or not an existing Order is selected.
        /// </summary>
        private void RefreshOrderMealEditBtn()
        {
            if (int.Parse(lblOrderID.Text) == 0)
                btnOrderMealEdit.Enabled = false;
            else
                btnOrderMealEdit.Enabled = true;
        }

        #endregion methods
    }
}