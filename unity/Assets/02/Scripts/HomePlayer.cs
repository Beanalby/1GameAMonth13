using UnityEngine;
using System.Collections;

public class HomePlayer : HomeBase {

    private int buttonWidth = 300;
    private int buttonHeight = 50;

    public GameObject templateJack;
    public GameObject templateEjector;

    public void OnGUI() {
        if (GUI.Button(new Rect(0, 0, buttonWidth, buttonHeight), "Spawn Jack")) {
            Spawn(templateJack);
        }
        if (GUI.Button(new Rect(0, buttonHeight + 10, buttonWidth, buttonHeight), "Spawn Ejector")) {
            Spawn(templateEjector);
        }
    }
}
