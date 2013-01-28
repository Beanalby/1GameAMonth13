using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (CapsuleCollider))]
public class PlayerController : MonoBehaviour {

    private float chargeTime = 1f;
    private float fallRate = .3f;
    private float hurtFlingScale = .1f;
    private float walkSpeed = 5f;
    public GameObject progressCircleTemplate;

    private BoxCollider attackCollider;
    private BoardController board;
    private CapsuleCollider cc;

    private bool canControl = true;
    private float chargeStart = -1;
    private SquareController chargeTarget;
    private float fireCooldown = .5f;
    private List<GameObject> inAttackEffect;
    private float lastFire = -1;
    private ProgressCircle pc = null;
    private Vector3 velocity = Vector3.zero;

	void Start() {
        cc = GetComponent<CapsuleCollider>();
        board = GameObject.Find("Board").GetComponent<BoardController>();
        inAttackEffect = new List<GameObject>();
        attackCollider = transform.Find("AttackArea").GetComponent<BoxCollider>();
	}
	
	void Update() {
        if(canControl) {
            HandleCharging();
            HandleFiring();
        }
    }

    void FixedUpdate() {
        UpdateMovement();
    }

    void OnCollisionEnter(Collision collision) {
        if((collision.gameObject.layer & LayerMask.NameToLayer("Attackable")) != 0) {
            GotHit(collision.contacts[0].point);
            collision.gameObject.SendMessage("DidHit", collision, SendMessageOptions.DontRequireReceiver);
        }
    }
    void OnTriggerEnter(Collider other) {
        if((other.gameObject.layer & LayerMask.NameToLayer("Attackable")) != 0) {
            inAttackEffect.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other) {
        if((other.gameObject.layer & LayerMask.NameToLayer("Attackable")) != 0) {
            inAttackEffect.Remove(other.gameObject);
        }
    }

    void OnDrawGizmos() {
        if(velocity != Vector3.zero) {
            Gizmos.DrawRay(transform.position, velocity * 100);
        }
        Gizmos.color = Color.red;
        if(lastFire + fireCooldown > Time.time) {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(attackCollider.center, attackCollider.size);
        }
    }

    void CancelCharge() {
        if(chargeStart != -1) {
            Destroy(pc.gameObject);
            chargeStart = -1;
            GetComponentInChildren<Animator>().SetBool("Charging", false);
        }
    }

    void HandleCharging()
    {
        if(chargeStart == -1 && Input.GetButtonDown("Jump"))  {
            StartCharge();
        } else if(chargeStart != -1)  {
            // cancel if they release the button, but only if they haven't
            // gone to far (we start the "pound" animation a little early)
            float chargePercent = (Time.time - chargeStart) / chargeTime;
            if(Input.GetButtonUp("Jump") && chargePercent < .78f)  {
                Debug.Log("Cancelling @ " + chargePercent);
                CancelCharge();
            } else if(Time.time > chargeStart + chargeTime) {
                FinishCharge();
            } else {
                UpdateCharge();
            }
        }
    }

    void HandleFiring()
    {
        if(Input.GetButtonDown("Fire1") && lastFire + fireCooldown < Time.time) {
            Fire();
        }

    }

    void FinishCharge() {
        Destroy(pc.gameObject);
        chargeStart = -1;
        board.SquareHit(chargeTarget);
        GetComponentInChildren<Animator>().SetBool("Pound", true);
        GetComponentInChildren<Animator>().SetBool("Charging", false);
        return;
    }

    void Fire()
    {
        lastFire = Time.time;
        foreach(GameObject obj in inAttackEffect) {
            obj.SendMessage("GotAttacked", gameObject, SendMessageOptions.RequireReceiver);
        }
        inAttackEffect.Clear();
    }

    void GotHit(Vector3 contactPoint) {
        CancelCharge();
        // throw player backwards from the contact point
        velocity = rigidbody.position - contactPoint;
        velocity.y = 0;
        velocity.Normalize();
        velocity.y = 1;
        velocity *= hurtFlingScale;
        canControl = false;
    }

    void StartCharge() {
        // started charging, grab the target square
        RaycastHit hit;
        Vector3 pos = transform.position;
        pos.y += .1f;
        if(Physics.Raycast(pos, -Vector3.up, out hit, cc.height * 1.1f))  {
            chargeTarget = hit.collider.gameObject.GetComponent<SquareController>();
            if (chargeTarget != null)  {
                pc = ((GameObject)GameObject.Instantiate(progressCircleTemplate)).GetComponent<ProgressCircle>();
                pos = chargeTarget.transform.position;
                pos.y += .1f;
                pc.transform.position = pos;
                pc.transform.rotation = Quaternion.Euler(new Vector3(90, 180, 0));
                chargeStart = Time.time;
                GetComponentInChildren<Animator>().SetBool("Charging", true);
                GetComponentInChildren<Animator>().SetBool("Pound", false);
            }
        }
    }

    void UpdateCharge() {
        float percent = Mathf.InverseLerp(chargeStart, chargeStart + chargeTime, Time.time);
        pc.percent = percent;
    }

    void UpdateMovement() {
        // don't allow any movement if they're charging
        if(chargeStart != -1)
            return;
        float h = Input.GetAxis("Horizontal"), v = Input.GetAxis("Vertical");
        Vector3 pos = transform.position + velocity;
        if(pos.y <= 0 && velocity != Vector3.zero) {
            velocity = Vector3.zero;
            pos.y = 0;
            canControl = true;
        }

        if(canControl) {
            pos.x += Time.deltaTime * walkSpeed * h;
            pos.z += Time.deltaTime * walkSpeed * v;
        }

        // Don't go outside the board game
        Rect bounds = board.GetBounds();
        pos.x = Mathf.Max(bounds.x+cc.radius, (Mathf.Min(bounds.width-cc.radius, pos.x)));
        pos.z = Mathf.Max(bounds.y+cc.radius, (Mathf.Min(bounds.height-cc.radius, pos.z)));

        // look towards the direction the user's pressing
        Vector3 lookTarget = new Vector3(h, 0, v);
        if(lookTarget != Vector3.zero)  {
            transform.rotation = Quaternion.LookRotation(lookTarget);
        }
        rigidbody.MovePosition(pos);
        if(velocity != Vector3.zero) {
            velocity.y -= fallRate * Time.deltaTime;
        }
	}

}
