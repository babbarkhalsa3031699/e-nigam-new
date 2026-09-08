using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PVSWebSite.Utility.ServiceAdapter
{
    public class ServiceAdaptor
    {
        public static String GetStartPageID()
        {
            String returnValue = "98046BE5-5E66-4A00-9A1E-09F13DCFDF49";
            return returnValue;
        }

        public static DataSet GetDataSetFromService(string MethodType, string MethodName, List<KeyValuePair<string, string>> methodPara)
        {
            byte[] result = GetbyteFromService(MethodType, MethodName, methodPara);
            return XmlHelper.ConvertEncryptedXMLToDataSet(result);
        }

        public static byte[] GetbyteFromService(string MethodType, string MethodName, List<KeyValuePair<string, string>> methodPara)
        {
            try
            {
                WebService objWebService = new WebService();
                ServiceRequestXml objServiceRequestXml = new ServiceRequestXml();
                byte[] parameters = objServiceRequestXml.CreateServiceRequestEncryptedXmlBytes(MethodType, MethodName, methodPara);
                byte[] result = objWebService.WebServiceMethod(parameters);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }            
        }
    }
}