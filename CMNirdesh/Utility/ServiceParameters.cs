using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace PVSWebSite.Utility
{
    public class ServiceParameters
    {
        public string ApplicationId { get; set; }

        public string ConnectionId { get; set; }

        public string MethodType { get; set; }

        public string MethodName { get; set; }

        public XmlNodeList parameters { get; set; }
    }
}