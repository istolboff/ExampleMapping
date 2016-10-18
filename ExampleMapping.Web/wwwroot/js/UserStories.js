function GetRuleCount() {
    var numberOfRules = 0;
    var inputElements = document.getElementsByTagName("input");
    var regex = new RegExp("^Rules\\[\\d+\\]\\.Name$");
    for (var i = 0; i !== inputElements.length; ++i) {
        if (regex.test(inputElements[i].name)) {
            ++numberOfRules;
        }
    }

    return numberOfRules;
}

function AddBlankRuleToUserStory() {
    var numberOfRules = GetRuleCount();
    var existingRulesContainer = document.getElementById("UserStoryContent");
    var newRuleText = document.createElement("input");
    newRuleText.type = "text";
    newRuleText.name = "Rules[" + numberOfRules + "].Name";
    existingRulesContainer.appendChild(newRuleText);
}