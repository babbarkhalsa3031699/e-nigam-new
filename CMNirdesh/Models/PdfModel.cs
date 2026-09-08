using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMNirdesh.Models
{
    public class PDFModel
    {
        public static void ConvertPdfUsingBatchFile(string htmlpath, string pdfpath, string batchFilePath)
        {
            try
            {
                // Prepare the arguments to pass to the batch file
                string arguments = String.Format("\"{0}\" \"{1}\"", htmlpath, pdfpath);

                // Check if the batch file exists
                if (!System.IO.File.Exists(batchFilePath))
                {
                    Console.WriteLine("Batch file not found.");
                    return;
                }


                Process process = new Process();

                // Specify the process start information
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",  // Command to run
                    Arguments = $"/c \"{batchFilePath}\" {arguments}", // "/c" option carries out the command and then terminates
                    RedirectStandardOutput = true, // Redirect standard output
                    RedirectStandardError = true, // Redirect standard error
                    UseShellExecute = false, // Don't use shell execute
                    CreateNoWindow = true // Don't create a window
                };

                // Set the start information for the process
                process.StartInfo = startInfo;

                // Start the process
                process.Start();

                // Read the output and error (if any)
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                // Wait for the process to finish
                process.WaitForExit();

                // Output the results
                Console.WriteLine("Output:");
                Console.WriteLine(output);
                Console.WriteLine("Error:");
                Console.WriteLine(error);

                // Check the exit code
                int exitCode = process.ExitCode;
                Console.WriteLine($"Exit Code: {exitCode}");

            }
            catch(Exception e)
            {
                string s=e.Message;
            }
        }
        public static void ConvertChromePdfRenderer(string htmlpath, string pdfpath)
        {
            try
            {
                StringBuilder paramsBuilder = new StringBuilder();
                //make CLI command
                //paramsBuilder.AppendFormat("\"{0}\" \"{1}\"", htmlpath, pdfpath);
                paramsBuilder.AppendFormat("--orientation Landscape \"{0}\" \"{1}\"", htmlpath, pdfpath);

                var options = new List<string>();

                //create new process
                using (Process process = new Process())
                {
                    //specify wkhtmltopdf.exe file path to execute above CLI
                    process.StartInfo.FileName = "C:\\Program Files\\wkhtmltopdf\\bin\\wkhtmltopdf.exe";
                    //assign CLI as process argument
                    process.StartInfo.Arguments = paramsBuilder.ToString();
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    //start execution
                    process.Start();


                    // Capture the output and error messages
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (!process.WaitForExit(60000))
                    {
                        process.Kill();
                    }
                }
            }
            catch (Exception e)
            {
                string s = e.Message;
            }
        }
        public class MockHttpPostedFileBase : HttpPostedFileBase
        {
            private readonly Stream _stream;
            private readonly string _fileName;

            public MockHttpPostedFileBase(Stream stream, string fileName)
            {
                _stream = stream ?? throw new ArgumentNullException(nameof(stream));
                _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            }

            public override int ContentLength => (int)_stream.Length;

            public override string ContentType => "application/octet-stream";

            public override string FileName => _fileName;

            public override Stream InputStream => _stream;

            public override void SaveAs(string filename)
            {
                using (var fileStream = System.IO.File.Create(filename))
                {
                    _stream.Seek(0, SeekOrigin.Begin);
                    _stream.CopyTo(fileStream);
                }
            }
        }
    }
}
