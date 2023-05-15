using UnityEngine;

[CreateAssetMenu(fileName = "PowerupSettings", menuName = "Game/Powerup Settings")]
public class PowerupSettings : ScriptableObject
{
    public PowerupEntity.PowerupType powerupType;
    public string playerTag;
    public float flyAmplitude = 1f;
    public float flyPeriod = 1f;
}
