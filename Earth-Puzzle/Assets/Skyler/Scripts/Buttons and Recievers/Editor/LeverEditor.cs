using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Lever))]
[CanEditMultipleObjects]
public class LeverEditor : Editor
{
    SerializedProperty triggerEvents;
    SerializedProperty stopEvents;
    SerializedProperty type;

    SerializedProperty inactive;
    SerializedProperty active;

    SerializedProperty idle;
    SerializedProperty activated;

    private void OnEnable()
    {
        type = serializedObject.FindProperty("type");
        triggerEvents = serializedObject.FindProperty("triggerEvents");
        stopEvents = serializedObject.FindProperty("stopEvents");


        inactive = serializedObject.FindProperty("inactive");
        active = serializedObject.FindProperty("active");

        idle = serializedObject.FindProperty("idle");
        activated = serializedObject.FindProperty("activated");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(triggerEvents);
        EditorGUILayout.PropertyField(stopEvents);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(type);
        EditorGUILayout.Space();

        if (type.enumValueFlag == (int)Lever.LeverType.Sprite)
        {
            EditorGUILayout.PropertyField(inactive);
            EditorGUILayout.PropertyField(active);
        }
        else if (type.enumValueFlag == (int)Lever.LeverType.Animation)
        {
            EditorGUILayout.PropertyField(idle);
            EditorGUILayout.PropertyField(activated);
        }

        serializedObject.ApplyModifiedProperties();
    }

}
