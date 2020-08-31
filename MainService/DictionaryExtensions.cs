using System.Collections.Generic;

namespace MainService
{
    public static class DictionaryExtensions
    {
        public static Dictionary<T1, Dictionary<T2, Dictionary<T3, T4>>>
            Merge<T1, T2, T3, T4>(this IEnumerable<Dictionary<T1, Dictionary<T2, Dictionary<T3, T4>>>> dictionaries)
        {
            var result = new Dictionary<T1, Dictionary<T2, Dictionary<T3, T4>>>();

            foreach (var dictionary in dictionaries)
            foreach (var (firstKey, firstValue) in dictionary)
            {
                if (!result.ContainsKey(firstKey))
                {
                    result.Add(firstKey, firstValue);
                }
                else
                {
                    foreach (var (secondKey, secondValue) in firstValue)
                    {
                        if (!result[firstKey].ContainsKey(secondKey))
                        {
                            result[firstKey].Add(secondKey, secondValue);
                        }
                        else
                        {
                            foreach (var (thirdKey, thirdValue) in secondValue)
                            {
                                result[firstKey][secondKey].Add(thirdKey, thirdValue);
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}