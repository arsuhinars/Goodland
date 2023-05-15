using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializedInterfacesArray<>))]
public class SerializedInterfacesArrayDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var componentsProperty = property.FindPropertyRelative("m_components");
        EditorGUI.PropertyField(position, componentsProperty, label);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var componentsProperty = property.FindPropertyRelative("m_components");

        return EditorGUI.GetPropertyHeight(componentsProperty);
    }
}
