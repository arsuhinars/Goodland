using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelSettings m_settings;
    [SerializeField] private Transform m_sectionsParent;
    [SerializeField] private LevelSection[] m_sectionsPrefabs;
    [Space]
    [SerializeField] private int m_initialSectionIndex;
    [SerializeField] private int m_initialSectionsCount;

    private IObjectPool<LevelSection>[] m_sectionsPools;
    private LinkedList<LevelSection> m_activeSections = new();
    //private LevelSection m_lastSection = null;
    private int m_sectionsCounter;

    private void Awake()
    {
        m_sectionsPools = new IObjectPool<LevelSection>[m_sectionsPrefabs.Length];
        for (int i = 0; i < m_sectionsPrefabs.Length; i++)
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
        GameManager.Instance.OnStart += OnGameStart;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnStart -= OnGameStart;
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

    private void OnGameStart()
    {
        m_sectionsCounter = 0;

        while (m_activeSections.Count > 0)
        {
            var section = m_activeSections.First.Value;
            section.Pool.Release(section);
        }

        var cameraBounds = GameManager.Instance.Camera.Bounds;

        m_activeSections.Clear();
        while(
            m_activeSections.Count == 0 ||
            cameraBounds.Intersects(m_activeSections.Last.Value.Bounds)
        ) {
            GetNextSection();
        }
    }

    private LevelSection GetNextSection()
    {
        int prefabIdx = m_sectionsCounter++ < m_initialSectionsCount
            ? m_initialSectionIndex
            : Random.Range(0, m_sectionsPools.Length);
        return m_sectionsPools[prefabIdx].Get();
    }

    private LevelSection CreatePoolItem(int prefabIdx)
    {
        var instance = Instantiate(
            m_sectionsPrefabs[prefabIdx],
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
