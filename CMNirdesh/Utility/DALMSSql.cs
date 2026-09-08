using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace PVSWebSite.Utility
{
    public class DALMSSql
    {

        private EncryptionHelper xmlencrypt = new EncryptionHelper();

        public byte[] SelectData(string MethodName, XmlNodeList objeParameter, string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            DataSet dataSet = new DataSet();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(MethodName, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (XmlElement elementToGetAttr in objeParameter)
                {
                    if (XmlUtility.GetStringAttribute(elementToGetAttr, "Value") == "")
                        sqlCommand.Parameters.AddWithValue(XmlUtility.GetStringAttribute(elementToGetAttr, "Name"), (object)DBNull.Value);
                    else if (XmlUtility.GetStringAttribute(elementToGetAttr, "Value") == "EmptyString")
                        sqlCommand.Parameters.AddWithValue(XmlUtility.GetStringAttribute(elementToGetAttr, "Name"), (object)"");
                    else
                        sqlCommand.Parameters.AddWithValue(XmlUtility.GetStringAttribute(elementToGetAttr, "Name"), (object)XmlUtility.GetStringAttribute(elementToGetAttr, "Value"));
                }
                new SqlDataAdapter() { SelectCommand = sqlCommand }.Fill(dataSet);
                return new EncryptionHelper().Encrypt(XmlUtility.DatasetToXml(dataSet));
            }
            catch (Exception ex)
            {
                return new EncryptionHelper().Encrypt(ex.Message);
            }
            finally
            {
                connection.Close();
                dataSet.Dispose();
            }
        }

        public byte[] InsertDataTable(
          string MethodName,
          XmlNodeList objeParameter,
          string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                SqlCommand sqlCommand = new SqlCommand(MethodName, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (XmlElement elementToGetAttr in objeParameter)
                {
                    DataSet dataSet = XmlUtility.ConvertXMLToDataSet(XmlUtility.GetStringAttribute(elementToGetAttr, "Value"));
                    sqlCommand.Parameters.AddWithValue(XmlUtility.GetStringAttribute(elementToGetAttr, "Name"), (object)dataSet.Tables[0]);
                }
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                return this.xmlencrypt.Encrypt("True");
            }
            catch (Exception ex)
            {
                return this.xmlencrypt.Encrypt(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public byte[] Insert(string MethodName, XmlNodeList objeParameter, string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                SqlCommand sqlCommand = new SqlCommand(MethodName, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (XmlElement elementToGetAttr in objeParameter)
                {
                    if (XmlUtility.GetStringAttribute(elementToGetAttr, "Value") == "")
                        sqlCommand.Parameters.AddWithValue(XmlUtility.GetStringAttribute(elementToGetAttr, "Name"), (object)DBNull.Value);
                    else if (XmlUtility.GetStringAttribute(elementToGetAttr, "Value") == "EmptyString")
                        sqlCommand.Parameters.AddWithValue(XmlUtility.GetStringAttribute(elementToGetAttr, "Name"), (object)"");
                    else
                        sqlCommand.Parameters.AddWithValue(XmlUtility.GetStringAttribute(elementToGetAttr, "Name"), (object)XmlUtility.GetStringAttribute(elementToGetAttr, "Value"));
                }
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                return this.xmlencrypt.Encrypt("True");
            }
            catch (Exception ex)
            {
                return this.xmlencrypt.Encrypt(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public byte[] Update(string MethodName, XmlNodeList objeParameter, string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                SqlCommand sqlCommand = new SqlCommand(MethodName, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (XmlElement elementToGetAttr in objeParameter)
                {
                    if (XmlUtility.GetStringAttribute(elementToGetAttr, "Value") == "")
                        sqlCommand.Parameters.AddWithValue(XmlUtility.GetStringAttribute(elementToGetAttr, "Name"), (object)DBNull.Value);
                    else if (XmlUtility.GetStringAttribute(elementToGetAttr, "Value") == "EmptyString")
                        sqlCommand.Parameters.AddWithValue(XmlUtility.GetStringAttribute(elementToGetAttr, "Name"), (object)"");
                    else
                        sqlCommand.Parameters.AddWithValue(XmlUtility.GetStringAttribute(elementToGetAttr, "Name"), (object)XmlUtility.GetStringAttribute(elementToGetAttr, "Value"));
                }
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                return this.xmlencrypt.Encrypt("True");
            }
            catch (Exception ex)
            {
                return this.xmlencrypt.Encrypt(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}