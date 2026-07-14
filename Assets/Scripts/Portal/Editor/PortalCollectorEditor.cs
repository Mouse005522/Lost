using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;

[CustomEditor(typeof(PortalCollector))]
public class PortalCollectorEditor : SerializedObjectEditor<PortalCollector>
{
    SerializedProperty scriptProp;
    SerializedProperty directionProp;
    SerializedProperty blockKeyProp;
    SerializedProperty entryKeyProp;

    protected override void OnEnable()
    {
        base.OnEnable();
        BlockEditorUtility.Refresh();
        scriptProp = serializedObject.FindProperty("m_Script");
        directionProp = serializedObject.FindProperty(nameof(PortalCollector.Direction));
        blockKeyProp = serializedObject.FindProperty(nameof(PortalCollector.blockKey));
        entryKeyProp = serializedObject.FindProperty(nameof(PortalCollector.entryKey));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(scriptProp);
        EditorGUI.EndDisabledGroup();

        PortalDirection direction = (PortalDirection)directionProp.intValue;
        directionProp.intValue = (int)EditorTool.DrawEnum(direction, "方向");
        DrawKeySelector("blockKey", blockKeyProp.stringValue, OpenBlockKeyPopup);
        DrawKeySelector("entryKey", entryKeyProp.stringValue, OpenEntryKeyPopup);

        serializedObject.ApplyModifiedProperties();
    }

    void DrawKeySelector(string label, string value, System.Action onClick)
    {
        EditorTool.DrawInHorizontal(() =>
        {
            EditorGUILayout.PrefixLabel(label);

            string buttonLabel = string.IsNullOrEmpty(value) ? "未設定" : value;
            if (GUILayout.Button(buttonLabel))
            {
                onClick?.Invoke();
            }
        });
    }

    void OpenBlockKeyPopup()
    {
        List<string> blockKeys = new List<string>(BlockEditorUtility.BlockKeys);
        if (blockKeys.Count == 0)
        {
            Debug.LogError("找不到可選擇的 blockKey，請先執行 BlockEditorUtility.Refresh");
            return;
        }

        Pop_UpSelectWindow.ShowWindow(value =>
        {
            SetStringProperty(nameof(PortalCollector.blockKey), value);
            SetStringProperty(nameof(PortalCollector.entryKey), string.Empty);
        }, blockKeyProp.stringValue, blockKeys);
    }

    void OpenEntryKeyPopup()
    {
        if (BlockEditorUtility.TryGetEntryKeys(blockKeyProp.stringValue, out List<string> entryKeys))
        {
            if (entryKeys.Count == 0)
            {
                Debug.LogError($"blockKey {blockKeyProp.stringValue} 找不到可選擇的 entryKey");
                return;
            }

            Pop_UpSelectWindow.ShowWindow(value =>
            {
                SetStringProperty(nameof(PortalCollector.entryKey), value);
            }, entryKeyProp.stringValue, entryKeys);
        }
        else
        {
            Debug.LogError($"找不到 blockKey {blockKeyProp.stringValue} 對應的 entryKey 資料");
        }
    }

    void SetStringProperty(string propertyName, string value)
    {
        SerializedObject so = new SerializedObject(runtimeScript);
        SerializedProperty property = so.FindProperty(propertyName);
        if (property != null)
        {
            so.Update();
            property.stringValue = value;
            so.ApplyModifiedProperties();
        }
        else
        {
            Debug.LogError($"找不到欄位: {propertyName}");
        }
    }
}
