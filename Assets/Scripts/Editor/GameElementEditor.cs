using UnityEditor;
using UnityEngine;

namespace Genesis.Tetris
{
    [CustomPropertyDrawer(typeof(ModelElement))]
    public class GameModelElementDrawer : PropertyDrawer
    {
        public SerializedProperty Blocks;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.y += 5;
            EditorGUI.BeginProperty(position, label, property);

            label.text = "Figure";
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            position.x += 80;
            EditorGUI.DrawRect(new Rect(position.x, position.y, 100, 1), Color.black);
            position.y += 5;

            // width
            var widthProp = property.FindPropertyRelative("Width");
            EditorGUI.PropertyField(new Rect(position.x, position.y, 30, 20), widthProp, GUIContent.none);

            // height
            var heightProp = property.FindPropertyRelative("Height");
            EditorGUI.PropertyField(new Rect(position.x, position.y + 22, 30, 20), heightProp, GUIContent.none);

            var colorProp = property.FindPropertyRelative("Color");
            EditorGUI.PropertyField(new Rect(position.x + 35, position.y, 50, 40), colorProp, GUIContent.none);


            var arrayProp = property.FindPropertyRelative("Blocks");
            var length = widthProp.intValue * heightProp.intValue;
            for (int i = 0; i < length; i++)
            {
                var x = i % widthProp.intValue;
                var y = i / widthProp.intValue;

                if (i >= arrayProp.arraySize)
                {
                    arrayProp.InsertArrayElementAtIndex(i);
                }
                SerializedProperty value = arrayProp.GetArrayElementAtIndex(i);
                value.boolValue = EditorGUI.Toggle(new Rect((position.x + 90) + (x * 20), (position.y - 2) + (y * 20), 20, 20), value.boolValue);
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var heightProp = property.FindPropertyRelative("Height");

            return (Mathf.Max(0, heightProp.intValue - 2)) * 20 + 50;
        }
    }
}
