Feature: Examples inside single Rule manipulation

Scenario: Adding examples to a Rule
	Given I started to create a new User Story with the name 'Doing Real Estate Business'
	And I added a new Rule that says 'Always be closing' 
	And I illustrated the Rule 'Always be closing' with the example 'Look, I've closed that one!'
	And I illustrated the Rule 'Always be closing' with the example 'And I've just closed two of'em.'
	When I complete editing the User Story 
	Then the 'Doing Real Estate Business' User Story should have the following Rules
	| Rule Text         | Rule Examples                                                    |
	| Always be closing | {Look, I've closed that one!}, {And I've just closed two of'em.} |


Scenario: Deleting an example from a Rule
	Given the User Story 'Doing Real Estate Business' has the following Rules
	| Rule Text         | Rule Examples                                                       |
	| Always be closing | {Look, I've closed that deal!}, {And I've just closed two of them.} |
	And I started to edit the User Story 'Doing Real Estate Business'
	When I delete the Example that says 'And I've just closed two of them.' from the Rule 'Always be closing'
	And I complete editing the User Story 
	Then the 'Doing Real Estate Business' User Story should have the following Rules
	| Rule Text         | Rule Examples                  |
	| Always be closing | {Look, I've closed that deal!} |

