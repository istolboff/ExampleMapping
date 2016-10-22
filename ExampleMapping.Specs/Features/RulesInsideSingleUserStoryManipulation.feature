Feature: Rules inside single User Story manipulation

Scenario: Adding rules to a User Story
	Given I started to create a new User Story with the name 'Proverbs'
	And I added a new Rule that says 'Every cloud should have a silver lining'
	And I added a new Rule that says 'One should never say never again'
	When I complete editing the User Story 
	Then the 'Proverbs' User Story should have the following Rules
	| Rule Text                               |
	| Every cloud should have a silver lining |
	| One should never say never again        |


Scenario: Deleting a rule from a User Story
	Given the User Story 'Some Proverbs' has the following Rules
	| Rule Text                               |
	| Every cloud should have a silver lining |
	| One should never say never again        |
	| Better safe than sorry                  |
	And I started to edit the User Story 'Some Proverbs'
	When I delete the Rule 'One should never say never again'
	And I complete editing the User Story 
	Then the 'Some Proverbs' User Story should have the following Rules
	| Rule Text                               |
	| Every cloud should have a silver lining |
	| Better safe than sorry                  |

