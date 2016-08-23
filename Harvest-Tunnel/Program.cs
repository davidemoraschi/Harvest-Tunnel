using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Tests;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DevDefined.OAuth.Storage;

namespace Harvest_Tunnel
{
    class Program
    {
        static void Main(string[] args)
        {
            //FirstTimeOnly_RequestToken();

            string AccessToken = ConfigurationManager.AppSettings["AccessToken"];
            string TokenSecret = ConfigurationManager.AppSettings["TokenSecret"];

            string requestUrl = "https://www.odesk.com/api/auth/v1/oauth/token/request";
            string userAuthorizeUrl = "https://www.odesk.com/services/api/auth";
            string accessUrl = "https://www.odesk.com/api/auth/v1/oauth/token/access";
            string callBackUrl = "http://harvestunnel.azurewebsites.net/";

            var consumerContext = new OAuthConsumerContext
            {
                ConsumerKey = "1c21671c3c3cb788b44025c1caa5f611",
                SignatureMethod = SignatureMethod.HmacSha1,
                ConsumerSecret = "c6e5f24acaf57ef3"                
            };
            
            var session = new OAuthSession(consumerContext, requestUrl, userAuthorizeUrl, accessUrl, callBackUrl);
            IToken accessToken = session.GetRequestToken("POST");
            accessToken.Token = AccessToken;
            accessToken.TokenSecret = TokenSecret;
            ///api/hr/v2/users/{user_reference}.{format}
            //string responseText = session.Request(accessToken).Get().ForUrl("https://www.odesk.com/api/auth/v1/info.json").ToString();
            string responseText = session.Request(accessToken).Get().ForUrl("https://www.odesk.com/api/hr/v2/users/me.json").ToString();
            //Console.WriteLine(responseText);

            ///api/hr/v2/teams.{format}
            /// /api/hr/v2/engagements.{format}
            //responseText = session.Request(accessToken).Get().ForUrl("https://www.odesk.com/api/hr/v2/companies.json").ToString();
            //responseText = session.Request(accessToken).Get().ForUrl("https://www.odesk.com/api/hr/v2/engagements.json").ToString();
            responseText = session.Request(accessToken).Get().ForUrl("https://www.odesk.com/api/hr/v2/teams.json").ToString();
            //Console.WriteLine(responseText);

            //responseText = session.Request(accessToken).Get().ForUrl("https://www.odesk.com/api/team/v1/snapshots/EricSouare/davidem/20150420T120000Z.json").ToString();
            //\/api\/team\/v1\/workdia ries\/EricSouare\/davidem\/20150420.json
            responseText = session.Request(accessToken).Get().ForUrl("https://www.odesk.com/api/team/v1/workdiaries/EricSouare/davidem/20150406.json").ToString();
            Console.WriteLine(responseText);
        }

        private static void FirstTimeOnly_RequestToken(/*string AccessToken*/)
        {
            string AccessToken = ConfigurationManager.AppSettings["AccessToken"];
            string TokenSecret = ConfigurationManager.AppSettings["TokenSecret"];
            string requestUrl = "https://www.odesk.com/api/auth/v1/oauth/token/request";
            string userAuthorizeUrl = "https://www.odesk.com/services/api/auth";
            string accessUrl = "https://www.odesk.com/api/auth/v1/oauth/token/access";
            string callBackUrl = "http://harvestunnel.azurewebsites.net/";

            var consumerContext = new OAuthConsumerContext
            {
                ConsumerKey = "1c21671c3c3cb788b44025c1caa5f611",
                SignatureMethod = SignatureMethod.HmacSha1,
                //Key = certificate.PrivateKey,
                ConsumerSecret = "c6e5f24acaf57ef3"
            };
            var session = new OAuthSession(consumerContext, requestUrl, userAuthorizeUrl, accessUrl, callBackUrl);
            IToken requestToken = session.GetRequestToken("POST");
            string authorizationLink = session.GetUserAuthorizationUrlForToken(requestToken, callBackUrl);

            IToken accessToken = session.ExchangeRequestTokenForAccessToken(requestToken, "POST", "3ac29df12eb02fabd279b401c18ebc29");

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["AccessToken"].Value = AccessToken;
            config.Save(ConfigurationSaveMode.Minimal);
        }
    }
}
