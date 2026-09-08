using System;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CMNirdesh.Controllers
{
    public class ParichayLoginController : Controller
    {

        public ActionResult Login()
        {
            return View();

        }


        [HttpGet]
        public string LoginButtonClik()
        {
            Debug.WriteLine("RAJEEVVVV");
            string Service = "eNigam";
            string ParichayUrl = "https://parichay.nic.in/";
            long timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string hmac_sign = hmacFunc(timeStamp, Service, ParichayUrl);
            string encrpt_sign = encrptFunc();
            string parichaybaseurl = $"https://parichay.nic.in/pnv1/api/login?sid={Service}&tid={timeStamp}&cs={hmac_sign}&string={encrpt_sign}&lang=en";
            Console.WriteLine(parichaybaseurl);
            //return RedirectToAction("Dashboard", "References");
            return parichaybaseurl;
        }

        [HttpGet]
        public ActionResult LoginButton()
        {
            Debug.WriteLine("RAJEEVVVV");
            string Service = "eNigam";
            string ParichayUrl = "https://parichay.nic.in/";
            long timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string hmac_sign = hmacFunc(timeStamp, Service, ParichayUrl);
            string encrpt_sign = encrptFunc();
            string parichaybaseurl = $"https://parichay.nic.in/pnv1/api/login?sid={Service}&tid={timeStamp}&cs={hmac_sign}&string={encrpt_sign}&lang=en";
            Console.WriteLine(parichaybaseurl);
            return Redirect(parichaybaseurl);
            
        }

        private static string encrptFunc()
        {
            Debug.WriteLine("RAJEEVVVV 53");
            string ENCR_URL = "http://localhost:8085/encryption";
            string requestBody = "{\"AESString\":\"gOg0E6EF762FE888\"}";
            using (var client = new HttpClient())
            {
                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var responseTask = client.PostAsync(ENCR_URL, content).Result;
                if (responseTask.StatusCode == HttpStatusCode.OK)
                {
                    ParichayClass resposedata = JsonConvert.DeserializeObject<ParichayClass>(responseTask.Content.ReadAsStringAsync().Result);

                    if (resposedata?.data?.signature != null)
                    {


                        return resposedata.data.signature;
                    }
                }
                return null;
            }

        }
        private static string hmacFunc(long timeStamp, string SERVICE, string PARICHAY_URL)
        {
            string hmacString = "Parichay" + timeStamp + PARICHAY_URL + "pnv1/api/login" + SERVICE;
            Console.WriteLine(hmacString);

            string requestBody = "{\"HmacString\":\"" + hmacString + "\"}";
            Console.WriteLine(requestBody);

            string HMAC_URL = "http://localhost:8085/hmac";
            using (var client = new HttpClient())
            {
                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var responseTask = client.PostAsync(HMAC_URL, content).Result;
                if (responseTask.StatusCode == HttpStatusCode.OK)
                {
                    ParichayClass resposedata = JsonConvert.DeserializeObject<ParichayClass>(responseTask.Content.ReadAsStringAsync().Result);
                    Debug.WriteLine(resposedata);
                    if (resposedata?.data?.signature != null)
                    {

                        return resposedata.data.signature;
                    }
                }

                else
                {
                    return responseTask.StatusCode.ToString();
                }
            }

            return null;
        }




    }

    public class ParichayClass
    {
        public string status { get; set; }
        public string message { get; set; }
        public dynamic data { get; set; }
    }
    public class Getdata_api
    {
        public string status { get; set; }
        public string message { get; set; }
        public dynamic data { get; set; }

        public dynamic signature { get; set; }
    }

}