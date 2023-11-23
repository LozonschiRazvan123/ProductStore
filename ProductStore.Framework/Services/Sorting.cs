using LinqKit;
using ProductStore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

            if (!IsNumericType(prop.PropertyType))
            {
                throw new ArgumentException($"Property '{sortBy}' is not a numeric type.");
            }

            int n = list.Count;

            Dictionary<object, List<T>> groupedByValue = new Dictionary<object, List<T>>();
            foreach (var item in list)
            {
                object value = prop.GetValue(item);
                if (groupedByValue.ContainsKey(value))
                    groupedByValue[value].Add(item);
                else
                    groupedByValue[value] = new List<T> { item };
            }

            int index = 0;
            foreach (var key in groupedByValue.Keys.OrderBy(k => k))
            {
                foreach (var item in groupedByValue[key])
                {
                    list[index] = item;
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
        public IQueryable<T> ApplyRadixSort<T>(IQueryable<T> query, string sortBy)
        {
            var list = query.AsEnumerable().ToList();
            RadixSort(list, sortBy);
            return list.AsQueryable();
        }


        private static void RadixSort<T>(List<T> list, string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            var type = propertyInfo.PropertyType;

            if (!IsNumericType(type))
            {
                throw new ArgumentException("The specified property must be a numeric type.");
            }

            var maxVal = GetMaxVal(list, propertyName);
            for (int exponent = 1; maxVal / exponent > 0; exponent *= 10)
                CountingSort(list, exponent, propertyName);
        }

        private static int GetMaxVal<T>(List<T> list, string propertyName)
        {
            var maxVal = list.Max(x => GetNumericValue(x, propertyName, 1));
            return maxVal;
        }

        private static void CountingSort<T>(List<T> list, int exponent, string propertyName)
        {
            var outputList = new List<T>[10];
            for (int i = 0; i < 10; i++)
                outputList[i] = new List<T>();

            foreach (var entity in list)
            {
                int numericValue = GetNumericValue(entity, propertyName, exponent);
                Console.WriteLine($"Numeric value: {numericValue}");
                outputList[(numericValue / exponent) % 10].Add(entity);
            }

            list.Clear();

            foreach (var outputBucket in outputList)
            {
                if (outputBucket.Count > 1)
                {
                    outputBucket.Sort((a, b) => GetNumericValue(a, propertyName, exponent).CompareTo(GetNumericValue(b, propertyName, exponent)));
                }

                list.AddRange(outputBucket);
            }
        }



        private static int GetNumericValue<T>(T entity, string propertyName, int exponent)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            var propertyValue = propertyInfo.GetValue(entity);


            if (propertyValue != null && propertyValue is IComparable)
            {
                int numericValue = Convert.ToInt32(propertyValue);
                return (numericValue / exponent) % 10;
            }

            return 0;
        }

        private static bool IsNumericType(Type type)
        {
            return type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(double) || type == typeof(decimal);
        }

        public IQueryable<T> BucketSort<T>(IQueryable<T> source, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return source;
            }

            PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo == null)
            {
                return source;
            }

            var minMaxValues = new
            {
                Min = source.Min(item => ((DateTime)propertyInfo.GetValue(item)).Ticks),
                Max = source.Max(item => ((DateTime)propertyInfo.GetValue(item)).Ticks)
            };

            int bucketCount = (int)Math.Sqrt(source.Count());
            bucketCount = Math.Max(bucketCount, 1);

            double bucketSize = (minMaxValues.Max - minMaxValues.Min) / bucketCount;

            var buckets = new List<List<T>>();
            for (int i = 0; i < bucketCount; i++)
            {
                buckets.Add(new List<T>());
            }

            foreach (var item in source)
            {
                double propertyValue = ((DateTime)propertyInfo.GetValue(item)).Ticks;
                int bucketIndex = (int)((propertyValue - minMaxValues.Min) / bucketSize);

                if (bucketIndex == bucketCount)
                {
                    bucketIndex--;
                }

                buckets[bucketIndex].Add(item);
            }

            var sortedQuery = buckets.SelectMany(bucket => bucket.AsQueryable().OrderBy(x => ((DateTime)propertyInfo.GetValue(x)).Ticks));

            return sortedQuery.AsQueryable();
        }

    }
}