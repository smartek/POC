using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SocialMediaInterface.OAuth;
using OAuth4Client.OAuth;
using SocialMediaInterface;
using System.Configuration;

namespace ConsumerWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        public OAuthContext OContext = null;
        
        /// <summary>
        /// Page load event with oauth token handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string channel = string.Empty;
                OContext = OAuthContext.Current;

                if (!IsPostBack && OContext != null)
                {
                    //End authentication and register tokens.
                    OContext.EndAuthenticationAndRegisterTokens();
                    //Retrieve token by verification
                    OContext.GetAccessToken();

                    //Restore tokens from session 
                    if (Session["fbAccessToken"] != null)
                        fbAccessToken.Text = Session["fbAccessToken"].ToString();
                    if (Session["twToken"] != null)
                        twToken.Text = Session["twToken"].ToString();
                    if (Session["twTknSecret"] != null)
                        twTknSecret.Text = Session["twTknSecret"].ToString();
                    if (Session["twVerifier"] != null)
                        twVerifier.Text = Session["twVerifier"].ToString();
                    if (Session["gpToken"] != null)
                        gpToken.Text = Session["gpToken"].ToString();
                    if (Session["gpTknSecret"] != null)
                        gpTknSecret.Text = Session["gpTknSecret"].ToString();
                    if (Session["gpVerifier"] != null)
                        gpVerifier.Text = Session["gpVerifier"].ToString();

                    if (Session["channel"] != null)
                        channel = Session["channel"].ToString();

                    if (channel == Channel.Facebook.ToString())
                    {
                        fbAccessToken.Text = OContext.AccessToken;
                        Session["fbAccessToken"] = OContext.AccessToken;
                    }
                    else if (channel == Channel.Twitter.ToString())
                    {
                        twToken.Text = OContext.AccessToken;
                        Session["twToken"] = OContext.AccessToken;

                        twTknSecret.Text = OContext.AccessTokenSecret;
                        Session["twTknSecret"] = OContext.AccessTokenSecret;

                        twVerifier.Text = OContext.Verifier;
                        Session["twVerifier"] = OContext.Verifier;
                    }
                    else if (channel == Channel.GooglePlus.ToString())
                    {
                        gpToken.Text = OContext.AccessToken;
                        Session["gpToken"] = OContext.AccessToken;

                        gpTknSecret.Text = OContext.AccessTokenSecret;
                        Session["gpTknSecret"] = OContext.AccessTokenSecret;

                        gpVerifier.Text = OContext.Verifier;
                        Session["gpVerifier"] = OContext.Verifier;
                    }

                }
            }
            catch (OAuthException ex)
            {
                txtFBResponse.Text = "OAuth Exception occurred " + ex.InnerException.Message;
            }
        }

        /// <summary>
        /// Facebook authentication
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fb_OnClick(object sender, EventArgs e)
        {
            try
            {
                Session["channel"] = Channel.Facebook.ToString();

                //Retrieve values from settings file
                var facebookConsumerKeySettings = ConfigurationManager.AppSettings["FacebookConsumerKey"];
                var facebookConsumerSecretSettings = ConfigurationManager.AppSettings["FacebookConsumerSecret"];
                var facebookVerifierUrlSettings = ConfigurationManager.AppSettings["FacebookVerifierUrl"];
                var facebookRequestAccessTokenUrlSettings = ConfigurationManager.AppSettings["FacebookRequestAccessTokenUrl"];
                var facebookRequestProfileUrlSettings = ConfigurationManager.AppSettings["FacebookRequestProfileUrl"];
                var facebookScopeSettings = ConfigurationManager.AppSettings["FacebookScope"];

                string facebookConsumerKey = string.Empty;
                string facebookConsumerSecret = string.Empty;
                string facebookVerifierUrl = string.Empty;
                string facebookRequestAccessTokenUrl = string.Empty;
                string facebookRequestProfileUrl = string.Empty;
                string facebookScope = string.Empty;

                if (facebookConsumerKeySettings != null)
                    facebookConsumerKey = facebookConsumerKeySettings.ToString();
                if (facebookConsumerSecretSettings != null)
                    facebookConsumerSecret = facebookConsumerSecretSettings.ToString();
                if (facebookVerifierUrlSettings != null)
                    facebookVerifierUrl = facebookVerifierUrlSettings.ToString();
                if (facebookRequestAccessTokenUrlSettings != null)
                    facebookRequestAccessTokenUrl = facebookRequestAccessTokenUrlSettings.ToString();
                if (facebookRequestProfileUrlSettings != null)
                    facebookRequestProfileUrl = facebookRequestProfileUrlSettings.ToString();
                if (facebookScopeSettings != null)
                    facebookScope = facebookScopeSettings.ToString();

                //Create Credential object with consumer specific credentials
                Credentials credential = new Credentials();
                credential.ConsumerKey = facebookConsumerKey;
                credential.ConsumerSecret = facebookConsumerSecret;
                credential.VerifierUrl = facebookVerifierUrl;
                credential.RequestAccessTokenUrl = facebookRequestAccessTokenUrl;
                credential.RequestProfileUrl = facebookRequestProfileUrl;
                credential.Scope = facebookScope;

                var tClient = new OAuthFacebookClient();
                tClient.BeginAuthentication(credential);
            }
            catch (Exception ex)
            {
                txtFBResponse.Text = "Error occurred while accessing facebook accesstoken! " + ex.Message;
            }
        }

        /// <summary>
        /// Facebook getuser profile event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGetUserProfile_Click(object sender, EventArgs e)
        {
            try
            {
                FacebookClient fbClient = new FacebookClient();
                txtFBResponse.Text = fbClient.getUserProfile(txtFBRequest.Text, fbAccessToken.Text);
            }
            catch (Exception ex)
            {
                txtFBResponse.Text = "Error occurred while accessing facebook user profile! " + ex.Message;
            }
        }

        protected void btnPostMsg_Click(object sender, EventArgs e)
        {
            try
            {
                FacebookClient fbClient = new FacebookClient();
                txtFBResponse.Text = fbClient.postMessage("me", fbAccessToken.Text, txtFBRequest.Text);
            }
            catch (Exception ex)
            {
                txtFBResponse.Text = "Error occurred while posting facebook post! " + ex.Message;
            }
        }

        /// <summary>
        /// Twitter authentication event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTwitter_OnClick(object sender, EventArgs e)
        {
            try
            {
                Session["channel"] = Channel.Twitter.ToString();

                //Retrieve values from settings file
                var twitterConsumerKeySettings = ConfigurationManager.AppSettings["TwitterConsumerKey"];
                var twitterConsumerSecretSettings = ConfigurationManager.AppSettings["TwitterConsumerSecret"];
                var twitterRequestTokenUrlSettings = ConfigurationManager.AppSettings["TwitterRequestTokenUrl"];
                var twitterVerifierUrlSettings = ConfigurationManager.AppSettings["TwitterVerifierUrl"];
                var twitterRequestAccessTokenUrlSettings = ConfigurationManager.AppSettings["TwitterRequestAccessTokenUrl"];
                var twitterRequestProfileUrlSettings = ConfigurationManager.AppSettings["TwitterRequestProfileUrl"];
                var twitterScopeSettings = ConfigurationManager.AppSettings["TwitterScope"];

                string twitterConsumerKey = string.Empty;
                string twitterConsumerSecret = string.Empty;
                string twitterRequestTokenUrl = string.Empty;
                string twitterVerifierUrl = string.Empty;
                string twitterRequestAccessTokenUrl = string.Empty;
                string twitterRequestProfileUrl = string.Empty;
                string twitterScope = string.Empty;

                if (twitterConsumerKeySettings != null)
                    twitterConsumerKey = twitterConsumerKeySettings.ToString();
                if (twitterConsumerSecretSettings != null)
                    twitterConsumerSecret = twitterConsumerSecretSettings.ToString();
                if (twitterRequestTokenUrlSettings != null)
                    twitterRequestTokenUrl = twitterRequestTokenUrlSettings.ToString();
                if (twitterVerifierUrlSettings != null)
                    twitterVerifierUrl = twitterVerifierUrlSettings.ToString();
                if (twitterRequestAccessTokenUrlSettings != null)
                    twitterRequestAccessTokenUrl = twitterRequestAccessTokenUrlSettings.ToString();
                if (twitterRequestProfileUrlSettings != null)
                    twitterRequestProfileUrl = twitterRequestProfileUrlSettings.ToString();
                if (twitterScopeSettings != null)
                    twitterScope = twitterScopeSettings.ToString();
                
                //Create Credential object with consumer specific credentials
                Credentials credential = new Credentials();
                credential.ConsumerKey = twitterConsumerKey;
                credential.ConsumerSecret = twitterConsumerSecret;
                credential.RequestTokenUrl = twitterRequestTokenUrl;
                credential.VerifierUrl = twitterVerifierUrl;
                credential.RequestAccessTokenUrl = twitterRequestAccessTokenUrl;
                credential.RequestProfileUrl = twitterRequestProfileUrl;
                credential.Scope = twitterScope;

                var tClient = new OAuthTwitterClient();
                tClient.BeginAuthentication(credential);
            }
            catch (Exception ex)
            {
                txtTWResponse.Text = "Error occurred while accessing twitter accesstoken! " + ex.Message;
            }
        }

        /// <summary>
        /// Twitter user profile event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTwUserProfile_Click(object sender, EventArgs e)
        {
            try
            {
                TwitterClient twClient = new TwitterClient();
                txtTWResponse.Text = twClient.getUserProfile(txtTWRequest.Text);
            }
            catch (Exception ex)
            {
                txtTWResponse.Text = "Error occurred while accessing twitter user profile! " + ex.Message;
            }
        }

        /// <summary>
        /// Get tweets event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTWGetTweets_Click(object sender, EventArgs e)
        {
            try
            {
                TwitterClient twClient = new TwitterClient();
                txtTWResponse.Text = twClient.getTweetsByQuery(txtTWRequest.Text);
            }
            catch (Exception ex)
            {
                txtTWResponse.Text = "Error occurred while accessing twitter tweets! " + ex.Message;
            }
        }

        /// <summary>
        /// Post tweet event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btntwPost_Click(object sender, EventArgs e)
        {
            try
            {
                TwitterClient tClient = new TwitterClient();
                txtTWResponse.Text = tClient.postTweet(txtTWRequest.Text, twToken.Text, twTknSecret.Text);
            }
            catch (Exception ex)
            {
                txtTWResponse.Text = "Error occurred while posting twitter post! " + ex.Message;
            }
        }

        /// <summary>
        /// Post facebook event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFBGetPost_Click(object sender, EventArgs e)
        {
            try
            {
                FacebookClient fbClient = new FacebookClient();
                txtFBResponse.Text = fbClient.getPostByQuery(txtFBRequest.Text);
            }
            catch (Exception ex)
            {
                txtTWResponse.Text = "Error occurred while retrieving facebook posts! " + ex.Message;
            }
        }

        /// <summary>
        /// Google plus authentication event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GP_OnClick(object sender, EventArgs e)
        {
            try
            {
                Session["channel"] = Channel.GooglePlus.ToString();

                //Retrieve values from settings file
                var googleConsumerKeySettings = ConfigurationManager.AppSettings["GoogleConsumerKey"];
                var googleConsumerSecretSettings = ConfigurationManager.AppSettings["GoogleConsumerSecret"];
                var googleRequestTokenUrlSettings = ConfigurationManager.AppSettings["GoogleRequestTokenUrl"];
                var googleVerifierUrlSettings = ConfigurationManager.AppSettings["GoogleVerifierUrl"];
                var googleRequestAccessTokenUrlSettings = ConfigurationManager.AppSettings["GoogleRequestAccessTokenUrl"];
                var googleRequestProfileUrlSettings = ConfigurationManager.AppSettings["GoogleRequestProfileUrl"];
                var googleScopeSettings = ConfigurationManager.AppSettings["GoogleScope"];

                string googleConsumerKey = string.Empty;
                string googleConsumerSecret = string.Empty;
                string googleRequestTokenUrl = string.Empty;
                string googleVerifierUrl = string.Empty;
                string googleRequestAccessTokenUrl = string.Empty;
                string googleRequestProfileUrl = string.Empty;
                string googleScope = string.Empty;

                if (googleConsumerKeySettings != null)
                    googleConsumerKey = googleConsumerKeySettings.ToString();
                if (googleConsumerSecretSettings != null)
                    googleConsumerSecret = googleConsumerSecretSettings.ToString();
                if (googleRequestTokenUrlSettings != null)
                    googleRequestTokenUrl = googleRequestTokenUrlSettings.ToString();
                if (googleVerifierUrlSettings != null)
                    googleVerifierUrl = googleVerifierUrlSettings.ToString();
                if (googleRequestAccessTokenUrlSettings != null)
                    googleRequestAccessTokenUrl = googleRequestAccessTokenUrlSettings.ToString();
                if (googleRequestProfileUrlSettings != null)
                    googleRequestProfileUrl = googleRequestProfileUrlSettings.ToString();
                if (googleScopeSettings != null)
                    googleScope = googleScopeSettings.ToString();
                

                //Create Credential object with consumer specific credentials
                Credentials credential = new Credentials();

                credential.ConsumerKey = googleConsumerKey;
                credential.ConsumerSecret = googleConsumerSecret;
                credential.RequestTokenUrl = googleRequestTokenUrl;
                credential.VerifierUrl = googleVerifierUrl;
                credential.RequestAccessTokenUrl = googleRequestAccessTokenUrl;
                credential.RequestProfileUrl = googleRequestProfileUrl;
                credential.Scope = googleScope;

                var tClient = new OAuthGoogleClient();
                tClient.BeginAuthentication(credential);
            }
            catch (Exception ex)
            {
                txtGPResponse.Text = "Error occurred while retrieving google token! " + ex.Message;
            }
        }

        /// <summary>
        /// Google Plus user profile event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGPUserProfile_Click(object sender, EventArgs e)
        {
            try
            {
                //Retrieve value from settings file
                var googleAPIKeySettings = ConfigurationManager.AppSettings["GoogleAPIKey"];
                string googleAPIKey = string.Empty;

                if (googleAPIKeySettings != null)
                    googleAPIKey = googleAPIKeySettings.ToString();

                GoogleClient gpClient = new GoogleClient();
                txtGPResponse.Text = gpClient.searchUsers(txtGPRequest.Text,googleAPIKey);
            }
            catch (Exception ex)
            {
                txtGPResponse.Text = "Error occurred while retrieving google+ user profile! " + ex.Message;
            }
        }
    }
}
