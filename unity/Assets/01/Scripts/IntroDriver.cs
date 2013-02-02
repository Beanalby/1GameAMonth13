using UnityEngine;
using System.Collections;

public class IntroDriver : MonoBehaviour {
    public GUISkin skin;
    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Application.LoadLevel("tutorial");
        }
    }
    void OnGUI() {
        if(skin)
            GUI.skin = skin;
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height / 4), "Rift Gambit\nv0.1");
        GUI.Label(new Rect(0, Screen.height * (3f / 4f), Screen.width, Screen.height / 4), "Press Space to begin");
    }
}
