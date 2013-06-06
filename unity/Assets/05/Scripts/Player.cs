using UnityEngine;
using System.Collections;

public delegate void ResetHandler();

[RequireComponent(typeof(BoxCollider))]
public class Player : MonoBehaviour {

    // when we want to place the player "on" something, need to put
    // him just above it.  Otherwise, his box will already intersect
    // the ground, and he'll just fall through.
    public static float NUDGE_UP = .01f;

    public event ResetHandler resetListeners;
    public bool IsDead {
        get { return isDead; }
    }

    public SpawnPoint spawnPoint = null;
    public AudioClip jumpSound;
    public AudioClip deathSound;

    private CameraManager cam;
    private Transform capsule; // used for what should be an animation in this GameObject
    private float colWidth;
    private float distanceToGround = 0f;
    private bool isDead = false;
    private bool doJump = false;
    private float jumpSpeed = 7f;
    private float respawnDelay = 2f;

    bool isGrounded {
        get {
            return GetGround() != null;
        }
    }
    private bool canControl {
        get { return !isDead; }
    }

    void Start() {
        cam = CameraManager.instance;
        colWidth = GetComponent<BoxCollider>().bounds.size.x;
        transform.position = spawnPoint.transform.position;
        if (spawnPoint.activeCamera != null) {
            cam.Current = spawnPoint.activeCamera;
        }
        capsule = transform.FindChild("Capsule");
        //rigidbody.velocity = new Vector3(3, 0, 0);
    }
    void Update() {
        if(canControl && Input.GetButtonDown("Jump")) {
            if (isGrounded) {
                doJump = true;
            } else {
                Debug.Log("Trying to jump, not grounded! pos=" + transform.position.ToString(".000"));
            }
        }
    }
    private GameObject GetGround() {
        Vector3 pos = transform.position;
        float dist = distanceToGround + .2f;
        foreach(float offset in new float[] { colWidth, -colWidth }) {
            pos = transform.position;
            pos.x += offset;
            // move our source up JUUUST a hare, in case we've temporarily
            // sunk down into the platform
            pos.y += .1f;
            Ray ray = new Ray(pos, Vector3.down);
            foreach(RaycastHit hit in Physics.RaycastAll(ray, dist)) {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    void FixedUpdate() {
        // check if either our left or right edge is over a treadmill
        if(rigidbody.velocity.y <= 0.1) {
            GameObject ground = GetGround();
            if(ground != null) {
                ground.SendMessage("LandedOn", this,
                    SendMessageOptions.DontRequireReceiver);
            }
        }
        if(doJump) {
            Vector3 velocity = rigidbody.velocity;
            velocity.y = jumpSpeed;
            rigidbody.velocity = velocity;
            doJump = false;
            AudioSource.PlayClipAtPoint(jumpSound,
                cam.Current.transform.position);
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            Respawn();
        }
    }
    void Respawn() {
        if(spawnPoint == null) {
            Debug.LogError("Dead with no spawn point!");
        }
        Vector3 pos = spawnPoint.transform.position;
        pos.y += NUDGE_UP;
        transform.position = pos;
        rigidbody.velocity = Vector3.zero;
        capsule.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
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
        //Debug.Log("Blarg I am dead!");
        isDead = true;
        //Debug.Log("NOT rotating.");
        capsule.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        AudioSource.PlayClipAtPoint(deathSound, cam.Current.transform.position);
        yield return new WaitForSeconds(respawnDelay);
        Respawn();
    }
    void SetSpawnPoint(SpawnPoint spawnPoint) {
        if(isDead) {
            return;
        }
        this.spawnPoint = spawnPoint;
        spawnPoint.activeCamera = CameraManager.instance.Current;
    }
}
