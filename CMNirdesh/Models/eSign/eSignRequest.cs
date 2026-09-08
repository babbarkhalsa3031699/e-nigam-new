using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models.eSign
{
    public class eSignRequest
    {
        public int Id { get; set; }

        public string TxnId { get; set; }

        public int AgendaId { get; set; }

        public int? ApprovalId { get; set; }

        public string UserId { get; set; }

        public string XmlData { get; set; }

        public string BlobPath { get; set; }

        public string SignatureType { get; set; }

        public bool IsProcessed { get; set; }

        public string SignedPdfUrl { get; set; }

        public DateTime CreatedOn { get; set; }


        public static class eSignRequestModel
        {
            public static void Save(eSignRequest req)
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_eSignRequest_Save", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TxnId", req.TxnId);
                    cmd.Parameters.AddWithValue("@AgendaId", req.AgendaId);
                    cmd.Parameters.AddWithValue("@ApprovalId", (object)req.ApprovalId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UserId", req.UserId);
                    cmd.Parameters.AddWithValue("@XmlData", req.XmlData);
                    cmd.Parameters.AddWithValue("@BlobPath", req.BlobPath);
                    cmd.Parameters.AddWithValue("@SignatureType", (object)req.SignatureType ?? DBNull.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            public static eSignRequest GetByTxn(string txn)
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_eSignRequest_GetByTxn", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TxnId", txn);

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (!dr.Read())
                            return null;

                        return new eSignRequest
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            TxnId = dr["TxnId"].ToString(),
                            AgendaId = Convert.ToInt32(dr["AgendaId"]),
                            ApprovalId = dr["ApprovalId"] as int?,
                            UserId = dr["UserId"].ToString(),
                            XmlData = dr["XmlData"].ToString(),
                            BlobPath = dr["BlobPath"].ToString(),
                            SignatureType = dr["SignatureType"].ToString(),
                            IsProcessed = Convert.ToBoolean(dr["IsProcessed"]),
                            SignedPdfUrl = dr["SignedPdfUrl"]?.ToString()
                        };
                    }
                }
            }

            public static void MarkProcessed(string txn, string signedPdfUrl)
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_eSignRequest_MarkProcessed", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TxnId", txn);
                    cmd.Parameters.AddWithValue("@SignedPdfUrl", signedPdfUrl);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            public static void MarkFailed(string txn)
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_eSignRequest_MarkFailed", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TxnId", txn);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}