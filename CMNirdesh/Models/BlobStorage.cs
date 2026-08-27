using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Azure.Storage.Blobs;
using Azure.Storage;
using Azure.Storage.Blobs.Specialized;
using System.Linq;
using Azure.Storage.Blobs.Models;
using System.Security.Cryptography;
using Azure;
using System.Diagnostics;
using Microsoft.Ajax.Utilities;
using System.Web.Hosting;
using Microsoft.WindowsAzure.Storage;
using System.Web.Mvc;

namespace CMNirdesh.Models
{
    public class BlobStorage
    {
        public object CloudConfigurationManager { get; private set; }
        private const int chunkSize = 2048 * 2048;

        public static void Savefile(HttpPostedFileBase file, string folderpath, string filename)
        {
            try
            {
                if (file == null || file.ContentLength == 0)
                {
                    throw new ArgumentException("Invalid file provided.");
                }

                string value = ConfigurationManager.AppSettings["StorageType"];
                string root = GetStoragePath(value);

                if (value == "Blob")
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;
                    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(root);
                    string blobName = folderpath + filename;
                    blobName = blobName.Replace('\\', '/');
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
                        if (filename.EndsWith(".pdf") || filename.EndsWith(".PDF"))
                        {
                            blockBlobClient.SetHttpHeaders(new BlobHttpHeaders { ContentType = "application/pdf" });
                        }
                    }
                }
                else
                {
                    string folder = Path.Combine(GetStoragePath(value), folderpath);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);

                    }
                    string filepath = Path.Combine(folder, Path.GetFileName(file.FileName));
                    filepath = filepath.Replace('\\', '/');
                    file.SaveAs(filepath);

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error uploading file: {ex.Message}");
            }
        }

     


        public static async Task<string> UploadSingedFile(byte[] fileBytes, string blobName)
        {
            string value = ConfigurationManager.AppSettings["StorageType"];
            string containerName = GetStoragePath(value);
            string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            using (MemoryStream ms = new MemoryStream(fileBytes))
            {
                await blobClient.UploadAsync(ms, new BlobHttpHeaders { ContentType = "application/pdf" });
                //await blobClient.UploadAsync(ms, true);
            }
            return blobClient.Uri.ToString();
        }

        public static async Task<byte[]> GetFileFromAzureAsync(string filepath)
        {
            string value = ConfigurationManager.AppSettings["StorageType"];
            string containerName = GetStoragePath(value);
            string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;

            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                BlobClient blobClient = containerClient.GetBlobClient(filepath);

                if (await blobClient.ExistsAsync())
                {
                    using (var ms = new MemoryStream())
                    {
                        await blobClient.DownloadToAsync(ms);
                        return ms.ToArray();
                    }
                }
                return Array.Empty<byte>();
            }
            catch (Exception ex)
            {
                // Log or display the exception message
                Console.WriteLine($"Error retrieving file from Azure Blob Storage: {ex.Message}");
                throw;
            }
        }

        public static string UploadeSignedFile(byte[] fileBytes, string blobName)
        {
            string value = ConfigurationManager.AppSettings["StorageType"];
            string containerName = GetStoragePath(value);
            string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;

            // Create the BlobServiceClient
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            containerClient.CreateIfNotExists();  // Synchronous call to create the container

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Upload the file synchronously
            using (MemoryStream ms = new MemoryStream(fileBytes))
            {
                blobClient.Upload(ms, new BlobHttpHeaders { ContentType = "application/pdf" });
            }

            // Return the URI of the uploaded blob
            return blobClient.Uri.ToString();
        }


        public static async Task<byte[]> ComputeMD5HashAsync(Uri blobUri)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;

            // Create a BlobServiceClient using the connection string
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Extract the container name and blob name from the URI
            string blobUriPath = blobUri.AbsolutePath.Trim('/');
            int firstSlashIndex = blobUriPath.IndexOf('/');
            string containerName = blobUriPath.Substring(0, firstSlashIndex);
            string blobName = blobUriPath.Substring(firstSlashIndex + 1);

            // Get the container client
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Get the blob client
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Define buffer size for chunking (e.g., 4 MB)
            const int bufferSize = 4 * 1024 * 1024; // 4 MB

            using (MD5 md5 = MD5.Create())
            {
                try
                {
                    // Download the blob in chunks
                    BlobDownloadInfo download = await blobClient.DownloadAsync();

                    using (Stream blobStream = download.Content)
                    {
                        byte[] buffer = new byte[bufferSize];
                        int bytesRead;

                        while ((bytesRead = await blobStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            md5.TransformBlock(buffer, 0, bytesRead, null, 0);
                        }

                        // Finalize the MD5 computation
                        md5.TransformFinalBlock(Array.Empty<byte>(), 0, 0);

                        return md5.Hash;
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., log the error)
                    throw new InvalidOperationException("Error computing MD5 hash for blob.", ex);
                }
            }
        }

        public static string GetStoragePath(string Type)
        {
            string st = "";
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@StroageType", Type);
                DataSet set = SqlHelper.ExecuteDataset(connection, "GetStoragePath", param);

                foreach (DataRow dr in set.Tables[0].Rows)
                {
                    st = Convert.ToString(dr["SettingValue"]);

                }

                return st;
            }
            catch (Exception)
            {
            }
            return st;

        }

        public static string GetStorageAcessingPath()
        {
            string value = ConfigurationManager.AppSettings["StorageType"];
            string st = "";
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@StroageType", value);
                DataSet set = SqlHelper.ExecuteDataset(connection, "GetStorageAcessingPath", param);

                foreach (DataRow dr in set.Tables[0].Rows)
                {
                    st = Convert.ToString(dr["SettingValue"]);
                }

                return st;
            }
            catch (Exception)
            {
            }
            return st;

        }

        public static async Task<string> GetFileBase64Async(string filepath)
        {
            try
            {
                string value = ConfigurationManager.AppSettings["StorageType"];
                string containerName = GetStoragePath(value);
                string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                BlobClient blobClient = containerClient.GetBlobClient(filepath);

                if (await blobClient.ExistsAsync())
                {
                    using (var ms = new MemoryStream())
                    {
                        await blobClient.DownloadToAsync(ms);
                        byte[] fileBytes = ms.ToArray();

                        // Convert byte array to Base64 string
                        return Convert.ToBase64String(fileBytes);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new InvalidOperationException("Error retrieving file from Azure Blob Storage.", ex);
            }
        }

        public static byte[] GetFileFromAzure(string filepath)
        {
            string value = ConfigurationManager.AppSettings["StorageType"];
            string containerName = GetStoragePath(value);
            string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(filepath);

            if (blobClient.ExistsAsync().Result)
            {
                using (var ms = new MemoryStream())
                {
                    blobClient.DownloadTo(ms);
                    return ms.ToArray();
                }
            }
            return new byte[0];
        }

        public static void UploadPdfToBlob(byte[] pdfBytes, string filepath)
        {
            string value = ConfigurationManager.AppSettings["StorageType"];
            string containerName = GetStoragePath(value);
            if (value == "Blob")
            {

                string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                try
                {
                   containerClient.CreateIfNotExists();
                   var blobHttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = "application/pdf",
                        ContentDisposition = "inline"
                    };

                    BlobClient blobClient = containerClient.GetBlobClient(filepath);
                    using (var stream = new MemoryStream(pdfBytes))
                    {
                        blobClient.Upload(stream, new BlobUploadOptions
                        {
                            HttpHeaders = blobHttpHeaders
                        });
                    }
                    Console.WriteLine($"PDF '{filepath}' uploaded to Azure Blob Storage successfully.");
                }
                catch (RequestFailedException ex)
                {
                    Console.WriteLine($"Request failed: {ex.Message}");
                    // Optionally log more details from the exception
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    // Handle or log the exception as needed
                }
            }
            else
            {
                //HostingEnvironment.MapPath
                var FileFolder = HostingEnvironment.MapPath("~/SecureFileStructure");
                string folderPath = Path.Combine(FileFolder, Path.GetDirectoryName(filepath));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Construct the full path for saving the file
                string fullFilePath = Path.Combine(folderPath, Path.GetFileName(filepath));

                // Save the PDF bytes to the local file system
                File.WriteAllBytes(fullFilePath, pdfBytes);

                Console.WriteLine($"PDF '{fullFilePath}' saved locally successfully.");
            }

        }


        public static string UploadApprovalsToBlob(byte[] pdfBytes, string filepath)
        {
            string value = ConfigurationManager.AppSettings["StorageType"];
            string containerName = GetStoragePath(value);

            if (value == "Blob")
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                try
                {
                    // Create the container if it doesn't exist
                    containerClient.CreateIfNotExists();

                    var blobHttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = "application/pdf"
                    };
                    // Create a blob client
                    BlobClient blobClient = containerClient.GetBlobClient(filepath);

                    // Convert the PDF byte array to a stream
                    using (var stream = new MemoryStream(pdfBytes))
                    {
                        // Upload the PDF stream to the blob
                        blobClient.Upload(stream, overwrite: true);
                    }

                    // Return the Blob URL
                    string blobUrl = blobClient.Uri.ToString();
                    Console.WriteLine($"PDF '{filepath}' uploaded to Azure Blob Storage successfully.");
                    return blobUrl; // Returning the Blob URL
                }
                catch (RequestFailedException ex)
                {
                    Console.WriteLine($"Request failed: {ex.Message}");
                    // Optionally log more details from the exception
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    // Handle or log the exception as needed
                }
            }
            else
            {
                string folderPath = Path.Combine(GetStoragePath(value), Path.GetDirectoryName(filepath));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Construct the full path for saving the file
                string fullFilePath = Path.Combine(folderPath, Path.GetFileName(filepath));

                // Save the PDF bytes to the local file system
                File.WriteAllBytes(fullFilePath, pdfBytes);

                Console.WriteLine($"PDF '{fullFilePath}' saved locally successfully.");
            }

            return null; // Return null if not using Blob Storage
        }



        // This is Genric function to copy the file to blob or file structure on the physical drive on the basis of storagetype (Blob)
        public static void CopyPdfLocalToBlob(string filePath, string blobName, string localPath)
        {
            string filepathtobecopyfrom = localPath;


            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;
                string storageType = ConfigurationManager.AppSettings["StorageType"];
                string containerName = GetStoragePath(storageType);

                if (storageType == "Blob")
                {
                    // This code is copy the file to the blob storage 
                    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                    containerClient.CreateIfNotExists();
                    string folderPath = Path.GetFileName(localPath);
                    blobName = Path.Combine(blobName, folderPath);
                    var blobHttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = "application/pdf"
                    };
                    BlobClient blobClient = containerClient.GetBlobClient(blobName);
                    using (FileStream fileStream = File.OpenRead(filePath))
                    {
                        var uploadOptions = new BlobUploadOptions
                        {
                            HttpHeaders = blobHttpHeaders
                        };
                        blobClient.Upload(fileStream, uploadOptions);
                    }

                    Console.WriteLine("File uploaded to Blob successfully.");
                }
                else
                {
                    // This code is to save the copy to normal file structure path
                    string folderPath = Path.GetDirectoryName(localPath);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    File.Copy(localPath,filePath, true);
                    Console.WriteLine("File copied locally successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public static void CopyLocalFileToBlob(string localFilePath, string folderPathInBlob)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;
                string containerName = GetStoragePath("Blob"); 
                string fileName = Path.GetFileName(localFilePath);
                string blobName = Path.Combine(folderPathInBlob, fileName).Replace("\\", "/");
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                containerClient.CreateIfNotExists();
                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = "application/pdf"
                };
                BlobClient blobClient = containerClient.GetBlobClient(blobName);
                using (FileStream fileStream = File.OpenRead(localFilePath))
                {
                    blobClient.Upload(fileStream, new BlobUploadOptions
                    {
                        HttpHeaders = blobHttpHeaders
                    });
                }

                Console.WriteLine($"File uploaded successfully to blob path: {blobName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public static void CopyLocalFileToBlobNew(string localFilePath, string folderPathInBlob, string fileName)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;
                string containerName = GetStoragePath("Blob");
                //string fileName = Path.GetFileName(localFilePath);
                string blobName = Path.Combine(folderPathInBlob, fileName).Replace("\\", "/");
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                containerClient.CreateIfNotExists();
                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = "application/pdf"
                };
                BlobClient blobClient = containerClient.GetBlobClient(blobName);
                using (FileStream fileStream = File.OpenRead(localFilePath))
                {
                    blobClient.Upload(fileStream, new BlobUploadOptions
                    {
                        HttpHeaders = blobHttpHeaders
                    });
                }

                Console.WriteLine($"File uploaded successfully to blob path: {blobName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}
