using UnityEngine;

[CreateAssetMenu(fileName = "MusicSettings", menuName = "Game/Music Settings")]
public class MusicSettings : ScriptableObject
{
    public AudioSource audioSourcePrefab;
    public AudioClip[] songs;
    public float musicTransitionDuration = 0f;
    public float musicDelay = 0f;
    [Range(0f, 1f)] public float mutedMusicVolume;
}
