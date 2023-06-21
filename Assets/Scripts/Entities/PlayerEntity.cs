using UnityEngine;
using static PowerupEntity;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerEntity : MonoBehaviour, ISpawnable
{
    public float FlyFactor
    {
        get => m_flyFactor;
        set => m_flyFactor = Mathf.Clamp01(value);
    }
    public float StrafeFactor
    {
        get => m_strafeFactor;
        set => m_strafeFactor = Mathf.Clamp(value, -1f, 1f);
    }
    public bool IsGrounded => m_isGrounded;

    [SerializeField] private PlayerSettings m_settings;

    private float m_flyFactor;
    private float m_strafeFactor;
    private bool m_isGrounded = false;
    private float m_impulseTimer = 0f;
    private float m_rotVel = 0f;
    private Vector2[] m_groundRaysDirs;
    private Rigidbody2D m_rb;

    public void Spawn()
    {
        transform.localScale = Vector3.one;
        m_rb.gravityScale = 1f;
    }

    public void Kill()
    {
        GameManager.Instance.EndGame(GameManager.GameEndReason.Died);
    }

    public void ApplyPowerup(PowerupType powerupType)
    {
        switch (powerupType)
        {
            case PowerupType.Reset:
                transform.localScale = Vector3.one;
                m_rb.gravityScale = 1f;
                break;
            case PowerupType.ScaleUp:
                transform.localScale = Vector3.one * m_settings.scaleUpFactor;
                m_rb.gravityScale = m_settings.scaleUpGravity;
                break;
            case PowerupType.ScaleDown:
                transform.localScale = Vector3.one * m_settings.scaleDownFactor;
                m_rb.gravityScale = m_settings.scaleDownGravity;
                break;
        }
    }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        int raysCount = m_settings.groundRaysCount;
        m_groundRaysDirs = new Vector2[raysCount];
        for (int i = 0; i < raysCount; ++i)
        {
            float angle = Mathf.PI / (raysCount + 1) * (i + 1);
            var dir = new Vector2(Mathf.Cos(angle), -Mathf.Sin(angle));
            m_groundRaysDirs[i] = dir;
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsStarted)
        {
            m_rb.freezeRotation = false;
            return;
        }

        if (m_impulseTimer <= 0f)
        {
            m_rb.AddForce(m_flyFactor * m_settings.flyImpulseFactor * Vector2.up, ForceMode2D.Impulse);
            m_impulseTimer = m_settings.flyImpulsePeriod;
        }
        else
        {
            m_impulseTimer -= Time.fixedDeltaTime;
        }

        m_isGrounded = CheckIsGrounded();
        m_rb.freezeRotation = !m_isGrounded;
        if (!m_isGrounded)
        {
            m_rb.AddForce(
                m_strafeFactor * m_settings.strafeForceFactor * Vector2.right
            );
            m_rb.rotation = Mathf.SmoothDampAngle(
                m_rb.rotation,
                m_settings.flyingRotationAngle,
                ref m_rotVel,
                m_settings.rotationSmoothTime
            );
        }
    }

    private bool CheckIsGrounded()
    {
        float scaledGroundDist = m_settings.maxGroundDistance * transform.localScale.y;
        float minDistance = float.MaxValue;
        for (int i = 0; i < m_groundRaysDirs.Length; ++i)
        {
            var dir = m_groundRaysDirs[i];
            var hit = Physics2D.Raycast(
                m_rb.position,
                dir,
                scaledGroundDist * 1.2f,
                m_settings.groundMask
            );
            if (hit.collider != null)
            {
                minDistance = Mathf.Min(minDistance, hit.distance);
            }
        }
        return minDistance < scaledGroundDist;
    }
}
