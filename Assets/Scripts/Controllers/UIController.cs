using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIController : MonoBehaviour
{
    private void Start()
    {
        var gameManager = GameManager.Instance;
        gameManager.OnStart += OnGameStart;
        gameManager.OnEnd += OnGameEnd;
        gameManager.OnPause += OnGamePause;
        gameManager.OnResume += OnGameResume;

        var actionManager = UIActionsManager.Instance;
        actionManager.SubscribeAction("PlayAgain", OnPlayAgainAction);
        actionManager.SubscribeAction("Resume", OnResumeAction);
        actionManager.SubscribeAction("Exit", OnExitAction);
    }

    private void OnDestroy()
    {
        var gameManager = GameManager.Instance;
        gameManager.OnStart -= OnGameStart;
        gameManager.OnEnd -= OnGameEnd;
        gameManager.OnPause -= OnGamePause;
        gameManager.OnResume -= OnGameResume;

        var actionManager = UIActionsManager.Instance;
        actionManager.UnsubscribeAction("PlayAgain", OnPlayAgainAction);
        actionManager.UnsubscribeAction("Resume", OnResumeAction);
        actionManager.UnsubscribeAction("Exit", OnExitAction);
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

    private void OnPlayAgainAction()
    {
        GameManager.Instance.StartGame();
    }

    private void OnResumeAction()
    {
        GameManager.Instance.ResumeGame();
    }

    private void OnExitAction()
    {
        Application.Quit();
    }
}
