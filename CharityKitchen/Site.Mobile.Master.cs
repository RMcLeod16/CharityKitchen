using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen
{
    public partial class Site_Mobile : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int userID = 0;
            try
            {
                userID = (int)Session["LoggedInUser_ID"];
            }
            catch
            {
                Response.Redirect("~/Login");
            }

            if (userID == 0)
                Response.Redirect("~/Login");
        }
    }
}