using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab_Mvc.Models
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Check if user is authenticated via session or cookie
            if (httpContext.Session["UserId"] != null)
            {
                return true; // User is authenticated via session
            }

            // Check for authentication cookie
            HttpCookie authCookie = httpContext.Request.Cookies["UserAuth"];
            if (authCookie != null)
            {
                // Restore session from cookie
                httpContext.Session["UserId"] = authCookie["UserId"];
                httpContext.Session["UserName"] = authCookie["UserName"];
                httpContext.Session["Contact"] = authCookie["Contact"];
                httpContext.Session["ComId"] = authCookie["ComId"];
                httpContext.Session["UserType"] = authCookie["UserType"];

                return true;
            }

            return false; // Not authorized
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Redirect to login page with return URL
            filterContext.Result = new RedirectResult("~/Login.aspx?returnUrl=" +
                HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.PathAndQuery));
        }
    }
}