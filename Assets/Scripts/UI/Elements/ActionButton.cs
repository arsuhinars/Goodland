using UnityEngine.UIElements;

public class ActionButton : Button
{
    public new class UxmlFactory : UxmlFactory<ActionButton, UxmlTraits> { }

    public new class UxmlTraits : Button.UxmlTraits
    {
        UxmlStringAttributeDescription m_ActionName = new()
        {
            name = "action-name"
        };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            (ve as ActionButton).ActionName = m_ActionName.GetValueFromBag(bag, cc);
        }
    }

    private string m_actionName;

    public string ActionName
    {
        get => m_actionName;
        set => m_actionName = value;
    }

    public ActionButton() : base()
    {
        clicked += OnClick;
    }

    private void OnClick()
    {
        UIActionsManager.Instance.InvokeAction(m_actionName);
    }
}
