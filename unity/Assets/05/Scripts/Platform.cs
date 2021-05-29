using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
    // fake up friction since it's a trigger
    public void LandedOn(Player player) {
        Vector3 v = player.GetComponent<Rigidbody>().velocity;
        v.x *= .5f;
        player.GetComponent<Rigidbody>().velocity = v;
    }
}
