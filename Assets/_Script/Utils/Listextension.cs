using System.Collections.Generic;

public static class Listextension 
{
    // T는 Template
    public static T Random<T>(this List<T> list)
    {
        int rnd = UnityEngine.Random.Range(0, list.Count);

        return list[rnd];
    }
}
