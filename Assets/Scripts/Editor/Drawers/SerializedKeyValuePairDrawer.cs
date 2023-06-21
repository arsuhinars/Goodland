using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializedKeyValuePair<,>))]
public class SerializedKeyValuePairDrawer : PropertyDrawer
{
    private const int PROPERTY_LABEL_WIDTH = 40;
    private const int HALF_SPACING = 4;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var keyProperty = property.FindPropertyRelative("key");
        var valueProperty = property.FindPropertyRelative("value");

        EditorGUI.BeginProperty(position, label, property);

        var keyLabelRect = new Rect(
            position.x, position.y, PROPERTY_LABEL_WIDTH, position.height
        );
        var keyPropRect = new Rect(
            position.x + PROPERTY_LABEL_WIDTH,
            position.y,
            position.width / 2f - HALF_SPACING - PROPERTY_LABEL_WIDTH,
            position.height
        );
        var valueLabelRect = new Rect(
            position.x + position.width / 2f + HALF_SPACING,
            position.y,
            PROPERTY_LABEL_WIDTH,
            position.height
        );
        var valuePropRect = new Rect(
            valueLabelRect.x + valueLabelRect.width,
            position.y,
            keyPropRect.width,
            position.height
        );

        EditorGUI.LabelField(keyLabelRect, keyProperty.displayName);
        EditorGUI.PropertyField(keyPropRect, keyProperty, GUIContent.none);
        EditorGUI.LabelField(valueLabelRect, valueProperty.displayName);
        EditorGUI.PropertyField(valuePropRect, valueProperty, GUIContent.none);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
