using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaveManager))]
public class WaveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.HelpBox("if you want to edit wave use wave editor.", MessageType.Info);
        if (GUILayout.Button("Wave Editor"))
        {
            WaveWindow.ShowWindow();
        }
    }
}
