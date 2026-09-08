using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace CMNirdesh.Models
{
    public class EsignRequestModel
    {
        public string DocumentName { get; set; }
        public byte[] DocumentContent { get; set; }
        public string SignerName { get; set; }
        public string SignerEmail { get; set; }

        [AllowHtml]
        public string eSignResponse { get; set;}
    }

    [Serializable]
    [XmlRoot("Esign")]
    public class ClsEsignXml
    {
        [XmlAttribute("ver")]
        public string ver { get; set; }
        [XmlAttribute("sc")]
        public string sc { get; set; }
        [XmlAttribute("ts")]
        public string ts { get; set; }
        [XmlAttribute("txn")]
        public string txn { get; set; }

        [XmlAttribute("ekycId")]
        public string ekycId { get; set; }
        [XmlAttribute("ekycIdType")]
        public string ekycIdType { get; set; }
        [XmlAttribute("aspId")]
        public string aspId { get; set; }
        [XmlAttribute("AuthMode")]
        public string AuthMode { get; set; }
        [XmlAttribute("responseSigType")]
        public string responseSigType { get; set; }

        [XmlAttribute("responseUrl")]
        public string responseUrl { get; set; }

        public docs Docs = new docs();
    }

    public class docs
    {
        public inputHash InputHash = new inputHash();
    }

    public class inputHash
    {
        [XmlAttribute("id")]
        public string id { get; set; }
        [XmlAttribute("hashAlgorithm")]
        public string hashAlgorithm { get; set; }
        [XmlAttribute("docInfo")]
        public string docInfo { get; set; }
        [XmlText]
        public string Hash { get; set; }
    }
}