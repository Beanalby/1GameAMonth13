using UnityEngine;
using System.Collections;

public class FruitIntro : MonoBehaviour {

    public GUISkin skin;

    GUIStyle mainText;

    public void Start() {
        mainText = new GUIStyle(skin.label);
        mainText.alignment = TextAnchor.UpperCenter;
    }
    public void Update() {
        if(Input.GetButtonDown("Fire1")) {
            Application.LoadLevel("game");
        }
    }
    public void OnGUI() {
        GUI.skin = skin;
        GUI.Label(new Rect(0, 200, Screen.width, 75),
            "Fruit Frenzy!", skin.customStyles[0]);
        GUI.Label(new Rect(0, 275, Screen.width, 75),
            "Move your robot to catch fruit and drop them in the correct bins.\n\nDon't let fruit hit the ground, and don't drop it in the wrong bin!",
            mainText);
        if(GUI.Button(new Rect(Screen.width / 2 - 100, 350, 200, 75),
            "Begin")) {
                Application.LoadLevel("game");
        }
    }
}
