using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class Player : MonoBehaviour {

    private GameObject lastSpawnPoint = null;

    private bool isDead = false;

    private float colWidth;
    private float distanceToGround = .5f;
    private float jumpSpeed = 7f;
    private float treadmillSpeed = 2f;

    bool isGrounded {
        get {
            return Physics.Raycast(new Ray(transform.position, Vector3.down),
                distanceToGround + .1f);
        }
    }
    private bool canControl {
        get { return !isDead; }
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

        if(canControl && Input.GetButtonUp("Jump") && isGrounded) {
            velocity.y = jumpSpeed;
        }
        rigidbody.velocity = velocity;
    }

    IEnumerator Respawn() {
        if(lastSpawnPoint == null) {
            Debug.LogError("Dead with no spawn point!");
        }
        yield return new WaitForSeconds(3f);
        transform.position = lastSpawnPoint.transform.position;
        rigidbody.velocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        isDead = false;
    }
    public void Die() {
        Debug.Log("Blarg I am dead!");
        isDead = true;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        StartCoroutine(Respawn());
    }
    void OnTriggerEnter(Collider other) {
        if(!isDead && other.tag == "DeathBox") {
            Die();
        } else if(other.tag == "SpawnPoint") {
            lastSpawnPoint = other.gameObject;
        }
    }
}
