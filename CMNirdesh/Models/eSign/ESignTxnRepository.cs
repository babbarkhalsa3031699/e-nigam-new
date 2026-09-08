using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models.eSign
{

    public class ESignTxnContext
    {
        public string TxnId { get; set; }
        public string AgendaId { get; set; }
        public string ApprovalId { get; set; }
        public string SignatureType { get; set; }
        public string PdfBlobPath { get; set; }
        public string RequestXML { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsConsumed { get; set; }
        public string DocuementHash { get; set; }
    }
    public class ESignTxnRepository
    {
       

        /* ---------------- SAVE ---------------- */

        public static void Save(ESignTxnContext ctx)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_ESignTxn_Save_Updated", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TxnId", ctx.TxnId);
                cmd.Parameters.AddWithValue("@AgendaId", (object)ctx.AgendaId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ApprovalId", (object)ctx.ApprovalId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SignatureType", (object)ctx.SignatureType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PdfBlobPath", (object)ctx.PdfBlobPath ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@RequestXML", (object)ctx.RequestXML ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", (object)ctx.CreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DocumentHash", ctx.DocuementHash);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /* ---------------- GET ---------------- */



        public static ESignTxnContext Get(string txnId)
        {
            if (string.IsNullOrWhiteSpace(txnId))
                return null;

            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_ESignTxn_Get", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TxnId", SqlDbType.VarChar, 50).Value = txnId;
              

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (!dr.Read())
                        return null;

                    return new ESignTxnContext
                    {
                        TxnId = dr["TxnId"] == DBNull.Value
                                    ? null
                                    : dr["TxnId"].ToString(),

                        AgendaId = dr["AgendaId"] == DBNull.Value
                                    ? null
                                    : dr["AgendaId"].ToString(),

                        ApprovalId = dr["ApprovalId"] == DBNull.Value
                                    ? null
                                    : dr["ApprovalId"].ToString(),

                        SignatureType = dr["SignatureType"] == DBNull.Value
                                    ? null
                                    : dr["SignatureType"].ToString(),

                        PdfBlobPath = dr["PdfBlobPath"] == DBNull.Value
                                    ? null
                                    : dr["PdfBlobPath"].ToString(),

                        RequestXML = dr["RequestXML"] == DBNull.Value
                                    ? null
                                    : dr["RequestXML"].ToString(),

                        CreatedBy = dr["CreatedBy"] == DBNull.Value
                                    ? null
                                    : dr["CreatedBy"].ToString(),

                        CreatedOn = dr["CreatedOn"] == DBNull.Value
                                    ? DateTime.MinValue
                                    : Convert.ToDateTime(dr["CreatedOn"]),

                        IsConsumed = dr["IsConsumed"] != DBNull.Value &&
                                     Convert.ToBoolean(dr["IsConsumed"])
                    };
                }
            }
        }


        public static ESignTxnContext GetTxn(string txnId, string UserID, string Hash)
        {
            if (string.IsNullOrWhiteSpace(txnId))
                return null;

            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_ESignTxn_Get_Updated", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TxnId", SqlDbType.VarChar, 50).Value = txnId;
                cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 50).Value = UserID;
                cmd.Parameters.Add("@Hash", SqlDbType.VarChar, 50).Value = Hash;

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (!dr.Read())
                        return null;

                    return new ESignTxnContext
                    {
                        TxnId = dr["TxnId"] == DBNull.Value
                                    ? null
                                    : dr["TxnId"].ToString(),

                        AgendaId = dr["AgendaId"] == DBNull.Value
                                    ? null
                                    : dr["AgendaId"].ToString(),

                        ApprovalId = dr["ApprovalId"] == DBNull.Value
                                    ? null
                                    : dr["ApprovalId"].ToString(),

                        SignatureType = dr["SignatureType"] == DBNull.Value
                                    ? null
                                    : dr["SignatureType"].ToString(),

                        PdfBlobPath = dr["PdfBlobPath"] == DBNull.Value
                                    ? null
                                    : dr["PdfBlobPath"].ToString(),

                        RequestXML = dr["RequestXML"] == DBNull.Value
                                    ? null
                                    : dr["RequestXML"].ToString(),

                        CreatedBy = dr["CreatedBy"] == DBNull.Value
                                    ? null
                                    : dr["CreatedBy"].ToString(),

                        CreatedOn = dr["CreatedOn"] == DBNull.Value
                                    ? DateTime.MinValue
                                    : Convert.ToDateTime(dr["CreatedOn"]),

                        IsConsumed = dr["IsConsumed"] != DBNull.Value &&
                                     Convert.ToBoolean(dr["IsConsumed"])
                    };
                }
            }
        }


        /* ---------------- MARK USED ---------------- */

        public static void MarkConsumed(string txnId)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_ESignTxn_MarkConsumed", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TxnId", txnId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
