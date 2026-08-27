using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace PVSWebSite.Utility
{
    public class DALMSAccess
    {
        public byte[] SelectData(string MethodName, XmlNodeList objeParameter, string connectionString)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            DataSet dataSet = new DataSet();
            try
            {
                OleDbCommand oleDbCommand = new OleDbCommand(MethodName, connection);
                oleDbCommand.CommandType = CommandType.StoredProcedure;
                foreach (XmlElement elementToGetAttr in objeParameter)
                    oleDbCommand.Parameters.AddWithValue(XmlUtility.GetStringAttribute(elementToGetAttr, "Name"), (object)XmlUtility.GetStringAttribute(elementToGetAttr, "Value"));
                new OleDbDataAdapter()
                {
                    SelectCommand = oleDbCommand
                }.Fill(dataSet);
                return new EncryptionHelper().Encrypt(XmlUtility.DatasetToXml(dataSet));
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                connection.Close();
                dataSet.Dispose();
            }
        }

        public byte[] InsertData(string MethodName, XmlNodeList objeParameter, string connectionString)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            try
            {
                OleDbCommand oleDbCommand = new OleDbCommand(MethodName, connection);
                foreach (XmlElement elementToGetAttr in objeParameter)
                {
                    DataSet dataSet = XmlUtility.ConvertXMLToDataSet(XmlUtility.GetStringAttribute(elementToGetAttr, "Value"));
                    oleDbCommand.Parameters.AddWithValue(XmlUtility.GetStringAttribute(elementToGetAttr, "Name"), (object)dataSet.Tables[0]);
                }
                connection.Open();
                oleDbCommand.ExecuteNonQuery();
                return Encoding.ASCII.GetBytes("True");
            }
            catch (Exception ex)
            {
                return Encoding.ASCII.GetBytes(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}