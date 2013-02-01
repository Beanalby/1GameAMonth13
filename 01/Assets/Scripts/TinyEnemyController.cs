using UnityEngine;
using System.Collections;

public class TinyEnemyController : MonoBehaviour {

    public GameObject deathEffect;

    private float deathFlingScale = 7f;
    private float deathDuration = .6f;
    private float turnSpeed = 1;
    private float moveSpeed = 1;

    private bool canControl = true;
    private bool isActive = true;
    private GameObject player;
    private Animator anim;

    private static int attackState = Animator.StringToHash("Base Layer.Attack");
    private static int dieState = Animator.StringToHash("Base Layer.Die");

    void Start() {
        player = GameObject.Find("Player");
        if(player == null) {
            Debug.LogError("Couldn't find player!");
        }
        anim = GetComponentInChildren<Animator>();
    }
    void Update() {
        int animState = anim.GetNextAnimatorStateInfo(0).nameHash;
        if(animState == attackState)
            anim.SetBool("Attack", false);
        if(animState == dieState)
            anim.SetBool("Die", false);
    }
    void FixedUpdate() {
        if (!canControl || player == null)
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
        // turn towards the player, so our attack animation goes towards them
        // even if they rammed us in the back
        Vector3 lookPos = collision.contacts[0].point - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);
        anim.SetBool("Attack", true);
    }
    void GotAttacked(GameObject attacker) {
        StartCoroutine(GotAttackedEffect(attacker));
    }
    IEnumerator GotAttackedEffect(GameObject attacker) {
        canControl = false;
        anim.SetBool("Die", true);
        // don't worry about collisions anymore
        GetComponent<Collider>().isTrigger = true;
        // turn towards the player
        Vector3 lookPos = attacker.transform.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);
        // throw player backwards from the player
        Vector3 velocity = transform.position - attacker.transform.position;
        velocity.y = 0;
        velocity.Normalize();
        velocity.y = 1.5f;
        velocity.Normalize();
        velocity *= deathFlingScale;
        rigidbody.velocity = velocity;
        yield return new WaitForSeconds(deathDuration);
        KillSelf();
    }
    public void KillSelf() {
        if(deathEffect) {
            GameObject obj = Instantiate(deathEffect) as GameObject;
            Vector3 pos = transform.position;
            pos.y += 1; // compensate for model moving up
            obj.transform.position = pos;
        }
        Destroy(gameObject);
    }
}
