using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.WatinExtensions
{
    internal sealed class DragAndDrop
    {
        public DragAndDrop(Document htmlDocument, Element draggedElement)
        {
            _htmlDocument = htmlDocument;
            _draggedElement = draggedElement;
        }

        public void DropTo(Element element)
        {
            _htmlDocument.RunScript(
                SimulateElementDragAndDropJavascriptTemplate
                    .Replace("sourceNode", _draggedElement.NativeElement.GetJavaScriptElementReference())
                    .Replace("destinationNode", element.NativeElement.GetJavaScriptElementReference()));
        }

        private readonly Document _htmlDocument;
        private readonly Element _draggedElement;

        private const string SimulateElementDragAndDropJavascriptTemplate =
@"
    debugger;
    var EVENT_TYPES = {
        DRAG_END: 'dragend',
        DRAG_START: 'dragstart',
        DROP: 'drop'
    }

    function createCustomEvent(type) {
        var event = null;
        try {
            event = new CustomEvent('CustomEvent');
        }
        catch (unused) {
            event = document.createEvent('CustomEvent');
        }

        event.initCustomEvent(type, true, true, null)

        event.dataTransfer = {
            data:
            {
            },
            setData: function(type, val) {
                this.data[type] = val
            },
            getData: function(type) {
                return this.data[type]
            }
        }
        return event
    }

        function dispatchEvent(node, type, event) {
            if (node.dispatchEvent)
            {
                return node.dispatchEvent(event)
        }
        if (node.fireEvent) {
            return node.fireEvent('on' + type, event)
        }
    }

    var event = createCustomEvent(EVENT_TYPES.DRAG_START)
    dispatchEvent(sourceNode, EVENT_TYPES.DRAG_START, event)

    var dropEvent = createCustomEvent(EVENT_TYPES.DROP)
    dropEvent.dataTransfer = event.dataTransfer
    dispatchEvent(destinationNode, EVENT_TYPES.DROP, dropEvent)

    var dragEndEvent = createCustomEvent(EVENT_TYPES.DRAG_END)
    dragEndEvent.dataTransfer = event.dataTransfer
    dispatchEvent(sourceNode, EVENT_TYPES.DRAG_END, dragEndEvent)
";
    }
}