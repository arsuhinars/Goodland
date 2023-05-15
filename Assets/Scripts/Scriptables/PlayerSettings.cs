using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName ="Game/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Moving")]
    public float flyImpulseFactor = 1f;
    public float flyImpulsePeriod = 1f;
    public float strafeForceFactor = 1f;
    public float rotationSmoothTime = 1f;
    public float flyingRotationAngle = 0f;
    [Header("Ground check")]
    public int groundRaysCount = 3;
    public float maxGroundDistance = 1f;
    public LayerMask groundMask;
    [Header("Powerups")]
    public float scaleUpFactor = 1f;
    public float scaleUpGravity = 1f;
    [Space]
    public float scaleDownFactor = 1f;
    public float scaleDownGravity = 1f;
}
