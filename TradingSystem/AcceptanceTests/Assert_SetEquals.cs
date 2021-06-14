using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace AcceptanceTests
{
    public delegate bool EqualityCompare<T>(T x1, T x2);

    public class Assert_SetEquals<T> : Assert_SetEquals<T, T> where T : notnull
    {
        public Assert_SetEquals(IEnumerable<T> expected)
            : base(expected, x => x, EqualityComparer<T>.Default.Equals)
        { }

        public Assert_SetEquals(IEnumerable<T> expected, EqualityCompare<T> valuesComparer)
            : base(expected, x => x, valuesComparer)
        { }

        public Assert_SetEquals(IDictionary<T, T> expected)
            : base(expected, x => x, EqualityComparer<T>.Default.Equals)
        { }

        public Assert_SetEquals(IDictionary<T, T> expected, EqualityCompare<T> valuesComparer)
            : base(expected, x => x, valuesComparer)
        { }
    }

    public class Assert_SetEquals<TKey, TValue> where TKey : notnull
    {
        private readonly IDictionary<TKey, TValue> expected;
        private readonly Func<TValue, TKey> getKey;
        private readonly EqualityCompare<TValue> valuesComparer;

        public Assert_SetEquals(IEnumerable<TValue> expected, Func<TValue, TKey> getKey)
            : this(expected, getKey, EqualityComparer<TValue>.Default.Equals)
        { }
        public Assert_SetEquals(IEnumerable<TValue> expected, Func<TValue, TKey> getKey, EqualityCompare<TValue> valuesComparer)
        {
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

            this.expected = new Dictionary<TKey, TValue>(expected.Select(x => new KeyValuePair<TKey, TValue>(getKey(x), x)));
            this.getKey = getKey;
            this.valuesComparer = valuesComparer;
        }

        public Assert_SetEquals(IDictionary<TKey, TValue> expected, Func<TValue, TKey> getKey)
            : this(expected, getKey, EqualityComparer<TValue>.Default.Equals)
        { }
        public Assert_SetEquals(IDictionary<TKey, TValue> expected, Func<TValue, TKey> getKey, EqualityCompare<TValue> valuesComparer)
        {
            if (expected is null)
            {
                throw new ArgumentNullException(nameof(expected));
            }

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
            Assert.IsNotNull(actual_nullable, $"null results");
            IEnumerable<TValue> actual = actual_nullable!;
            AssertSameCount(actual);
            AssertSetEquals(actual);
        }

        private void AssertSameCount(IEnumerable<TValue> actual)
        {
            if (expected.Count == 0)
            {
                Assert.IsEmpty(actual, $"expected empty (no) results");
            }
            else
            {
                int actual_count = actual.Count();
                Assert.IsTrue(expected.Count == actual_count, $"sets aren't equal, items count is different. expected {expected.Count} items but got {actual_count}");
            }
        }

        private void AssertSetEquals(IEnumerable<TValue> actual)
        {
            foreach (TValue item_actual in actual)
            {
                TValue item_expected;
                TKey key = getKey(item_actual);
                Assert.IsTrue(expected.TryGetValue(key, out item_expected), $"sets aren't equal: expected is missing key '{key}'");
                Assert.IsTrue(valuesComparer(item_expected!, item_actual));
            }
        }
    }
}