using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

    public void OnTriggerEnter(Collider other) {
        other.SendMessage("SetSpawnPoint", this.gameObject,
            SendMessageOptions.DontRequireReceiver);
    }
}
