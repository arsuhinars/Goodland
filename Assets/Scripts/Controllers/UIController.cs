using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIController : MonoBehaviour
{
    private void Start()
    {
        var gameManager = GameManager.Instance;
        gameManager.OnEnterMenu += OnEnterMenu;
        gameManager.OnStart += OnGameStart;
        gameManager.OnEnd += OnGameEnd;
        gameManager.OnPause += OnGamePause;
        gameManager.OnResume += OnGameResume;

        var actionManager = UIActionsManager.Instance;
        actionManager.SubscribeAction("Play", OnPlayAction);
        actionManager.SubscribeAction("PlayAgain", OnPlayAgainAction);
        actionManager.SubscribeAction("Resume", OnResumeAction);
        actionManager.SubscribeAction("Leave", OnLeaveAction);
        actionManager.SubscribeAction("Exit", OnExitAction);
    }

    private void OnDestroy()
    {
        var gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.OnEnterMenu -= OnEnterMenu;
            gameManager.OnStart -= OnGameStart;
            gameManager.OnEnd -= OnGameEnd;
            gameManager.OnPause -= OnGamePause;
            gameManager.OnResume -= OnGameResume;
        }

        var actionManager = UIActionsManager.Instance;
        if (actionManager != null)
        {
            actionManager.UnsubscribeAction("Play", OnPlayAction);
            actionManager.UnsubscribeAction("PlayAgain", OnPlayAgainAction);
            actionManager.UnsubscribeAction("Resume", OnResumeAction);
            actionManager.UnsubscribeAction("Leave", OnLeaveAction);
            actionManager.UnsubscribeAction("Exit", OnExitAction);
        }
    }

    private void OnEnterMenu()
    {
        UIViewsManager.Instance.SetView("Main");
    }

    private void OnGameStart()
    {
        UIViewsManager.Instance.SetView("Game");
    }

    private void OnGameEnd(GameManager.GameEndReason reason)
    {
        UIViewsManager.Instance.SetView("GameEnd");
    }

    private void OnGamePause()
    {
        UIViewsManager.Instance.SetView("Pause");
    }

    private void OnGameResume()
    {
        UIViewsManager.Instance.SetView("Game");
    }

    private void OnPlayAction()
    {
        GameManager.Instance.StartGame();
    }

    private void OnPlayAgainAction()
    {
        GameManager.Instance.StartGame();
    }

    private void OnResumeAction()
    {
        GameManager.Instance.ResumeGame();
    }

    private void OnLeaveAction()
    {
        GameManager.Instance.EnterMenu();
    }

    private void OnExitAction()
    {
        Application.Quit();
    }
}
