using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Data.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            list.ToList().ForEach(action);
        }

    }
}