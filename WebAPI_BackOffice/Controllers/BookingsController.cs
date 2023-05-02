using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using WebAPI_BackOffice.DB;
using WebAPI_BackOffice.Models;

namespace WebAPI_BackOffice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : Controller
    {
        BookingsRequest bookingsRequest = new BookingsRequest();
        [HttpGet("getBookingDetails")]
        public IEnumerable<Bookings> GetBookingsDetails()
        {
            DataTable dtResults = bookingsRequest.getBookingDetails();
            List<Bookings> employeeList = dtResults.ConvertDataTableToList<Bookings>();
            return employeeList;
        }
        [HttpGet("addBookings")]
        public bool AddBookingsDetails(string Name,string Address)
        {
            return bookingsRequest.AddBookingDetails(Name, Address);
        }

        [HttpGet("doOperations")]
        public bool DoOperations(string Type, int Id)
        {
            return bookingsRequest.DoOperations(Type, Id);
        }

        [NonAction]
        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty; 
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        [NonAction]
        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }


        [NonAction]
        public static List<T> BindList<T>(DataTable dt)
        {
            // Example 1:
            // Get private fields + non properties
            //var fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Example 2: Your case
            // Get all public fields
            var fields = typeof(T).GetFields();

            List<T> lst = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                // Create the object of T
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        // Matching the columns with fields
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            // Get the value from the datatable cell
                            object value = dr[dc.ColumnName];

                            // Set the value into the object
                            fieldInfo.SetValue(ob, value);
                            break;
                        }
                    }
                }

                lst.Add(ob);
            }

            return lst;
        }

    }
}
