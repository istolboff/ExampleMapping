Feature: User Stories Manipulation

Scenario: Newly created User Story should appear in the list of all stories
	Given I have created a User Story with the name 'To be or not to be -- that is the question!'
	Then the list of all stories should contain only the following items
	  | User Story Name                             |
	  | To be or not to be -- that is the question! |