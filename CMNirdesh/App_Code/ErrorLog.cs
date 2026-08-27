using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SBL.eLegistrator.HouseController.Web.SeoFriendly
{
    public static class ErrorLog
    {
        public static void WriteToLog(Exception ex, string CustomMessage)
        {
            //  C:\inetpub\e_Vidhan\FileStructure\
            string filePath = Path.Combine("C:\\inetpub\\e_Vidhan\\FileStructure\\", "ExceptionLog");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);

            }
            filePath = Path.Combine(filePath, "ErrorLog.txt");
            FileStream fs = new FileStream(filePath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);

            if (ex != null)
            {

                sw.Write("Custom Message : " + CustomMessage + Environment.NewLine + " Message :" + ex.Message + Environment.NewLine + "StackTrace :" + ex.StackTrace +
           "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
            }
            else
            {
                sw.Write("Custom Message : " + CustomMessage + Environment.NewLine + "Date :" + DateTime.Now.ToString());
            }
            string New = Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine;
            sw.Write(New);

            sw.Flush();
            sw.Close();
            fs.Close();


        }

        public static void WriteToLog(string CustomMessage)
        {
            //  C:\inetpub\e_Vidhan\FileStructure\
            string filePath = Path.Combine("C:\\inetpub\\e_Vidhan\\FileStructure\\", "ExceptionLog");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);

            }
            filePath = Path.Combine(filePath, "ErrorLog.txt");
            FileStream fs = new FileStream(filePath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);

           

                sw.Write("Custom Message : " + CustomMessage + "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
            
            
            string New = Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine;
            sw.Write(New);

            sw.Flush();
            sw.Close();
            fs.Close();


        }
    }
}