using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace UrlShortener
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            // --- Identifying if a short URL key was used ---
            // short URLs will always be in the form of "domain.com/ABCD123
            if (Request.Url.Segments.Length == 2)
            {
                // a short URL key does not have a dot or a file name extension. Check this
                // first to quickly disqualify a request URL
                if (Request.Url.Segments[1].Contains(".") == false)
                {
                    string strKey = Request.Url.Segments[1];
                    string strRedirectUrl = UrlShortener.App_Code.DataLayer.Get_Url(strKey);
                    if (strRedirectUrl != "")
                    {
                        // If a URL corresponding to the supplied key was found, redirect to it
                        Response.Redirect(strRedirectUrl);
                        Response.End();
                    }
                }
            }
        }
    }
}