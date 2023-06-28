using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenInputManager : MonoBehaviour
{
    public event Action<float> OnFlyAction;
    public event Action<float> OnStrafeAction;
    public event Action OnPauseClickAction;

    public static ScreenInputManager Instance { get; private set; }

    [SerializeField] private ScreenInputSettings m_settings;

    private VisualElement m_pauseButtonEl;
    private VisualElement m_upButtonEl;
    private VisualElement m_leftButtonEl;
    private VisualElement m_rightButtonEl;

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

    private IEnumerator Start()
    {
        yield return null;

        var rootEl = UIViewsManager.Instance.RootElement;

        m_pauseButtonEl = rootEl.Q(m_settings.pauseButtonElementName);
        m_upButtonEl = rootEl.Q(m_settings.upButtonElementName);
        m_leftButtonEl = rootEl.Q(m_settings.leftButtonElementName);
        m_rightButtonEl = rootEl.Q(m_settings.rightButtonElementName);

        m_pauseButtonEl.RegisterCallback<ClickEvent>(OnPauseClick);
        m_upButtonEl.RegisterCallback<PointerDownEvent>(OnUpButtonPress, TrickleDown.TrickleDown);
        m_upButtonEl.RegisterCallback<PointerUpEvent>(OnUpButtonRelease, TrickleDown.TrickleDown);
        m_leftButtonEl.RegisterCallback<PointerDownEvent>(OnLeftButtonPress, TrickleDown.TrickleDown);
        m_leftButtonEl.RegisterCallback<PointerUpEvent>(OnLeftButtonRelease, TrickleDown.TrickleDown);
        m_rightButtonEl.RegisterCallback<PointerDownEvent>(OnRightButtonPress, TrickleDown.TrickleDown);
        m_rightButtonEl.RegisterCallback<PointerUpEvent>(OnRightButtonRelease, TrickleDown.TrickleDown);
    }

    private void OnDestroy()
    {
        m_pauseButtonEl.UnregisterCallback<ClickEvent>(OnPauseClick);
        m_upButtonEl.UnregisterCallback<PointerDownEvent>(OnUpButtonPress);
        m_upButtonEl.UnregisterCallback<PointerUpEvent>(OnUpButtonRelease);
        m_leftButtonEl.UnregisterCallback<PointerDownEvent>(OnLeftButtonPress);
        m_leftButtonEl.UnregisterCallback<PointerUpEvent>(OnLeftButtonRelease);
        m_rightButtonEl.UnregisterCallback<PointerDownEvent>(OnRightButtonPress);
        m_rightButtonEl.UnregisterCallback<PointerUpEvent>(OnRightButtonRelease);

        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void OnPauseClick(ClickEvent ev)
    {
        OnPauseClickAction?.Invoke();
    }

    private void OnUpButtonPress(PointerDownEvent ev)
    {
        OnFlyAction?.Invoke(1f);
    }

    private void OnUpButtonRelease(PointerUpEvent ev)
    {
        OnFlyAction?.Invoke(0f);
    }

    private void OnLeftButtonPress(PointerDownEvent ev)
    {
        OnStrafeAction?.Invoke(-1f);
    }

    private void OnLeftButtonRelease(PointerUpEvent ev)
    {
        OnStrafeAction?.Invoke(0f);
    }

    private void OnRightButtonPress(PointerDownEvent ev)
    {
        OnStrafeAction?.Invoke(1f);
    }

    private void OnRightButtonRelease(PointerUpEvent ev)
    {
        OnStrafeAction?.Invoke(0f);
    }
}
