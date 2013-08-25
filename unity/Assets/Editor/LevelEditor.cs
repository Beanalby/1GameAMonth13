using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        Level level = (Level)target;
        string newName = level.scene;
        if(level.name != newName) {
            level.name = newName;
        }
    }
}
