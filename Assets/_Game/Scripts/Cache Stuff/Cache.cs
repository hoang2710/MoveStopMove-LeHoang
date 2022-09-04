using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache<Tkey, Tvalue> where Tkey : Component
{
    public static Dictionary<Tkey, Tvalue> dict = new Dictionary<Tkey, Tvalue>();

    public static Tvalue Get(Tkey key)
    {
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, key.GetComponent<Tvalue>());
        }

        return dict[key];
    }
}
