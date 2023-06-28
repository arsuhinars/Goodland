using UnityEngine.UIElements;

public class NavigationButton : Button
{
    public new class UxmlFactory : UxmlFactory<NavigationButton, UxmlTraits> { }

    public new class UxmlTraits : Button.UxmlTraits
    {
        UxmlStringAttributeDescription m_ViewName = new()
        {
            name = "view-name"
        };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            (ve as NavigationButton).ViewName = m_ViewName.GetValueFromBag(bag, cc);
        }
    }

    private string m_viewName;

    public string ViewName
    {
        get => m_viewName;
        set => m_viewName = value;
    }

    public NavigationButton() : base()
    {
        clicked += OnClick;
    }

    private void OnClick()
    {
        UIViewsManager.Instance.SetView(m_viewName);
    }
}
