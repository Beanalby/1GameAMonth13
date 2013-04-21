using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class Player : MonoBehaviour {

    private float colWidth;
    private float distanceToGround = .5f;
    private float jumpSpeed = 7f;
    private float treadmillSpeed = 2f;

    bool isGrounded {
        get {
            return Physics.Raycast(new Ray(transform.position, Vector3.down), distanceToGround + .1f);
        }
    }

    void Start() {
        colWidth = GetComponent<BoxCollider>().bounds.size.x;
    }
    void Update() {
        Vector3 velocity = rigidbody.velocity;
        // check if either our left or right edge is over a treadmill
        if(rigidbody.velocity.y <= 0.1) {
            Vector3 pos = transform.position;
            float dist = distanceToGround + .1f;
            bool isOnTreadmill = false;
            foreach(float offset in new float[] { colWidth, -colWidth }) {
                pos = transform.position;
                pos.x += offset;
                Ray ray = new Ray(pos, Vector3.down);
                foreach(RaycastHit hit in Physics.RaycastAll(ray, dist)) {
                    if(hit.collider.tag == "Treadmill") {
                        isOnTreadmill = true;
                    }
                }
                if(isOnTreadmill) {
                    break;
                }
            }
            if(isOnTreadmill) {
                velocity.x = treadmillSpeed;
            }
        }

        if(Input.GetButtonUp("Jump")) {
            velocity.y = jumpSpeed;
        }
        rigidbody.velocity = velocity;
    }
}
