using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIViewsManager : MonoBehaviour
{
    public static UIViewsManager Instance { get; private set; }

    public VisualElement RootElement => m_rootElement;

    [SerializeField]
    private SerializedKeyValuePair<string, VisualTreeAsset>[] m_views;
    [SerializeField] private string m_rootElementName;
    [SerializeField] private string m_viewRootClass;

    private Dictionary<string, TransitionElement> m_viewsRoots = new();
    private string m_activeView = "";
    private VisualElement m_rootElement;
    private UIDocument m_document;

    public string ActiveView => m_activeView;

    public void SetView(string viewName)
    {
        if (m_activeView.Length > 0)
        {
            m_viewsRoots[m_activeView].State = false;
        }

        if (viewName.Length == 0)
        {
            HideAllViews();
            return;
        }

        try
        {
            m_viewsRoots[viewName].State = true;
            m_activeView = viewName;
        }
        catch (KeyNotFoundException)
        {
            Debug.LogError($"Unable to open view with name \"{viewName}\"");
        }
    }

    public void HideAllViews()
    {
        foreach (var item in m_viewsRoots)
        {
            item.Value.State = false;
            item.Value.EndTransition();
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

        m_document = GetComponent<UIDocument>();
    }

    private void Start()
    {
        AddViewsToRoot();
        HideAllViews();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void AddViewsToRoot()
    {
        m_rootElement = m_document.rootVisualElement.Q<VisualElement>(m_rootElementName);

        foreach (var item in m_views)
        {
            var viewRoot = new VisualElement();
            viewRoot.AddToClassList(m_viewRootClass);
            viewRoot.pickingMode = PickingMode.Ignore;

            item.value.CloneTree(viewRoot);
            m_rootElement.Add(viewRoot);

            m_viewsRoots[item.key] = viewRoot.Q<TransitionElement>();
        }
    }
}
