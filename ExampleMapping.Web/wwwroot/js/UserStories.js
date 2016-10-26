function newRuleCollection() {
    return {
        Count: function () {
            var numberOfRules = 0;
            var inputElements = document.getElementsByTagName("input");
            var regex = new RegExp("^Rules\\[\\d+\\]\\.Name$");
            for (var i = 0; i !== inputElements.length; ++i) {
                if (regex.test(inputElements[i].name)) {
                    ++numberOfRules;
                }
            }

            return numberOfRules;
        },

        AddBlankRule: function () {
            var newRuleIndex = this.Count();

            var newRuleElementsGroup = document.createElement("div");

            var newRuleText = document.createElement("input");
            newRuleText.type = "text";
            newRuleText.className = "ruleWording";
            newRuleText.name = this.MakeRuleElementName(newRuleIndex, "Name");
            newRuleElementsGroup.appendChild(newRuleText);

            var thisClosure = this;
            var newDeleteRuleButton = document.createElement("input");
            newDeleteRuleButton.type = "button";
            newDeleteRuleButton.className = "deleteRule";
            newDeleteRuleButton.value = "Delete";
            newDeleteRuleButton.onclick = function () { thisClosure.RemoveRuleDomElements(newRuleElementsGroup); };
            newRuleElementsGroup.appendChild(newDeleteRuleButton);

            var existingRulesContainer = document.getElementById("UserStoryContent");
            existingRulesContainer.appendChild(newRuleElementsGroup);

            newRuleText.focus();
        },

        MarkRuleAsDeleted: function(ruleElementsGroupId) {
            var ruleElementsGroup = document.getElementById(ruleElementsGroupId);
            var ruleIdElement = this.Single(ruleElementsGroup.children, function (element) { return element.id !== null && element.id === "RuleId"; });
            ruleIdElement.value = -ruleIdElement.value;
            ruleElementsGroup.style.display = "none";
        },

        MakeRuleElementName: function (ruleIndex, backingPropertyName) {
            return "Rules[" + ruleIndex + "]." + backingPropertyName;
        },

        RemoveRuleDomElements: function (ruleElementsGroup) {
            ruleElementsGroup.parentNode.removeChild(ruleElementsGroup);
        },

        Single: function (arrayLike, predicate) {
            var matchingElement = null;
            var matchingElementFound = false;
            for (var i = 0; i !== arrayLike.length; ++i) {
                if (predicate(arrayLike[i])) {
                    if (matchingElementFound) {
                        throw "Program Logic Exception: non-unique element found.";
                    }

                    matchingElementFound = true;
                    matchingElement = arrayLike[i];
                }
            }

            if (!matchingElementFound) {
                throw "Program Logic Exception: single element matching the criteria was expected to be presetn in collection, but none was found.";
            }

            return matchingElement;
        }
    };
}

var AllRulesCollection = newRuleCollection();