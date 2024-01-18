Feature: Account Service Web API
![Calculator](https://specflow.org/wp-content/uploads/2020/09/calculator.png)
Simple calculator for adding **two** numbers

Link to a feature: [Calculator](ICAP_AccountService.Specs/Features/Calculator.feature)
***Further read***: **[Learn more about how to generate Living Documentation](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Generating-Documentation.html)**

Scenario: Get Accounts
	Given I am a client
	And the repository has user data
	When I make a GET request to '/users'
	Then the response status code should be '200'
	And the response json should be a list of user objects

Scenario: Add Account
	Given I am a client
	When I make a POST request to '/users'
	Then the response status code should be '201'
	And the response json should be the created user object