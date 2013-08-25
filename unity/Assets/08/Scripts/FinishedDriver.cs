using UnityEngine;
using System.Collections;

public class FinishedDriver : MonoBehaviour {

    public void OnGUI() {
        GUI.Label(new Rect(0, 0, Screen.width, 200), "You winz.");
    }
}
