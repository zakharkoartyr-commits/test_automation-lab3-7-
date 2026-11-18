using Newtonsoft.Json;

namespace Lab3_automation.Models
{
    public class BookingDates
    {
        [JsonProperty("checkin")]
        public string Checkin { get; set; }

        [JsonProperty("checkout")]
        public string Checkout { get; set; }
    }

    public class BookingRequest
    {
        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("totalprice")]
        public int Totalprice { get; set; }

        [JsonProperty("depositpaid")]
        public bool Depositpaid { get; set; }

        [JsonProperty("bookingdates")]
        public BookingDates Bookingdates { get; set; }

        [JsonProperty("additionalneeds")]
        public string Additionalneeds { get; set; }
    }

    public class PatchBookingRequest
    {
        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }
    }
}