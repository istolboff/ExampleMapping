﻿Feature: User Stories Manipulation

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
