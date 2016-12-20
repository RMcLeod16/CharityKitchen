using CharityKitchen.CharityKitchenDataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen
{
    public partial class Roles : System.Web.UI.Page
    {
        /// <summary>
        /// Default event for Page Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the User's Access Level for this page.
            int roleAccess = 0;
            try
            {
                roleAccess = (int)Session["RolesAccess"];
            }
            catch
            {
                Response.Redirect("~/Default");
            }

            // If they have No Access to this page, kick them out.
            if (roleAccess < 1)
                Response.Redirect("~/Default");

            // If they have only Read access, disable all saving controls.
            if (roleAccess < 2)
            {
                btnNew.Enabled = false;
                btnSave.Enabled = false;
            }

            // Get Role data from DB.
            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();
            ServiceOperation operation = svc.GetRoles();

            // Bind data to GridView on page.
            if (operation.Success)
            {
                gvRoles.DataSource = operation.Data;
                gvRoles.DataBind();
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
        protected void gvRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected row and populate the page controls with its data.
            GridViewRow row = gvRoles.SelectedRow;
            lblRoleID.Text = row.Cells[1].Text;
            txtRoleDescription.Text = row.Cells[2].Text;
        }

        /// <summary>
        /// Event for when the "Save" Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Some sanity checks for TextBox contents.
            if (txtRoleDescription.Text == "")
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Please enter a Description for the Role";
                return;
            }

            if (!txtRoleDescription.Text.IsAlphaNumeric())
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Description for the Role is not allowed. Needs to be AlphaNumeric (Letters and numbers only). Please enter a valid Description.";
                return;
            }

            // Bundle data from page controls into object to send to DB.
            Role role = new Role();

            try
            {
                role.ID = int.Parse(lblRoleID.Text);
                role.Description = txtRoleDescription.Text;
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

            if (role.ID == 0)
                operation = svc.AddRole(role);
            else
                operation = svc.UpdateRole(role);

            if (operation.Success)
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;

                // Refresh GridView if data was modified.
                gvRoles.DataSource = svc.GetRoles().Data;
                gvRoles.DataBind();
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
            lblRoleID.Text = "0";
            txtRoleDescription.Text = "";
        }

        /// <summary>
        /// Event for when the "How TO Use" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnHelp_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Help/RolesHelp");
        }
    }
}