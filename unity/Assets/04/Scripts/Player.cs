using UnityEngine;
using System.Collections;

public delegate void ResetHandler();

[RequireComponent(typeof(BoxCollider))]
public class Player : MonoBehaviour {


    public event ResetHandler resetListeners;

    private float colWidth;
    private float distanceToGround = .5f;
    private bool isDead = false;
    private float jumpSpeed = 7f;
    private GameObject lastSpawnPoint = null;

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
    void FixedUpdate() {
        // check if either our left or right edge is over a treadmill
        if(rigidbody.velocity.y <= 0.1) {
            Vector3 pos = transform.position;
            float dist = distanceToGround + .1f;
            bool gotHit = false;
            foreach(float offset in new float[] { colWidth, -colWidth }) {
                pos = transform.position;
                pos.x += offset;
                Ray ray = new Ray(pos, Vector3.down);
                foreach(RaycastHit hit in Physics.RaycastAll(ray, dist)) {
                    hit.collider.gameObject.SendMessage("LandedOn", this,
                        SendMessageOptions.DontRequireReceiver);
                    gotHit = true;
                }
                if(gotHit) {
                    break;
                }
            }
        }

        if(canControl && Input.GetButtonUp("Jump") && isGrounded) {
            Vector3 velocity = rigidbody.velocity;
            velocity.y = jumpSpeed;
            rigidbody.velocity = velocity;
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            Respawn();
        }
    }
    void Respawn() {
        if(lastSpawnPoint == null) {
            Debug.LogError("Dead with no spawn point!");
        }
        transform.position = lastSpawnPoint.transform.position;
        rigidbody.velocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        isDead = false;
        if(resetListeners != null) {
            resetListeners();
        }
    }
    public void Die() {
        StartCoroutine(_Die());
    }
    private IEnumerator _Die() {
        if(isDead) {
            yield break;
        }
        Debug.Log("Blarg I am dead!");
        isDead = true;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        yield return new WaitForSeconds(3f);
        Respawn();
    }
    public void OnCollisionEnter(Collision col) {
        // collisions on the ground have a tendency to "nudge" the player
        // to the side.  If the direction of this collision was mostly up,
        // Set our horizontal velocity to theirs (if any) to avoid drift.
        if(Vector3.Angle(Vector3.up, col.relativeVelocity) < 20) {
            Vector3 v = rigidbody.velocity;
            if(col.rigidbody) {
                v.x = col.rigidbody.velocity.x;
            } else {
                v.x = 0;
            }
            rigidbody.velocity = v;
        }
    }
    void SetSpawnPoint(GameObject spawnPoint) {
        this.lastSpawnPoint = spawnPoint;
    }
}
