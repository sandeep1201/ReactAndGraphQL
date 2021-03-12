using DCF.Timelimits.Rules.Actions;
using EnsureThat;

namespace DCF.Timelimits.Rules.Operations
{
    public static class OperationExtensions
    {
            /// <summary>
            /// Combines the current operation instance with another operation instance
            /// and returns the combined operation which represents that both the current and
            /// the given operation must be satisfied by the given object.
            /// </summary>
            /// <param name="operation">The operation</param>
            /// <param name="other">The operation instance with which the current operation is combined.</param>
            /// <returns>The combined operation instance.</returns>
            public static IOperation<T> And<T>(this IOperation<T> operation, IOperation<T> other)
            {
                Ensure.That(operation, nameof(operation)).IsNotNull();
                Ensure.That(other, nameof(other)).IsNotNull();

                return new DualOperation<T>(operation, other);
            }

        // TODO: AndAlso?
    }
}