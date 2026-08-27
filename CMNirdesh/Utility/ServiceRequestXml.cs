using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace PVSWebSite.Utility
{
    public class ServiceRequestXml
    {
        public XmlDocument CreateServiceRequestXml(string MethodType, string MethodName, List<KeyValuePair<string, string>> methodPara)
        {
            XmlDocument parameters = new XmlDocument();
            parameters.XmlResolver = null;
            parameters.LoadXml("<eVidhan/>");

            XmlNode tblsNode = parameters.CreateElement("ServiceParameters");
            parameters.DocumentElement.AppendChild(tblsNode);
            XmlElement tableNode = parameters.CreateElement("ServiceParameter");            
            tableNode.SetAttribute("MethodType", MethodType);
            tableNode.SetAttribute("MethodName", MethodName);
            tblsNode.AppendChild(tableNode);
            XmlNode methodParameters = parameters.CreateElement("methodParameters");
            tableNode.AppendChild(methodParameters);
            if (methodPara != null)
            {
                foreach (var element in methodPara)
                {
                    XmlElement methodParameter = parameters.CreateElement("methodParameter");
                    methodParameter.SetAttribute("Name", element.Key);
                    methodParameter.SetAttribute("Value", element.Value);
                    methodParameters.AppendChild(methodParameter);
                }
            }
            return parameters;
        }

        public byte[] CreateServiceRequestEncryptedXmlBytes(string MethodType, string MethodName, DataSet methodPara)
        {
            XmlDocument Xmlparameters = CreateServiceRequestXml(MethodType, MethodName, methodPara);
            EncryptionHelper objEncry = new EncryptionHelper();
            byte[] parameters = objEncry.Encrypt(Xmlparameters.InnerXml);
            return parameters;
        }



        public byte[] CreateServiceRequestEncryptedXmlBytes(string MethodType, string MethodName, List<KeyValuePair<string, string>> methodPara)
        {
            XmlDocument Xmlparameters = CreateServiceRequestXml(MethodType, MethodName, methodPara);
            EncryptionHelper objEncry = new EncryptionHelper();
            byte[] parameters = objEncry.Encrypt(Xmlparameters.InnerXml);
            return parameters;
        }

        public XmlDocument CreateServiceRequestXml(string MethodType, string MethodName, DataSet methodPara)
        {
            XmlDocument parameters = new XmlDocument();
            parameters.XmlResolver = null;
            parameters.LoadXml("<eVidhan/>");

            XmlNode tblsNode = parameters.CreateElement("ServiceParameters");
            parameters.DocumentElement.AppendChild(tblsNode);
            XmlElement tableNode = parameters.CreateElement("ServiceParameter");            
            tableNode.SetAttribute("MethodType", MethodType);
            tableNode.SetAttribute("MethodName", MethodName);
            tblsNode.AppendChild(tableNode);
            XmlNode methodParameters = parameters.CreateElement("methodParameters");
            tableNode.AppendChild(methodParameters);
            string dsparameters = XmlHelper.ToXml(methodPara);
            XmlElement methodParameter = parameters.CreateElement("methodParameter");
            methodParameter.SetAttribute("Name", "@data");
            methodParameter.SetAttribute("Value", dsparameters);
            methodParameters.AppendChild(methodParameter);


            //if (methodPara != null)
            //{
            // foreach (var element in methodPara)
            // {
            // XmlElement methodParameter = parameters.CreateElement("methodParameter");
            // methodParameter.SetAttribute("Name", element.Key);
            // methodParameter.SetAttribute("Value", element.Value);
            // methodParameters.AppendChild(methodParameter);
            // }
            //}
            return parameters;
        }
    }
}