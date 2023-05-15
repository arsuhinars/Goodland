using UnityEngine;

[CreateAssetMenu(fileName = "LevelSectionSettings", menuName = "Game/Level Section Settings")]
public class LevelSectionSettings : ScriptableObject
{
    public string cameraTagName;
    public Vector2 size;
}
