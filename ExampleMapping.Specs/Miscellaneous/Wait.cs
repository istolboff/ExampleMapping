using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ExampleMapping.Specs.Miscellaneous
{
    internal static class Wait
    {
        public static void Until(Func<bool> predicate, TimeSpan timeout, [CallerMemberName] string callerMemberName = null)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            Until(predicate, value => value, timeout, null, callerMemberName);
            // ReSharper restore ExplicitCallerInfoArgument
        }

        public static T Until<T>(
            Func<T> selectValue,
            Predicate<T> checkValue,
            TimeSpan timeout,
            Func<T, Exception> makeInnerException,
            [CallerMemberName] string callerMemberName = null)
        {
            Contract.Requires(selectValue != null);
            Contract.Requires(checkValue != null);
            Contract.Requires(makeInnerException != null);

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            while (true)
            {
                var value = selectValue();
                if (checkValue(value))
                {
                    return value;
                }

                if (stopWatch.Elapsed >= timeout)
                {
                    throw makeInnerException == null
                        ? new TimeoutException("Waiting failed.")
                        : new TimeoutException("Waiting failed.", makeInnerException(value));
                }

                Trace.WriteLine($"Probing in Wait.Until() called from {callerMemberName} failed, sleeping for a while.");
                Thread.Sleep(TimeSpan.FromMilliseconds(250));
            }
        }
    }
}
