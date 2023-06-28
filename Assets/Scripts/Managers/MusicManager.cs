using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private MusicSettings m_settings;

    private int m_activeSongIdx = 0;
    private float m_initialVolume;
    private float m_mutedVolume;
    private AudioSource m_audioSource;
    private Coroutine m_switchMusicRoutine;

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

    private void Start()
    {
        m_audioSource = Instantiate(m_settings.audioSourcePrefab, transform);
        m_initialVolume = m_audioSource.volume;
        m_mutedVolume = m_audioSource.volume * m_settings.mutedMusicVolume;

        m_activeSongIdx = Random.Range(0, m_settings.songs.Length);

        var gameManager = GameManager.Instance;

        gameManager.OnEnterMenu += OnEnterMenu;
        gameManager.OnStart += OnGameStart;
        gameManager.OnEnd += OnGameEnd;
        gameManager.OnPause += OnPause;
        gameManager.OnResume += OnResume;
    }

    private void OnDestroy()
    {
        var gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.OnEnterMenu -= OnEnterMenu;
            gameManager.OnStart -= OnGameStart;
            gameManager.OnEnd -= OnGameEnd;
            gameManager.OnPause -= OnPause;
            gameManager.OnResume -= OnResume;
        }

        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void PlayMusic()
    {
        if (m_audioSource.isPlaying)
        {
            m_audioSource.DOFade(
                m_initialVolume, m_settings.musicTransitionDuration
            ).SetUpdate(true).SetAutoKill();
            return;
        }

        int songIdx = m_activeSongIdx++ % m_settings.songs.Length;

        m_audioSource.clip = m_settings.songs[songIdx];
        m_audioSource.loop = false;
        m_audioSource.volume = m_initialVolume;
        m_audioSource.Play();

        m_switchMusicRoutine = StartCoroutine(SwitchMusicRoutine());
    }

    private void MuteMusic()
    {
        m_audioSource.DOFade(
            m_mutedVolume, m_settings.musicTransitionDuration
        ).SetUpdate(true).SetAutoKill();
    }

    private void StopMusic()
    {
        m_audioSource.Stop();
        if (m_switchMusicRoutine != null)
        {
            StopCoroutine(m_switchMusicRoutine);
            m_switchMusicRoutine = null;
        }
    }

    private IEnumerator SwitchMusicRoutine()
    {
        yield return new WaitForSecondsRealtime(m_audioSource.clip.length);
        m_audioSource.Stop();
        yield return new WaitForSecondsRealtime(m_settings.musicDelay);
        PlayMusic();
    }

    private void OnGameStart()
    {
        PlayMusic();
    }

    private void OnGameEnd(GameManager.GameEndReason reason)
    {
        MuteMusic();
    }

    private void OnEnterMenu()
    {
        StopMusic();
    }

    private void OnPause()
    {
        MuteMusic();
    }

    private void OnResume()
    {
        PlayMusic();
    }
}
