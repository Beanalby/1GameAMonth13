using UnityEngine;
using System.Collections;

public delegate void ResetHandler();

[RequireComponent(typeof(BoxCollider))]
public class Player : MonoBehaviour {

    public event ResetHandler resetListeners;
    public bool IsDead {
        get { return isDead; }
    }

    public SpawnPoint spawnPoint = null;

    private float colWidth;
    private float distanceToGround = 0f;
    private bool isDead = false;
    private bool doJump = false;
    private float jumpSpeed = 7f;
    private float respawnDelay = 2f;

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
        transform.position = spawnPoint.transform.position;
        if (spawnPoint.activeCamera != null) {
            CameraManager.instance.Current = spawnPoint.activeCamera;
        }
        rigidbody.velocity = new Vector3(3, 0, 0);
    }
    void Update() {
        if(canControl && Input.GetButtonUp("Jump") && isGrounded) {
            doJump = true;
        }
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

        if(doJump) {
            Vector3 velocity = rigidbody.velocity;
            velocity.y = jumpSpeed;
            rigidbody.velocity = velocity;
            doJump = false;
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            Respawn();
        }
    }
    void Respawn() {
        if(spawnPoint == null) {
            Debug.LogError("Dead with no spawn point!");
        }
        transform.position = spawnPoint.transform.position;
        rigidbody.velocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        isDead = false;
        if(resetListeners != null) {
            resetListeners();
        }
        CameraManager.instance.Current = spawnPoint.activeCamera;
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
        yield return new WaitForSeconds(respawnDelay);
        Respawn();
    }
    public void OnCollisionEnter(Collision col) {
        // collisions on the ground have a tendency to "nudge" the player
        // to the side.  If the direction of this collision was mostly up
        // (for landing straight down) or mostly down (for going up through
        // a platform), set our horizontal velocity to theirs (if any)
        float angle = Vector3.Angle(Vector3.up, col.relativeVelocity);
        if(angle < 20 || angle > 170) {
            Vector3 v = rigidbody.velocity;
            if(col.rigidbody) {
                v.x = col.rigidbody.velocity.x;
            } else {
                v.x = 0;
            }
            rigidbody.velocity = v;
        }
    }
    void SetSpawnPoint(SpawnPoint spawnPoint) {
        if(isDead) {
            return;
        }
        this.spawnPoint = spawnPoint;
        spawnPoint.activeCamera = CameraManager.instance.Current;
    }
}
