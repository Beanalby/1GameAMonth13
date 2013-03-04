using UnityEngine;
using System.Collections;

public class ExampleUI : MonoBehaviour {

    public GUISkin skin;

    void OnGUI() {
        GUI.skin = skin;
        GUILayout.BeginArea(new Rect(0, 0, Screen.width / 4, Screen.height));
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Diffuse\nMaterial");
        GUILayout.FlexibleSpace();
        GUILayout.Label("Bumped\nMaterial");
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width * (3f/4f), 0, Screen.width / 4, Screen.height));
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Specular\nMaterial");
        GUILayout.FlexibleSpace();
        GUILayout.Label("Bumped Specular\nMaterial");
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
