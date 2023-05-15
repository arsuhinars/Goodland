using UnityEngine;

[CreateAssetMenu(fileName = "RotatableObstacleSettings", menuName = "Game/Rotatable Obstacle Settings")]
public class RotatableObstacleSettings : ScriptableObject
{
    public float rotationSpeed = 1f;
    public string playerTag;
    public bool harmPlayer = false;
}
