Feature: Restful Booker API CRUD Operations

  Scenario: Successful Full CRUD and PATCH Lifecycle for Booking
    Given I have a valid REST client for Restful Booker
    When I authenticate with username "admin" and password "password123"
    Then I should receive a valid auth token
    
    When I create a new booking with firstname "Artur", lastname "Zakharko", totalprice 150
    Then the response status code should be 200
    And the created booking should have valid ID

    When I retrieve the booking by ID
    Then the response status code should be 200
    And the firstname should be "Artur"

    When I update the booking firstname to "Artur_Updated" with PUT method
    Then the response status code should be 200
    And the updated firstname should be "Artur_Updated"

    When I patch the booking firstname to "Artur_Patched" with PATCH method
    Then the response status code should be 200
    And the updated firstname should be "Artur_Patched"

    When I delete the booking by ID with auth token
    Then the response status code should be 201

    When I retrieve the deleted booking by ID
    Then the response status code should be 404