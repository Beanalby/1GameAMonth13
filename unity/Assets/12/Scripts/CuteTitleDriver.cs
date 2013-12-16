using UnityEngine;
using System.Collections;

public class CuteTitleDriver : MonoBehaviour {
    public Texture2D titleImage;
    public string nextStage;

    public void Update() {
        if(Input.GetButtonDown("Fire1")) {
            Application.LoadLevel(nextStage);
        }
    }
    public void OnGUI() {
        DrawTitle();
    }

    private void DrawTitle() {
        Rect titleRect = new Rect(Screen.width/2 - (titleImage.width/2),
            25, titleImage.width, titleImage.height);
        Rect buttonRect = new Rect(Screen.width / 2 - 200, Screen.height - 125,
            400, 50);

        GUI.DrawTexture(titleRect, titleImage);
        if(GUI.Button(buttonRect, "Press Space to begin")) {
            Application.LoadLevel(nextStage);
        }
    }
}
