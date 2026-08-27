using iTextSharp.text.pdf;
using System.IO;

namespace CMNirdesh.Models
{
    public class ASPSession
    {
        // Singleton instance of ASPSession
        private static readonly ASPSession _current = new ASPSession();

        // Private constructor to prevent direct instantiation
        private ASPSession() { }

        // Public property to access the singleton instance
        public static ASPSession Current => _current;

        // Properties to store session-related data
        public string Txn { get; set; }
        public PdfReader Reader { get; set; }
        public MemoryStream Fout { get; set; }
        public PdfSignatureAppearance Appearance { get; set; }
        public string XmlData { get; set; }
        public string AgendaID { get; set; }
        public string ApprovalId { get; set; }
        public string SignatureType { get; set; }
    }
}
