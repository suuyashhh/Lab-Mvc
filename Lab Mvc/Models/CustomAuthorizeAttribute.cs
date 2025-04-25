using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;

namespace Lab_Mvc.Models
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Check if user is authenticated via session or cookie
            string contact = null;
            string password = null;
            string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

            // Try to get credentials from session first
            if (httpContext.Session["Contact"] != null && httpContext.Session["Password"] != null)
            {
                contact = httpContext.Session["Contact"].ToString();
                password = httpContext.Session["Password"].ToString();
            }
            // If not in session, try to get from cookie
            else
            {
                HttpCookie authCookie = httpContext.Request.Cookies["UserAuth"];
                if (authCookie != null)
                {
                    contact = authCookie["Contact"];
                    password = authCookie["Password"];
                }
            }

            // If we have credentials, validate them against database
            if (!string.IsNullOrEmpty(contact) && !string.IsNullOrEmpty(password))
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"SELECT USER_ID, NAME, CONTACT, PASSWORD, COM_ID, USER_LOGIN 
                                    FROM (
                                        SELECT ADMIN_ID AS USER_ID, ADMIN_NAME AS NAME, ADMIN_CONTACT AS CONTACT, 
                                               ADMIN_PASSWORD AS PASSWORD, COM_ID, 'ADMIN' AS USER_LOGIN 
                                        FROM MST_ADMIN 
                                        UNION ALL 
                                        SELECT EMP_ID AS USER_ID, EMP_NAME AS NAME, EMP_CONTACT AS CONTACT, 
                                               EMP_PASSWORD AS PASSWORD, COM_ID, 'EMPLOYEE' AS USER_LOGIN 
                                        FROM MST_EMPLOYEE
                                    ) AS CombinedUsers 
                                    WHERE CONTACT = @contact AND PASSWORD = @pass";

                    using (SqlCommand sqlCommand = new SqlCommand(query, con))
                    {
                        sqlCommand.Parameters.AddWithValue("@contact", contact);
                        sqlCommand.Parameters.AddWithValue("@pass", password);

                        con.Open();
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                // Update session with current user data
                                httpContext.Session["UserId"] = reader["USER_ID"];
                                httpContext.Session["UserName"] = reader["NAME"];
                                httpContext.Session["Contact"] = reader["CONTACT"];
                                httpContext.Session["Password"] = reader["PASSWORD"];
                                httpContext.Session["ComId"] = reader["COM_ID"];
                                httpContext.Session["UserType"] = reader["USER_LOGIN"];

                                return true; // Valid credentials found
                            }
                        }
                    }
                }
            }

            return false; // Not authorized
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Clear any existing session/cookie if validation failed
            filterContext.HttpContext.Session.Clear();
            if (filterContext.HttpContext.Request.Cookies["UserAuth"] != null)
            {
                filterContext.HttpContext.Response.Cookies["UserAuth"].Expires = DateTime.Now.AddDays(-1);
            }

            // Redirect to login page with return URL
            filterContext.Result = new RedirectResult("~/Login.aspx?returnUrl=" +
                HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.PathAndQuery));
        }
    }
}