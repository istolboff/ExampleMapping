using System.Linq;
using System.Reflection;

namespace ExampleMapping.Specs.Miscellaneous
{
    internal static class GenericTypeExtensions
    {
        public static T? AsNullable<T>(this T value) where T : struct
        {
            return value;
        }

        public static object GetPropertyValueByName<T>(this T @this, string propertyName) 
        {
            var props = @this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var prop = props.FirstOrDefault(p => p.Name == propertyName);
            Verify.That(prop != null, $"Property '{propertyName}' not found at type '{@this.GetType().FullName}'.");
            return prop.GetValue(@this);
        }
    }
}
