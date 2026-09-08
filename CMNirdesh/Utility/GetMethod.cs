using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace PVSWebSite.Utility
{
    public class GetMethod
    {
        private DALMSSql objDalMsSql = new DALMSSql();
        private DALMSAccess objDalMsAccess = new DALMSAccess();

        public byte[] GetData(ServiceParameters serviceParmeters)
        {
            string connection = ConfigurationManager.ConnectionStrings["pvsDatabase"].ConnectionString;
            switch (serviceParmeters.MethodType)
            {
                case "SelectMSSql":

                    return this.objDalMsSql.SelectData(serviceParmeters.MethodName, serviceParmeters.parameters, connection);
                case "InsertMSSqlTable":
                    return this.objDalMsSql.InsertDataTable(serviceParmeters.MethodName, serviceParmeters.parameters, connection);
                case "InsertMSSql":
                    return this.objDalMsSql.Insert(serviceParmeters.MethodName, serviceParmeters.parameters, connection);
                case "UpdateMSSql":
                    return this.objDalMsSql.Update(serviceParmeters.MethodName, serviceParmeters.parameters, connection);
                case "SelectMSAccess":
                    return this.objDalMsAccess.SelectData(serviceParmeters.MethodName, serviceParmeters.parameters, connection);
                case "InsertMSAccess":
                    return this.objDalMsAccess.InsertData(serviceParmeters.MethodName, serviceParmeters.parameters, connection);
                default:
                    return Encoding.ASCII.GetBytes("");
            }
        }
    }
}