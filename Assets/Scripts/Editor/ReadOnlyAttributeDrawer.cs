﻿using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label) {
        GUI.enabled = false;
        EditorGUI.PropertyField(rect, prop);
        GUI.enabled = true;
    }
}