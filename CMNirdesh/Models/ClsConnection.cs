
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
namespace CMNirdesh.Models
{
    internal class ClsConnection
    {
        public static SqlConnection GetConnection()
        {
            // SqlConnection con = new SqlConnection(@"Data Source=10.147.213.80;initial catalog=MCdatabase;Persist Security Info=True;User ID=sa;Password=admin!123;");


            //SqlConnection con = new SqlConnection(@"Data Source=10.147.213.80;initial catalog=MCPatiala;Persist Security Info=True;User ID=sa;Password=admin!123;");
            //SqlConnection con = new SqlConnection(@"Data Source=10.147.213.80;initial catalog=MCPatiala11012024;Persist Security Info=True;User ID=sa;Password=admin!123;");

            //string str = "Data Source=LOKESH-PC\\MSSQLSERVER2014;Initial Catalog=HPMS_IntranetApp;Persist Security Info = True; User ID =sa;Password =123";


            //SqlConnection con = new SqlConnection(@"Data Source=10.249.97.201;initial catalog=PunjabAssemblyBook;Persist Security Info=True;User ID=sa;Password=eVidhanDb@123;");

            //SqlConnection con = new SqlConnection(@"Data Source=10.147.2.8;initial catalog=HPMS_IntranetAppCommon1;Persist Security Info=True;User ID=sa;Password=admin!123;");

            // SqlConnection con = new SqlConnection(@"Data Source=10.147.2.8;initial catalog=HPMS_IntranetAppCommon1;Persist Security Info=True;User ID=sa;Password=admin!123;");

            //SqlConnection con = new SqlConnection(@"Data Source=10.147.2.8;initial catalog=HPMS_IntranetAppCommon1;Persist Security Info=True;User ID=sa;Password=admin!123;");

            //SqlConnection con = new SqlConnection(@"Data Source=10.44.84.212;initial catalog=PVS_eConnect;Persist Security Info=True;User ID=sa;Password=eVidhanDb@123;");

            //SqlConnection con = new SqlConnection(@"Data Source=10.44.84.212;initial catalog=eNigamMC2;Persist Security Info=True;User ID=sa;Password=eVidhanDb@123;");
            //SqlConnection con = new SqlConnection(@"Data Source=10.147.213.80;initial catalog=eNigamMC2;Persist Security Info=True;User ID=sa;Password=admin!123;");
            //SqlConnection con = new SqlConnection(@"Data Source=enigamsqldb01.database.windows.net;initial catalog=eNigamMC;Persist Security Info=True;User ID=saadmin;Password=enigam@@2k50000Z;");
            //SqlConnection con = new SqlConnection(@"Data Source=192.168.2.6;initial catalog=eNigamMCPunjab;Persist Security Info=True;User ID=sa;Password=admin!123;");
            //   SqlConnection con = new SqlConnection("Data Source=HP\\SQLEXPRESS;Initial Catalog=eNigamMCPunjab;Integrated Security=true;TrustServerCertificate=true");
            //SqlConnection con = new SqlConnection(@"Data Source=10.44.86.184;initial catalog=eNigamMCPunjab;Persist Security Info=True;User ID=sa;Password=admin!123;");
            SqlConnection con = new SqlConnection(@"Data Source=10.44.86.184;initial catalog=eNigamMCPunjab;Persist Security Info=True;User ID=sa;Password=admin!123;");
            //SqlConnection con = new SqlConnection(@"Data Source=10.147.213.39;initial catalog=MC;Persist Security Info=True;User ID=sa;Password=Admin!123;");
            //SqlConnection con = new SqlConnection(@"Data Source=10.249.97.201;initial catalog=PunjabAssemblyBook;Persist Security Info=True;User ID=sa;Password=eVidhanDb@123;");

            //SqlConnection con = new SqlConnection(@"Data Source=10.44.86.141;Initial Catalog=Ecabinet;Persist Security Info=True;User ID=sa;Password=admin!123;");
            //SqlConnection con = new SqlConnection(@"Data Source=10.44.86.141;initial catalog=Ecabinet;Persist Security Info=True;User ID=sa;Password=admin!123;");
            // SqlConnection con = new SqlConnection(@"Data Source=10.147.30.109;initial catalog=HPMS_IntranetAppCommon1;Persist Security Info = True;User ID=sa;Password=admin!123;");
            return con;
        }

        //public static SqlConnection GetConnection2()
        //{
        //    // SqlConnection con = new SqlConnection(@"Data Source=10.147.213.80;initial catalog=MCdatabase;Persist Security Info=True;User ID=sa;Password=admin!123;");


        //    //SqlConnection con = new SqlConnection(@"Data Source=10.147.213.80;initial catalog=MCPatiala;Persist Security Info=True;User ID=sa;Password=admin!123;");
        //    //SqlConnection con = new SqlConnection(@"Data Source=10.147.213.80;initial catalog=MCPatiala11012024;Persist Security Info=True;User ID=sa;Password=admin!123;");

        //    //string str = "Data Source=LOKESH-PC\\MSSQLSERVER2014;Initial Catalog=HPMS_IntranetApp;Persist Security Info = True; User ID =sa;Password =123";


        //    //SqlConnection con = new SqlConnection(@"Data Source=10.249.97.201;initial catalog=PunjabAssemblyBook;Persist Security Info=True;User ID=sa;Password=eVidhanDb@123;");

        //    //SqlConnection con = new SqlConnection(@"Data Source=10.147.2.8;initial catalog=HPMS_IntranetAppCommon1;Persist Security Info=True;User ID=sa;Password=admin!123;");

        //    // SqlConnection con = new SqlConnection(@"Data Source=10.147.2.8;initial catalog=HPMS_IntranetAppCommon1;Persist Security Info=True;User ID=sa;Password=admin!123;");

        //    //SqlConnection con = new SqlConnection(@"Data Source=10.147.2.8;initial catalog=HPMS_IntranetAppCommon1;Persist Security Info=True;User ID=sa;Password=admin!123;");

        //    SqlConnection con = new SqlConnection(@"Data Source=10.44.84.212;initial catalog=PVS_IntranetApp;Persist Security Info=True;User ID=sa;Password=eVidhanDb@123;");



        //    // SqlConnection con = new SqlConnection(@"Data Source=enigamsqldb01.database.windows.net;initial catalog=eNigamMC;Persist Security Info=True;User ID=saadmin;Password=enigam@@2k50000Z;");


        //    //SqlConnection con = new SqlConnection(@"Data Source=10.147.2.8;initial catalog=HPMS_IntranetAppCommon1;Persist Security Info=True;User ID=sa;Password=admin!123;");
        //    //SqlConnection con = new SqlConnection(@"Data Source=10.249.97.201;initial catalog=PunjabAssemblyBook;Persist Security Info=True;User ID=sa;Password=eVidhanDb@123;");

        //    //SqlConnection con = new SqlConnection(@"Data Source=10.44.86.141;initial catalog=HPMS_IntranetAppCommon1;Persist Security Info=True;User ID=sa;Password=admin!123;");
        //    // SqlConnection con = new SqlConnection(@"Data Source=10.147.30.109;initial catalog=HPMS_IntranetAppCommon1;Persist Security Info = True;User ID=sa;Password=admin!123;");
        //    return con;
        //}

    }
}