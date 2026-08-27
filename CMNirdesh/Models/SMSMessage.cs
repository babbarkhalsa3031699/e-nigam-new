using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CMNirdesh.Models
{
    public partial class SMSMessage
    {
        public int MessageID;
        public string MobileNo;
        public string SMSText;
        public bool? isLocale;
        public int QueueStateID;
        public string TemplateId;

        public Task<string> SendForgetPasswordOtp(string MobileNos, string Otp)
        {

            return Task.Run(() =>
            {

                SMSService _smsService = new SMSService();
                SMSMessageList list = new SMSMessageList();
                List<string> numbersList = MobileNos.Split(',').ToList();

                //Parameters are needed
                list.TemplateId = "1407170659295018696";
                list.ID = 10;
                list.SMSText = _smsService.SingleVariableTemplate(list.TemplateId, list.ID, Otp);
                list.MobileNos = MobileNos;

                //sends sms with saving the response. 
                Task<string> asyncTask = _smsService.SendSingleSMS(list);
                asyncTask.Wait();
                string result = asyncTask.Result;


                return result;
            });


        }
        public async Task<string> SendMessageMultipleVariables(string MobileNos, string[] vars)
        {

            string SuccessMessage = "";
            try
            {
                // string mobileNumbers = "8262881090,9023436638,7018026001";
                string mobileNumbers = "8262881090";
                //string OtpTest = "1234";
                SMSService _smsService = new SMSService();
                SMSMessageList list = new SMSMessageList();
                List<string> numbersList = mobileNumbers.Split(',').ToList();

                //Parameters are needed
                list.TemplateId = "1407169960073689031";
                list.ID = 6;
                list.SMSText = _smsService.MultipleVariableTemplate(list.TemplateId, list.ID, vars);
                list.MobileNos = MobileNos;


                //sends sms with saving the response. 
                string result = await _smsService.SendSingleSMS(list);
                SuccessMessage = "SMS Sent Successfully!";
                return SuccessMessage;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return SuccessMessage;
        }
        public async Task<string> SendMeetingMessageMultipleVariables(string MobileNos, string[] vars)
        {

            string SuccessMessage = "";
            try
            {
                // string mobileNumbers = "8262881090,9023436638,7018026001";
                //string mobileNumbers = "8262881090";
                //string OtpTest = "1234";
                SMSService _smsService = new SMSService();
                SMSMessageList list = new SMSMessageList();
                List<string> numbersList = MobileNos.Split(',').ToList();

                //Parameters are needed
                list.TemplateId = "1407169960073689031";
                list.ID = 6;
                list.SMSText = _smsService.MultipleVariableTemplate(list.TemplateId, list.ID, vars);
                list.MobileNos = MobileNos;


                //sends sms with saving the response. 
                string result = await _smsService.SendSingleSMS(list);
                SuccessMessage = "SMS Sent Successfully!";
                return SuccessMessage;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return SuccessMessage;
        }

        public async Task<string> SendProceedingMessageMultipleVariables(string MobileNos, string[] vars)
        {

            string SuccessMessage = "";
            try
            {
                // string mobileNumbers = "8262881090,9023436638,7018026001";
                //string mobileNumbers = "8262881090";
                //string OtpTest = "1234";
                SMSService _smsService = new SMSService();
                SMSMessageList list = new SMSMessageList();
                List<string> numbersList = MobileNos.Split(',').ToList();

                //Parameters are needed
                list.TemplateId = "1407169960090837956";
                list.ID = 4;
                list.SMSText = _smsService.MultipleVariableTemplate(list.TemplateId, list.ID, vars);
                list.MobileNos = MobileNos;


                //sends sms with saving the response. 
                string result = await _smsService.SendSingleSMS(list);
                SuccessMessage = "SMS Sent Successfully!";
                return SuccessMessage;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return SuccessMessage;
        }

        public async Task<string> SendLoginMessageMultipleVariables(string MobileNos, string[] vars, string templateid)
        {

            string SuccessMessage = "";
            try
            {
                
                SMSService _smsService = new SMSService();
                SMSMessageList list = new SMSMessageList();
                List<string> numbersList = MobileNos.Split(',').ToList();

                //Parameters are needed
                list.TemplateId = templateid;
                list.ID = 11;
                list.SMSText = _smsService.userregistered(list.TemplateId, list.ID, vars);
                list.MobileNos = MobileNos;


                //sends sms with saving the response. 
                string result = await _smsService.SendSingleSMS(list);
                SuccessMessage = "SMS Sent Successfully!";
                return SuccessMessage;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return SuccessMessage;
        }




    }

    public class SMSMessageList
    {
        public SMSMessageList()
        {
            MobileNo = new List<string>();
            ProccessDate = DateTime.Now.AddDays(-1);
        }
        public List<string> MobileNo;
        public string MobileNos;
        public string SMSText;
        public DateTime ProccessDate;
        public bool? isLocale;
        public string TemplateId;
        public int ID;
        public int ModuleActionID { get; set; }
        public long UniqueIdentificationID { get; set; }

    }

    [Serializable]
    public class SMSLogModel
    {
        public long LogID { get; set; }
        public string ResponseContent { get; set; }
        public int ResponseCode { get; set; }
        public System.Nullable<System.DateTime> TimeStamp { get; set; }

        public string mobilenumbers;
        public string Reason { get; set; }
        public int ReasonCode { get; set; }
    }



}





