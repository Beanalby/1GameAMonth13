using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

    [HideInInspector]
    public GameObject activeCamera;

    public void OnTriggerEnter(Collider other) {
        other.SendMessage("SetSpawnPoint", this, 
            SendMessageOptions.DontRequireReceiver);
    }
}
