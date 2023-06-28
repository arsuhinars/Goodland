using UnityEngine;
using UnityEngine.UIElements;

public class TransitionElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<TransitionElement, UxmlTraits> {}

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_TransitionName = new() { name = "transition-name" };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            (ve as TransitionElement).TransitionName = m_TransitionName.GetValueFromBag(bag, cc);
        }
    }

    private string m_transitionName;

    private string ussTransitionEnterActive;
    private string ussTransitionEnterFrom;
    private string ussTransitionEnterTo;

    private string ussTransitionLeaveActive;
    private string ussTransitionLeaveFrom;
    private string ussTransitionLeaveTo;

    private TransitionState m_transitionState;

    private enum TransitionState
    {
        NONE,
        ENTERING, ENTERED,
        LEAVING, LEAVED
    }

    public TransitionElement()
    {
        RegisterCallback<TransitionEndEvent>(OnTransitionEnds);
    }

    public string TransitionName
    {
        get => m_transitionName;
        set
        {
            m_transitionName = value;

            ussTransitionEnterActive = m_transitionName + "-enter-active";
            ussTransitionEnterFrom = m_transitionName + "-enter-from";
            ussTransitionEnterTo = m_transitionName + "-enter-to";

            ussTransitionLeaveActive = m_transitionName + "-leave-active";
            ussTransitionLeaveFrom = m_transitionName + "-leave-from";
            ussTransitionLeaveTo = m_transitionName + "-leave-to";
        }
    }

    public bool State
    {
        get =>
            m_transitionState == TransitionState.ENTERING
            || m_transitionState == TransitionState.ENTERED;
        set
        {
            if (value
                && m_transitionState != TransitionState.ENTERING
                && m_transitionState != TransitionState.ENTERED)
            {
                Enter();
            }

            if (!value
                && m_transitionState != TransitionState.LEAVING
                && m_transitionState != TransitionState.LEAVED)
            {
                Leave();
            }
        }
    }

    public bool IsTransitioning =>
        m_transitionState == TransitionState.LEAVING
        || m_transitionState == TransitionState.ENTERING;

    public void Enter()
    {
        Reset();

        style.display = DisplayStyle.Flex;

        EnableInClassList(ussTransitionEnterFrom, true);
        EnableInClassList(ussTransitionEnterActive, true);

        UIManager.Instance.DoOnNextFrame(() =>
        {
            EnableInClassList(ussTransitionEnterFrom, false);
            EnableInClassList(ussTransitionEnterTo, true);
        });

        m_transitionState = TransitionState.ENTERING;
    }

    public void Leave()
    {
        Reset();

        EnableInClassList(ussTransitionLeaveFrom, true);
        EnableInClassList(ussTransitionLeaveActive, true);

        UIManager.Instance.DoOnNextFrame(() =>
        {
            EnableInClassList(ussTransitionLeaveFrom, false);
            EnableInClassList(ussTransitionLeaveTo, true);
        });

        m_transitionState = TransitionState.LEAVING;
    }

    public void Reset()
    {
        EnableInClassList(ussTransitionEnterActive, false);
        EnableInClassList(ussTransitionEnterFrom, false);
        EnableInClassList(ussTransitionEnterTo, false);
        EnableInClassList(ussTransitionLeaveActive, false);
        EnableInClassList(ussTransitionLeaveFrom, false);
        EnableInClassList(ussTransitionLeaveTo, false);

        m_transitionState = TransitionState.NONE;
    }

    public void EndTransition()
    {
        switch (m_transitionState)
        {
            case TransitionState.ENTERING:
                m_transitionState = TransitionState.ENTERED;
                EnableInClassList(ussTransitionLeaveActive, false);
                break;
            case TransitionState.LEAVING:
                m_transitionState = TransitionState.LEAVED;
                EnableInClassList(ussTransitionLeaveActive, false);
                style.display = DisplayStyle.None;
                break;
        }
    }

    private void OnTransitionEnds(TransitionEndEvent ev)
    {
        EndTransition();
    }
}
