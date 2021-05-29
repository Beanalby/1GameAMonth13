using UnityEngine;
using System.Collections;

public class Treadmill : MonoBehaviour {
    public float speed = 2f;

    private Material mat;
    void Start() {
        mat = transform.parent.GetComponentInChildren<Renderer>().materials[1];
    }
    void Update() {
        mat.SetTextureOffset("_MainTex", new Vector2(speed * Time.time, 0));
    }

    public void LandedOn(Player player) {
        Vector3 v = player.GetComponent<Rigidbody>().velocity;
        v.x = speed;
        player.GetComponent<Rigidbody>().velocity = v;
    }
}
