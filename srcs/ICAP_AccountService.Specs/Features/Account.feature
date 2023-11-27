Feature: Account API

@mytag
Scenario: User gets added successfully
	When I create users with the following details
		| Name		| Email				|
		| Nordin	| Nording@gmail.com	|
		| Nadia		| Nadia@gmail.com	|
	Then the users should be successfully added