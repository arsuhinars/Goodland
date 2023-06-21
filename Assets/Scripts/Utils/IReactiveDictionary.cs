using System.Collections.Generic;

public delegate void ItemUpdateHandler<K, V>(K key, V oldValue, V newValue);

public interface IReactiveDictionary<K, V> : IDictionary<K, V>
{
    public void AddHandler(K key, ItemUpdateHandler<K, V> handler);

    public void RemoveHandler(K key, ItemUpdateHandler<K, V> handler);

    public void UpdateFrom(IDictionary<K, V> dictionary);
}
