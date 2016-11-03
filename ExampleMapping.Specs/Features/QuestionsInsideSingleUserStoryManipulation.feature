Feature: Questions inside single User Story manipulation

Scenario: Adding questions to a User Story
	Given I started to create a new User Story with the name 'Proverbs'
	And I added a new Rule that says 'Every cloud should have a silver lining'
	And I added a new Question that says 'What about thundercloud?'
	And I added a new Question that says 'How much does that silver cost in USD (approximately)?'
	When I complete editing the User Story 
	Then the 'Proverbs' User Story should have the following Questions
	| Question Text                                          |
	| What about thundercloud?                               |
	| How much does that silver cost in USD (approximately)? |


Scenario: Deleting a question from a User Story
	Given the User Story 'Some Proverbs' has the following Questions
	| Question Text                                          |
	| What about thundercloud?                               |
	| How much does that silver cost in USD (approximately)? |
	| I don't know what else to ask                          |
	And I started to edit the User Story 'Some Proverbs'
	When I delete the Question 'What about thundercloud?'
	And I complete editing the User Story 
	Then the 'Some Proverbs' User Story should have the following Questions
	| Question Text                                          |
	| How much does that silver cost in USD (approximately)? |
	| I don't know what else to ask                          |
