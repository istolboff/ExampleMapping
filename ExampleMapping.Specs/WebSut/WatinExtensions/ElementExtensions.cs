using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.WatinExtensions
{
    internal static class ElementExtensions
    {
        public static void EnterText(this TextField textField, string text)
        {
            textField.Click();
            textField.Value = text;
            textField.Change();
        }
    }
}
