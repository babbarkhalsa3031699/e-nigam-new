using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Data;

namespace PVSWebSite.Utility
{
    public class XmlUtility
    {
        public static string GetStringAttribute(XmlElement elementToGetAttr, string attributeName)
        {
            try
            {
                return elementToGetAttr == null || string.IsNullOrEmpty(attributeName) || elementToGetAttr.Attributes[attributeName] == null ? string.Empty : elementToGetAttr.Attributes[attributeName].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return string.Empty;
        }

        public static string DatasetToXml(DataSet ds)
        {
            StringWriter writer = new StringWriter();
            ds.WriteXml((TextWriter)writer, XmlWriteMode.WriteSchema);
            return writer.ToString();
        }

    public static DataSet ConvertXMLToDataSet(string xmlData)
    {
            StringReader stream = new StringReader(xmlData);
            XmlTextReader reader = new XmlTextReader(stream) ;
      try
      {
        DataSet dataSet = new DataSet();
                 //reader = new XmlTextReader((TextReader) new StringReader(xmlData));
        int num = (int) dataSet.ReadXml((XmlReader) reader);
        return dataSet;
      }
      catch(Exception ex)
      {
        return (DataSet) null;
      }
      finally
      {
                 reader.Close();
      }
    }
    }
}