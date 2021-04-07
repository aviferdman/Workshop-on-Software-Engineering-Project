using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace AcceptanceTests.AppInterface
{
    public delegate bool EqualityCompare<T>(T x1, T x2);

    public class Assert_SetEquals<T> : Assert_SetEquals<T, T> where T : notnull
    {
        public Assert_SetEquals(string testName, IEnumerable<T> expected)
            : base(testName, expected, x => x, EqualityComparer<T>.Default.Equals)
        { }

        public Assert_SetEquals(string testName, IEnumerable<T> expected, EqualityCompare<T> valuesComparer)
            : base(testName, expected, x => x, valuesComparer)
        { }

        public Assert_SetEquals(string testName, IDictionary<T, T> expected)
            : base(testName, expected, x => x, EqualityComparer<T>.Default.Equals)
        { }

        public Assert_SetEquals(string testName, IDictionary<T, T> expected, EqualityCompare<T> valuesComparer)
            : base(testName, expected, x => x, valuesComparer)
        { }
    }

    public class Assert_SetEquals<TKey, TValue> where TKey : notnull
    {
        private readonly IDictionary<TKey, TValue> expected;
        private readonly Func<TValue, TKey> getKey;
        private readonly EqualityCompare<TValue> valuesComparer;

        public string TestName { get; }

        public Assert_SetEquals(string testName, IEnumerable<TValue> expected, Func<TValue, TKey> getKey)
            : this(testName, expected, getKey, EqualityComparer<TValue>.Default.Equals)
        { }
        public Assert_SetEquals(string testName, IEnumerable<TValue> expected, Func<TValue, TKey> getKey, EqualityCompare<TValue> valuesComparer)
        {
            if (string.IsNullOrWhiteSpace(testName))
            {
                throw new ArgumentException($"'{nameof(testName)}' cannot be null or whitespace.", nameof(testName));
            }

            if (expected is null)
            {
                throw new ArgumentNullException(nameof(expected));
            }

            if (getKey is null)
            {
                throw new ArgumentNullException(nameof(getKey));
            }

            if (valuesComparer is null)
            {
                throw new ArgumentNullException(nameof(valuesComparer));
            }

            TestName = testName;
            this.expected = new Dictionary<TKey, TValue>(expected.Select(x => new KeyValuePair<TKey, TValue>(getKey(x), x)));
            this.getKey = getKey;
            this.valuesComparer = valuesComparer;
        }

        public Assert_SetEquals(string testName, IDictionary<TKey, TValue> expected, Func<TValue, TKey> getKey)
            : this(testName, expected, getKey, EqualityComparer<TValue>.Default.Equals)
        { }
        public Assert_SetEquals(string testName, IDictionary<TKey, TValue> expected, Func<TValue, TKey> getKey, EqualityCompare<TValue> valuesComparer)
        {
            if (string.IsNullOrWhiteSpace(testName))
            {
                throw new ArgumentException($"'{nameof(testName)}' cannot be null or whitespace.", nameof(testName));
            }

            if (expected is null)
            {
                throw new ArgumentNullException(nameof(expected));
            }

            TestName = testName;
            this.expected = new Dictionary<TKey, TValue>(expected);
            this.getKey = getKey;
            this.valuesComparer = valuesComparer;
        }

        public void AssertEquals(params TValue[]? actual)
        {
            AssertEquals((IEnumerable<TValue>?)actual);
        }
        public void AssertEquals(IEnumerable<TValue>? actual_nullable)
        {
            Assert.IsNotNull(actual_nullable, $"{TestName} - null results");
            IEnumerable<TValue> actual = actual_nullable!;
            AssertSameCount(actual);
            AssertSetEquals(actual);
        }

        private void AssertSameCount(IEnumerable<TValue> actual)
        {
            if (expected.Count == 0)
            {
                Assert.IsEmpty(actual, $"{TestName} - expected empty (no) results");
            }
            else
            {
                int actual_count = actual.Count();
                Assert.IsTrue(expected.Count == actual_count, $"{TestName} - expected {expected.Count} results but got {actual_count}");
            }
        }

        private void AssertSetEquals(IEnumerable<TValue> actual)
        {
            foreach (TValue item_actual in actual)
            {
                TValue item_expected;
                Assert.IsTrue(expected.TryGetValue(getKey(item_actual), out item_expected), $"{TestName} - sets aren't equal");
                Assert.IsTrue(valuesComparer(item_expected!, item_actual));
            }
        }
    }
}
