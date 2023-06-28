using UnityEngine;
using UnityEngine.UIElements;

public class UIStateManager : MonoBehaviour
{
    public static UIStateManager Instance { get; private set; }

    private ReactiveDictionary<string, string> m_state = new();

    public IReactiveDictionary<string, string> State => m_state;

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
