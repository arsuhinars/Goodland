using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnStart;
    public event Action<GameEndReason> OnEnd;
    public event Action OnPause;
    public event Action OnResume;

    public bool IsStarted => m_isStarted;
    public bool IsPaused => m_isPaused;
    public CameraEntity Camera => m_camera;

    [SerializeField] private GameSettings m_settings;

    private bool m_isStarted = false;
    private bool m_isPaused = false;
    private CameraEntity m_camera;

    public void StartGame()
    {
        if (m_isStarted)
        {
            return;
        }

        m_isPaused = false;
        m_isStarted = true;
        OnStart?.Invoke();
    }

    public void EndGame(GameEndReason reason)
    {
        if (!m_isStarted)
        {
            return;
        }

        m_isPaused = false;
        m_isStarted = false;
        OnEnd?.Invoke(reason);
    }

    public void PauseGame()
    {
        if (m_isPaused || !m_isStarted)
        {
            return;
        }

        m_isPaused = true;
        Time.timeScale = 0f;
        OnPause?.Invoke();
    }

    public void ResumeGame()
    {
        if (!m_isPaused || !m_isStarted)
        {
            return;
        }

        m_isPaused = false;
        Time.timeScale = 1f;
        OnResume?.Invoke();
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

        m_camera = FindObjectOfType<CameraEntity>();
    }

    private IEnumerator Start()
    {
        yield return null;
        StartGame();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public enum GameEndReason
    {
        Finished, Died
    }
}
