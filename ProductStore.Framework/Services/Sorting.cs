using LinqKit;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Utilities;
using ProductStore.Core.Interface;
using System;
using System.Collections;
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

        public IQueryable<T> InsertionSort<T>(IQueryable<T> source, string propertyName)
        {
            PropertyInfo property = typeof(T).GetProperty(propertyName);

            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'.");
            }

            List<T> list = source.ToList();

            for (int i = 1; i < list.Count; i++)
            {
                T key = list[i];
                int j = i - 1;

                while (j >= 0 && Comparer.Default.Compare(property.GetValue(list[j]), property.GetValue(key)) > 0)
                {
                    list[j + 1] = list[j];
                    j = j - 1;
                }

                list[j + 1] = key;
            }

            return list.AsQueryable();
        }

        public IQueryable<T> QuickSort<T>(IQueryable<T> source, string propertyName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var elements = source.ToArray();
            QuickSort(elements, propertyName, 0, elements.Length - 1);
            return elements.AsQueryable();
        }

        private static void QuickSort<T>(T[] elements, string propertyName, int left, int right)
        {
            if (left < right)
            {
                int pivotIndex = Partition(elements, propertyName, left, right);

                if (pivotIndex > 1)
                {
                    QuickSort(elements, propertyName, left, pivotIndex - 1);
                }

                if (pivotIndex + 1 < right)
                {
                    QuickSort(elements, propertyName, pivotIndex + 1, right);
                }
            }
        }

        private static int Partition<T>(T[] elements, string propertyName, int left, int right)
        {
            T pivot = elements[left];
            while (true)
            {
                while (((IComparable)pivot.GetType().GetProperty(propertyName).GetValue(pivot)).CompareTo(elements[left].GetType().GetProperty(propertyName).GetValue(elements[left])) > 0)
                {
                    left++;
                }

                while (((IComparable)pivot.GetType().GetProperty(propertyName).GetValue(pivot)).CompareTo(elements[right].GetType().GetProperty(propertyName).GetValue(elements[right])) < 0)
                {
                    right--;
                }

                if (left < right)
                {
                    T temp = elements[left];
                    elements[left] = elements[right];
                    elements[right] = temp;
                    left++;
                    right--;
                }
                else
                {
                    return right;
                }
            }
        }

        public IQueryable<T> BubleSort<T>(IQueryable<T> source, string propertyName)
        {
            var list = source.ToArray();
            BubbleSort(list, propertyName);
            return list.AsQueryable();
        }

        private static void BubbleSort<T>(T[] array, string propertyName)
        {
            var n = array.Length;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    var currentPropertyValue = typeof(T).GetProperty(propertyName)?.GetValue(array[j]);
                    var nextPropertyValue = typeof(T).GetProperty(propertyName)?.GetValue(array[j + 1]);

                    if (currentPropertyValue != null && nextPropertyValue != null)
                    {
                        if (currentPropertyValue is IComparable comparable && nextPropertyValue is IComparable)
                        {
                            if (comparable.CompareTo(nextPropertyValue) > 0)
                            {
                                var tempVar = array[j];
                                array[j] = array[j + 1];
                                array[j + 1] = tempVar;
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("Property values are not of comparable types.");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Property values cannot be null.");
                    }
                }
            }
        }

        public IQueryable<T> SelectionSort<T>(IQueryable<T> source, string propertyName)
        {
            var list = source.ToArray();
            SelectionSort(list, propertyName);
            return list.AsQueryable();
        }

        public static void SelectionSort<T>(T[] array, string propertyName)
        {
            var arrayLength = array.Length;
            for (int i = 0; i < arrayLength - 1; i++)
            {
                var smallestVal = i;
                for (int j = i + 1; j < arrayLength; j++)
                {
                    var value1 = typeof(T).GetProperty(propertyName)?.GetValue(array[j]);
                    var value2 = typeof(T).GetProperty(propertyName)?.GetValue(array[smallestVal]);

                    if (value1 != null && value2 != null)
                    {
                        if (value1 is IComparable comparable && value2 is IComparable)
                        {
                            if (comparable.CompareTo(value2) < 0)
                            {
                                smallestVal = j;
                            }
                        }
                        else
                        {
                            var strValue1 = value1.ToString();
                            var strValue2 = value2.ToString();

                            if (String.Compare(strValue1, strValue2) < 0)
                            {
                                smallestVal = j;
                            }
                        }
                    }
                }

                if (smallestVal != i)
                {
                    var tempVar = array[smallestVal];
                    array[smallestVal] = array[i];
                    array[i] = tempVar;
                }
            }
        }

        public IQueryable<T> MergeSort<T>(IQueryable<T> source, string propertyName)
        {
            var list = source.ToArray();
            SortArray(list,0, list.Length-1, propertyName);
            return list.AsQueryable();
        }

        public static void SortArray<T>(T[] array, int left, int right, string propertyName)
        {
            if (left < right)
            {
                int middle = left + (right - left) / 2;
                SortArray(array, left, middle, propertyName);
                SortArray(array, middle + 1, right, propertyName);
                MergeArray(array, left, middle, right, propertyName);
            }
        }

        public static void MergeArray<T>(T[] array, int left, int middle, int right, string propertyName)
        {
            var leftArrayLength = middle - left + 1;
            var rightArrayLength = right - middle;
            var leftTempArray = new T[leftArrayLength];
            var rightTempArray = new T[rightArrayLength];
            int i, j;

            for (i = 0; i < leftArrayLength; ++i)
                leftTempArray[i] = array[left + i];
            for (j = 0; j < rightArrayLength; ++j)
                rightTempArray[j] = array[middle + 1 + j];

            i = 0;
            j = 0;
            int k = left;

            while (i < leftArrayLength && j < rightArrayLength)
            {
                var value1 = typeof(T).GetProperty(propertyName)?.GetValue(leftTempArray[i]);
                var value2 = typeof(T).GetProperty(propertyName)?.GetValue(rightTempArray[j]);
                if (value1 != null && value2 != null)
                {
                    if (value1 is IComparable comparable && value2 is IComparable)
                    {
                        if (comparable.CompareTo(value2) <= 0)
                        {
                            array[k++] = leftTempArray[i++];
                        }
                        else
                        {
                            array[k++] = rightTempArray[j++];
                        }
                    }
                }
            }

            while (i < leftArrayLength)
            {
                array[k++] = (T)leftTempArray[i++];
            }

            while (j < rightArrayLength)
            {
                array[k++] = rightTempArray[j++];
            }
        }

        public IQueryable<T> HeapSort<T>(IQueryable<T> source, string propertyName)
        {
            var list = source.ToList();
            SortArrayHeap(list, list.Count, propertyName);
            return list.AsQueryable();
        }
        public static void SortArrayHeap<T>(List<T> array, int size, string propertyName)
        {
            if (size <= 1)
                return;

            for (int i = size / 2 - 1; i >= 0; i--)
            {
                Heapify(array, size, i, propertyName);
            }

            for (int i = size - 1; i > 0; i--)
            {
                var tempVar = array[0];
                array[0] = array[i];
                array[i] = tempVar;

                Heapify(array, i, 0, propertyName);
            }
        }
        public static void Heapify<T>(List<T> array, int size, int index, string propertyName)
        {
            int largestIndex = index;
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;

            if (leftChild < size)
            {
                var value1 = typeof(T).GetProperty(propertyName)?.GetValue(array[leftChild]);
                var value2 = typeof(T).GetProperty(propertyName)?.GetValue(array[largestIndex]);

                if (value1 != null && value2 != null && value1 is IComparable comparable && value2 is IComparable)
                {
                    if (comparable.CompareTo(value2) > 0)
                    {
                        largestIndex = leftChild;
                    }
                }
            }

            if (rightChild < size)
            {
                var value1 = typeof(T).GetProperty(propertyName)?.GetValue(array[rightChild]);
                var value2 = typeof(T).GetProperty(propertyName)?.GetValue(array[largestIndex]);

                if (value1 != null && value2 != null && value1 is IComparable comparable && value2 is IComparable)
                {
                    if (comparable.CompareTo(value1) > 0) 
                    {
                        largestIndex = rightChild;
                    }
                }
            }

            if (largestIndex != index)
            {
                var tempVar = array[index];
                array[index] = array[largestIndex];
                array[largestIndex] = tempVar;

                Heapify(array, size, largestIndex, propertyName);
            }
        }
    }
}