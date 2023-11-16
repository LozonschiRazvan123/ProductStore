using ProductStore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Framework.Services
{
    public class Sorting : ISorting
    {
        public IQueryable<T> ApplyCountingSort<T>(IQueryable<T> query, string sortBy)
        {
            var list = query.ToList();
            CountingSort(list, sortBy);

            return list.AsQueryable();
        }

        private void CountingSort<T>(List<T> list, string sortBy)
        {
            PropertyInfo prop = typeof(T).GetProperty(sortBy);

            if (prop == null)
            {
                throw new ArgumentException($"Property '{sortBy}' not found in type '{typeof(T).Name}'.");
            }

            int n = list.Count;

            Dictionary<object, int> count = new Dictionary<object, int>();
            foreach (var item in list)
            {
                object value = prop.GetValue(item);
                if (count.ContainsKey(value))
                    count[value]++;
                else
                    count[value] = 1;
            }

            int index = 0;
            foreach (var key in count.Keys.OrderBy(k => k))
            {
                int frequency = count[key];
                for (int i = 0; i < frequency; i++)
                {
                    prop.SetValue(list[index], key, null);
                    index++;
                }
            }
        }

        public IQueryable<T> ApplyShellSort<T>(IQueryable<T> query, string sortBy)
        {
            var list = query.ToList();
            ShellSort(list, sortBy);

            return list.AsQueryable();
        }

        private void ShellSort<T>(List<T> list, string sortBy)
        {
            int n = list.Count;
            PropertyInfo prop = typeof(T).GetProperty(sortBy);

            if (prop == null)
                throw new ArgumentException($"Property {sortBy} not found on type {typeof(T).Name}");

            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < n; i += 1)
                {
                    T temp = list[i];

                    int j;
                    for (j = i; j >= gap && ((IComparable)prop.GetValue(list[j - gap])).CompareTo(prop.GetValue(temp)) > 0; j -= gap)
                    {
                        list[j] = list[j - gap];
                    }

                    list[j] = temp;
                }
            }
        }

    }
}
