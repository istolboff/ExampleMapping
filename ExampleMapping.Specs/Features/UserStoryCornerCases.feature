Feature: User Story Corner Cases

Scenario: It should be impossible to create a User Story with empty name
    Then Fail


Scenario: It should be impossible to add a rule with empty wording
    Then Fail

Scenario: It should be impossible to add a rule with the same description twice
	Given I started to create a new User Story with the name 'Proverbs'
	And I added a new Rule that says 'Every cloud should have a silver lining'
	When I try to add a new Rule that says 'Every cloud should have a silver lining'
	Then the addition of the rule should be refused with the explanation that 'The User Story "Proverbs" already has the rule "Every cloud should have a silver lining"'

