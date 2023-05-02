using MauiAuth0App.Auth0;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace WebAPI_BackOffice.DB
{

    public class BookingsRequest
    {

        public DataTable getBookingDetails()
        {
            SqlConnection sqlConnection = new SqlConnection("Server=.;Database=FRNSWdemo;uid=sa;pwd=pnws@123;");
            SqlCommand cmd = new SqlCommand("sp_getBookingDetails", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }
        public bool AddBookingDetails(string Name, string Address)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Server=.;Database=FRNSWdemo;uid=sa;pwd=pnws@123;"))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("sp_add_request", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Address", Address);
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                    return true;
                else
                    return false;
            }
        }
        public bool DoOperations(string Type, int Id)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Server=.;Database=FRNSWdemo;uid=sa;pwd=pnws@123;"))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("sp_Do_Operations", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", Type);
                cmd.Parameters.AddWithValue("@id", Id);
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
