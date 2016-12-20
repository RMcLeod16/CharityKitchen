using CharityKitchen.CharityKitchenDataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen
{
    /// <summary>
    /// Code Behind for the Users page.
    /// </summary>
    public partial class Users : System.Web.UI.Page
    {
        #region vars

        /// <summary>
        /// Page-wide var to store the User's Access Level for this particular page.
        /// </summary>
        private int usersAccess = 0;

        #endregion vars

        #region Events

        /// <summary>
        /// Default event for Page Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the User's Access Level for this page
            try
            {
                usersAccess = (int)Session["UsersAccess"];
            }
            catch
            {
                Response.Redirect("~/Default");
            }

            // If they have No Access to this page, kick them out.
            if (usersAccess < 1)
                Response.Redirect("~/Default");

            // If they have only Read access, disable all saving controls.
            if (usersAccess < 2)
            {
                btnNewUser.Enabled = false;
                btnNewUserCreate.Enabled = false;
                btnPasswordReset.Enabled = false;
                btnSave.Enabled = false;
            }

            // Get the UserIDs and Usernames from DB (but NOT Passwords).
            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();
            ServiceOperation operation = svc.GetUserIDNameCombos();

            // Bind data to GridView on page.
            if (operation.Success)
            {
                gvUsers.DataSource = operation.Data;
                gvUsers.DataBind();
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message;
            }

            // If UserID is 0 (meaning no User selected), disable Edit/Saving controls.
            if (int.Parse(lblUserID.Text) == 0)
            {
                btnSave.Enabled = false;
                btnUserRolesEdit.Enabled = false;
            }
        }

        /// <summary>
        /// Event for when a GridView row is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Populate controls with data from Gridview.
            lblUserID.Text = gvUsers.SelectedRow.Cells[1].Text;
            txtUsername.Text = gvUsers.SelectedRow.Cells[2].Text;

            // Only if there is a User selected AND the logged in User has write access to this section, do we then enable the Save/Edit conrols.
            if (int.Parse(lblUserID.Text) != 0 && usersAccess > 1)
            {
                btnSave.Enabled = true;
                btnUserRolesEdit.Enabled = true;
            }
        }

        /// <summary>
        /// Event ffor when the Save Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Some sanity checks for TextBox contents.
            if (txtUsername.Text == "")
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Please enter a Username.";
                return;
            }

            if (!txtUsername.Text.IsAlphaNumeric())
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Username is not allowed. Needs to be AlphaNumeric (Letters and numbers only). Please enter a valid Username.";
                return;
            }

            // Bundle data from page controls into object to send to DB.
            UserIDNameCombo userCombo = new UserIDNameCombo();

            try
            {
                userCombo.ID = int.Parse(lblUserID.Text);
                userCombo.Username = txtUsername.Text;
            }
            catch (Exception ex)
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = ex.Message;
                return;
            }

            if (userCombo.ID == 0)
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "ID for existing User is somehow 0. THIS SHOULD NOT BE HAPPENING. Saving Halted.";
                return;
            }

            // Send object to DB.
            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();
            ServiceOperation operation = svc.UpdateUser(userCombo);

            if (operation.Success)
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;

                // Refresh GridView if data was modified.
                gvUsers.DataSource = svc.GetUserIDNameCombos().Data;
                gvUsers.DataBind();
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message + Environment.NewLine + operation.Exception;
            }
        }

        /// <summary>
        /// Event for when the "View/Edit User Roles" Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUserRolesEdit_Click(object sender, EventArgs e)
        {
            // Grab selected User's data and hold in session, then go to UserRoles page.
            Session["UserToEdit_ID"] = int.Parse(gvUsers.SelectedRow.Cells[1].Text);
            Session["UserToEdit_Name"] = gvUsers.SelectedRow.Cells[2].Text;
            Response.Redirect("~/UserRoles");
        }

        /// <summary>
        /// Event for when the "New" Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNewUser_Click(object sender, EventArgs e)
        {
            // Set page controls' values to default.
            txtNewUsername.Text = "";
            txtNewUserPass.Text = "PASSDefault";
            txtNewUserPassConfirm.Text = "DefaultPASS";
        }

        /// <summary>
        /// Event for when the "Create" Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNewUserCreate_Click(object sender, EventArgs e)
        {
            // Soome sanity checks for TextBox contents.
            if (txtNewUsername.Text == "")
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Please enter a Username for the New User.";
                return;
            }

            if (!txtNewUsername.Text.IsAlphaNumeric())
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "New Username is not allowed. Needs to be AlphaNumeric (Letters and numbers only). Please enter a valid Username.";
                return;
            }

            if (txtNewUserPass.Text == "")
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Please enter a Password for the New User.";
                return;
            }

            if (txtNewUserPassConfirm.Text == "")
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Please confirm the Password for the New User.";
                return;
            }

            if (txtNewUserPass.Text != txtNewUserPassConfirm.Text)
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Password and Password-Confirmation for the New User do not match. Please reenter these.";
                return;
            }

            if (!txtNewUserPass.Text.IsAlphaNumeric())
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Password is not allowed. Needs to be AlphaNumeric (Letters and numbers only). Please enter a valid Password.";
                return;
            }

            // Bundle data from page controls into object tto send to DB.
            User newUser = new User();

            newUser.Username = txtNewUsername.Text;
            newUser.Password = txtNewUserPass.Text;

            // Send object to DB.
            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();
            ServiceOperation operation = svc.AddNewUser(newUser);

            if (operation.Success)
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;

                gvUsers.DataSource = svc.GetUserIDNameCombos().Data;
                gvUsers.DataBind();
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message + Environment.NewLine + operation.Exception;
            }
        }

        /// <summary>
        /// Event for when the "RESET PASSWORD" Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPasswordReset_Click(object sender, EventArgs e)
        {
            // Some sanity checks for TextBox contents.
            if (txtPassReset.Text == "")
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Please enter a New Password for the User \"" + txtUsername.Text + "\".";
                return;
            }

            if (txtPassResetConfirm.Text == "")
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Please confirm the New Password for User \"" + txtUsername.Text + "\".";
                return;
            }

            if (txtPassReset.Text != txtPassResetConfirm.Text)
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "New Password and New Password-Confirmation for User \"" + txtUsername.Text + "\" do not match. Please reenter these.";
                return;
            }

            if (!txtPassReset.Text.IsAlphaNumeric())
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "New Password is not allowed. Needs to be AlphaNumeric (Letters and numbers only). Please enter a valid Password.";
                return;
            }

            // Bundle data from page controls into object to send to DB.
            User user = new User();

            try
            {
                user.ID = int.Parse(lblUserID.Text);
                user.Password = txtPassReset.Text;
            }
            catch (Exception ex)
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = ex.Message;
                return;
            }

            if (user.ID == 0)
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "ID for existing User is somehow 0. THIS SHOULD NOT BE HAPPENING. Password Reset Halted.";
                return;
            }

            // Send object to DB.
            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();
            ServiceOperation operation = svc.ResetUserPass(user);

            if (operation.Success)
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;

                gvUsers.DataSource = svc.GetUserIDNameCombos().Data;
                gvUsers.DataBind();
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message + Environment.NewLine + operation.Exception;
            }
        }

        /// <summary>
        /// Event for when the "How To Use" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnHelp_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Help/UsersHelp");
        }

        #endregion events
    }
}