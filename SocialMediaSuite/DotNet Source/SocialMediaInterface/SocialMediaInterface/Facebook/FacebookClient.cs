using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocialMediaInterface.Facebook;
using System.Xml;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using SocialMediaInterface.Util;
using fb = Facebook;
using System.Dynamic;

namespace SocialMediaInterface
{
    public class FacebookClient
    {

        #region Automation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataFile"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public List<string> GetURL(string queryPath)
        {
            List<string> urls = new List<string>();
            string urlPrifix = string.Empty;
            StringBuilder qry;

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(queryPath);

            foreach (XmlNode item in xDoc.GetElementsByTagName("Facebook"))
            {
                urlPrifix = item.Attributes.GetNamedItem("url").Value;
                foreach (XmlNode qItem in item.ChildNodes)
                {

                    if (null == qItem.Attributes)
                        break;

                    qry = new StringBuilder();
                    foreach (XmlAttribute qParam in qItem.Attributes)
                    {
                        if (qParam.Name.Equals("duration"))
                        {
                            // Current UTC time as timestamp
                            DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                            long currentUTC = (long)(DateTime.UtcNow - UnixEpoch).TotalSeconds;

                            //until param with configuration value
                            qry.Append(string.Format("{0}={1}&", "until", currentUTC));

                            //since param with configuration value
                            long configSec = 0;
                            long.TryParse(qParam.Value, out configSec);
                            long until = currentUTC - configSec;
                            qry.Append(string.Format("{0}={1}&", "since", until));
                        }
                        else
                        {
                            qry.Append(string.Format("{0}={1}&", qParam.Name, qParam.Value));
                        }
                    }
                    urls.Add(string.Format("{0}{1}", urlPrifix, qry.ToString()));
                }
            }

            return urls;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Post> FetchData(string queryPath)
        {
            List<Post> repData = new List<Post>();
            foreach (string url in GetURL(queryPath))
            {
                repData.AddRange(MakeRequest(url));
            }
            return repData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string WriteToTSV(string queryPath)
        {
            StringBuilder userTSV = new StringBuilder();
            StringBuilder msgTSV = new StringBuilder();
            List<Post> responses = this.FetchData(queryPath);

            if (null == responses && responses.Count == 0)
                return null;

            foreach (Post post in responses)
            {

                userTSV.Append(string.Format("{0}\t{1}\t{2}\t{3}\r\n", post.from.id, post.from.name, post.place, post.picture));
                msgTSV.Append(string.Format("{0}\t{1}\t{2}\r\n", post.from.id, post.from.name, post.message));
            }

            this.WriteUserInfo(userTSV.ToString());
            this.WriteTweetInfo(msgTSV.ToString());

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string WriteToDB(string queryPath)
        {
            StringBuilder userInsert = new StringBuilder();
            StringBuilder msgInsert = new StringBuilder();
            List<Post> responses = this.FetchData(queryPath);

            if (null == responses && responses.Count == 0)
                return null;

            foreach (Post post in responses)
            {

                userInsert.Append(string.Format("if (not exists (select * from facebook_users as f where f.user_id = '{0}')) begin insert into facebook_users(user_id, user_name, image_url) values({0},'{1}','{2}') end;",
                    post.from.id == null ? "" : post.from.id.Replace("'", "''"),
                    post.from.name == null ? "" : post.from.name.Replace("'", "''"),
                    post.picture == null ? "" : post.picture.Replace("'", "''")));

                msgInsert.Append(string.Format("if (not exists (select * from facebook_posts as f where f.post_id = '{0}')) begin insert into facebook_posts(post_id, from_user_id,from_user_name,message,picture,link,name,description,source,privacy,type,place,comments,object_id) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}') end;",
                    post.id == null ? "" : post.id.Replace("'", "''"),
                    post.from.id == null ? "" : post.from.id.Replace("'", "''"),
                    post.from.name == null ? "" : post.from.name.Replace("'", "''"),
                    post.message == null ? "" : post.message.Replace("'", "''"),
                    post.picture == null ? "" : post.picture.Replace("'", "''"),
                    post.link == null ? "" : post.link.Replace("'", "''"),
                    post.name == null ? "" : post.name.Replace("'", "''"),
                    post.description == null ? "" : post.description.Replace("'", "''"),
                    post.source == null ? "" : post.source.Replace("'", "''"),
                    post.privacy == null ? "" : post.privacy.Replace("'", "''"),
                    post.type == null ? "" : post.type.Replace("'", "''"),
                    post.place == null ? "" : post.place.Replace("'", "''"),
                    post.comments == null ? "" : post.comments.Replace("'", "''"),
                    post.object_id == null ? "" : post.object_id.Replace("'", "''")));
            }

            DBUtil dbUtil = new DBUtil();
            dbUtil.ExecuteQuery(userInsert.ToString());
            dbUtil.ExecuteQuery(msgInsert.ToString());

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        private void WriteUserInfo(string userInfo)
        {
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }
            using (StreamWriter file = new StreamWriter(@"Data\FacebookUserInfo.tsv",true))
            {
                file.Write(userInfo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tweetInfo"></param>
        private void WriteTweetInfo(string tweetInfo)
        {
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }
            using (StreamWriter file = new StreamWriter(@"Data\FacebookPostInfo.tsv",true))
            {
                file.Write(tweetInfo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public List<Post> MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));

                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(FacebookResponse));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    FacebookResponse jsonResponse = objResponse as FacebookResponse;
                    return jsonResponse.data;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Post>();
            }
        }

        #endregion

        /// <summary>
        /// Retreives user profile from facebook
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public string getUserProfile(string userId, string accessToken)
        {
            try
            {
                fb.FacebookClient fbClient = null;

                if (string.IsNullOrEmpty(accessToken))
                    fbClient = new fb.FacebookClient();
                else
                    fbClient = new fb.FacebookClient(accessToken);

                fb.JsonObject result = (fb.JsonObject)fbClient.Get(userId);

                return result.ToString();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Post message to facebook
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessToken"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string postMessage(string userId, string accessToken, string msg)
        {
            try
            {
                fb.FacebookClient fbClient = null;

                if (string.IsNullOrEmpty(accessToken))
                    fbClient = new fb.FacebookClient();
                else
                    fbClient = new fb.FacebookClient(accessToken);

                dynamic parameters = new ExpandoObject();
                parameters.message = msg;
                dynamic result = fbClient.Post(userId + "/feed", parameters);
                object obj = result;
                return obj.ToString();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// search facebook for the given query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string getPostByQuery(string query)
        {
            try
            {
                fb.FacebookClient fbClient = new fb.FacebookClient();

                dynamic parameters = new ExpandoObject();
                parameters.q = query;
                parameters.type = "POST";

                dynamic result = fbClient.Get(parameters);
                object obj = result;
                return obj.ToString();
            }
            catch
            {
                throw;
            }
        }
    
    }
}
