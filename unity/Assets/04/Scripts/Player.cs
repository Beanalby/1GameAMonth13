using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    //Vector3 velocity;

    private float distanceToGround = .5f;
    private float jumpSpeed = 10;

    bool isGrounded {
        get {
            return Physics.Raycast(new Ray(transform.position, Vector3.down), distanceToGround+.1f);
        }
    }
    void Update() {
        Vector3 velocity = rigidbody.velocity;
        velocity.x = Input.GetAxis("Horizontal");
        if(Input.GetButtonUp("Jump")) {
            velocity.y = jumpSpeed;
        }
        rigidbody.velocity = velocity;
        //if(!isGrounded) {
        //    velocity += Physics.gravity * Time.deltaTime;
        //}

        //rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
    }
    void OnCollisionEnter(Collision collision) {
        Debug.Log("Collided at " + collision.contacts[0].point);
        //velocity.y = 0;
    }
}
