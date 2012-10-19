﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DelftTools.Utils.Collections
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns the type of the (first) element in the Enumerable. Returns null if the enumerable is empty.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Type GetInnerType(this IEnumerable source)
        {
            Type inner = null;

            foreach(var item in source)
            {
                return item.GetType();
            }
            return inner;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var i = 0;
            foreach (var item in source)
            {
                action(item, i);
                i++;
            }
        }

        public static bool HasExactlyOneValue(this IEnumerable values)
        {
            int i = 0;
            foreach (object o in values)
            {
                i++;
                if (i > 1)
                    break;
            }
            return (i == 1);
        }

        /// <summary>
        /// Returns values1 except values2.
        /// </summary>
        /// <param name="values1"></param>
        /// <param name="values2"></param>
        /// <returns></returns>
        public static IEnumerable Except(this IEnumerable values1, IList values2)
        {
            foreach (object o in values1)
            {
                if (!values2.Contains(o))
                {
                    yield return o;
                }
            }
        }

        public static string ConvertToString(this IEnumerable<int> values)
        {
            return String.Join(",", values.Select(v => v.ToString()).ToArray());
        }

        public static IEnumerable<T> QuickSort<T>(this IEnumerable<T> list)
            where T : IComparable
        {
            if (!list.Any())
            {
                return Enumerable.Empty<T>();
            }
            T pivot = (T) list.First();
            IEnumerable<T> smaller = list.Where(item => item.CompareTo(pivot) <= 0).QuickSort();
            IEnumerable<T> larger = list.Where(item => item.CompareTo(pivot) > 0).QuickSort();
            
            return smaller.Concat(new[] { pivot }).Concat(larger);
        }

        public static IEnumerable<int> Substract(this IEnumerable<int> index1, IEnumerable<int> index2)
        {
            var i1 = index1.GetEnumerator();
            var i2 = index2.GetEnumerator();

            while (i1.MoveNext() && i2.MoveNext())
            {
                yield return i1.Current - i2.Current;
            }
        }

        public static IEnumerable<int> Add(this IEnumerable<int> index1, IEnumerable<int> index2)
        {
            var i1 = index1.GetEnumerator();
            var i2 = index2.GetEnumerator();

            while (i1.MoveNext() && i2.MoveNext())
            {
                yield return i1.Current + i2.Current;
            }
        }

        public static IEnumerable<int> Add(this IEnumerable<int> index1, int value)
        {
            var i1 = index1.GetEnumerator();

            while (i1.MoveNext())
            {
                yield return i1.Current + value;
            }
        }

        public static IEnumerable<int> Substract(this IEnumerable<int> index1, int value)
        {
            var i1 = index1.GetEnumerator();

            while (i1.MoveNext())
            {
                yield return i1.Current - value;
            }
        }

        // From http://community.bartdesmet.net/blogs/bart/archive/2008/11/03/c-4-0-feature-focus-part-3-intermezzo-linq-s-new-zip-operator.aspx
        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> func)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (func == null)
                throw new ArgumentNullException("func");
            using (var ie1 = first.GetEnumerator())
            using (var ie2 = second.GetEnumerator())
                while (ie1.MoveNext() && ie2.MoveNext())
                    yield return func(ie1.Current, ie2.Current);
        }

        public static IEnumerable<Tuple<T, R>> Zip<T, R>(this IEnumerable<T> first, IEnumerable<R> second)
        {
            return first.Zip(second, (f, s) => new Tuple<T, R>(f, s));
        }

        public static bool HasUniqueValues<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Distinct().Count() == enumerable.Count();
        }

        public static bool IsMonotonousAscending<T>(this IEnumerable<T> enumerable) where T : IComparable
        {
            T previousValue = default(T);
            foreach (var value in enumerable)
            {
                if (previousValue != null && !previousValue.Equals(default(T)))
                {
                    if (value.CompareTo(previousValue)<0)
                    {
                        return false;//the next value is smaller than the previous...not monotonous ascending
                    }
                }
                previousValue = value;
            }
            return true;
        }/*

        public static void Times(this int times,Action action)
        {
            for (int i =0;i<times;i++)
            {
                action();
            }
        }
        public static void Times(this Action action,int times)
        {
            times.Times(action);
        }*/
    }
}