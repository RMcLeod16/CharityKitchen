using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblUsername.Text = (string)Session["LoggedInUser_Name"];
            }
            catch
            {
                lblUsername.Text = "";
            }
        }
    }
}