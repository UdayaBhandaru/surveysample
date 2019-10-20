using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sample.Survey.Api.IntegrationTest
{
    public static class AssertExtension
    {
        public static T ShouldBeGreaterThan<T>(this T actual, T expected, string message = null)
    where T : IComparable
        {
            Assert.True(actual.CompareTo(expected) > 0, message);
            return actual;
        }
    }
}
