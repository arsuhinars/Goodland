using DG.Tweening;
using UnityEngine;

public class PowerupEntity : MonoBehaviour, ISpawnable
{
    [SerializeField] private PowerupSettings m_settings;
    [SerializeField] private GameObject m_spritesRoot;

    private Tween m_tween;

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        var up = Vector3.up * m_settings.flyAmplitude;
        var down = -up;

        m_spritesRoot.transform.localPosition = down;

        var upTween = m_spritesRoot.transform.DOLocalMove(
            up, m_settings.flyPeriod
        );
        upTween.SetEase(Ease.InOutSine);

        var downTween = m_spritesRoot.transform.DOLocalMove(
            down, m_settings.flyPeriod
        );
        downTween.SetEase(Ease.InOutSine);

        var sequence = DOTween.Sequence();
        sequence.Append(upTween);
        sequence.Append(downTween);
        sequence.SetLoops(-1);

        m_tween = sequence;
    }

    private void OnDestroy()
    {
        m_tween.Kill();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(m_settings.playerTag))
        {
            var player = collision.gameObject.GetComponent<PlayerEntity>();
            Kill();
            player.ApplyPowerup(m_settings.powerupType);
        }
    }

    public enum PowerupType
    {
        Reset, ScaleUp, ScaleDown
    }
}
