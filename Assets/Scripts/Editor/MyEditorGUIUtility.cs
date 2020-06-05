using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public static class MyEditorGUIUtility
{
    public static void DrawHorizontalLine()
    {
        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), Color.gray);
    }
}
