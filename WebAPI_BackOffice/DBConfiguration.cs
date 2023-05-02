using Microsoft.Extensions.Configuration;

namespace WebAPI_BackOffice
{
    public class DBConfiguration
    { 
        public string connectionString { get; set; }
        public DBConfiguration(string connString)
        {
            connectionString = connString;
        }
    }
}
