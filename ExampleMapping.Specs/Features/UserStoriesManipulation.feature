Feature: User Stories Manipulation

Scenario: Newly created User Story should appear in the list of all stories
	Given I have created a User Story with the name 'Keep it simple!'
	Then the list of all stories should contain only the following items
	  | User Story Name |
	  | Keep it simple! |


Scenario: Edited user story should change its name in the list of all stories
	Given I have created a User Story with the name 'My name will be changed'
	And I changed the name of the story from 'My name will be changed' to 'The name has changed'
	Then the list of all stories should contain only the following items
	  | User Story Name      |
	  | The name has changed |

Scenario: Adding several user stories
    Given I have created a User Story with the name 'First story'
	And I have created a User Story with the name 'Another story'
	And I have created a User Story with the name 'Yet another story'
	And I have created a User Story with the name 'Last story'
	Then the list of all stories should contain only the following items
	  | User Story Name   |
	  | First story       |
	  | Another story     |
	  | Yet another story |
	  | Last story        |

Scenario: Adding rules to a User Story
	Given I have started to create a new User Story with the name 'Proverbs'
	And I added a new Rule that says 'Every cloud should have a silver lining'
	And I added a new Rule that says 'One should never say never again'
	When I complete editing the User Story 
	Then the 'Proverbs' User Story should have the following content
	| Object Type | Object Description                      | 
	| Rule        | Every cloud should have a silver lining |
	| Rule        | One should never say never again        |

Scenario: It should be impossible to add a rule with the same description twice
	Given I have started to create a new User Story with the name 'Proverbs'
	And I added a new Rule that says 'Every cloud should have a silver lining'
	When I try to add a new Rule that says 'Every cloud should have a silver lining'
	Then the addition of the rule should be refused with the explanation that 'The User Story "Proverbs" already has the rule "Every cloud should have a silver lining"'

Scenario: Deleting a rule from a User Story
	Given the User Story 'Some Proverbs' and the following Rules
	| Rule Text                               |
	| Every cloud should have a silver lining |
	| One should never say never again        |
	| Better safe than sorry                  |
	And I have started to edit the User Story 'Some Proverbs'
	When I delete the Rule 'One should never say never again'
	And I complete editing the User Story 
	Then the 'Some Proverbs' User Story should have the following content
	| Object Type | Object Description                      |
	| Rule        | Every cloud should have a silver lining |
	| Rule        | Better safe than sorry                  |

