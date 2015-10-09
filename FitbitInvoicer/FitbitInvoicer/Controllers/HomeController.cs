using FitbitInvoicer.Models;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FitbitInvoicer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAuthorization()
        {
            var url = string.Empty;
            var clientid = HttpUtility.UrlEncode(ConfigData.GetClientId());
            var redirectURI = HttpUtility.UrlEncode(ConfigData.GetAuthorizationRedirect());
            var scope = HttpUtility.UrlEncode("activity nutrition heartrate location nutrition profile settings sleep social weight");
            var baseURL = ConfigData.GetAuthorizationUrl();
            url = String.Format("{0}?response_type=code&client_id={1}&redirect_uri={2}&scope={3}", baseURL, clientid, redirectURI, scope);
            return Redirect(url); 
        }

        public ActionResult AuthorizationCallback()
        {
            var data = Request.QueryString;
            var code = data["code"];

            var request = WebRequest.Create(ConfigData.GetTokenUrl());
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var security = ConfigData.GetClientId() + ":" + ConfigData.GetClientSecret();
            var base64 = "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(security));
            request.Headers.Add("Authorization", base64);
            var clientId = "229VY5";
            var grantType = "authorization_code";
            var redirectURI = ConfigData.GetAuthorizationRedirect();

            string postData = String.Format("client_id={0}&grant_type={1}&redirect_uri={2}&code={3}", clientId, grantType, redirectURI, code);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            
            var accessTokenResponse = new JavaScriptSerializer().Deserialize<AccessTokenResponse>(responseFromServer);

            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month+1, 1);
            var url = String.Format("https://api.fitbit.com/1/user/{0}/activities/steps/date/{1}/1m.json", accessTokenResponse.user_id, startOfMonth.ToString("yyyy-MM-dd"));
            request = WebRequest.Create(url);
            base64 = "Bearer " + accessTokenResponse.access_token;
            request.Headers.Add("Authorization", base64);
            request.Method = "GET";
            response = request.GetResponse();            
            dataStream = response.GetResponseStream();
            reader = new StreamReader(dataStream);
            responseFromServer = reader.ReadToEnd();
            reader.Close();
            response.Close();

            var stepDataResponse = new JavaScriptSerializer().Deserialize<dynamic>(responseFromServer);
            int totalSteps = 0;
            for (int i = 0; i < stepDataResponse["activities-steps"].Length; i++)
            {
                var stepsPerDay = stepDataResponse["activities-steps"][i]["value"];
                totalSteps += Convert.ToInt32(stepsPerDay);
            }

            double invoiceAmount = totalSteps * 0.01;

            ViewBag.StepsTaken = totalSteps;
            ViewBag.AmountInvoiced = invoiceAmount;

            return View();
        }

    }
}