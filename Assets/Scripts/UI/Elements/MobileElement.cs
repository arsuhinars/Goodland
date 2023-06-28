using UnityEngine;
using UnityEngine.UIElements;

public class MobileElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<MobileElement, UxmlTraits> { }

    public MobileElement()
    {
        RegisterCallback<AttachToPanelEvent>(OnAttachedToPanel);
    }

    private void OnAttachedToPanel(AttachToPanelEvent ev)
    {
        style.display =
            (Application.isMobilePlatform || Application.isEditor)
            ? DisplayStyle.Flex
            : DisplayStyle.None;
    }
}
