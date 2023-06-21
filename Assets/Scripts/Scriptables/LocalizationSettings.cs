using UnityEngine;

[CreateAssetMenu(fileName = "LocalizationSettings", menuName = "Localization/Localization Settings")]
public class LocalizationSettings : ScriptableObject
{
    public SystemLanguage fallbackLocale;
    public string localesResourcesPath;
}
