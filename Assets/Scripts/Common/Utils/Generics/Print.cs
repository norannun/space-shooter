using UnityEngine;
using System.Collections.Generic;

public class Print
{
    public static void PrintHashSet<T>(HashSet<T> hashset)
    {
        string result = "";

        foreach (T value in hashset)
        {
            result += $"{value} | ";
        }

        Debug.Log(result);
    }

    public static void PrintDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionaty)
    {
        string result = "";

        foreach (var kvp in dictionaty)
        {
            result += $"({kvp.Key} - {kvp.Value}) | ";
        }

        Debug.Log(result);
    }
}
