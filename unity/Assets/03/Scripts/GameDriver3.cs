using UnityEngine;
using System.Collections;

public class GameDriver3 : MonoBehaviour {

    private int score = 0;

    public void OnGUI() {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Score: " + score);
    }
    public void ShotSuccess(Rim rim) {
        score += rim.value;
    }
}
