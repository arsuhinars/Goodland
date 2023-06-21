using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool LayerMaskContainsLayer(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static IDictionary<K, V> CreateDictionaryFromItems<K, V>(ICollection<SerializedKeyValuePair<K, V>> collection)
    {
        var dict = new Dictionary<K, V>();
        foreach (var item in collection)
        {
            dict.Add(item.key, item.value);
        }
        return dict;
    }
}
