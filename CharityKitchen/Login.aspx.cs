using CharityKitchen.CharityKitchenDataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen
{
    public partial class Login : System.Web.UI.Page
    {
        /// <summary>
        /// Default event for Page Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Nothing really needs to be done here.
        }

        /// <summary>
        /// Event for when the "Login" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblInfo.Text = "Logging in...";

            // get available Roles for use to determine permissions.
            CharityKitchenDataServiceSoapClient svc = new CharityKitchenDataServiceSoapClient();
            ServiceOperation operation = svc.GetRoles();
            Dictionary<int, string> roles = new Dictionary<int, string>();

            if (operation.Success)
            {
                foreach (Role role in operation.Data)
                    roles.Add(role.ID, role.Description);
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Could not get User Roles, login halted." + Environment.NewLine + operation.Message + Environment.NewLine + operation.Exception;
                return;
            }

            // Get User data based on given Username and Password from page controls.
            operation = svc.UserLogin(txtUsername.Text, txtPassword.Text);

            // If login was successful (user data found), setup site page permissions based on UserRoles for that User in Session data, then redirect to Default main page.
            // If failure, display error message.
            if (operation.Success)
            {
                if (operation.Data.Count > 0)
                {
                    if (operation.Data.Count == 1)
                    {
                        int userID = ((User)operation.Data[0]).ID;
                        Session["LoggedInUser_ID"] = userID;
                        Session["LoggedInUser_Name"] = ((User)operation.Data[0]).Username;

                        operation = svc.GetUserRoles(userID);

                        if (operation.Success)
                        {
                            foreach (UserRole userRole in operation.Data)
                            {
                                switch (roles[userRole.RoleID])
                                {
                                    case "Orders":
                                        Session["OrdersAccess"] = userRole.AccessLevel;
                                        break;

                                    case "Meals":
                                        Session["MealsAccess"] = userRole.AccessLevel;
                                        break;

                                    case "Ingredients":
                                        Session["IngredientsAccess"] = userRole.AccessLevel;
                                        break;

                                    case "Users":
                                        Session["UsersAccess"] = userRole.AccessLevel;
                                        break;

                                    case "Roles":
                                        Session["RolesAccess"] = userRole.AccessLevel;
                                        break;
                                }
                            }
                            Response.Redirect("~/Default");  
                        }
                        else
                        {
                            lblInfo.ForeColor = System.Drawing.Color.Red;
                            lblInfo.Text = "Could not get Roles for specified user, login halted." + Environment.NewLine + operation.Message + Environment.NewLine + operation.Exception;
                        }
                    }
                    else
                    {
                        lblInfo.ForeColor = System.Drawing.Color.Red;
                        lblInfo.Text = "More than one User with these credentials?!? Halting Login.";
                    }
                }
                else
                {
                    lblInfo.ForeColor = System.Drawing.Color.Red;
                    lblInfo.Text = "Invalid credentials.";
                }
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = operation.Message + Environment.NewLine + operation.Exception;
            }
        }
    }
}