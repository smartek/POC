
namespace SocialMediaInterface.OAuth
{
    /// <summary>
    /// class holdes credentials and endpoints of different social sites
    /// </summary>
    public class Credentials
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string RequestTokenUrl { get; set; }
        public string VerifierUrl { get; set; }
        public string RequestAccessTokenUrl { get; set; }
        public string RequestProfileUrl { get; set; }
        public string Scope { get; set; }
    }
}