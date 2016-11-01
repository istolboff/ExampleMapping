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

function newAspNetModelBindingFriendlyNamesSetter(ruleWordingClassName, exampleElementsGroupClassName) {
    // Generate html elements' names so that ASP.Net Core Model Binding could do its job.
    // (see https://docs.asp.net/en/latest/mvc/models/model-binding.html for details)

    return {
        RuleCount: function () {
            return document.getElementsByClassName(ruleWordingClassName).length;
        },

        SetRuleWordingElementName: function (ruleWordingHtmlElement) {
            ruleWordingHtmlElement.name = "Rules[" + this.RuleCount() + "].Name";
        },

        SetExampleWordingElementName: function (exampleWordingHtmlElement, ruleElementsGroup) {
            var ruleWordingHtmlElement = this.FindRuleHtmlElement(ruleElementsGroup);
            var existingExamplesCount = Enumerable.Count(
                ruleElementsGroup.children,
                function (htmlElement) { return htmlElement.className !== null && htmlElement.className === exampleElementsGroupClassName; });
            exampleWordingHtmlElement.name = ruleWordingHtmlElement.name.replace(".Name", ".Examples[" + existingExamplesCount + "].Name");
        },

        MakeRuleElementName: function (ruleIndex, backingPropertyName) {
            return "Rules[" + ruleIndex + "]." + backingPropertyName;
        },

        FindRuleHtmlElement: function (ruleElementsGroup) {
            return Enumerable.Single(
                ruleElementsGroup.children,
                function (htmlElement) { return htmlElement.className !== null && htmlElement.className === "ruleWording"; });
        }
    };
}

function newRuleOperations(ruleWordingClassName, exampleElementsGroupClassName, htmlElementNamesSetter) {
    return {
        DragDropProcessor: null,

        AddBlankRule: function () {
            var that = this;
            var dragDropProcessor = this.DragDropProcessor;

            var newRuleElementsGroup = document.createElement("div");
            newRuleElementsGroup.className = "ruleElementsGroup";
            newRuleElementsGroup.ondragover = function (event) { dragDropProcessor.DragOver(event, newRuleElementsGroup); };
            newRuleElementsGroup.ondrop = function (event) { dragDropProcessor.Drop(event, newRuleElementsGroup); };

            var newRuleText = document.createElement("input");
            newRuleText.type = "text";
            newRuleText.className = ruleWordingClassName;
            htmlElementNamesSetter.SetRuleWordingElementName(newRuleText);
            newRuleElementsGroup.appendChild(newRuleText);

            var newDeleteRuleButton = document.createElement("input");
            newDeleteRuleButton.type = "button";
            newDeleteRuleButton.className = "deleteRule";
            newDeleteRuleButton.value = "Delete";
            newDeleteRuleButton.onclick = function () { that.RemoveDomElement(newRuleElementsGroup); };
            newRuleElementsGroup.appendChild(newDeleteRuleButton);

            var existingRulesContainer = document.getElementById("UserStoryContent");
            existingRulesContainer.appendChild(newRuleElementsGroup);

            newRuleText.focus();
        },

        MarkRuleAsDeleted: function(ruleElementsGroupId) {
            var ruleElementsGroup = document.getElementById(ruleElementsGroupId);
            var ruleIdElement = Enumerable.Single(ruleElementsGroup.children, function (element) { return element.id !== null && element.id === "RuleId"; });
            ruleIdElement.value = -ruleIdElement.value;
            ruleElementsGroup.style.display = "none";
        },

        AddBlankExampleToRule: function (ruleElementsGroup) {
            var that = this;

            var newExampleElementsGroup = document.createElement("div");
            newExampleElementsGroup.className = exampleElementsGroupClassName;

            var newExampleText = document.createElement("input");
            newExampleText.type = "text";
            newExampleText.className = "exampleWording";
            htmlElementNamesSetter.SetExampleWordingElementName(newExampleText, ruleElementsGroup);
            newExampleElementsGroup.appendChild(newExampleText);

            var newDeleteExampleButton = document.createElement("input");
            newDeleteExampleButton.type = "button";
            newDeleteExampleButton.className = "deleteExample";
            newDeleteExampleButton.value = "Delete";
            newDeleteExampleButton.onclick = function () { that.RemoveDomElement(newExampleElementsGroup); };
            newExampleElementsGroup.appendChild(newDeleteExampleButton);

            ruleElementsGroup.appendChild(newExampleElementsGroup);

            newExampleText.focus();
        },

        MarkExampleAsDeleted: function (exampleElementsGroupId) {
            var exampleElementsGroup = document.getElementById(exampleElementsGroupId);
            var exampleIdElement = Enumerable.Single(exampleElementsGroup.children, function (element) { return element.id !== null && element.id === "ExampleId"; });
            exampleIdElement.value = -exampleIdElement.value;
            exampleElementsGroup.style.display = "none";
        },

        RemoveDomElement: function (domElement) {
            domElement.parentNode.removeChild(domElement);
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


function newGlobals() {
    var ruleWordingClassName = "ruleWording";
    var exampleElementsGroupClassName = "exampleElementsGroup";

    var htmlElementNamesSetter = newAspNetModelBindingFriendlyNamesSetter(ruleWordingClassName, exampleElementsGroupClassName);
    var ruleOperations = newRuleOperations(ruleWordingClassName, exampleElementsGroupClassName, htmlElementNamesSetter);
    var dragDropProcessor = newDragDropProcessor(ruleOperations);

    ruleOperations.DragDropProcessor = dragDropProcessor;

    return {
        RuleOperations: ruleOperations,
        DragDropProcessor: dragDropProcessor
    };
}

var Globals = newGlobals();
