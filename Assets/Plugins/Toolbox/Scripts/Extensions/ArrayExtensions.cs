using R = UnityEngine.Random;
using System;
using System.Collections.Generic;

namespace ToolBox
{
    public static class ArrayExtensions
    {
        public static T[] Fill<T>(this T[] src, T value)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentException("src");
#endif

            for (long i = 0; i < src.LongLength; i++)
                src[i] = value;
            return src;
        }

        public static void Fill<T>(this T[][] src, T value)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentException("src");
#endif

            for (long i = 0; i < src.LongLength; i++)
                src[i].Fill(value);
        }

        public static void Fill<T>(this T[][][] src, T value)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentException("src");
#endif

            for (long i = 0; i < src.LongLength; i++)
                src[i].Fill(value);
        }

        public static void Each<T>(this T[] src, Action<T> function)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentException("src");

            if (function == null)
                throw new ArgumentException("function");
#endif
            for (int i = 0; i < src.Length; i++)
                function.Invoke(src[i]);
        }

        public static void Each<T>(this T[][] src, Action<T> function)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentException("src");

            if (function == null)
                throw new ArgumentException("function");
#endif
            for (int i = 0; i < src.Length; i++)
                src[i].Each(function);
        }

        public static void Each<T>(this T[][][] src, Action<T> function)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentException("src");

            if (function == null)
                throw new ArgumentException("function");
