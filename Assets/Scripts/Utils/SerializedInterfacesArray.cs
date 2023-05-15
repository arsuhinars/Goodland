using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedInterfacesArray<T> where T : class
{
    public T[] Array
    {
        get
        {
            if (m_cachedComponents == null)
            {
                Initialize();
            }
            
            return m_cachedComponents;
        }
    }

    [SerializeField] private Component[] m_components;
    private T[] m_cachedComponents;

    public void Initialize()
    {
        if (m_cachedComponents != null)
        {
            return;
        }

        if (m_components == null)
        {
            m_cachedComponents = new T[0];
            return;
        }

        var cachedComponents = new List<T>(m_components.Length);
        for (int i = 0; i < m_components.Length; i++)
        {
            var component = m_components[i];
            if (component != null &&
                typeof(T).IsAssignableFrom(component.GetType())
            ) {
                cachedComponents.Add(component as T);
            }
            else
            {
                Debug.LogError($"Items in SerializedInterfacesArray must be inherited from {typeof(T)}");
            }
        }

        m_cachedComponents = cachedComponents.ToArray();
    }
}
