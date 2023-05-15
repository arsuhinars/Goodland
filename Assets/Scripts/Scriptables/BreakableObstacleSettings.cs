using UnityEngine;

[CreateAssetMenu(fileName = "BreakableObstacleSettings", menuName = "Game/Breakable Obstacle Settings")]
public class BreakableObstacleSettings : ScriptableObject
{
    public string playerTag;
    public float minBreakVelocity = 0f;
    public LayerMask groundMask;
}
