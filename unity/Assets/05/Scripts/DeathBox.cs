using UnityEngine;
using System.Collections;

public class DeathBox : MonoBehaviour {

    public void OnTriggerEnter(Collider other) {
        other.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
    }
}
