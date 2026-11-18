using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RestSharp;
using TechTalk.SpecFlow;

namespace Lab3_automation.StepDefinitions
{
    [Binding]
    public class RiotApiSteps
    {
        private RestClient _client;
        private RestResponse _response;

        [Given(@"I set up the Riot Games API client")]
        public void GivenClient()
        {
            _client = new RestClient("https://euw1.api.riotgames.com");
        }

        [When(@"I request champion data with API key ""(.*)""")]
        public void WhenRequest(string apiKey)
        {
            var request = new RestRequest("/lol/platform/v3/champion-rotations", Method.Get);
            request.AddHeader("X-Riot-Token", apiKey);
            _response = _client.Execute(request);
        }

        [Then(@"the Riot API response should return a valid HTTP status code")]
        public void ThenVerifyResponse()
        {
            int statusCode = (int)_response.StatusCode;

            Console.WriteLine($"---> [Riot API Status]: {statusCode} ({_response.StatusCode})");
            Console.WriteLine($"---> [Riot API Content]: {_response.Content}");

            Assert.IsTrue(statusCode == 200 || statusCode == 401 || statusCode == 403,
                $"Unexpected HTTP status code: {statusCode}");

            var json = JObject.Parse(_response.Content);

            if (statusCode == 200)
            {
                Assert.IsTrue(json["sr"] is JArray sr && sr.Count > 0,
                    "Expected non-empty 'sr' array in response.");
                Assert.IsTrue(json["newplayer"] is JArray np && np.Count > 0,
                    "Expected non-empty 'newplayer' array in response.");
            }
            else
            {
                Assert.IsNotNull(json["status"], "Riot API error response structure expected.");
                Assert.IsNotNull(json["status"]["message"], "Expected 'status.message' in error response.");
                Assert.AreEqual(statusCode, (int?)json["status"]["status_code"],
                    "status_code in body should match HTTP status.");
            }
        }
    }
}