using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Checkpoint))]
[CanEditMultipleObjects]
public class CheckpointEditor : Editor
{
    SerializedProperty useTransformPosition;
    SerializedProperty respawnPosition;

    private void OnEnable()
    {
        useTransformPosition = serializedObject.FindProperty("useTransformPosition");
        respawnPosition = serializedObject.FindProperty("respawnPosition");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(useTransformPosition);

        if (!useTransformPosition.boolValue)
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(respawnPosition);
        }

        serializedObject.ApplyModifiedProperties();
    }

}
