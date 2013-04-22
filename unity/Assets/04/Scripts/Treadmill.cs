using UnityEngine;
using System.Collections;

public class Treadmill : MonoBehaviour {
    public float speed = 2f;

    public void LandedOn(Player player) {
        Vector3 v = player.rigidbody.velocity;
        v.x = speed;
        player.rigidbody.velocity = v;
    }
}
