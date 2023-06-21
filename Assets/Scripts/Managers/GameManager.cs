using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string RECORD_PREFS_KEY = "record";

    public static GameManager Instance { get; private set; }

    public event Action OnStart;
    public event Action<GameEndReason> OnEnd;
    public event Action OnPause;
    public event Action OnResume;

    public bool IsStarted => m_isStarted;
    public bool IsPaused => m_isPaused;
    public int Score
    {
        get => m_score;
        private set
        {
            UIStateManager.Instance.State["score"] = value.ToString();
            m_score = value;
        }
    }
    public int Record => m_record;

    public CameraEntity Camera => m_camera;
    public PlayerEntity Player => m_player;

    [SerializeField] private GameSettings m_settings;

    private bool m_isStarted = false;
    private bool m_isPaused = false;
    private int m_score = 0;
    private int m_record = 0;
    private float m_initialPlayerOffset;
    private float m_maxPlayerOffset;
    private CameraEntity m_camera;
    private PlayerEntity m_player;

    public enum GameEndReason
    {
        Finished, Died
    }

    public void StartGame()
    {
        if (m_isStarted)
        {
            return;
        }

        m_isPaused = false;
        m_isStarted = true;
        Score = 0;
        UpdateRecord();
        OnStart?.Invoke();

        m_initialPlayerOffset = Player.transform.position.x;
        m_maxPlayerOffset = 0f;
    }

    public void EndGame(GameEndReason reason)
    {
        if (!m_isStarted)
        {
            return;
        }

        UpdateRecord();
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
        m_player = FindObjectOfType<PlayerEntity>();
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

    private void Update()
    {
        if (IsStarted)
        {
            float offset = Player.transform.position.x - m_initialPlayerOffset;
            m_maxPlayerOffset = Mathf.Max(m_maxPlayerOffset, offset);
            Score = Mathf.FloorToInt(m_maxPlayerOffset * m_settings.scorePerUnit);
        }
    }

    private void UpdateRecord()
    {
        m_record = PlayerPrefs.GetInt(RECORD_PREFS_KEY, 0);

        if (m_score > m_record)
        {
            m_record = m_score;
            PlayerPrefs.SetInt(RECORD_PREFS_KEY, m_record);
        }

        UIStateManager.Instance.State["record"] = m_record.ToString();
    }
}
