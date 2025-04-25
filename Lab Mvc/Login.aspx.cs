using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Lab_Mvc
{
    public partial class Login : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["loggedout"] == "true")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert",
                    "swal('You are logged out successfully','','success');", true);
            }
            // Check if there is already a login cookie
            if (Session["UserName"] == null && Request.Cookies["UserAuth"] != null)
            {
                // Restore session from the cookie
                HttpCookie cookie = Request.Cookies["UserAuth"];
                Session["UserName"] = cookie["UserName"];
                Session["UserId"] = cookie["UserId"];
                Session["Contact"] = cookie["Contact"];
                Session["ComId"] = cookie["ComId"];
                Session["UserType"] = cookie["UserType"];

                // Redirect to home page
                Response.Redirect("~/CasePaper/Index");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT USER_ID, NAME, CONTACT, PASSWORD, COM_ID, USER_LOGIN FROM(SELECT ADMIN_ID AS USER_ID, ADMIN_NAME AS NAME, ADMIN_CONTACT AS CONTACT, ADMIN_PASSWORD AS PASSWORD, COM_ID, 'ADMIN' AS USER_LOGIN FROM MST_ADMIN UNION ALL SELECT EMP_ID AS USER_ID, EMP_NAME AS NAME, EMP_CONTACT AS CONTACT, EMP_PASSWORD AS PASSWORD, COM_ID, 'EMPLOYEE' AS USER_LOGIN FROM MST_EMPLOYEE) AS CombinedUsers WHERE CONTACT = @contact AND PASSWORD = @pass;", con);
            sqlCommand.Parameters.AddWithValue("@contact", UserContact.Text);
            sqlCommand.Parameters.AddWithValue("@pass", UserPassword.Text);

            con.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                Session["UserId"] = reader.GetValue(0);
                Session["UserName"] = reader.GetValue(1);               
                Session["Contact"] = reader.GetValue(2);               
                Session["Password"] = reader.GetValue(3);           
                Session["ComId"] = reader.GetValue(4);           
                Session["UserType"] = reader.GetValue(5);

                // Create a persistent cookie
                HttpCookie authCookie = new HttpCookie("UserAuth");
                authCookie["UserName"] = reader.GetValue(1).ToString();
                authCookie["UserId"] = reader.GetValue(0).ToString();
                authCookie["Contact"] = reader.GetValue(2).ToString();
                authCookie["Password"] = reader.GetValue(3).ToString();
                authCookie["ComId"] = reader.GetValue(4).ToString();
                authCookie["UserType"] = reader.GetValue(5).ToString();
                authCookie.Expires = DateTime.Now.AddDays(30); // Cookie valid for 30 days
                Response.Cookies.Add(authCookie);

                // Clear the input fields
                UserContact.Text = "";
                UserPassword.Text = "";

                // Redirect to the corresponding page
                if (Request.QueryString["type"] != null)
                {
                    string type = Request.QueryString["type"];
                    Response.Redirect($"{type}.aspx");
                }
                else
                {
                    Response.Redirect("~/CasePaper/Index");

                }
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", "swal('Invalid Login..!','','error');", true);
            }

            con.Close();
           
        }
    }
}