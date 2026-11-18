Feature: Riot Games Public API Testing

  Scenario: Verify Champion Rotation Endpoint Authorization and Data
    Given I set up the Riot Games API client
    When I request champion data with API key "RGAPI-463be794-3e55-4e6c-a978-6cd2519e4fc6"
    Then the Riot API response should return a valid HTTP status code