using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {
    public string SpecialExit;

    public void OnTriggerEnter(Collider other) {
        GameObject.Find("LevelDriver").SendMessage("ExitActivated", SpecialExit);
        transform.Find("particleFinished").GetComponent<ParticleSystem>().Play();
    }
}
