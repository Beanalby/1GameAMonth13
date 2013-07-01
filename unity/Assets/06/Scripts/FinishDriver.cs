using UnityEngine;
using System.Collections;

public class FinishDriver : MonoBehaviour {

    public GUISkin skin;

    public void OnGUI() {
        GUI.skin = skin;
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.BeginVertical();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("http://twitter.com/RealSiflAndOlly");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("http://beanalby.net");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayoutOption[] opts = new GUILayoutOption[] {
            GUILayout.Width(200), GUILayout.Height(50)};
        if(GUILayout.Button("Play Again", opts)) {
            Application.LoadLevel("06-title");
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
