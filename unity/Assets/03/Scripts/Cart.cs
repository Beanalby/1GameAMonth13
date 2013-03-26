using UnityEngine;
using System.Collections;

public class Cart : MonoBehaviour {

    private float speed = 1f;
    private int min = -2;
    private int max = 2;
    float direction = 1f;
    // Use this for initialization
    void Start () {

        rigidbody.velocity = new Vector3(0, 0, speed*direction);
    }
    
    // Update is called once per frame
    void Update () {
        if(transform.position.z >= max || transform.position.z <= min) {
            direction *= -1;
        }
        rigidbody.velocity = new Vector3(0, 0, speed * direction);
    }
}
