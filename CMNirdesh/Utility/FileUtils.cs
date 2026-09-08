using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Web;
using PVSWebSite.Models;
using System.Configuration;

namespace PVSWebSite.Utility
{
    public class FileUtils
    {
         public static string ValidPDFFile1(HttpPostedFileBase file)
        {
            string fileName = file.FileName;
            string fileExtension = System.IO.Path.GetExtension(fileName);
            string fileMimeType = file.ContentType;
            int fileLengthInKB = file.ContentLength / 1024;


            string[] matchExtension = { ".pdf", ".PDF" };
            string[] matchMimeType = { "application/pdf" };
            string contenttype = String.Empty;
            Stream checkStream = file.InputStream;
            using (BinaryReader chkBinary = new BinaryReader(checkStream))
            {
                Byte[] chkbytes = chkBinary.ReadBytes(0x10);

                string data_as_hex = BitConverter.ToString(chkbytes);
                string magicCheck = data_as_hex.Substring(0, 11);
                if (matchExtension.Contains(fileExtension.ToLower()) && matchMimeType.Contains(fileMimeType) && magicCheck == "25-50-44-46")
                {
                    if (fileLengthInKB <= 1024000)
                    {
                        return "OK";
                    }
                    else
                    {
                        return "File Size Exceeded";
                    }
                }
                else
                {
                    return "File Extension Not Valid";
                }
            }
              
        }
        public static bool ValidPDFFile(HttpPostedFileBase file)
        {
            string fileName = file.FileName;
            string fileExtension = System.IO.Path.GetExtension(fileName);
            string fileMimeType = file.ContentType;
            int fileLengthInKB = file.ContentLength / 1024;


            string[] matchExtension = { ".pdf", ".PDF" };
            string[] matchMimeType = { "application/pdf" };
            string contenttype = String.Empty;
            Stream checkStream = file.InputStream;
            BinaryReader chkBinary = new BinaryReader(checkStream);
            Byte[] chkbytes = chkBinary.ReadBytes(0x10);

            string data_as_hex = BitConverter.ToString(chkbytes);
            string magicCheck = data_as_hex.Substring(0, 11);
            if (matchExtension.Contains(fileExtension.ToLower()) && matchMimeType.Contains(fileMimeType) && magicCheck == "25-50-44-46")
            {
                if (fileLengthInKB <= 102400)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool ValidImageFile(HttpPostedFileBase file)
        {
            if (file != null)
            {
                int len = file.ContentLength;
                string fileName = file.FileName;
                string fileExtension = System.IO.Path.GetExtension(fileName);
                string fileMimeType = file.ContentType;
                int fileLengthInKB = len / 1024;
                //string[] matchExtension = { ".jpg", ".png", ".jpeg" };
                //string[] matchMimeType = { "image/jpeg", "image/png", "image/gif" };
                //string[] matchHexData = { "FF-D8-FF-E1", "FF-D8-FF-E0", "89-50-4E-47", "47-49-46-38", "7F-45-4C-46" };
                string[] matchExtension = { ".jpg", ".png", ".gif", ".bmp", ".jpeg" };
                string[] matchMimeType = { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/x-windows-bmp" };
                string[] matchHexData = { "FF-D8-FF-E1", "FF-D8-FF-E0", "89-50-4E-47", "47-49-46-38", "7F-45-4C-46", "42-4D-38-84", "FF-D8-FF-E2", "FF-D8-FF-E3" };
                string contenttype = String.Empty;
                Stream checkStream = file.InputStream;
                using (StreamReader sr = new StreamReader(checkStream))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        sr.BaseStream.CopyTo(ms);
                        Byte[] chkbytes = ms.ToArray();
                        string data_as_hex = BitConverter.ToString(chkbytes);
                        string magicCheck = data_as_hex.Substring(0, 11);
                        if (matchExtension.Contains(fileExtension.ToLower()) && matchMimeType.Contains(fileMimeType) && matchHexData.Contains(magicCheck))
                        {
                            if (fileLengthInKB <= 51200)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                        else
                        {
                            return false;
                        }
                    }

                }
                //    BinaryReader chkBinary = new BinaryReader(checkStream);
                //Byte[] chkbytes = chkBinary.ReadBytes(0x10);

                //string data_as_hex = BitConverter.ToString(chkbytes);
                //string magicCheck = data_as_hex.Substring(0, 11);
                //if (matchExtension.Contains(fileExtension.ToLower()) && matchMimeType.Contains(fileMimeType) && matchHexData.Contains(magicCheck))
                //{
                //    if (fileLengthInKB <= 51200)
                //    {
                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }

                //}
                //else
                //{
                //    return false;
                //}
            }
            else
            {
                return false;
            }

        }

        public static string GetMimeTypeByWindowsRegistry(string fileNameOrExtension)
        {
            string mimeType = "application/unknown";
            string ext = (fileNameOrExtension.Contains(".")) ? System.IO.Path.GetExtension(fileNameOrExtension).ToLower() : "." + fileNameOrExtension;
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public static bool ValidImageFileFromFilePath(string filePath)
        {
            try { 
            if (filePath != null)
            {

                FileInfo fi = new FileInfo(filePath);
                string fileName = fi.Name;
                string fileExtension = fi.Extension;
                string fileMimeType = GetMimeTypeByWindowsRegistry(fileName);
                long fileLengthInKB = fi.Length / 1024;

                string[] matchExtension = { ".jpg", ".png", ".gif", ".bmp", ".jpeg" };
                string[] matchMimeType = { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/x-windows-bmp" };
                string[] matchHexData = { "FF-D8-FF-E1", "FF-D8-FF-E0", "89-50-4E-47", "47-49-46-38", "7F-45-4C-46", "42-4D-38-84", "FF-D8-FF-E2", "FF-D8-FF-E3" };
                string contenttype = String.Empty;
                FileStream checkStream = new FileStream(filePath, FileMode.Open);
               using (BinaryReader chkBinary = new BinaryReader(checkStream))
                {
                    Byte[] chkbytes = chkBinary.ReadBytes(0x10);

                    string data_as_hex = BitConverter.ToString(chkbytes);
                    string magicCheck = data_as_hex.Substring(0, 11);
                    if (matchExtension.Contains(fileExtension.ToLower()) && matchMimeType.Contains(fileMimeType) && matchHexData.Contains(magicCheck))
                    {
                        if (fileLengthInKB <= 51200)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {
                        return false;
                    }
                }
             
            }
            else
            {
                return false;
            }
            }
            catch (Exception ex) { return false; }

        }


        public static string GetFileSetting(string strValue)
        {
            string strOutValue = "";
            try
            {
                List<KeyValuePair<string, string>> methodParameter = new List<KeyValuePair<string, string>>();
                methodParameter.Add(new KeyValuePair<string, string>("@SettingName", Convert.ToString(strValue)));
                DataSet userTable = PVSWebSite.Utility.ServiceAdapter.ServiceAdaptor.GetDataSetFromService("SelectMSSql", "sp_GetGeneralSettingbyValue", methodParameter);
                if (userTable != null)
                {
                    for (int i = 0; i < userTable.Tables[0].Rows.Count; i++)
                    {
                        strOutValue = Convert.ToString(userTable.Tables[0].Rows[i]["SettingValue"]);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonModel.SendErrorToText(ex);
                throw ex;
            }

            return strOutValue;
        }

        public static string insertErroLog(string FormName,string TaskPerformed,string LoginNo,string ErrorDetail)
        {
            string strOutValue = "";
            string machineIP =HttpContext.Current.Request.UserHostAddress.ToString();// GetLocalIPAddress();
            try
            {
                List<KeyValuePair<string, string>> methodParameter = new List<KeyValuePair<string, string>>();
                methodParameter.Add(new KeyValuePair<string, string>("@FormName", Convert.ToString(FormName)));
                methodParameter.Add(new KeyValuePair<string, string>("@TaskPerformed", Convert.ToString(TaskPerformed)));
                methodParameter.Add(new KeyValuePair<string, string>("@LoginId", Convert.ToString(LoginNo)));
                methodParameter.Add(new KeyValuePair<string, string>("@MachineIP", Convert.ToString(machineIP)));
                methodParameter.Add(new KeyValuePair<string, string>("@ErrorDetail", Convert.ToString(ErrorDetail)));
                DataSet userTable = PVSWebSite.Utility.ServiceAdapter.ServiceAdaptor.GetDataSetFromService("SelectMSSql", "sp_insertLoggDetail", methodParameter);
                if (userTable != null)
                {
                    for (int i = 0; i < userTable.Tables[0].Rows.Count; i++)
                    {
                        strOutValue = Convert.ToString(userTable.Tables[0].Rows[i]["LatestId"]);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonModel.SendErrorToText(ex);
                throw ex;
            }

            return strOutValue;
        }

        public static string insertLoginLogout(string IpAddress, string SessionId, string UserName, string Controller, string Method, string LoginLogOutStatus)
        {
            string strOutValue = "";
            // string machineIP = GetLocalIPAddress();
            try
            {
                List<KeyValuePair<string, string>> methodParameter = new List<KeyValuePair<string, string>>();

                methodParameter.Add(new KeyValuePair<string, string>("@UserName", Convert.ToString(UserName)));
                methodParameter.Add(new KeyValuePair<string, string>("@MachineIP", Convert.ToString(IpAddress)));
                methodParameter.Add(new KeyValuePair<string, string>("@ControllerName", Convert.ToString(Controller)));
                methodParameter.Add(new KeyValuePair<string, string>("@MethodName", Convert.ToString(Method)));
                methodParameter.Add(new KeyValuePair<string, string>("@LoginLogOutStatus", Convert.ToString(LoginLogOutStatus)));
                methodParameter.Add(new KeyValuePair<string, string>("@SessionId", Convert.ToString(SessionId)));
                DataSet userTable = PVSWebSite.Utility.ServiceAdapter.ServiceAdaptor.GetDataSetFromService("SelectMSSql", "sp_insertLogInLogOutDetails", methodParameter);
                if (userTable != null)
                {
                    for (int i = 0; i < userTable.Tables[0].Rows.Count; i++)
                    {
                        strOutValue = Convert.ToString(userTable.Tables[0].Rows[i]["ErrorMessage"]);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonModel.SendErrorToText(ex);
                throw ex;
            }

            return strOutValue;
        }


        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string GetUserSessionId(string UserName)
        {
            try
            {
                var methodParameter = new List<KeyValuePair<string, string>>();
                methodParameter.Add(new KeyValuePair<string, string>("@userName", UserName));
                DataSet dataSetCertificate = PVSWebSite.Utility.ServiceAdapter.ServiceAdaptor.GetDataSetFromService("SelectMSSql", "GetLoginUserId", methodParameter);
                if (null != dataSetCertificate && dataSetCertificate.Tables[0].Rows.Count != 0)
                {
                    var loginSessionId = dataSetCertificate.Tables[0].Rows[0]["SessionId"].ToString();

                    return loginSessionId.ToString();
                }
                else
                {

                    return "";
                }
            }
            catch (Exception ex)
            {
                CommonModel.SendErrorToText(ex);
                string insertdetail = Utility.FileUtils.insertErroLog("FileUtils", "GetUserSessionId", UserName, ex.Message.ToString());
                return ex.Message.ToString();
            }
        }

       
    }
}