using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEditor.EventSystems
{
    [CustomPropertyDrawer(typeof(SelectionRule), true)]
    public class SelectionRuleDrawer : PropertyDrawer
    {
        const string kType = "m_Type";
        const string kTarget = "m_Target";
        const float kTypeWidth = 90f;
        const float kSpacing = 4f;

        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            rect.height = EditorGUIUtility.singleLineHeight;

            Rect typeRect = rect;
            typeRect.width = kTypeWidth;

            Rect targetRect = rect;
            targetRect.xMin += kTypeWidth + kSpacing;

            EditorGUI.BeginProperty(rect, label, prop);
            EditorGUI.PropertyField(typeRect, prop.FindPropertyRelative(kType), GUIContent.none);
            EditorGUI.PropertyField(targetRect, prop.FindPropertyRelative(kTarget), GUIContent.none);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
