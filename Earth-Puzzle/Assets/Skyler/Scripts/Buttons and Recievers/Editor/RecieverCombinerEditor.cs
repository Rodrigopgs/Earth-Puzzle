using UnityEditor;

[CustomEditor(typeof(RecieverCombiner))]
[CanEditMultipleObjects]
public class RecieverCombinerEditor : Editor
{
    SerializedProperty triggerEvents;
    SerializedProperty stopEvents;
    SerializedProperty activationsRequired;

    SerializedProperty type;

    SerializedProperty inactive;
    SerializedProperty active;

    SerializedProperty idle;
    SerializedProperty activated;

    private void OnEnable()
    {
        triggerEvents = serializedObject.FindProperty("triggerEvents");
        stopEvents = serializedObject.FindProperty("stopEvents");
        activationsRequired = serializedObject.FindProperty("activationsRequired");

        type = serializedObject.FindProperty("type");

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
        EditorGUILayout.PropertyField(activationsRequired);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(type);
        EditorGUILayout.Space();

        if (type.enumValueFlag == (int)RecieverCombiner.RecieverCombinerType.Sprite)
        {
            EditorGUILayout.PropertyField(inactive);
            EditorGUILayout.PropertyField(active);
        }
        else if (type.enumValueFlag == (int)RecieverCombiner.RecieverCombinerType.Animation)
        {
            EditorGUILayout.PropertyField(idle);
            EditorGUILayout.PropertyField(activated);
        }

        serializedObject.ApplyModifiedProperties();
    }

}
