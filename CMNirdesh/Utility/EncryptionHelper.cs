using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Web;
using System.Xml;

namespace PVSWebSite.Utility
{
    public class EncryptionHelper
    {
        private static string RSAPUBLICKEY = "";
        private static string RSAPRIVATEKEY = "";

        public static string GenerateKeys()
        {
            try
            {
                using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider())
                {
                    EncryptionHelper.RSAPUBLICKEY = cryptoServiceProvider.ToXmlString(false);
                    EncryptionHelper.RSAPRIVATEKEY = cryptoServiceProvider.ToXmlString(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EncryptionHelper.RSAPUBLICKEY;
        }

        public XmlDocument EncryptAsymmetric(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.XmlResolver = null;
            document.LoadXml(xml);
            using (RSACryptoServiceProvider keyObject = new RSACryptoServiceProvider())
            {
                XmlElement documentElement = document.DocumentElement;
                EncryptedXml encryptedXml = new EncryptedXml(document);
                EncryptionHelper.RSAPUBLICKEY = "<RSAKeyValue><Modulus>65pXiiOp7BuM6C90o0/37+iwyITRyy4G8ZKaXM+CQz+cIKDlHj1MoQSqtmSPP8VtxnfZmxCt55/GGRGN5yk3BmukH56NyI03C3ROY8Jj8y1eZSAPfEutLeYYmKA/lx6C4nEoKErfSURBKmGWTc4RK0Ypz2z8WzM4dlMPt/GIQsE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
                keyObject.FromXmlString(EncryptionHelper.RSAPUBLICKEY);
                encryptedXml.AddKeyNameMapping("asyncKey", (object)keyObject);
                EncryptedData encryptedData = encryptedXml.Encrypt(documentElement, "asyncKey");
                EncryptedXml.ReplaceElement(documentElement, encryptedData, false);
            }
            string outerXml = document.OuterXml;
            return document;
        }

        public XmlDocument DecryptAsymmetric(string xml)
        {
            string xmlString = "<RSAKeyValue><Modulus>65pXiiOp7BuM6C90o0/37+iwyITRyy4G8ZKaXM+CQz+cIKDlHj1MoQSqtmSPP8VtxnfZmxCt55/GGRGN5yk3BmukH56NyI03C3ROY8Jj8y1eZSAPfEutLeYYmKA/lx6C4nEoKErfSURBKmGWTc4RK0Ypz2z8WzM4dlMPt/GIQsE=</Modulus><Exponent>AQAB</Exponent><P>+Fi+aJF9BL7j8RwfaSGieG0HnXstMORKvK/01vKECPiJ7I30Ka+5MO9BcnZmOaG7IGfRfK1UyvTxDl25QzP9Ew==</P><Q>8t0PZLmV/afEU2o7ka3LLdY6dokMNPmpUPE43sOZw7GKT85qgEtBqvWwhnPig2N+owZ64WXOtLbx5Ec8CPgfWw==</Q><DP>cmYGyAqEyWPZgl6PBZGt0sV+pYdxKL1ww/xVz5IFSlCa0DIP0AgXSbhcsIpjypZ6qZHJSSJbFebBB/oadh+Dqw==</DP><DQ>IswFnprwoK1e9cysyEysZd7h9YXhV93ForFNQq2n5GAVvyWGIOenewVEy57i/4xL4rPU+2KI4V+s/NYwBeD3LQ==</DQ><InverseQ>ignVViW/J8blktdP24Ir1ZHrQvsSr0iT/MKThPl6Bo8IOv5lthZC4+8jzPC0ULu8ASgYCDQO1pr0sB9jLW3pXA==</InverseQ><D>id+4es6EEffNbdLXnvqdTXgOfEm9u/kjdxsj2kxVHqWK6E0/x4J35YKlpDcU3Wzb4NkiLuyD2JhXhTQQpsYnvCqsZJThRPwHFwRPO1QQRlPaPZg+vtddfax/tcGq0aRkC8g5ubS95mSMEzat9AkVxC+NLBT9xer8yQITd/TtCEE=</D></RSAKeyValue>";
            XmlDocument document = new XmlDocument();
            document.XmlResolver = null;
            document.LoadXml(xml);
            EncryptedXml encryptedXml = new EncryptedXml(document);
            using (RSACryptoServiceProvider keyObject = new RSACryptoServiceProvider())
            {
                keyObject.FromXmlString(xmlString);
                encryptedXml.AddKeyNameMapping("asyncKey", (object)keyObject);
                encryptedXml.DecryptDocument();
            }
            return document;
        }

        public byte[] EncryptXmlToByte(string xml)
        {
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            EncryptionHelper.RSAPUBLICKEY = "<RSAKeyValue><Modulus>65pXiiOp7BuM6C90o0/37+iwyITRyy4G8ZKaXM+CQz+cIKDlHj1MoQSqtmSPP8VtxnfZmxCt55/GGRGN5yk3BmukH56NyI03C3ROY8Jj8y1eZSAPfEutLeYYmKA/lx6C4nEoKErfSURBKmGWTc4RK0Ypz2z8WzM4dlMPt/GIQsE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            cryptoServiceProvider.FromXmlString(EncryptionHelper.RSAPUBLICKEY);
            byte[] bytes = Encoding.ASCII.GetBytes(xml);
            return cryptoServiceProvider.Encrypt(bytes, false);

        }


        public string DecryptXml(byte[] xml)
        {
            string xmlString = "<RSAKeyValue><Modulus>65pXiiOp7BuM6C90o0/37+iwyITRyy4G8ZKaXM+CQz+cIKDlHj1MoQSqtmSPP8VtxnfZmxCt55/GGRGN5yk3BmukH56NyI03C3ROY8Jj8y1eZSAPfEutLeYYmKA/lx6C4nEoKErfSURBKmGWTc4RK0Ypz2z8WzM4dlMPt/GIQsE=</Modulus><Exponent>AQAB</Exponent><P>+Fi+aJF9BL7j8RwfaSGieG0HnXstMORKvK/01vKECPiJ7I30Ka+5MO9BcnZmOaG7IGfRfK1UyvTxDl25QzP9Ew==</P><Q>8t0PZLmV/afEU2o7ka3LLdY6dokMNPmpUPE43sOZw7GKT85qgEtBqvWwhnPig2N+owZ64WXOtLbx5Ec8CPgfWw==</Q><DP>cmYGyAqEyWPZgl6PBZGt0sV+pYdxKL1ww/xVz5IFSlCa0DIP0AgXSbhcsIpjypZ6qZHJSSJbFebBB/oadh+Dqw==</DP><DQ>IswFnprwoK1e9cysyEysZd7h9YXhV93ForFNQq2n5GAVvyWGIOenewVEy57i/4xL4rPU+2KI4V+s/NYwBeD3LQ==</DQ><InverseQ>ignVViW/J8blktdP24Ir1ZHrQvsSr0iT/MKThPl6Bo8IOv5lthZC4+8jzPC0ULu8ASgYCDQO1pr0sB9jLW3pXA==</InverseQ><D>id+4es6EEffNbdLXnvqdTXgOfEm9u/kjdxsj2kxVHqWK6E0/x4J35YKlpDcU3Wzb4NkiLuyD2JhXhTQQpsYnvCqsZJThRPwHFwRPO1QQRlPaPZg+vtddfax/tcGq0aRkC8g5ubS95mSMEzat9AkVxC+NLBT9xer8yQITd/TtCEE=</D></RSAKeyValue>";
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            cryptoServiceProvider.FromXmlString(xmlString);
            return Encoding.ASCII.GetString(cryptoServiceProvider.Decrypt(xml, false));
        }

        public byte[] Encrypt(string xml)
        {
            string str = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("+", "");
            byte[] bytes = Encoding.ASCII.GetBytes(EncryptionHelper.EncryptStringSymmetric(xml, str));
            byte[] src = this.EncryptXmlToByte(str);
            byte[] dst = new byte[bytes.Length + src.Length];
            Buffer.BlockCopy((Array)bytes, 0, (Array)dst, 0, bytes.Length);
            Buffer.BlockCopy((Array)src, 0, (Array)dst, bytes.Length, src.Length);
            return dst;
        }

        public string Decrypt(byte[] data)
        {
            byte[] numArray1 = new byte[128];
            byte[] numArray2 = new byte[data.Length - 128];
            Buffer.BlockCopy((Array)data, data.Length - 128, (Array)numArray1, 0, 128);
            string Password = this.DecryptXml(numArray1);
            Buffer.BlockCopy((Array)data, 0, (Array)numArray2, 0, data.Length - 128);
            return EncryptionHelper.DecryptString(Encoding.ASCII.GetString(numArray2), Password);
        }

        private static string EncryptStringSymmetric(string InputText, string Password)
        {            
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            byte[] bytes1 = Encoding.Unicode.GetBytes(InputText);
            byte[] bytes2 = Encoding.ASCII.GetBytes(Password.Length.ToString());
            PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, bytes2);
            ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(bytes1, 0, bytes1.Length);
            cryptoStream.FlushFinalBlock();
            byte[] array = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(array);
        }

        private static string DecryptString(string InputText, string Password)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            byte[] buffer = Convert.FromBase64String(InputText);
            byte[] bytes = Encoding.ASCII.GetBytes(Password.Length.ToString());
            PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, bytes);
            ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream(buffer);
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] numArray = new byte[buffer.Length];
            int count = cryptoStream.Read(numArray, 0, numArray.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.Unicode.GetString(numArray, 0, count);
        }
    }
}