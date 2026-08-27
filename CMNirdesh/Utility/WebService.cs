using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace PVSWebSite.Utility
{
    public class WebService
    {
        private GetMethod objgetMethod = new GetMethod();
        public byte[] WebServiceMethod(byte[] serviceParameter)
        {
            EncryptionHelper encryptionHelper = new EncryptionHelper();
            if (serviceParameter == null)
                return (byte[])null;
            try
            {
                string xml = encryptionHelper.Decrypt(serviceParameter);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.XmlResolver = null;
                xmlDocument.LoadXml(xml);
                ServiceParameters serviceParmeters = new ServiceParameters();
                XmlElement elementToGetAttr = xmlDocument.SelectSingleNode("eVidhan/ServiceParameters/ServiceParameter") as XmlElement;
                serviceParmeters.ApplicationId = XmlUtility.GetStringAttribute(elementToGetAttr, "ApplicationId");
                serviceParmeters.ConnectionId = XmlUtility.GetStringAttribute(elementToGetAttr, "ConnectionId");
                serviceParmeters.MethodType = XmlUtility.GetStringAttribute(elementToGetAttr, "MethodType");
                serviceParmeters.MethodName = XmlUtility.GetStringAttribute(elementToGetAttr, "MethodName");
                serviceParmeters.parameters = elementToGetAttr.SelectNodes("methodParameters/methodParameter");
                return this.objgetMethod.GetData(serviceParmeters);
            }
            catch (Exception ex)
            {
                return (byte[])null;
            }
        }
    }
}