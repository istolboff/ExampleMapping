using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.WatinExtensions
{
    internal sealed class DragAndDrop
    {
        public void DropTo(Element element)
        {
            element.MouseEnter();
            element.MouseUp();
        }
    }
}