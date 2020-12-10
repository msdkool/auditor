using System;
using System.Collections.Generic;

namespace Auditor.Helper
{
    public static class MiscExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null) => new HashSet<T>(source, comparer);
    }
}
