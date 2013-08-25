using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

    public void OnTriggerEnter(Collider other) {
        GameObject.Find("LevelDriver").SendMessage("ExitActivated");
    }
}
