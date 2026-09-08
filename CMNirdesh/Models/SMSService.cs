using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;

namespace CMNirdesh.Models
{
    public class SMSService
    {
        private string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];
        private string Username = System.Configuration.ConfigurationManager.AppSettings["Username"];
        private string Password = System.Configuration.ConfigurationManager.AppSettings["Password"];

        public async Task<string> SendSmsAsync(
            string phoneNumber,
            string message1,
            string templateId
        )
        {
            using (HttpClient client = new HttpClient())
            {
                var parameters = new
                {
                    un = Username,
                    pwd = Password,
                    mobilenumbers = phoneNumber,
                    message = message1,
                    templateid = templateId
                };

                var response = await client.GetAsync(ApiUrl + ToQueryString(parameters));
                var SMSLogModel = new SMSLogModel
                {
                    ResponseContent = response.Content.ReadAsStringAsync().Result,
                    ResponseCode = (int)response.StatusCode,
                    TimeStamp = System.DateTime.Now,
                    Reason = response.ReasonPhrase.ToString(),
                    mobilenumbers = phoneNumber
                };
                InsertData(SMSLogModel);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return "Failed to send SMS. Status code: " + response.StatusCode;
                }
            }
        }
        static string GetValueFromResponse(string response, string key)
        {
            string pattern = $@"{key}\s*=\s*([^&]+)";
            Match match = Regex.Match(response, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            return null;
        }

        private string ToQueryString(object obj)
        {
            var keyValuePairs = System.Web.HttpUtility.ParseQueryString(string.Empty);
            foreach (var property in obj.GetType().GetProperties())
            {
                keyValuePairs[property.Name] = Convert.ToString(property.GetValue(obj));
            }
            return "?" + keyValuePairs.ToString();
        }

        public Task<string> SendSingleSMS(SMSMessageList smsparam)
        {
            return Task.Run(
                async () =>
                {
                    string SuccessMessage = "";

                    SMSService _smsService = new SMSService();
                    string result = await _smsService.SendSmsAsync(
                        smsparam.MobileNos,
                        smsparam.SMSText,
                        smsparam.TemplateId
                    );
                    SuccessMessage = "SMS Sent Successfully!";
                    return SuccessMessage;
                }
            );
        }

        public string SingleVariableTemplate(string TemplateID, int ID, string TextVariable)
        {
            string Template = "";
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@TemplateID", Convert.ToString(TemplateID)));
                p.Add(new SqlParameter("@ID", ID));
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "GetTemplateText", p.ToArray());
                string Message = ds.Tables[0].Rows[0][0].ToString();
                Template = Message.Replace("{#var#}", TextVariable);
                return Template;
            }
            catch (Exception)
            {
                return Template;
            }
        }



        public string userregistered(string TemplateID, int ID, string[] TextVariable)
        {
            string Template = "";
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@TemplateID", Convert.ToString(TemplateID)));
                p.Add(new SqlParameter("@ID", ID));
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "GetTemplateText", p.ToArray());
                string Message = ds.Tables[0].Rows[0][0].ToString();
                for (int i = 0; i < TextVariable.Length; i++)
                {
                    int wordIndex = FindWordIndex(Message, "{#var#}");
                    int wordIndexToReplace = wordIndex;
                    string replacementWord = TextVariable[i];
                    Message = ReplaceWordAtIndex(Message, wordIndexToReplace, replacementWord);
                    //Message = Message.Replace("ram", "sham", i);
                }
                Template = Message;
                return Template;
            }
            catch (Exception)
            {
                return Template;
            }
        }

        public string MultipleVariableTemplate(string TemplateID, int ID, string[] TextVariable)
       {
            string Template = "";
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@TemplateID", Convert.ToString(TemplateID)));
                p.Add(new SqlParameter("@ID", ID));
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "GetTemplateText", p.ToArray());
                string Message = ds.Tables[0].Rows[0][0].ToString();
                for (int i = 0; i < TextVariable.Length; i++)
                {
                    int wordIndex = FindWordIndex(Message, "{#var#}");
                    int wordIndexToReplace = wordIndex;
                    string replacementWord = TextVariable[i];
                    Message = ReplaceWordAtIndex(Message, wordIndexToReplace, replacementWord);
                }
                Template = Message;
                return Template;
            }
            catch (Exception)
            {
                return Template;
            }
        }

        public void InsertData(SMSLogModel data)
        {
            string responseContent = data.ResponseContent;
            int responseCode = data.ResponseCode;
            DateTime timeStamp = DateTime.UtcNow;
            string mobileNumbers = data.mobilenumbers;
            string reason = data.Reason;

            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("InsertSMSLog", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ResponseContent", responseContent);
                    command.Parameters.AddWithValue("@ResponseCode", responseCode);
                    command.Parameters.AddWithValue("@TimeStamp", timeStamp);
                    command.Parameters.AddWithValue("@mobilenumbers", mobileNumbers);
                    command.Parameters.AddWithValue("@Reason", reason);

                    try
                    {
                        con.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }

        static int FindWordIndex(string sentence, string word)
        {
            string[] words = sentence.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                if (string.Equals(words[i], word, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        static string ReplaceWordAtIndex(string sentence, int index, string replacementWord)
        {
            string[] words = sentence.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (index >= 0 && index < words.Length)
            {
                words[index] = replacementWord;
                return string.Join(" ", words);
            }
            else
            {
                Console.WriteLine("Index is out of range.");
                return sentence;
            }
        }
    }
}
