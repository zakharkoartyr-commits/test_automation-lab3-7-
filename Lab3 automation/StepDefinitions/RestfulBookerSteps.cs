using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RestSharp;
using TechTalk.SpecFlow;
using Lab3_automation.Models;

namespace Lab3_automation.StepDefinitions
{
    [Binding]
    public class RestfulBookerSteps
    {
        private RestClient _client;
        private RestResponse _response;
        private string _token;
        private string _bookingId;

        [Given(@"I have a valid REST client for Restful Booker")]
        public void GivenClient()
        {
            var options = new RestClientOptions("https://restful-booker.herokuapp.com") { UserAgent = "MyClient" };
            _client = new RestClient(options);
        }

        [When(@"I authenticate with username ""(.*)"" and password ""(.*)""")]
        public void WhenAuth(string user, string pass)
        {
            var request = new RestRequest("/auth", Method.Post);
            request.AddJsonBody(new AuthRequest { Username = user, Password = pass });
            _response = _client.Execute(request);

            var json = JObject.Parse(_response.Content);
            _token = json["token"]?.ToString();
        }

        [Then(@"I should receive a valid auth token")]
        public void ThenToken()
        {
            Assert.IsFalse(string.IsNullOrEmpty(_token), "Auth Token should not be null or empty.");
        }

        [When(@"I create a new booking with firstname ""(.*)"", lastname ""(.*)"", totalprice (.*)")]
        public void WhenCreate(string fname, string lname, int price)
        {
            var request = new RestRequest("/booking", Method.Post);
            request.AddHeader("Accept", "application/json");

            var body = new BookingRequest
            {
                Firstname = fname,
                Lastname = lname,
                Totalprice = price,
                Depositpaid = true,
                Bookingdates = new BookingDates { Checkin = "2026-11-01", Checkout = "2026-11-10" }
            };

            request.AddJsonBody(body);
            _response = _client.Execute(request);

            var json = JObject.Parse(_response.Content);
            _bookingId = json["bookingid"]?.ToString();
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenStatus(int code)
        {
            Assert.AreEqual(code, (int)_response.StatusCode, $"Expected HTTP status {code}, but got {_response.StatusCode}");
        }

        [Then(@"the created booking should have valid ID")]
        public void ThenValidId()
        {
            Assert.IsFalse(string.IsNullOrEmpty(_bookingId), "Booking ID is missing.");
        }

        [When(@"I retrieve the booking by ID")]
        public void WhenGet()
        {
            var request = new RestRequest($"/booking/{_bookingId}", Method.Get);
            request.AddHeader("Accept", "application/json");
            _response = _client.Execute(request);
        }

        [Then(@"the firstname should be ""(.*)""")]
        public void ThenName(string name)
        {
            Assert.IsTrue(_response.Content.Contains(name), $"Response content does not contain name: {name}");
        }

        [When(@"I update the booking firstname to ""(.*)"" with PUT method")]
        public void WhenUpdatePut(string newName)
        {
            var request = new RestRequest($"/booking/{_bookingId}", Method.Put);
            request.AddHeader("Cookie", $"token={_token}");
            request.AddHeader("Accept", "application/json");

            var body = new BookingRequest
            {
                Firstname = newName,
                Lastname = "Zakharko",
                Totalprice = 150,
                Depositpaid = true,
                Bookingdates = new BookingDates { Checkin = "2026-11-01", Checkout = "2026-11-10" }
            };

            request.AddJsonBody(body);
            _response = _client.Execute(request);
        }

        [When(@"I patch the booking firstname to ""(.*)"" with PATCH method")]
        public void WhenPatch(string newName)
        {
            var request = new RestRequest($"/booking/{_bookingId}", Method.Patch);
            request.AddHeader("Cookie", $"token={_token}");
            request.AddHeader("Accept", "application/json");

            var body = new PatchBookingRequest
            {
                Firstname = newName
            };

            request.AddJsonBody(body);
            _response = _client.Execute(request);
        }

        [Then(@"the updated firstname should be ""(.*)""")]
        public void ThenUpdatedName(string name)
        {
            Assert.IsTrue(_response.Content.Contains(name), $"Updated name {name} not found in response.");
        }

        [When(@"I delete the booking by ID with auth token")]
        public void WhenDelete()
        {
            var request = new RestRequest($"/booking/{_bookingId}", Method.Delete);
            request.AddHeader("Cookie", $"token={_token}");
            request.AddHeader("Content-Type", "application/json");
            _response = _client.Execute(request);
        }

        [When(@"I retrieve the deleted booking by ID")]
        public void WhenGetDeleted()
        {
            var request = new RestRequest($"/booking/{_bookingId}", Method.Get);
            request.AddHeader("Accept", "application/json");
            _response = _client.Execute(request);
        }
    }
}