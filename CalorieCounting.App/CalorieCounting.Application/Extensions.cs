using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounting.Application
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)            
                action(item);            
        }

        public static async Task ForEach<T>(this IEnumerable<T> source, Func<T,Task> action)
        {
            foreach(var item in source)
                await action(item);
        }
    }
}
