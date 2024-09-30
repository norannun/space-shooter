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

    public static void PrintArray<T>(T[,] array)
    {
        string result = "";

        // Create string array width back
        int lengthX = array.GetLength(0);
        int lengthY = array.GetLength(1);
        for (int i = lengthY - 1; i >= 0; i--)
        {
            string substring = "";
            for (int j = 0; j < lengthX; j++)
            {
                substring += array[j, i].ToString();
            }
            result += $"{substring}\n";
        }

        Debug.Log(result);
    }

    public static void PrintList2D<T>(List<List<T>> list)
    {
        string result = "";

        // Create string array width back
        int lengthX = list.Count;
        int lengthY = list[0].Count;
        for (int i = lengthY - 1; i >= 0; i--)
        {
            string substring = "";
            for (int j = 0; j < lengthX; j++)
            {
                substring += list[j][i].ToString();
            }
            result += $"{substring}\n";
        }

        Debug.Log(result);
    }
}
