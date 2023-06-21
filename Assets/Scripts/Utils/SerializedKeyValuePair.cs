using System;

[Serializable]
public struct SerializedKeyValuePair<K, V>
{
    public K key;
    public V value;
}
