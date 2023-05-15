using UnityEngine;
using UnityEngine.Pool;

public class LevelSection : MonoBehaviour, ISpawnable
{
    public Vector2 Size => m_settings.size;
    public Bounds Bounds => m_bounds;
    public IObjectPool<LevelSection> Pool
    {
        get => m_pool; set => m_pool = value;
    }

    [SerializeField] private LevelSectionSettings m_settings;
    [SerializeField] private SerializedInterfacesArray<ISpawnable> m_spawnables;

    private IObjectPool<LevelSection> m_pool;
    private Bounds m_bounds;
    private bool m_insideCameraView = false;
    private CameraEntity m_cameraEntity;

    public void Spawn()
    {
        gameObject.SetActive(true);

        m_bounds.size = new Vector3(
            m_settings.size.x,
            m_settings.size.y,
            float.MaxValue
        );
        m_bounds.center = transform.position;
        m_insideCameraView = false;

        for (int i = 0; i < m_spawnables.Array.Length; i++)
        {
            m_spawnables.Array[i].Spawn();
        }
    }

    public void Kill()
    {
        for (int i = 0; i < m_spawnables.Array.Length; i++)
        {
            m_spawnables.Array[i].Kill();
        }

        gameObject.SetActive(false);
    }

    private void Start()
    {
        m_cameraEntity = GameManager.Instance.Camera;
    }

    private void Update()
    {
        bool insideCameraView = m_cameraEntity.Bounds.Intersects(m_bounds);
        if (!insideCameraView && m_insideCameraView)
        {
            m_pool.Release(this);
        }
        m_insideCameraView = insideCameraView;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (m_settings == null)
        {
            return;
        }

        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(m_settings.size.x, m_settings.size.y, 1f)
        );
    }
#endif
}
