using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UrlShortener.App_Code
{
    public class ErrorHandler
    {
        // Auto-implemented properties
        public string Err_Message { get; }
        public string Page_Url { get; }
        private DateTime dateCreated;


        // Constructors
        public ErrorHandler(Exception ex, string strPageUrl = "")
        {
            Err_Message = ex.Message + Environment.NewLine + ex.StackTrace;
            Page_Url = strPageUrl;
            dateCreated = DateTime.Now;
        }
        public ErrorHandler(string strErrorMessage, string strPageUrl = "")
        {
            Err_Message = strErrorMessage;
            Page_Url = strPageUrl;
            dateCreated = DateTime.Now;
        }


        // Functions
        public void Log_Error()
        {
            DataLayer.Log_ErrorInDB(dateCreated, Page_Url, Err_Message);
        }
    }
}