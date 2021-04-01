using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace AcceptanceTests.AppInterface
{
    public class Assert_SetEquals<T>
    {
        private ISet<T> expectedSet;

        public string TestName { get; }

        public Assert_SetEquals(string testName, params T[] expected)
        {
            if (string.IsNullOrWhiteSpace(testName))
            {
                throw new ArgumentException($"'{nameof(testName)}' cannot be null or whitespace.", nameof(testName));
            }

            if (expected is null)
            {
                throw new ArgumentNullException(nameof(expected));
            }

            expectedSet = new HashSet<T>(expected);
            TestName = testName;
        }
        public Assert_SetEquals(string testName, IEnumerable<T> expected)
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
            expectedSet = new HashSet<T>(expected);
        }
        public Assert_SetEquals(string testName, Func<ISet<T>> setFactory)
        {
            if (string.IsNullOrWhiteSpace(testName))
            {
                throw new ArgumentException($"'{nameof(testName)}' cannot be null or whitespace.", nameof(testName));
            }

            if (setFactory is null)
            {
                throw new ArgumentNullException(nameof(setFactory));
            }

            TestName = testName;
            expectedSet = setFactory();
        }

        public void AssertEquals(params T[]? actual)
        {
            AssertEquals(actual == null ? null : new HashSet<T>(actual));
        }
        public void AssertEquals(IEnumerable<T>? actual)
        {
            AssertEquals(actual == null ? null : new HashSet<T>(actual));
        }
        public void AssertEquals(ISet<T>? other_nullable)
        {
            Assert.IsNotNull(other_nullable, $"{TestName} - null results");
            ISet<T> other = other_nullable!;
            if (expectedSet.Count == 0)
            {
                Assert.IsEmpty(other, $"{TestName} - expected empty (no) results");
            }
            else
            {
                Assert.IsTrue(expectedSet.Count == other.Count, $"{TestName} - expected {expectedSet.Count} results but got {other.Count}");
            }

            Assert.IsTrue(expectedSet.SetEquals(other), $"{TestName} - sets aren't equal");
        }
    }
}
