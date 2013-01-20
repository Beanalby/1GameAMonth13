using UnityEngine;
using System.Collections;

public class TinyEnemyController : MonoBehaviour {

    private float turnSpeed = 1;
    private float moveSpeed = 1;

    private bool isActive = true;
    private GameObject player;
    
    void Start() {
        player = GameObject.Find("Player");
        if(player == null) {
            Debug.LogError("Couldn't find player!");
        }
    }
    void FixedUpdate() {
        if (player == null)
            return;
        Vector3 moveTarget = rigidbody.position;
        Vector3 playerDir = player.transform.position - transform.position;
        // don't attempt to move up or down
        playerDir.y = transform.position.y;

        Quaternion rotateTarget = Quaternion.LookRotation(playerDir);
        if(isActive) {
            rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, rotateTarget, Time.deltaTime * turnSpeed));
        }

        moveTarget += (transform.forward * moveSpeed * Time.deltaTime);

        if(isActive)
            rigidbody.MovePosition(moveTarget);
    }

    void DidHit(Collision collision) {
        // cancel out any movement we may have gotten from the collision
        collision.gameObject.rigidbody.velocity = Vector3.zero;
        collision.gameObject.rigidbody.angularVelocity = Vector3.zero;
    }
    void GotAttacked() {
        // BLARG!
        Destroy(gameObject);
    }
}
