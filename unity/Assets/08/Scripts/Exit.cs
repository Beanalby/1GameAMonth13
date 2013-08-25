using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

    public void OnTriggerEnter(Collider other) {
        GameDriver8.instance.LevelFinished();
    }
}
