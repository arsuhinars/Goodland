using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "English", menuName = "Localization/Localization Table")]
public class LocalizationTable : ScriptableObject
{
    public List<SerializedKeyValuePair<string, string>> pairs;
}
