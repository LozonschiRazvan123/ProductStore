using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Core.Interface
{
    public interface ISorting
    {
        public IQueryable<T> ApplyShellSort<T>(IQueryable<T> query, string sortBy);
        public IQueryable<T> ApplyCountingSort<T>(IQueryable<T> query, string sortBy);
        public IQueryable<T> ApplyRadixSort<T>(IQueryable<T> query, string sortBy);
        public IQueryable<T> BucketSort<T>(IQueryable<T> source, string propertyName);
        public IQueryable<T> InsertionSort<T>(IQueryable<T> source, string propertyName);
        public IQueryable<T> QuickSort<T>(IQueryable<T> source, string propertyName);
        public IQueryable<T> BubleSort<T>(IQueryable<T> source, string propertyName);
        public IQueryable<T> SelectionSort<T>(IQueryable<T> source, string propertyName);
        public IQueryable<T> MergeSort<T>(IQueryable<T> source, string propertyName);
    }
}
