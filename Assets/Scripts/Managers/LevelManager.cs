using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelSettings m_settings;
    [SerializeField] private Transform m_sectionsParent;

    private IObjectPool<LevelSection>[] m_sectionsPools;
    private LinkedList<LevelSection> m_activeSections = new();

    private void Awake()
    {
        m_sectionsPools = new IObjectPool<LevelSection>[
            m_settings.sectionsPrefabs.Length
        ];
        for (int i = 0; i < m_settings.sectionsPrefabs.Length; i++)
        {
            int prefabIdx = i;
            m_sectionsPools[i] = new ObjectPool<LevelSection>(
                () => CreatePoolItem(prefabIdx),
                OnGetPoolItem,
                OnReleasePoolItem,
                OnDestroyPoolItem
            );
        }
    }

    private void Start()
    {
        var gameManager = GameManager.Instance;
        gameManager.OnEnterMenu += OnEnterMenu;
        gameManager.OnStart += OnGameStart;
    }

    private void OnDestroy()
    {
        var gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.OnEnterMenu -= OnEnterMenu;
            gameManager.OnStart -= OnGameStart;
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.IsStarted)
        {
            return;
        }

        var cameraBounds = GameManager.Instance.Camera.Bounds;
        if (
            m_activeSections.Count == 0
            || cameraBounds.Intersects(m_activeSections.Last.Value.Bounds)
        ) {
            GetNextSection();
        }
    }

    private void OnEnterMenu()
    {
        ClearAllSections();
    }

    private void OnGameStart()
    {
        ClearAllSections();

        var cameraBounds = GameManager.Instance.Camera.Bounds;
        float playerOffset = GameManager.Instance.InitialPlayerOffset;

        var lastSec = GetNextSection(m_settings.initialSectionIndex);
        while (cameraBounds.Intersects(lastSec.Bounds)) {
            int idxOverride = -1;

            if (lastSec.Bounds.center.x < playerOffset)
            {
                idxOverride = m_settings.initialSectionIndex;
            }

            lastSec = GetNextSection(idxOverride);
        }
    }

    private void ClearAllSections()
    {
        while (m_activeSections.Count > 0)
        {
            var section = m_activeSections.First.Value;
            section.Pool.Release(section);
        }
    }

    private LevelSection GetNextSection(int prefabOverrideIdx = -1)
    {
        int prefabIdx = prefabOverrideIdx >= 0
            ? prefabOverrideIdx
            : Random.Range(0, m_sectionsPools.Length);
        return m_sectionsPools[prefabIdx].Get();
    }

    private LevelSection CreatePoolItem(int prefabIdx)
    {
        var instance = Instantiate(
            m_settings.sectionsPrefabs[prefabIdx],
            m_sectionsParent
        );
        instance.Pool = m_sectionsPools[prefabIdx];
        return instance;
    }

    private void OnGetPoolItem(LevelSection section)
    {
        Vector3 pos;

        if (m_activeSections.Count > 0)
        {
            var lastSection = m_activeSections.Last.Value;
            pos = lastSection.transform.position;
            pos.x += (lastSection.Size.x + section.Size.x) * 0.5f;
        }
        else
        {
            var camera = GameManager.Instance.Camera;

            pos = m_settings.initialSectionsPos;
            pos.x = camera.Bounds.min.x + section.Size.x * 0.5f;
        }

        section.transform.position = pos;
        section.Spawn();

        m_activeSections.AddLast(section);
    }

    private void OnReleasePoolItem(LevelSection section)
    {
        section.Kill();
        section.gameObject.SetActive(false);

        m_activeSections.Remove(section);
    }

    private void OnDestroyPoolItem(LevelSection section)
    {
        Destroy(section);
    }
}
