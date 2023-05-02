namespace WebAPI_BackOffice.Models
{
    public class Bookings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int status { get; set; }
        public string StatusMessage { get; set; }
    }
}
