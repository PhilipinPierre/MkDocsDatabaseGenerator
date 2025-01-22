using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator.Extension
{
    public static class LinqExtension
    {
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (TSource item in source)
                action(item);
        }

        public static async Task ForEachAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task> action)
        {
            foreach (TSource item in source)
                await action(item);
        }
    }
}