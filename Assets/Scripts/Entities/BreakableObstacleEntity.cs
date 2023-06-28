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
    private float m_sqrKillVel;
    private float m_sqrBreakVel;

    public void Spawn()
    {
        for (int i = 0; i < m_colliders.Length; ++i)
        {
            m_colliders[i].enabled = true;
        }

        m_sprite.enabled = true;
        m_rb.isKinematic = true;
        m_rb.velocity = Vector2.zero;
        m_rb.angularVelocity = 0f;
        transform.SetLocalPositionAndRotation(m_initialPos, m_initialRot);
        m_destroyParticles.Stop();
    }

    public void Kill()
    {
        for (int i = 0; i < m_colliders.Length; ++i)
        {
            m_colliders[i].enabled = false;
        }

        if (!m_rb.isKinematic)
        {
            m_destroyParticles.Play();
        }

        m_sprite.enabled = false;
        m_rb.isKinematic = true;
        m_rb.velocity = Vector2.zero;
        m_rb.angularVelocity = 0f;
    }

    public void Drop()
    {
        m_rb.isKinematic = false;
    }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_sqrKillVel = Mathf.Pow(m_settings.minKillingVelocity, 2f);
        m_sqrBreakVel = Mathf.Pow(m_settings.minBreakVelocity, 2f);
        m_initialPos = transform.localPosition;
        m_initialRot = transform.localRotation;
        m_colliders = new Collider2D[m_rb.attachedColliderCount];
        m_rb.GetAttachedColliders(m_colliders);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.sqrMagnitude > m_sqrBreakVel)
        {
            Kill();
        }

        if (collision.gameObject.CompareTag(m_settings.playerTag))
        {
            m_touchingPlayer = collision.gameObject.GetComponent<PlayerEntity>();
            CheckPlayerCollision(collision.relativeVelocity);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (m_touchingPlayer == null ||
            collision.gameObject != m_touchingPlayer.gameObject
        ) {
            return;
        }

        CheckPlayerCollision(collision.relativeVelocity);
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

    private void CheckPlayerCollision(Vector2 relativeVelocity)
    {
        if (m_touchingPlayer == null || relativeVelocity.sqrMagnitude < m_sqrKillVel)
        {
            return;
        }

        if (m_touchingPlayer.transform.position.y < m_rb.position.y)
        {
            m_touchingPlayer.Kill();
        }
    }
}
