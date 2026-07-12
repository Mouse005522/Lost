using Kun.Tool;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (BlockCollector))]
public class BlockCollectorEditor : SerializedObjectEditor<BlockCollector>
{
    SerializedProperty isRootProp;
    SerializedProperty rootKeyProp;

    protected override void OnEnable ()
    {
        base.OnEnable ();
        isRootProp = serializedObject.FindProperty (nameof (BlockCollector.isRoot));
        rootKeyProp = serializedObject.FindProperty (nameof (BlockCollector.rootKey));
    }

    public override void OnInspectorGUI ()
    {
        serializedObject.Update ();

        DrawPropertiesExcluding (serializedObject, nameof (BlockCollector.isRoot), nameof (BlockCollector.rootKey));

        EditorGUILayout.PropertyField (isRootProp, new GUIContent ("起始點位"));

        if (isRootProp.boolValue)
        {
            EditorGUILayout.PropertyField (rootKeyProp, new GUIContent ("起始key"));
        }

        serializedObject.ApplyModifiedProperties ();
    }
}
