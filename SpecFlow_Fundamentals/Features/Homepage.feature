@Homepage
Feature: Homepage

A short summary of the feature

@tag1
Scenario: Verify that the home page is loading
Given I am on the login page
When I enter the default username and password
And I click the login button
Then I should be redirected to the inventory page
