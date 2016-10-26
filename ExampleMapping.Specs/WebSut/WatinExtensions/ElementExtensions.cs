using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.WatinExtensions
{
    internal static class ElementExtensions
    {
        public static DragAndDrop Drag(this Element element)
        {
            element.MouseDown();
            return new DragAndDrop();
        }
    }
}
