using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using CMNirdesh.Models;


namespace CMNirdesh.Models
{


    public class Mail
    {
        public string From
        {
            get;
            set;
        }
        public string To
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
        public string Body
        {
            get;
            set;
        }
    }

}