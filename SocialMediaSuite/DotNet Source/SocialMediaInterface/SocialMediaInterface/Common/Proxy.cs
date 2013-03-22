using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SocialMediaInterface
{
    /// <summary>
    /// Provides a proxy layer of abstraction
    /// </summary>
    public class Proxy
    {
        /// <summary>
        /// Retrieves User profile from various social media websites
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="channels"></param>
        /// <param name="fbAccessToken"></param>
        /// <returns></returns>
        public List<Profile> getUserProfile(string userId, string channels, string fbAccessToken)
        {
            List<Profile> profiles = new List<Profile>();

            if (channels.ToLower().Contains(Channel.Facebook.ToString().ToLower()))
            {
                try
                {

                    FacebookClient fbClient = new FacebookClient();
                    string fbUser = fbClient.getUserProfile(userId, fbAccessToken);

                    JObject o = JObject.Parse(fbUser);

                    Profile profile = new Profile();

                    profile.id = (string)o.SelectToken("id");
                    profile.name = (string)o.SelectToken("name");
                    profile.firstName = (string)o.SelectToken("first_name");
                    profile.lastName = (string)o.SelectToken("last_name");
                    profile.url = (string)o.SelectToken("link");
                    profile.username = (string)o.SelectToken("username");
                    profile.location = (string)o.SelectToken("location.name");
                    profile.gender = (string)o.SelectToken("gender");
                    profile.locale = (string)o.SelectToken("locale");
                    profile.updatedTime = (string)o.SelectToken("updated_time");
                    profile.email = (string)o.SelectToken("email");
                    profile.channel = Channel.Facebook.ToString();
                    profiles.Add(profile);
                }
                catch
                {
                    throw;
                }
            }

            if (channels.ToLower().Contains(Channel.Twitter.ToString().ToLower()))
            {

                try
                {
                    TwitterClient twClient = new TwitterClient();
                    string twUser = twClient.getUserProfile(userId);

                    JObject o = JObject.Parse(twUser);

                    Profile profile = new Profile();

                    int id = (Int32)o.SelectToken("Id");
                    profile.id = id.ToString();
                    profile.name = (string)o.SelectToken("Name");
                    profile.url = (string)o.SelectToken("Url");
                    profile.username = (string)o.SelectToken("ScreenName");
                    profile.location = (string)o.SelectToken("Location");
                    profile.profileImage = (string)o.SelectToken("ProfileImageUrl");
                    profile.channel = Channel.Twitter.ToString();
                    profiles.Add(profile);
                }
                catch
                {
                    throw;
                }
            }
            if (channels.ToLower().Contains(Channel.GooglePlus.ToString().ToLower()))
            {

                try
                {
                    string APIKey = "AIzaSyDfNJfYRM68bj_jRKnD1utAqBaVTrV2WSk";

                    GoogleClient gpClient = new GoogleClient();

                    string gpUsers = gpClient.searchUsers(userId, APIKey);
                    JObject joUsers = JObject.Parse(gpUsers);

                    var userList = joUsers["items"].ToList();

                    foreach (var user in userList)
                    {
                        if (user["id"] != null)
                        {
                            try
                            {
                                string gpUser = gpClient.getUserProfile((string)user["id"], APIKey);

                                JObject o = JObject.Parse(gpUser);

                                Profile profile = new Profile();

                                profile.id = (string)o.SelectToken("id");
                                profile.name = (string)o.SelectToken("displayName");
                                profile.firstName = (string)o.SelectToken("name.givenName");
                                profile.lastName = (string)o.SelectToken("name.familyName");
                                profile.gender = (string)o.SelectToken("gender");
                                profile.url = (string)o.SelectToken("url");
                                profile.profileImage = (string)o.SelectToken("image.url");
                                profile.channel = Channel.GooglePlus.ToString();

                                var placeList = o.SelectToken("placesLived") == null ? new List<JToken>() : o.SelectToken("placesLived").ToList();

                                foreach (var place in placeList)
                                {
                                    if (place.Last.ToString().Contains("primary"))
                                        profile.location = place.First.First.ToString();
                                }

                                profiles.Add(profile);
                            }
                            catch
                            {
                                throw;
                            }
                        }
                    }                    
                }
                catch
                {
                    throw;
                }
            }

            return profiles;
        }

        /// <summary>
        /// Retrieves search messages from various social media sites
        /// </summary>
        /// <param name="query"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        public List<Message> searchMessage(string query, string channels)
        {
            List<Message> messages = new List<Message>();

            if (channels.ToLower().Contains(Channel.Facebook.ToString().ToLower()))
            {
                try
                {
                    FacebookClient fbClient = new FacebookClient();
                    string fbmsgs = fbClient.getPostByQuery(query);

                    JObject o = JObject.Parse(fbmsgs);

                    var msgList = o[""]["data"].ToList();

                    foreach (var msg in msgList)
                    {
                        JObject m = JObject.Parse(msg.ToString());

                        Message message = new Message();

                        message.id = (string)m.SelectToken("id");
                        message.username = (string)m.SelectToken("from.name");
                        message.message = (string)m.SelectToken("message");
                        message.type = (string)m.SelectToken("type");

                        DateTime createdTime = (DateTime)m.SelectToken("created_time");
                        DateTime updatedTime = (DateTime)m.SelectToken("updated_time");

                        message.createdTime = createdTime.ToString();
                        message.updatedTime = updatedTime.ToString();
                        message.channel = Channel.Facebook.ToString();

                        messages.Add(message);
                    }
                }
                catch
                {
                    throw;
                }
            }

            if (channels.ToLower().Contains(Channel.Twitter.ToString().ToLower()))
            {
                try
                {
                    TwitterClient twClient = new TwitterClient();
                    string twmsgs = twClient.getTweetsByQuery(query);

                    JObject o = JObject.Parse(twmsgs);

                    var msgList = o["results"].ToList();

                    foreach (var msg in msgList)
                    {
                        JObject m = JObject.Parse(msg.ToString());

                        Message message = new Message();

                        Int64 id = (Int64)m.SelectToken("Id");
                        message.id = id.ToString();
                        message.username = (string)m.SelectToken("Author.ScreenName");
                        message.message = (string)m.SelectToken("Text");

                        DateTime createdTime = (DateTime)m.SelectToken("created_at");

                        message.createdTime = createdTime.ToString();
                        message.channel = Channel.Twitter.ToString();

                        messages.Add(message);
                    }
                }
                catch
                {
                    throw;
                }
            }
            return messages;
        }

        /// <summary>
        /// Post messages to various social media sites
        /// </summary>
        /// <param name="message"></param>
        /// <param name="channels"></param>
        /// <param name="fbUserId"></param>
        /// <param name="fbToken"></param>
        /// <param name="twToken"></param>
        /// <param name="twTokenSecret"></param>
        /// <returns></returns>
        public List<PostResponse> postMessage(string message, string channels, string fbUserId, string fbToken, string twToken, string twTokenSecret)
        {
            List<PostResponse> response = new List<PostResponse>();

            if (channels.ToLower().Contains(Channel.Facebook.ToString().ToLower()))
            {
                try
                {
                    FacebookClient fbClient = new FacebookClient();
                    string fbPostId = fbClient.postMessage(fbUserId, fbToken, message);

                    PostResponse pr = new PostResponse();
                    pr.messageId = fbPostId;
                    pr.channel = Channel.Facebook.ToString();

                    response.Add(pr);
                }
                catch
                {
                    throw;
                }
            }

            if (channels.ToLower().Contains(Channel.Twitter.ToString().ToLower()))
            {
                try
                {
                    TwitterClient twClient = new TwitterClient();
                    string twId = twClient.postTweet(message, twToken, twTokenSecret);

                    PostResponse pr = new PostResponse();
                    pr.messageId = twId;
                    pr.channel = Channel.Twitter.ToString();

                    response.Add(pr);
                }
                catch
                {
                    throw;
                }
            }
            return response;
        }
    }
}
