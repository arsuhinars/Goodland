using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RotatableObstacleEntity : MonoBehaviour, ISpawnable
{
    [SerializeField] private RotatableObstacleSettings m_settings;

    private Rigidbody2D m_rb;

    public void Spawn()
    {
        gameObject.SetActive(true);
        m_rb.rotation = 0f;
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var deltaAngle = m_settings.rotationSpeed * Time.fixedDeltaTime;
        m_rb.MoveRotation(m_rb.rotation + deltaAngle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_settings.doHarm)
        {
            collision.gameObject.GetComponent<ISpawnable>()?.Kill();
        }
    }
}
