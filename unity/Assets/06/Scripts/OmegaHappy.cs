using UnityEngine;
using System.Collections;

public class OmegaHappy : OmegaDriver {

    public GUISkin skin;

    // show a message so they know there's nothing else
    public void OnGUI() {
        GUI.skin = skin;
        if(showStart != -1) {
            float diff = Time.time - showStart;
            if(diff > .25f && hideStart < .5f) {
                GUILayout.BeginArea(new Rect(0, 0, Screen.width, 100));
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Omega Complete!  Enjoy the rest of the song.");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
        }
    }
}
