using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Stage {
    public string name;
    public string scene;
    public string description;
    public int time;
    public Texture preview;
    public Stage(string name, string scene, string description,
            int time, Texture preview) {
        this.name = name;
        this.scene = scene;
        this.description = description;
        this.time = time;
        this.preview = preview;
    }
}
public class StageSelect : MonoBehaviour {

    public GUISkin skin;
    public List<Stage> stages;

    private Stage selected = null;

    public void Start() {
    }
    public void OnGUI() {
        int current = 0;
        int height = 50;
        GUI.skin = skin;
        Rect playNow = new Rect(Screen.width - 150, Screen.height - height,
            150, height);
        Rect titlePos = new Rect(Screen.width / 2, 10,
            Screen.width / 2, height);
        foreach(Stage stage in stages) {
            Rect rect = new Rect(50, current * height + 50, 300, height);
            if(GUI.Button(rect, new GUIContent(stage.name, stage.preview))) {
                selected = stage;
            }
            current++;
        }
        if(selected == null) {
            GUI.Label(titlePos, "Choose a Stage");
            GUI.enabled = false;
            GUI.Button(playNow, "Play Now!");
            GUI.enabled = true;
        } else {
            GUI.Label(titlePos, selected.name);
            current = (int)(titlePos.y + titlePos.height);
            Texture img = selected.preview;
            Rect previewPos = new Rect(Screen.width * .75f - img.width / 2f,
                current, img.width, img.height);
            GUI.DrawTexture(previewPos, img);
            current += img.height;
            GUI.Label(new Rect(Screen.width / 2, current,
                Screen.width / 2, 400), selected.description, skin.customStyles[0]);
            GUI.Button(playNow, "Play Now!");
        }

    }
}
