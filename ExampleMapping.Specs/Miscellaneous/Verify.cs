using JetBrains.Annotations;

namespace ExampleMapping.Specs.Miscellaneous
{
    internal static class Verify
    {
        [ContractAnnotation("condition:false => halt")]
        public static void That([UsedImplicitly] bool condition, [UsedImplicitly] string message)
        {
#if DEBUG
            System.Diagnostics.Contracts.Contract.Assume(condition, message);
#else
            if (!condition)
            {
                throw new System.InvalidOperationException(message);
            }
#endif
        }

        [ContractAnnotation("condition:false => halt")]
        public static void That([UsedImplicitly] bool condition, [UsedImplicitly] System.Func<string> buildMessage)
        {
#if DEBUG
            if (!condition)
            {
                System.Diagnostics.Contracts.Contract.Assume(condition, buildMessage());
            }
#else
            if (!condition)
            {
                throw new System.InvalidOperationException(buildMessage());
            }
#endif
        }
    }
}
