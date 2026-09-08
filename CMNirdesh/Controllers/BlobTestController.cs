using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using CMNirdesh.Models;
using ErrorLog = CMNirdesh.Error.ErrorLog;
namespace CMNirdesh.Controllers
{
    public class BlobTestController : Controller
    {
        //private const string connectionString = "<connec_string>";
        private const string blobContainerName = "securefilestructure";
        private const int chunkSize = 1024 * 1024;
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        //public async Task<ActionResult> Upload(HttpPostedFileBase file)
        //{
        //    try
        //    {
        //        if (file == null || file.ContentLength == 0)
        //        {
        //            ErrorLog.WriteToLog( " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
        //            return RedirectToAction("Error");
        //        }

        //        BlobServiceClient blobServiceClient = new BlobServiceClient(ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString);
        //        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
        //        string blobName = Guid.NewGuid().ToString();
        //        BlockBlobClient blockBlobClient = containerClient.GetBlockBlobClient(blobName);

        //        using (Stream stream = file.InputStream)
        //        {
        //            byte[] buffer = new byte[chunkSize];
        //            int bytesRead;
        //            int blockNumber = 0;

        //            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        //            {
        //                using (MemoryStream chunkStream = new MemoryStream(buffer, 0, bytesRead))
        //                {
        //                    string blockId = Convert.ToBase64String(BitConverter.GetBytes(blockNumber));
        //                    await blockBlobClient.StageBlockAsync(blockId, chunkStream);

        //                    blockNumber++;
        //                }
        //            }
        //            var blockList = Enumerable.Range(0, blockNumber)
        //                .Select(n => Convert.ToBase64String(BitConverter.GetBytes(n)))
        //                .ToList();
        //            await blockBlobClient.CommitBlockListAsync(blockList);
        //        }

        //        return RedirectToAction("Success");
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
        //        return RedirectToAction("Error");
        //    }
        //}

        public ActionResult Upload(HttpPostedFileBase file)
        {
            try
            {
                if (file == null || file.ContentLength == 0)
                {
                    ErrorLog.WriteToLog(" Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return RedirectToAction("Error");
                }

                string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
                string blobName = "MCTest/Mohali" + Guid.NewGuid().ToString();
                BlockBlobClient blockBlobClient = containerClient.GetBlockBlobClient(blobName);

                using (Stream stream = file.InputStream)
                {
                    byte[] buffer = new byte[chunkSize];
                    int bytesRead;
                    int blockNumber = 0;

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        byte[] chunk = new byte[bytesRead];
                        Array.Copy(buffer, chunk, bytesRead);

                        string blockId = Convert.ToBase64String(BitConverter.GetBytes(blockNumber));
                        blockBlobClient.StageBlock(blockId, new MemoryStream(chunk));

                        blockNumber++;
                    }

                    var blockList = Enumerable.Range(0, blockNumber)
                                              .Select(n => Convert.ToBase64String(BitConverter.GetBytes(n)))
                                              .ToList();
                    blockBlobClient.CommitBlockList(blockList);
                }

                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex," Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(HttpPostedFileBase file)
        {
            try
            {
                
                if (file != null && file.ContentLength > 0)
                {
                    using (var stream = file.InputStream)
                    {
                        //await BlobStorage.UploadFileAsync("securefilestructure", file.FileName, stream);
                        ErrorLog.WriteToLog(" Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    }
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message.ToString();
                ViewBag.Message = x;
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return RedirectToAction("Index", "BlobTest");
            }
            ErrorLog.WriteToLog( " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
            return RedirectToAction("Index", "BlobTest");
        }
    }
}

