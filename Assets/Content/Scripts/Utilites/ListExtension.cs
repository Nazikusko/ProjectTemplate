using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class ListExtensions
{
    public static T GetRandomElement<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
            throw new InvalidOperationException("List is null or empty.");

        int index = Random.Range(0, list.Count);
        return list[index];
    }
}