using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if(GUILayout.Button("Toggle Door")) {
            Door door = (Door)target;
            Transform tmp;
            Vector3 pos;
            tmp = door.transform.Find("doorLeft");
            pos = tmp.localPosition;
            if(pos.x == Door.OPEN_LEFT.x) {
                pos = Vector3.zero;
            } else {
                pos = Door.OPEN_LEFT;
            }
            tmp.localPosition = pos;

            tmp = door.transform.Find("doorRight");
            pos = tmp.localPosition;
            if(pos.x == Door.OPEN_RIGHT.x) {
                pos = Vector3.zero;
            } else {
                pos = Door.OPEN_RIGHT;
            }
            tmp.localPosition = pos;
        }
    }
}
