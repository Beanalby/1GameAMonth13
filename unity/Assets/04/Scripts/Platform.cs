using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
    // fake up friction since it's a trigger
    public void LandedOn(Player player) {
        Vector3 v = player.rigidbody.velocity;
        v.x *= .5f;
        player.rigidbody.velocity = v;
    }
}
