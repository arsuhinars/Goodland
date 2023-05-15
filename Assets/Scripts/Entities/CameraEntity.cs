using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraEntity : MonoBehaviour
{
    public Camera Camera => m_camera;
    public Bounds Bounds => m_bounds;

    [SerializeField] private CameraSettings m_settings;

    private Camera m_camera;
    private Bounds m_bounds;
    private float m_delayTimer;

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
    }

    private void Start()
    {
        m_bounds = new(
            m_settings.initialPos,
            new Vector3(
                m_camera.aspect * m_camera.orthographicSize * 2,
                m_camera.orthographicSize * 2,
                float.MaxValue
            )
        );

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

        if (m_delayTimer > 0f)
        {
            m_delayTimer -= Time.deltaTime;
            return;
        }

        var delta = m_settings.moveSpeed * Time.deltaTime * Vector3.right;
        transform.position += delta;

        m_bounds.center = transform.position;
    }

    private void OnGameStart()
    {
        m_delayTimer = m_settings.startGameDelay;
        transform.position = m_settings.initialPos;
    }
}
