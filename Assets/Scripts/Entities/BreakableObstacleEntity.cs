using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BreakableObstacleEntity : MonoBehaviour, ISpawnable
{
    [SerializeField] private BreakableObstacleSettings m_settings;
    [SerializeField] private SpriteRenderer m_sprite;
    [SerializeField] private ParticleSystem m_destroyParticles;

    private Rigidbody2D m_rb;
    private Collider2D[] m_colliders;
    private PlayerEntity m_touchingPlayer;
    private Vector3 m_initialPos;
    private Quaternion m_initialRot;
    private float m_sqrBreakVel;
    private bool m_isAlive = false;

    public void Spawn()
    {
        if (m_isAlive)
        {
            return;
        }

        gameObject.SetActive(true);

        for (int i = 0; i < m_colliders.Length; ++i)
        {
            m_colliders[i].enabled = true;
        }

        m_isAlive = true;
        m_sprite.enabled = true;
        m_rb.isKinematic = true;
        m_rb.velocity = Vector2.zero;
        m_rb.angularVelocity = 0f;
        transform.SetLocalPositionAndRotation(m_initialPos, m_initialRot);
        m_destroyParticles.Stop();
    }

    public void Kill()
    {
        if (!m_isAlive)
        {
            return;
        }

        for (int i = 0; i < m_colliders.Length; ++i)
        {
            m_colliders[i].enabled = false;
        }

        m_isAlive = false;
        m_sprite.enabled = false;
        m_rb.isKinematic = true;
        m_rb.velocity = Vector2.zero;
        m_rb.angularVelocity = 0f;
        m_destroyParticles.Play();

        gameObject.SetActive(false);
    }

    public void Drop()
    {
        m_rb.isKinematic = false;
    }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_sqrBreakVel = Mathf.Pow(m_settings.minBreakVelocity, 2f);
        m_initialPos = transform.localPosition;
        m_initialRot = transform.localRotation;
        m_colliders = new Collider2D[m_rb.attachedColliderCount];
        m_rb.GetAttachedColliders(m_colliders);
    }

    private void Start()
    {
        Spawn();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Utils.LayerMaskContainsLayer(m_settings.groundMask, collision.gameObject.layer) &&
            collision.relativeVelocity.sqrMagnitude > m_sqrBreakVel
        ) {
            Kill();
        }

        if (collision.gameObject.CompareTag(m_settings.playerTag))
        {
            m_touchingPlayer = collision.gameObject.GetComponent<PlayerEntity>();
            CheckPlayerCollision();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (m_touchingPlayer == null ||
            collision.gameObject != m_touchingPlayer.gameObject
        ) {
            return;
        }

        CheckPlayerCollision();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (m_touchingPlayer != null &&
            collision.gameObject == m_touchingPlayer.gameObject
        ) {
            m_touchingPlayer = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(m_settings.playerTag))
        {
            Drop();
        }
    }

    private void CheckPlayerCollision()
    {
        if (m_touchingPlayer == null)
        {
            return;
        }

        if (m_touchingPlayer.IsGrounded &&
            m_touchingPlayer.transform.position.y < m_rb.position.y
        )
        {
            m_touchingPlayer.Kill();
        }
    }
}
