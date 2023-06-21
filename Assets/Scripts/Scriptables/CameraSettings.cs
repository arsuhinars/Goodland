using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Game/Camera Settings")]
public class CameraSettings : ScriptableObject
{
    public Vector3 initialPos;
    public float moveSpeed = 1f;
    public float startGameDelay = 1f;
    public string playerTag;
    public float killThreshold = 0f;
}
