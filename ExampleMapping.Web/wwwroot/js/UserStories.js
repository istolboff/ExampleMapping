var Enumerable = {
    Count: function (arrayLike, predicate) {
        var result = 0;
        for (var i = 0; i !== arrayLike.length; ++i) {
            if (predicate(arrayLike[i])) {
                ++result;
            }
        }

        return result;
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

function newAspNetModelBindingFriendlyNamesSetter(ruleWordingClassName, exampleElementsGroupClassName, questionWordingClassName) {
    // Generate html elements' names so that ASP.Net Core Model Binding could do its job.
    // (see https://docs.asp.net/en/latest/mvc/models/model-binding.html for details)

    return {
        SetRuleWordingElementName: function (ruleWordingHtmlElement) {
            var exisitngRulesCount = document.getElementsByClassName(ruleWordingClassName).length;
            ruleWordingHtmlElement.name = "Rules[" + exisitngRulesCount + "].Name";
        },

        SetExampleWordingElementName: function (exampleWordingHtmlElement, ruleElementsGroup) {
            var ruleWordingHtmlElement = this.FindRuleHtmlElement(ruleElementsGroup);
            var existingExamplesCount = Enumerable.Count(
                ruleElementsGroup.children,
                function (htmlElement) { return htmlElement.className !== null && htmlElement.className === exampleElementsGroupClassName; });
            exampleWordingHtmlElement.name = ruleWordingHtmlElement.name.replace(".Name", ".Examples[" + existingExamplesCount + "].Name");
        },

        SetQuestionWordingElementName: function (questionWordingHtmlElement) {
            var exisitngQuestionsCount = document.getElementsByClassName(questionWordingClassName).length;
            questionWordingHtmlElement.name = "Questions[" + exisitngQuestionsCount + "].Name";
        },

        FindRuleHtmlElement: function (ruleElementsGroup) {
            return Enumerable.Single(
                ruleElementsGroup.children,
                function (htmlElement) { return htmlElement.className !== null && htmlElement.className === "ruleWording"; });
        }
    };
}

function newUserStoryContentOperations(ruleWordingClassName, exampleElementsGroupClassName, htmlElementNamesSetter) {
    return {
        _dragDropProcessor: null,

        AddBlankRule: function () {
            var dragDropProcessor = this._dragDropProcessor;

            var newRuleElementsGroup = document.createElement("div");
            newRuleElementsGroup.className = "ruleElementsGroup";
            newRuleElementsGroup.ondragover = function (event) { dragDropProcessor.DragOver(event, newRuleElementsGroup); };
            newRuleElementsGroup.ondrop = function (event) { dragDropProcessor.Drop(event, newRuleElementsGroup); };

            var newRuleText = this.AddEntityTextElement(ruleWordingClassName, newRuleElementsGroup);
            htmlElementNamesSetter.SetRuleWordingElementName(newRuleText);

            this.AddEntityDeleterElement("deleteRule", newRuleElementsGroup);

            var existingRulesContainer = document.getElementById("UserStoryContent");
            existingRulesContainer.appendChild(newRuleElementsGroup);

            newRuleText.focus();
        },

        MarkRuleAsDeleted: function (ruleElementsGroupId) {
            this.MarkEntityAsDeleted(ruleElementsGroupId, "RuleId");
        },

        AddBlankExampleToRule: function (ruleElementsGroup) {
            var newExampleElementsGroup = document.createElement("div");
            newExampleElementsGroup.className = exampleElementsGroupClassName;

            var newExampleText = this.AddEntityTextElement("exampleWording", newExampleElementsGroup);
            htmlElementNamesSetter.SetExampleWordingElementName(newExampleText, ruleElementsGroup);

            this.AddEntityDeleterElement("deleteExample", newExampleElementsGroup);

            ruleElementsGroup.appendChild(newExampleElementsGroup);

            newExampleText.focus();
        },

        MarkExampleAsDeleted: function (exampleElementsGroupId) {
            this.MarkEntityAsDeleted(exampleElementsGroupId, "ExampleId");
        },

        AddBlankQuestion: function() {
            var newQuestionElementsGroup = document.createElement("div");
            newQuestionElementsGroup.className = "questionElementsGroup";

            var newQuestionText = this.AddEntityTextElement("questionWording", newQuestionElementsGroup);
            htmlElementNamesSetter.SetQuestionWordingElementName(newQuestionText);

            this.AddEntityDeleterElement("deleteRule", newQuestionElementsGroup);

            var existingRulesContainer = document.getElementById("UserStoryContent");
            existingRulesContainer.appendChild(newQuestionElementsGroup);

            newQuestionText.focus();
        },

        MarkQuestionAsDeleted: function (questionElementsGroupId) {
            this.MarkEntityAsDeleted(questionElementsGroupId, "QuestionId");
        },

        AddEntityTextElement: function(inputElementClassName, entityElementsGroup) {
            var entityText = document.createElement("input");
            entityText.type = "text";
            entityText.className = inputElementClassName;
            entityElementsGroup.appendChild(entityText);
            return entityText;
        },

        AddEntityDeleterElement: function (deleterClassName, entityElementsGroup) {
            var newDeleteEntityButton = document.createElement("input");
            newDeleteEntityButton.type = "button";
            newDeleteEntityButton.className = deleterClassName;
            newDeleteEntityButton.value = "Delete";
            newDeleteEntityButton.onclick = function () { entityElementsGroup.parentNode.removeChild(entityElementsGroup); };
            entityElementsGroup.appendChild(newDeleteEntityButton);
        },

        MarkEntityAsDeleted: function (entityElementsGroupId, idOfhiddenInputWithEndtityId) {
            var entityElementsGroup = document.getElementById(entityElementsGroupId);
            var entityIdElement = Enumerable.Single(entityElementsGroup.children, function (element) { return element.id !== null && element.id === idOfhiddenInputWithEndtityId; });
            entityIdElement.value = -entityIdElement.value;
            entityElementsGroup.style.display = "none";
        }
    };
}

function newDragDropProcessor(ruleOperations) {
    var constRuleType = "95D959BF-8F42-4F22-BBDA-2CC38BFA9548";
    var constExampleType = "A24BC9D5-4DB9-42BF-86DE-E31C0C585044";

    return {
        StartDraggingBlankRule: function (dragEvent) {
            dragEvent.dataTransfer.setData("text", constRuleType);
        },

        StartDraggingBlankExample: function (dragEvent) {
            dragEvent.dataTransfer.setData("text", constExampleType);
        },

        DragOver: function (dragEvent, dropTargetHtmlElement) {
            if (this.CanDrop(dragEvent, dropTargetHtmlElement)) {
                dragEvent.preventDefault();
            }
        },

        Drop: function (dropEvent, dropTargetHtmlElement) {
            if (!this.CanDrop(dropEvent, dropTargetHtmlElement)) {
                return;
            }

            dropEvent.preventDefault();

            if (dropTargetHtmlElement.className === "ruleElementsGroup") {
                ruleOperations.AddBlankExampleToRule(dropTargetHtmlElement);
            }
        },

        CanDrop: function (dragEvent, dropTargetHtmlElement) {
            return dragEvent.dataTransfer.getData("text") === constExampleType && dropTargetHtmlElement.className === "ruleElementsGroup";
        }
    };
}

var Globals = function () {
    var ruleWordingClassName = "ruleWording";
    var exampleElementsGroupClassName = "exampleElementsGroup";
    var questionWordingClassName = "questionWording";

    var htmlElementNamesSetter = newAspNetModelBindingFriendlyNamesSetter(ruleWordingClassName, exampleElementsGroupClassName, questionWordingClassName);
    var userStoryContentOperations = newUserStoryContentOperations(ruleWordingClassName, exampleElementsGroupClassName, htmlElementNamesSetter);
    var dragDropProcessor = newDragDropProcessor(userStoryContentOperations);

    userStoryContentOperations._dragDropProcessor = dragDropProcessor;

    return {
        UserStoryContentOperations: userStoryContentOperations,
        DragDropProcessor: dragDropProcessor
    };
}();