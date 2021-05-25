using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UrlShortener.App_Code
{
    public static class DataLayer
    {
        /// <summary>
        /// Retrieves the required connection string from web config and creates a connection object
        /// </summary>
        /// <returns>Returns a connection to the relevant database</returns>
        private static SqlConnection Get_Connection()
        {
            string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["URL-SHORT-DB"].ConnectionString;
            return new SqlConnection(strConn);
        }


        /// <summary>
        /// This function attempts to save a URL and a given key in the UrlKeys table.
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strUrl"></param>
        /// <returns>
        /// A single row datatable with a "Result" column (0/1) and a "Message"
        /// column containing any potential error messages
        /// </returns>
        public static DataTable Create_ShortUrl(string strKey, string strUrl)
        {
            // --- Defining objects and variables ---
            string strQuery = "IF EXISTS(SELECT 1 FROM UrlKeys WHERE UrlKeys.UrlKey = @UrlKey) "
                            + "SELECT 0 As Result, 'Key already exists' As Message; "
                            + "ELSE BEGIN TRY "
                            + "INSERT INTO UrlKeys(UrlKey, FullUrl) VALUES (@UrlKey, @FullUrl); "
                            + "SELECT 1 As Result, 'URL key successfully written to table' As Message; "
                            + "END TRY BEGIN CATCH "
                            + "SELECT 0 As Result, ERROR_MESSAGE() As Message; "
                            + "END CATCH ";
            SqlDataAdapter da;
            DataTable dtOutput = new DataTable();


            using (SqlConnection conn = Get_Connection())
            {
                da = new SqlDataAdapter(strQuery, conn);
                da.SelectCommand.Parameters.Add(new SqlParameter("@UrlKey", strKey));
                da.SelectCommand.Parameters.Add(new SqlParameter("@FullUrl", strUrl));
                try
                {
                    da.Fill(dtOutput);
                }
                catch (Exception ex)
                {
                    ErrorHandler eh = new ErrorHandler(ex);
                    // Do not throw exception. Error will be logged and a fail-safe value returned
                }
                finally
                {
                    da.Dispose();
                }
            }


            // --- Returning result ---
            return dtOutput;
        }


        /// <summary>
        /// This function looks to see if the database already contains the supplied
        /// URL and returns the associated key if so. An empty string is returned if
        /// the URL is not found.
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns>Url key as string</returns>
        public static string Get_Key(string strUrl)
        {
            // --- Defining objects and variables ---
            string strQuery = "SELECT IsNull((SELECT Top 1 UrlKey FROM UrlKeys WHERE FullUrl = @FullUrl), '') As UrlKey; ";
            string strOutput = "";
            SqlCommand cmd;


            // --- Constructing command ---
            using (SqlConnection conn = Get_Connection())
            {
                cmd = new SqlCommand(strQuery, conn);
                cmd.Parameters.Add(new SqlParameter("@FullUrl", strUrl));
                try
                {
                    conn.Open();
                    strOutput = cmd.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    ErrorHandler eh = new ErrorHandler(ex);
                    // Do not throw exception. Error will be logged and a fail-safe value returned
                }
            }
            return strOutput;
        }


        /// <summary>
        /// This function retrieves the full URL associated with the shorthand URL key. If no URL
        /// is found, this function will return an empty string.
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns>A URL as a string</returns>
        public static string Get_Url(string strKey)
        {
            // --- Defining objects and variables ---
            string strQuery = "SELECT IsNull((SELECT Top 1 FullUrl FROM UrlKeys WHERE UrlKey = @UrlKey), '') As FullUrl; ";
            string strOutput = "";
            SqlCommand cmd;


            // --- Constructing command ---
            using (SqlConnection conn = Get_Connection())
            {
                cmd = new SqlCommand(strQuery, conn);
                cmd.Parameters.Add(new SqlParameter("@UrlKey", strKey));
                try
                {
                    conn.Open();
                    strOutput = cmd.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    ErrorHandler eh = new ErrorHandler(ex);
                    // Do not throw exception. Error will be logged and a fail-safe value returned
                }
            }
            return strOutput;
        }


        /// <summary>
        /// This function logs an error in the Log_Errors database table.
        /// </summary>
        /// <param name="dateLogged"></param>
        /// <param name="strPageUrl"></param>
        /// <param name="strErrorMessage"></param>
        public static void Log_ErrorInDB(DateTime dateLogged, string strPageUrl, string strErrorMessage)
        {
            // --- Defining objects and variables ---
            string strQuery = "INSERT INTO Log_Errors(DateLogged, PageUrl, ErrorMessage) VALUES (@DateLogged, @PagePath, @ErrorMessage); ";
            int intOutput = 0;
            SqlCommand cmd;


            // --- Constructing command ---
            using (SqlConnection conn = Get_Connection())
            {
                cmd = new SqlCommand(strQuery, conn);
                cmd.Parameters.Add(new SqlParameter("@DateLogged", dateLogged));
                cmd.Parameters.Add(new SqlParameter("@PagePath", strPageUrl));
                cmd.Parameters.Add(new SqlParameter("@ErrorMessage", strErrorMessage));
                try
                {
                    conn.Open();
                    intOutput = cmd.ExecuteNonQuery();
                    if (intOutput < 1)
                    {
                        // ### Perform any further attempts to log / flag errors here. Logging the error in the database did not work.
                    }
                }
                catch (Exception)
                {
                    // ### Perform any further attempts to log / flag errors here. Logging the error in the database did not work.
                }
            }
        }

    }
}