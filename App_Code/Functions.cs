using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace UrlShortener.App_Code
{
    public static class Functions
    {

        /// <summary>
        /// This function attempts to return a valid short URL when given a standard URL input.
        /// It first attempts to find the supplied URL in the database (returning the already
        /// listed short URL if so), otherwise it attempts to create a new short URL and store
        /// it in the database table.
        /// </summary>
        /// <param name="strInputUrl"></param>
        /// <returns>A short URL key as string</returns>
        public static string CreateShortUrl(string strInputUrl)
        {
            // --- Defining initial objects and variables ---
            const int MAX_ATTEMPTS = 3;
            string strKey = "";
            int intAttemptCount = 0;
            bool blnAttemptSuccess = false;
            string strMessage = "Unknown error occurred in CreateShortUrl function";    // default error message


            // --- Pre-processing input URL ---
            // making sure that the input is prefixed with either http:// or https://
            if (strInputUrl.IndexOf("http://") != 0 && strInputUrl.IndexOf("https://") != 0 )
            {
                strInputUrl = "http://" + strInputUrl;
            }


            // --- Check if URL already exists in database ---
            strKey = DataLayer.Get_Key(strInputUrl);    // Key will remain as an empty string if the input URL is not already in the database


            // - If a key does not already exist for this url...
            if (strKey == "")
            {

                // --- Loop through a max of 3 attempts to create and store a key ---
                while (blnAttemptSuccess == false && intAttemptCount < MAX_ATTEMPTS)
                {
                    intAttemptCount += 1;
                    strKey = GenerateKey();
                    DataTable dtUrlStored = DataLayer.Create_ShortUrl(strKey, strInputUrl);


                    // --- Examining returned table ---
                    if (dtUrlStored != null)
                    {
                        if (dtUrlStored.Columns.Contains("Result") && dtUrlStored.Columns.Contains("Message") && dtUrlStored.Rows.Count >= 1)
                        {
                            if (dtUrlStored.Rows[0]["Result"].ToString() == "1")
                            {
                                blnAttemptSuccess = true;
                            } else
                            {
                                strMessage = dtUrlStored.Rows[0]["Message"].ToString();
                            }
                        }
                    }
                }
            } else
            {
                blnAttemptSuccess = true;
            }


            // --- Coping with failure ---
            if (blnAttemptSuccess == false)
            {
                ErrorHandler eh = new ErrorHandler(strMessage, "");
                eh.Log_Error();
            }


            // --- Returning key ---
            // if no key was found or could be created, an empty string will be returned.
            return strKey;
        }


        /// <summary>
        /// Creates a short key based on the Epoch time in milliseconds.
        /// </summary>
        /// <returns></returns>
        public static string GenerateKey()
        {
            // Getting epoch time in milliseconds
            long lngEpochMS = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Converting to base36
            string strOutput = ConvertToBase36(lngEpochMS);

            // If frequent collisions start happening in the database, add a random element to
            // the key here.

            // Returning result
            return strOutput;
        }


        /// <summary>
        /// Converts the supplied long integer to base 36 (0-9, a-z)
        /// </summary>
        /// <param name="lngInput"></param>
        /// <returns></returns>
        public static string ConvertToBase36(long lngInput)
        {

            // --- Defining objects and variables ---
            string strCharacters = "0123456789abcdefghijklmnopqrstuvwxyz";
            string strOutput = "";
            int intRemainder;


            // --- Looping through increasing orders of magnitude ---
            // Each time the loop executes, the modulo function determines the value
            // that should be written for the current order of magnitude. The order
            // of magnitude is then increased by dividing by 36 (fractional remnant
            // is truncated)
            while (lngInput >= 36)
            {
                intRemainder = (int)(lngInput % 36);
                strOutput = strCharacters[intRemainder] + strOutput;
                lngInput = lngInput / 36;
            }
            strOutput = strCharacters[(int)lngInput] + strOutput;

            // --- Returning result ---
            return strOutput;
        }

    }
}