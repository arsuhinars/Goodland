using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Game/Level Settings")]
public class LevelSettings : ScriptableObject
{
    public Vector3 initialSectionsPos;
    [Space]
    public LevelSection[] sectionsPrefabs;
    public int initialSectionIndex;
}
