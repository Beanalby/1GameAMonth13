using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {
    public string SpecialExit;

    public void OnTriggerEnter(Collider collider) {
        PlayerDriver player = collider.GetComponent<PlayerDriver>();
        if (player != null) {
            GameObject.Find("LevelDriver").SendMessage("ExitActivated", SpecialExit);
            transform.Find("particleFinished").GetComponent<ParticleSystem>().Play();
        }
    }
}
