@Authentcation
Feature: Login

In order to access the site's features
  As a user
  I want to be able to log in to the site
 

Scenario: Successful login with valid credentials
  Given I am on the login page
  When I enter the default username and password
  And I click the login button
  Then I should be redirected to the inventory page

Scenario: Unsuccessful login with incorrect credentials
  Given I am on the login page
  When I enter the incorrect username and incorrect password
  And I click the login button
  Then I should see the error message
  And the error message shoud contain the text "Epic sadface: Username and password do not match any user in this service"

Scenario: Successful login locked out user error
  Given I am on the login page
  When I enter the locked out username and password
  And I click the login button
  Then I should see the error message
  And the error message shoud contain the text "Epic sadface: Sorry, this user has been locked out."

Scenario: Successful login problem user
  Given I am on the login page
  When I enter the problem username and password
  And I click the login button
  Then I should be redirected to the inventory page

Scenario: Successful login performace user
  Given I am on the login page
  When I enter the performance glitch username and password
  And I click the login button
  Then I should be redirected to the inventory page

Scenario: Successful login error user
  Given I am on the login page
  When I enter the error user username and password
  And I click the login button
  Then I should be redirected to the inventory page

Scenario: Successful login visual user
  Given I am on the login page
  When I enter the visual user username and password
  And I click the login button
  Then I should be redirected to the inventory page
