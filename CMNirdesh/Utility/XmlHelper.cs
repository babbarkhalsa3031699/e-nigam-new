using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace PVSWebSite.Utility
{
    public class XmlHelper
    {

        /// <summary>
        /// Reads and xml from file to an object.
        /// </summary>
        /// <typeparam name="T">Type of object to which xml is read.</typeparam>
        /// <param name="path">Absoulute path to the xml file.</param>
        /// <returns>The object.</returns>
        public static T ReadXml<T>(string path) where T : class, new()
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(string.Format("File not found at \"{0}\"", path));
            FileHelper.SetFileAccessToNormal(path);
            T result;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            //using (FileStream fileStream = new FileStream(path, FileMode.Open))
            //{
            //    result = (T)serializer.Deserialize(fileStream);
            //}
            FileStream fileStream = new FileStream(path, FileMode.Open);
        
                result = (T)serializer.Deserialize(fileStream);
          
            return result;
        }

        /// <summary>
        /// Saves a class instance as an xml.
        /// </summary>
        /// <typeparam name="T">Type of object from which xml is populated.</typeparam>
        /// <param name="model">The instance of T.</param>
        /// <param name="path">Absoulute path to the xml file.</param>
        public static void SaveAsXml<T>(T model, string path) where T : class, new()
        {
            FileHelper.SetFileAccessToNormal(path);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StreamWriter writer = new StreamWriter(path);
            serializer.Serialize(writer, model);
            writer.Close();
        }

        public static string DecryptByte(byte[] Data)
        {
            EncryptionHelper objEncry = new EncryptionHelper();
            return objEncry.Decrypt(Data);
        }
        public static DataSet ConvertEncryptedXMLToDataSet(byte[] Data)
        {
            string xmlData = DecryptByte(Data);
            if (string.IsNullOrEmpty(xmlData)) return null;
            return ConvertXMLToDataSet(xmlData);
        }
        public static DataSet ConvertXMLToDataSet(string xmlData)
        {
            StringReader stream = new StringReader(xmlData);
            XmlTextReader reader = new XmlTextReader(stream);
            try
            {
                DataSet xmlDS = new DataSet();
              
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public static string ToXml(DataSet ds)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(DataSet));
                    xmlSerializer.Serialize(streamWriter, ds);
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }
    }
}