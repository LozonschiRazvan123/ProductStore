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
            RadixSortNumeric(list, sortBy);
            return list.AsQueryable();
        }

        private static void RadixSortNumeric<T>(List<T> list, string propertyName)
        {
            PropertyInfo property = typeof(T).GetProperty(propertyName);
            if (property == null || !IsNumericType(property.PropertyType))
            {
                throw new ArgumentException("Invalid property or property type is not numeric.");
            }

            int maxLength = GetMaxLength(list, propertyName);

            for (int i = 0; i < maxLength; i++)
            {
                CountingSort(list, i, propertyName);
            }
        }

        private static void CountingSort<T>(List<T> list, int position, string propertyName)
        {
            const int NUMERIC_RANGE = 10; // Numeric characters range (0-9)

            var bucketLists = new List<List<T>>(NUMERIC_RANGE);
            for (int i = 0; i < NUMERIC_RANGE; i++)
            {
                bucketLists.Add(new List<T>());
            }

            foreach (var entity in list)
            {
                int? numericValue = GetNumericValue(entity, propertyName, position);
                if (numericValue != null)
                {
                    bucketLists[numericValue.Value].Add(entity);
                }
            }

            list.Clear();

            foreach (var bucketList in bucketLists)
            {
                list.AddRange(bucketList);
            }
        }


        private static int GetMaxLength<T>(List<T> list, string propertyName)
        {
            int maxLength = 0;
            foreach (var entity in list)
            {
                int length = GetNumericValue(entity, propertyName, 0)?.ToString().Length ?? 0;
                maxLength = Math.Max(maxLength, length);
            }

            return maxLength;
        }

        private static int? GetNumericValue<T>(T entity, string propertyName, int position)
        {
            PropertyInfo property = typeof(T).GetProperty(propertyName);
            var propertyValue = property?.GetValue(entity);

            if (propertyValue != null && propertyValue is IComparable)
            {
                string stringValue = propertyValue.ToString().PadLeft(position + 1, '0');
                int length = stringValue.Length;

                if (position >= 0 && position < length)
                {
                    int numericValue;
                    bool isValidNumericValue = int.TryParse(stringValue[position].ToString(), out numericValue);
                    if (isValidNumericValue)
                    {
                        return numericValue;
                    }
                }
            }

            return null;
        }

        private static bool IsNumericType(Type type)
        {
            return type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(double) || type == typeof(decimal);
        }


    }
}