#endif
            for (int i = 0; i < src.Length; i++)
                src[i].Each(function);
        }

        public static void SetEach<T>(this T[] src, Func<T> set)
        {
            for (int i = 0; i < src.Length; i++)
                src[i] = set.Invoke();
        }

        public static bool TryResolve<T1, T2>(this T1[] src, T2 id, out T1 value) where T1 : IIdentifiable<T2>
        {
#if DEBUG
            if (src == null)
                throw new ArgumentException("src");

            if (src.Length == 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i].Id.Equals(id))
                {
                    value = src[i];
                    return true;
                }
            }

            value = default;
            return false;
        }

        public static T1 Resolve<T1, T2>(this T1[] src, T2 id) where T1 : IIdentifiable<T2>
        {
#if DEBUG
            if (src == null)
                throw new ArgumentException("src");

            if (src.Length == 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif
            for (int i = 0; i < src.Length; i++)
                if (src[i].Id.Equals(id))
                    return src[i];

            throw new ArgumentOutOfRangeException($"{id.ToString()} could not be resolved.");
        }

        public static bool TryFindFirst<T>(this T[] src, Func<T, bool> predicate, out T result)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentException("src");

            if (predicate == null)
                throw new ArgumentException("predicate");
#endif

            for (int i = 0; i < src.Length; i++)
            {
                if (predicate.Invoke(src[i]))
                {
                    result = src[i];
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static bool TryFindLast<T>(this T[] src, Func<T, bool> predicate, out T result)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentException("src");

            if (predicate == null)
                throw new ArgumentException("predicate");
#endif
            for (int i = src.Length - 1; i >= 0; i--)
            {
                if (predicate.Invoke(src[i]))
                {
                    result = src[i];
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static T FindFirst<T>(this T[] src, Func<T, bool> predicate)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");
            if (predicate == null)
                throw new ArgumentNullException("predicate");
#endif

            for (int i = 0; i < src.Length; i++)
                if (predicate.Invoke(src[i]))
                    return src[i];

            throw new ArgumentOutOfRangeException("Couldn't find any instance with the given predicate");
        }

        public static T FindLast<T>(this T[] src, Func<T, bool> predicate)
        {
            for (int i = src.Length - 1; i >= 0; i--)
                if (predicate.Invoke(src[i]))
                    return src[i];

            throw new ArgumentOutOfRangeException("Couldn't find any instance with the given predicate");
        }

        public static Type FindMatch<T>(this Type[] src)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentException("src");
#endif

            for (long i = 0; i < src.LongLength; i++)
                if (typeof(T).IsAssignableFrom(src[i]))
                    return src[i];

            throw new ArgumentOutOfRangeException($"{typeof(T).Name} could not be resolved.");
        }

        public static Dictionary<T2, T1> ToDict<T1, T2>(this T1[] src) where T1 : IIdentifiable<T2>
        {
            Dictionary<T2, T1> dictionary = new Dictionary<T2, T1>();
            src.Each(x => dictionary.Add(x.Id, x));
            return dictionary;
        }

        public static T Random<T>(this T[] src)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentException("src");

            if (src.Length == 0)
                throw new ArgumentException("src length is notNo allowed to be 0");
#endif

            return src[R.Range(0, src.Length)];
        }

        public static T WeightedRandom<T>(this T[] src) where T : IWeighted
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");

            if (src.Length == 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif

            return src.WeightChoose(R.Range(0, src.WeightTotal()));
        }

        private static T WeightChoose<T>(this T[] src, float weight) where T : IWeighted
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");

            if (weight < 0)
                throw new ArgumentException($"Weight is not allowed to be negative (weight given is {weight})");

            if (weight > src.WeightTotal())
                throw new ArgumentException($"Weight is not allowed to be bigger than the total weight of the array (weight given is {weight})");
#endif

            float check = 0;
            for (int i = 0; i < src.Length; i++)
            {
                check += src[i].Weight;
                if (weight < check)
                    return src[i];
            }
            throw new InvalidOperationException();
        }

        private static float WeightTotal<T>(this T[] src) where T : IWeighted
        {
            float total = 0;
            for (int i = 0; i < src.Length; i++)
                total += src[i].Weight;
            return total;
        }

        public static float Average(this float[] src)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");

            if (src.Length <= 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif

            float a = 0;
            for (long i = 0; i < src.LongLength; i++)
                a += src[i];
            return a / src.Length;
        }

        public static double Average(this double[] src)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");

            if (src.Length <= 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif
            double a = 0;
            for (long i = 0; i < src.LongLength; i++)
                a += src[i];
            return a / src.Length;
        }

        public static double Average(this int[] src)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");

            if (src.Length <= 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif
            double a = 0;
            for (long i = 0; i < src.LongLength; i++)
                a += src[i];
            return a / src.Length;
        }

        public static double Average(this long[] src)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");

            if (src.Length <= 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif
            double a = 0;
            for (long i = 0; i < src.LongLength; i++)
                a += src[i];
            return a / src.Length;
        }

        public static float Max(this float[] src)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");

            if (src.Length <= 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif
            float max = float.NegativeInfinity;
            for (long i = 0; i < src.LongLength; i++)
                if (max < src[i])
                    max = src[i];
            return max;
        }

        public static double Max(this double[] src)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");

            if (src.Length <= 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif
            double max = double.NegativeInfinity;
            for (long i = 0; i < src.LongLength; i++)
                if (max < src[i])
                    max = src[i];
            return max;
        }

        public static int Max(this int[] src)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");

            if (src.Length <= 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif
            int max = int.MinValue;
            for (long i = 0; i < src.LongLength; i++)
                if (max < src[i])
                    max = src[i];
            return max;
        }

        public static long Max(this long[] src)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");

            if (src.Length <= 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif
            long max = long.MinValue;
            for (long i = 0; i < src.LongLength; i++)
                if (max < src[i])
                    max = src[i];
            return max;
        }

        public static float Min(this float[] src)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");

            if (src.Length <= 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif
            float max = float.PositiveInfinity;
            for (long i = 0; i < src.LongLength; i++)
                if (max > src[i])
                    max = src[i];
            return max;
        }

        public static double Min(this double[] src)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");

            if (src.Length <= 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif
            double max = double.PositiveInfinity;
            for (long i = 0; i < src.LongLength; i++)
                if (max > src[i])
                    max = src[i];
            return max;
        }

        public static int Min(this int[] src)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");

            if (src.Length <= 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif
            int max = int.MaxValue;
            for (long i = 0; i < src.LongLength; i++)
                if (max > src[i])
                    max = src[i];
            return max;
        }

        public static long Min(this long[] src)
        {
#if DEBUG
            if (src == null)
                throw new ArgumentNullException("src");

            if (src.Length <= 0)
                throw new ArgumentException("src length is not allowed to be 0");
#endif
            long max = long.MaxValue;
            for (long i = 0; i < src.LongLength; i++)
                if (max > src[i])
                    max = src[i];
            return max;
        }
    }
}