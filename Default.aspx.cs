using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UrlShortener
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                // --- Clearing any previous response ---
                pnlResponse.Controls.Clear();

                // --- Retrieving ID of postback control ---
                string strPostbackControlName = Request.Params["__EVENTTARGET"];

                // --- If the postback control was the textbox, attempt to shorten the URL ---
                if (strPostbackControlName == txtInputUrl.UniqueID)
                {
                    string strInputUrl = txtInputUrl.Text;
                    string strOutputKey = App_Code.Functions.CreateShortUrl(strInputUrl);
                    if (strOutputKey != "")
                    {
                        // --- URL shorten succeeded. Show results ---
                        // label
                        Label lblShortUrl = new Label();
                        lblShortUrl.Text = "Short URL: ";
                        pnlResponse.Controls.Add(lblShortUrl);

                        // link with short url, to open in new tab
                        string strOutputUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/" + strOutputKey;
                        HyperLink hlShortUrl = new HyperLink();
                        hlShortUrl.Text = strOutputUrl;
                        hlShortUrl.NavigateUrl = strOutputUrl;
                        hlShortUrl.Target = "_blank";
                        pnlResponse.Controls.Add(hlShortUrl);

                    }
                    else
                    {
                        // --- URL shorten failed. Show failure message ---
                        Label lblUrlShortenFailed = new Label();
                        lblUrlShortenFailed.ID = "lblUrlShortenFailed";
                        lblUrlShortenFailed.Text = "The supplied URL could not be shortened at this time. Please try again later.";
                        pnlResponse.Controls.Add(lblUrlShortenFailed);
                    }
                }
            }
            
        }


    }
}