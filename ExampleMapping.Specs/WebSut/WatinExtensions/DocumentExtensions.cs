using System;
using System.Linq;
using WatiN.Core;
using ExampleMapping.Specs.Miscellaneous;

namespace ExampleMapping.Specs.WebSut.WatinExtensions
{
    internal static class DocumentExtensions
    {
        public static void CheckRenderedPageForServerErrors(this Document htmlDocument)
        {
            Verify.That(
                !htmlDocument.Html.Contains("An unhandled exception occurred while processing the request"),
                () => ExtractErrorInfo(htmlDocument));
        }

        public static DragAndDrop Drag(this Document htmlDocument, Element element)
        {
            return new DragAndDrop(htmlDocument, element);
        }

        private static string ExtractErrorInfo(Document htmlDocument)
        {
            var errorTitleDiv = htmlDocument.Elements<Div>(Find.ByClass("titleerror")).FirstOrDefault();
            if (errorTitleDiv != null)
            {
                return $"{errorTitleDiv.InnerHtml}{Environment.NewLine}{Environment.NewLine}The whole page source is:{Environment.NewLine}{htmlDocument.Body.Parent.OuterHtml}";
            }

            return htmlDocument.Body.Parent.OuterHtml;
        }
    }
}
