using UnityEngine;

[CreateAssetMenu(fileName = "ScreenInputSettings", menuName = "Game/Screen Input Settings")]
public class ScreenInputSettings : ScriptableObject
{
    public string pauseButtonElementName;
    public string upButtonElementName;
    public string leftButtonElementName;
    public string rightButtonElementName;
}
