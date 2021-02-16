namespace Grim.ObjectActionSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(ElementActionController))]
    public class ElementActionControllerEditor : ActionControllerEditor
    {
        ElementActionController controller;

        public override void OnInspectorGUI()
        {
            controller = (ElementActionController)target;
            DrawInspector();
            DrawInspectorFooter();
        }

        public override void DrawReaction_Header(SerializedProperty reactionToDraw)
        {
            var rect = EditorGUILayout.BeginHorizontal();

            //reactionToDraw.FindPropertyRelative("reactionName").stringValue = EditorGUILayout.TextField(reactionToDraw.FindPropertyRelative("reactionName").stringValue);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(reactionToDraw.FindPropertyRelative("element"));

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}