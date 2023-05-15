using UnityEngine;
using UnityEngine.Pool;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelSettings m_settings;
    [SerializeField] private Transform m_sectionsParent;
    [SerializeField] private LevelSection[] m_sectionsPrefabs;

    private IObjectPool<LevelSection>[] m_sectionsPools;
    private LevelSection m_lastSection = null;

    private void Awake()
    {

    }

    private void Start()
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
        if (m_lastSection == null || cameraBounds.Intersects(m_lastSection.Bounds))
        {
            m_lastSection = GetNextSection();
        }
    }

    private void OnGameStart()
    {
        for (int i = 0; i < m_sectionsPools.Length; i++)
        {
            m_sectionsPools[i].Clear();
        }

        var cameraBounds = GameManager.Instance.Camera.Bounds;
        m_lastSection = null;
        while (
            m_lastSection == null ||
            cameraBounds.Intersects(m_lastSection.Bounds)
        ) {
            m_lastSection = GetNextSection();
        }
    }

    private LevelSection GetNextSection()
    {
        int prefabIdx = Random.Range(0, m_sectionsPools.Length);
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

        if (m_lastSection != null)
        {
            pos = m_lastSection.transform.position;
            pos.x += (m_lastSection.Size.x + section.Size.x) * 0.5f;
        }
        else
        {
            var camera = GameManager.Instance.Camera;

            pos = m_settings.initialSectionsPos;
            pos.x = camera.Bounds.min.x + section.Size.x * 0.5f;
        }

        section.transform.position = pos;
        section.Spawn();
    }

    private void OnReleasePoolItem(LevelSection section)
    {
        section.Kill();
        section.gameObject.SetActive(false);
    }

    private void OnDestroyPoolItem(LevelSection section)
    {
        Destroy(section);
    }
}
