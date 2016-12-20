using CharityKitchen.CharityKitchenDataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen
{
    public partial class UserRoles : System.Web.UI.Page
    {
        #region vars

        /// <summary>
        /// List of ListItems for easy lookup of Available Roles and their data
        /// </summary>
        private List<ListItem> roles;

        #endregion vars

        #region events

        /// <summary>
        /// Default even for Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the User's Access Level for this page
            int usersAccess = 0;
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
                btnNew.Enabled = false;
                btnSave.Enabled = false;
            }

            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();

            // Get and store list of roles on page load.
            if (!PopulateRoles(svc)) return;
            if (IsPostBack) return;

            // if not a postback, setup DropDownLists and GridView with data from DB.
            BindRolesDDL();
            BindAccessDDL();

            GetUserRoles(svc);
        }

        /// <summary>
        /// Event for when a GridView row is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUserRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected row and populate the page controls with its data
            GridViewRow row = gvUserRoles.SelectedRow;

            lblUserRoleID.Text = row.Cells[1].Text;

            foreach (ListItem rl in roles)
            {
                if (rl.Text == row.Cells[3].Text)
                    ddlRoles.SelectedValue = rl.Value;
            }

            ddlAccesLevel.SelectedValue = row.Cells[4].Text;
        }

        /// <summary>
        /// Event for when the "Save" Button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Bundle data from page controls into object to send to DB.
            UserRole userRole = new UserRole();

            try
            {
                int userID = (int)Session["UserToEdit_ID"];
                userRole.ID = int.Parse(lblUserRoleID.Text);
                userRole.UserID = userID;
                userRole.RoleID = int.Parse(ddlRoles.SelectedValue);
                userRole.AccessLevel = int.Parse(ddlAccesLevel.SelectedValue);
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

            if (userRole.ID == 0)
                operation = svc.AddUserRole(userRole);
            else
                operation = svc.UpdateUserRole(userRole);

            if (operation.Success)
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = operation.Message;

                // Refresh GridView if data was modified.
                GetUserRoles(svc);
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
            lblUserRoleID.Text = "0";
            ddlRoles.SelectedIndex = 0;
            ddlAccesLevel.SelectedIndex = 0;
        }

        /// <summary>
        /// Event for when the "How To Use"" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnHelp_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Help/UserRolesHelp");
        }

        #endregion events

        #region methods

        /// <summary>
        /// Method to Get all the UserRoles for a specified User from the DB and display in a GridView on the page.
        /// </summary>
        /// <param name="svc">The instantiated service object to use.</param>
        private void GetUserRoles(CharityKitchenDataServiceSoapClient svc)
        {
            // get UserID of User-to-edit stored in Session data by Users page.
            int userID = 0; //default

            try
            {
                userID = (int)Session["UserToEdit_ID"];
            }
            catch
            {
                Response.Redirect("~/Users");
            }

            // If UserId is an actual ID, continue, else kick user back to Users page
            if (userID != 0)
            {
                // Get data from DB
                ServiceOperation operation = svc.GetUserRoles(userID);

                if (operation.Success)
                {
                    // Display data on page
                    lblUserIDName.Text = userID.ToString() + " - " + (string)Session["UserToEdit_Name"];

                    gvUserRoles.DataSource = operation.Data;
                    gvUserRoles.DataBind();

                    // Prettyifying things, makes GridView display Role's name for user instead of ID
                    if (operation.Data.Count > 0)
                    {
                        gvUserRoles.HeaderRow.Cells[3].Text = "RoleDescription";

                        for (int i = 0; i < gvUserRoles.Rows.Count; i++)
                            foreach (ListItem rl in roles)
                                if (rl.Value == gvUserRoles.Rows[i].Cells[3].Text)
                                    gvUserRoles.Rows[i].Cells[3].Text = rl.Text;
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
                Response.Redirect("~/Users");
            }
        }

        /// <summary>
        /// Method to get all Roles and their IDs from DB and store them in a List for this page.
        /// </summary>
        /// <param name="svc">The instantiated service object to use.</param>
        /// <returns>True if successful, false if failure<./returns>
        private bool PopulateRoles(CharityKitchenDataServiceSoapClient svc)
        {
            ServiceOperation operation = svc.GetRoles();

            if (operation.Success)
            {
                roles = new List<ListItem>();

                foreach (object record in operation.Data)
                {
                    var rl = record as Role;
                    roles.Add(new ListItem(rl.Description, rl.ID.ToString()));
                }
                return true;
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Failed to load Roles";
                btnSave.Enabled = false;
                return false;
            }
        }

        /// <summary>
        /// Method to Bind data from roles List to the respective DropDownList
        /// </summary>
        private void BindRolesDDL()
        {
            ddlRoles.DataSource = roles;
            ddlRoles.DataTextField = "Text";
            ddlRoles.DataValueField = "Value";
            ddlRoles.DataBind();
        }

        /// <summary>
        /// Method to setup data for Access Levels and Bind to the respective DropDownList
        /// </summary>
        private void BindAccessDDL()
        {
            List<ListItem> accessLevels = new List<ListItem>();
            accessLevels.Add(new ListItem("No Access", "0"));
            accessLevels.Add(new ListItem("Read", "1"));
            accessLevels.Add(new ListItem("Write", "2"));

            ddlAccesLevel.DataSource = accessLevels;
            ddlAccesLevel.DataTextField = "Text";
            ddlAccesLevel.DataValueField = "Value";
            ddlAccesLevel.DataBind();
        }

        #endregion methods
    }
}