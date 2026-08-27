using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMNirdesh.Models
{
    public class OutsourceSmsService
    {
        public async Task<string> SendAsync(
            string mobile,
            string finalMessage,
            string token)
        {
            var payload = new
            {
                RequestInfo = new
                {
                    apiId = ConfigurationManager.AppSettings["SmsApiId"],
                    ver = ConfigurationManager.AppSettings["SmsApiVersion"],
                    action = ConfigurationManager.AppSettings["SmsAction"],
                    did = ConfigurationManager.AppSettings["SmsDid"],
                    msgId = ConfigurationManager.AppSettings["SmsMsgId"],
                    authToken = token
                },
                sms = new
                {
                    mobileNumber = mobile,
                    message = finalMessage,
                    category = ConfigurationManager.AppSettings["SmsCategory"]
                }
            };

            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(
                    ConfigurationManager.AppSettings["SmsApiUrl"],
                    content
                );

                return await response.Content.ReadAsStringAsync();
            }
        }
    }

    public class SmsOrchestrationService
    {
        private readonly OAuthService _authService;
        private readonly TemplateService _templateService;
        private readonly OutsourceSmsService _smsService;

        public SmsOrchestrationService()
        {
            _authService = new OAuthService();
            _templateService = new TemplateService();
            _smsService = new OutsourceSmsService();
        }

        /// <summary>
        /// Sends SMS using existing template master + GetTemplateText SP
        /// </summary>
        public async Task SendAgendaSmsAsync(List<string> mobiles,string templateCode,string[] variables)
        {
            if (mobiles == null || mobiles.Count == 0)
                return;

            // 1️⃣ Resolve TemplateID from existing template table
            string templateId = GetTemplateIdByCode(templateCode);

            if (string.IsNullOrWhiteSpace(templateId))
                throw new Exception($"SMS Template not found for code: {templateCode}");

            // 2️⃣ Get OAuth token (cached)
            string token = await _authService.GetTokenAsync();

            // 3️⃣ Send SMS one by one
            foreach (string mobile in mobiles)
            {
                if (string.IsNullOrWhiteSpace(mobile))
                    continue;

                string message =
                    _templateService.BuildMessage(templateId, variables);

                await _smsService.SendAsync(mobile, message, token);
            }
        }

        /// <summary>
        /// Fetches TemplateID using existing Template Master table
        /// </summary>
        private string GetTemplateIdByCode(string templateCode)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    @"SELECT TEMPLATEID 
                  FROM SMSTemplateMaster
                  WHERE TEMPLATENAME = @TemplateCode",
                    con);

                cmd.Parameters.AddWithValue("@TemplateCode", templateCode);

                object result = cmd.ExecuteScalar();
                return result == null ? string.Empty : result.ToString();
            }
        }

        public async Task<string> SendForgetPasswordOtp(string mobileNos, string otp, string username)
        {
            if (string.IsNullOrWhiteSpace(mobileNos) || string.IsNullOrWhiteSpace(otp))
                return "Invalid input";

            // OTP template code (exists in your template table)
            // From your data: MC_HOUSE_APPLICATION_EN / PB
            string templateCode = "MC_HOUSE_APPLICATION_ENN";

            OAuthService authService = new OAuthService();
            TemplateService templateService = new TemplateService();
            OutsourceSmsService smsService = new OutsourceSmsService();

            // 1️⃣ Resolve TemplateID from existing template master
            string templateId = GetTemplateIdByCode(templateCode);

            if (string.IsNullOrEmpty(templateId))
                throw new Exception("OTP SMS template not found");

            // 2️⃣ Build final SMS message (DLT format)
            string finalMessage = templateService.BuildMessage(
                templateId,
                new[] { otp, otp }
            );

            // 3️⃣ Get OAuth token (cached)
            string token = await authService.GetTokenAsync();

            // 4️⃣ Send SMS to all numbers
            foreach (string mobile in mobileNos.Split(','))
            {
                if (string.IsNullOrWhiteSpace(mobile))
                    continue;

                await smsService.SendAsync(mobile.Trim(), finalMessage, token);
            }

            return "OTP SMS sent successfully";
        }

        internal async Task<string> SendForgetPasswordOtp(string mobno, string randomOTP, object username)
        {
            throw new NotImplementedException();
        }
    }

}