using System.Web;
using SocialMediaInterface.OAuth;


namespace OAuth4Client.OAuth
{
    public class OAuthFacebookClient
    {
        /// <summary>
        /// Oauth begin authentication method for facebook
        /// </summary>
        /// <param name="credential"></param>
        public void BeginAuthentication(Credentials credential)
        {
            var oContext = new OAuthContext
            {
                ConsumerKey = credential.ConsumerKey,
                ConsumerSecret = credential.ConsumerSecret,
                VerifierUrl = credential.VerifierUrl,
                RequestAccessTokenUrl = credential.RequestAccessTokenUrl,
                RequestProfileUrl = credential.RequestProfileUrl,
                Scope = credential.Scope,
                OAuthVersion = OAuthVersion.V2,
                SocialSiteName = "Facebook"
            };

            //In version 2.0 no need to get request token
            oContext.ObtainVerifier();
        }
    }

    public class OAuthTwitterClient
    {
        /// <summary>
        /// Oauth begin authentication method for twitter
        /// </summary>
        /// <param name="credential"></param>
        public void BeginAuthentication(Credentials credential)
        {
            var oContext = new OAuthContext
            {
                ConsumerKey = credential.ConsumerKey,
                ConsumerSecret = credential.ConsumerSecret,
                RequestTokenUrl = credential.RequestTokenUrl,
                VerifierUrl = credential.VerifierUrl,
                RequestAccessTokenUrl = credential.RequestAccessTokenUrl,
                RequestProfileUrl = credential.RequestProfileUrl,
                Realm = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.DnsSafeHost + HttpContext.Current.Request.ApplicationPath,
                OAuthVersion = OAuthVersion.V1,
                SocialSiteName = "Twitter"
            };

            oContext.GetRequestToken();
            oContext.ObtainVerifier();
        }
    }

    public class OAuthGoogleClient
    {
        /// <summary>
        /// Oauth begin authentication method for google
        /// </summary>
        /// <param name="credential"></param>
        public void BeginAuthentication(Credentials credential)
        {
            var oContext = new OAuthContext
            {
                ConsumerKey = credential.ConsumerKey,
                ConsumerSecret = credential.ConsumerSecret,
                RequestTokenUrl = credential.RequestTokenUrl,
                VerifierUrl = credential.VerifierUrl,
                RequestAccessTokenUrl = credential.RequestAccessTokenUrl,
                RequestProfileUrl = credential.RequestProfileUrl,
                Realm = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.DnsSafeHost + HttpContext.Current.Request.ApplicationPath,
                OAuthVersion = OAuthVersion.V1,
                SocialSiteName = "Google"
            };

            oContext.GetRequestToken();
            oContext.ObtainVerifier();
        }
    }
}