using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unclassified
{
    public static Vector2 GetRandomPosition(HashSet<Vector2> hashset)
    {
        int hashsetLength = hashset.Count;

        if (hashsetLength == 0)
        {
            return GlobalValuesManager.Instance.BadPosiiton;
        }

        Vector2[] values = new Vector2[hashsetLength];
        hashset.CopyTo(values, 0);

        return values[UnityEngine.Random.Range(0, hashsetLength)];
    }

    public static TKey GetRandomKey<TKey, TValue>(Dictionary<TKey, TValue> dict)
    {
        int dictLength = dict.Count;

        if (dictLength == 0)
        {
            return default;
        }

        TKey[] values = new TKey[dictLength];
        dict.Keys.CopyTo(values, 0);

        return values[UnityEngine.Random.Range(0, dictLength)];
    }

    public static int GetActiveChildrenCount(GameObject obj)
    {
        int count = 0;

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Transform child = obj.transform.GetChild(i);

            if (child.gameObject.activeInHierarchy)
            {
                count += 1;
            }
        }

        return count;
    }
}
