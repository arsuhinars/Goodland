using System.Collections.Generic;

public class ReactiveDictionary<K, V> : Dictionary<K, V>, IReactiveDictionary<K, V>
{
    private readonly Dictionary<K, HashSet<ItemUpdateHandler<K, V>>> m_handlers = new();

    public new V this[K key]
    {
        get => base[key];
        set
        {
            TryGetValue(key, out var oldValue);

            base[key] = value;

            TryInvokeUpdateHandler(key, oldValue, value);
        }
    }

    public new void Add(K key, V value)
    {
        base.Add(key, value);
        TryInvokeUpdateHandler(key, default, value);
    }

    public void Add(KeyValuePair<K, V> item)
    {
        Add(item.Key, item.Value);
    }

    public new void Clear()
    {
        foreach (var item in this)
        {
            TryInvokeUpdateHandler(item.Key, item.Value, default);
        }

        base.Clear();
    }

    public new bool Remove(K key)
    {
        if (!TryGetValue(key, out var value))
        {
            return false;
        }

        base.Remove(key);
        TryInvokeUpdateHandler(key, value, default);
        return true;
    }

    public void AddHandler(K key, ItemUpdateHandler<K, V> handler)
    {
        if (!m_handlers.TryGetValue(key, out var handlers))
        {
            handlers = new HashSet<ItemUpdateHandler<K, V>>();
            m_handlers.Add(key, handlers);
        }

        handlers.Add(handler);
    }

    public void RemoveHandler(K key, ItemUpdateHandler<K, V> handler)
    {
        m_handlers[key].Remove(handler);
    }

    public void UpdateFrom(IDictionary<K, V> dictionary)
    {
        foreach (var item in dictionary)
        {
            this[item.Key] = item.Value;
        }
    }

    private bool TryInvokeUpdateHandler(K key, V oldValue, V newValue)
    {
        if (oldValue == null ? newValue == null : oldValue.Equals(newValue))
        {
            return false;
        }

        if (!m_handlers.TryGetValue(key, out var handlers))
        {
            return false;
        }

        foreach (var handler in handlers)
        {
            handler(key, oldValue, newValue);
        }

        return true;
    }
}
