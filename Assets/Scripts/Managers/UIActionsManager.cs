using System;
using System.Collections.Generic;
using UnityEngine;

public class UIActionsManager : MonoBehaviour
{
    public static UIActionsManager Instance { get; private set; }

    private Dictionary<string, HashSet<Action>> m_actionHandlers = new();

    public void SubscribeAction(string name, Action handler)
    {
        if (!m_actionHandlers.TryGetValue(name, out var handlers))
        {
            handlers = new HashSet<Action>();
            m_actionHandlers.Add(name, handlers);
        }

        handlers.Add(handler);
    }

    public void UnsubscribeAction(string name, Action handler)
    {
        if (m_actionHandlers.TryGetValue(name, out var handlers))
        {
            handlers.Remove(handler);
        }
        else
        {
            Debug.LogError($"Unable to remove handler from \"{name}\" action, because it doesn't exist");
        }
    }

    public void InvokeAction(string name)
    {
        if (m_actionHandlers.TryGetValue(name, out var handlers))
        {
            foreach (var handler in handlers)
            {
                handler();
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
