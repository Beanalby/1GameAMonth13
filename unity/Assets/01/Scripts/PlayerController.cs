using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (CapsuleCollider))]
public class PlayerController : MonoBehaviour {

    private static int gotHitState = Animator.StringToHash("Base Layer.GotHit");
    private static int attackState = Animator.StringToHash("Base Layer.Attack");

    private float chargeTime = 1f;
    private float hurtFlingScale = 3f;
    private float walkAcceleration = 20f;
    private float walkSpeed = 5f;
    private float attackDistance;
    public GameObject progressCircleTemplate;
    public bool sceneFrozen = false;
    public SquareController restrictedCharge = null;
    public bool canCharge = true;
    public GameObject driver = null;

    private BoxCollider attackCollider;
    private BoardController board;
    private CapsuleCollider cc;
    private Animator anim;

    private bool attackFrozen = false;
    private Vector3 attackMoveTarget = Vector3.zero;
    private bool hitFrozen = false;
    private float chargeStart = -1;
    private SquareController chargeTarget;
    private float attackCooldown = .5f;
    private List<GameObject> inAttackEffect;
    private float lastAttack = -1;
    private ProgressCircle pc = null;
    private Vector3 velocity = Vector3.zero;

    public bool canControl {
        get { return !hitFrozen && chargeStart == -1 && !attackFrozen && !sceneFrozen; }
    }
    public bool canMove {
        get { return chargeStart == -1; }
    }

	void Start() {
        cc = GetComponent<CapsuleCollider>();
        board = GameObject.Find("Board").GetComponent<BoardController>();
        inAttackEffect = new List<GameObject>();
        attackCollider = transform.Find("AttackArea").GetComponent<BoxCollider>();
        attackDistance = attackCollider.size.z * .75f;
        anim = transform.GetComponentInChildren<Animator>();
	}
	
	void Update() {
        HandleCharging();
        HandleAttacking();
        ResetAnimatorFlags();
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
            Gizmos.DrawRay(transform.position, velocity);
        }
        Gizmos.color = Color.red;
        if(lastAttack + attackCooldown > Time.time) {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(attackCollider.center, attackCollider.size);
        }
    }

    void CancelCharge() {
        if(chargeStart != -1) {
            Destroy(pc.gameObject);
            chargeStart = -1;
            anim.SetBool("Charging", false);
        }
    }

    void HandleCharging()
    {
        if(canControl && canCharge && chargeStart == -1 && Input.GetButtonDown("Jump"))  {
            StartCharge();
        } else if(chargeStart != -1)  {
            // cancel if they release the button, but only if they haven't
            // gone to far (we start the "pound" animation a little early)
            float chargePercent = (Time.time - chargeStart) / chargeTime;
            if(Input.GetButtonUp("Jump") && chargePercent < .78f)  {
                CancelCharge();
            } else if(Time.time > chargeStart + chargeTime) {
                FinishCharge();
            } else {
                UpdateCharge();
            }
        }
    }

    void HandleAttacking()
    {
        if(canControl && Input.GetButtonDown("Fire1") && lastAttack + attackCooldown < Time.time) {
            Attack();
        } else if(attackFrozen) {
            if(lastAttack + (attackCooldown * .75f) < Time.time) {
                attackFrozen = false;
                attackMoveTarget = Vector3.zero;
            }
        }
    }

    void FinishCharge() {
        Destroy(pc.gameObject);
        chargeStart = -1;
        board.SquareHit(chargeTarget);
        anim.SetBool("Pound", true);
        anim.SetBool("Charging", false);
        Camera.main.SendMessage("DidPound", SendMessageOptions.DontRequireReceiver);
        if(driver != null)
            driver.SendMessage("ToggledSquare", chargeTarget, SendMessageOptions.DontRequireReceiver);
        return;
    }

    void Attack()
    {
        lastAttack = Time.time;
        anim.SetBool("Attack", true);
        attackMoveTarget = transform.position + transform.forward * attackDistance;
        attackFrozen = true;
        velocity = Vector3.zero;
        foreach(GameObject obj in inAttackEffect) {
            obj.SendMessage("GotAttacked", gameObject, SendMessageOptions.RequireReceiver);
        }
        inAttackEffect.Clear();
    }

    void GotHit(Vector3 contactPoint) {
        // if we're frozen in the attacking animation, ignore it
        if(attackFrozen)
            return;
        CancelCharge();
        // turn towards the thing that hit us so the "thrown back"
        // animation looks normal
        Vector3 lookPos = contactPoint - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);

        // throw player backwards from the contact point
        velocity = GetComponent<Rigidbody>().position - contactPoint;
        velocity.y = 0;
        velocity.Normalize();
        velocity.y = 1;
        velocity *= hurtFlingScale;
        hitFrozen = true;
        anim.SetBool("GotHit", true);
    }

    void ResetAnimatorFlags() {
        int state = anim.GetCurrentAnimatorStateInfo(0).nameHash;
        if(state == gotHitState)
            anim.SetBool("GotHit", false);
        if(state == attackState)
            anim.SetBool("Attack", false);
    }
   
    void StartCharge() {
        // started charging, grab the target square
        RaycastHit hit;
        Vector3 pos = transform.position;
        pos.y += .1f;
        if(!Physics.Raycast(pos, -Vector3.up, out hit, cc.height * 1.1f))  {
            Debug.Log("Missed a square?!?");
            return;
        }
        chargeTarget = hit.collider.gameObject.GetComponent<SquareController>();
        if (chargeTarget == null)  {
            Debug.Log("No SquareController on target!");
            return;
        }
        // if we're currently restricted to only toggling a
        // certain square (such as during the tutorial)
        // make sure this is it
        if(restrictedCharge != null && restrictedCharge != chargeTarget)
            return;
        pc = ((GameObject)GameObject.Instantiate(progressCircleTemplate)).GetComponent<ProgressCircle>();
        pc.Percent = 0;
        pc.FullColor = Color.white;
        pc.EmptyColor = Color.white;
        pos = chargeTarget.transform.position;
        pos.y += .1f;
        pc.transform.position = pos;
        pc.transform.rotation = Quaternion.Euler(new Vector3(90, 180, 0));
        chargeStart = Time.time;
        anim.SetBool("Charging", true);
        anim.SetBool("Pound", false);
        velocity = Vector3.zero;
    }

    void UpdateCharge() {
        float percent = Mathf.InverseLerp(chargeStart, chargeStart + chargeTime, Time.time);
        Debug.Log("Set to " + percent.ToString(".000"));
        pc.Percent = percent;
    }

    void UpdateMovement() {
        if(!canMove)
            return;

        float inputHorizontal = Input.GetAxisRaw("Horizontal"), inputVertical = Input.GetAxisRaw("Vertical");
        if(canControl) {
            if(inputHorizontal != 0) {
                // if they're swapping direction, immediately snap
                if(inputHorizontal > 0 && velocity.x < 0)
                    velocity.x = 0;
                if(inputHorizontal < 0 && velocity.x > 0)
                    velocity.x = 0;
                velocity.x += inputHorizontal * Time.deltaTime * walkAcceleration;
                velocity.x = Mathf.Min(walkSpeed, Mathf.Max(-walkSpeed, velocity.x));
            } else {
                // not moving horizontally, apply some friction
                if(velocity.x > 0)
                    velocity.x = Mathf.Max(0, velocity.x - walkAcceleration * Time.deltaTime);
                else
                    velocity.x = Mathf.Min(0, velocity.x + walkAcceleration * Time.deltaTime);
            }
            if(inputVertical != 0) {
                // if they're swapping direction, immediately snap
                if(inputVertical > 0 && velocity.z < 0)
                    velocity.z = 0;
                if(inputVertical < 0 && velocity.z > 0)
                    velocity.z = 0;
                velocity.z += inputVertical * Time.deltaTime * walkAcceleration;
                velocity.z = Mathf.Min(walkSpeed, Mathf.Max(-walkSpeed, velocity.z));
            } else {
                // not moving vertically, apply some friction
                if(velocity.z > 0)
                    velocity.z = Mathf.Max(0, velocity.z - walkAcceleration * Time.deltaTime);
                else
                    velocity.z = Mathf.Min(0, velocity.z + walkAcceleration * Time.deltaTime);
            }
        }
        Vector3 pos = transform.position + (velocity * Time.deltaTime);
        if(pos.y <= 0 && velocity.y != 0) {
            velocity.y = 0;
            pos.y = 0;
            hitFrozen = false;
        }
        if(attackMoveTarget != Vector3.zero) {
            pos = Vector3.Lerp(pos, attackMoveTarget, .25f);
        }

        // Don't go outside the board game
        Rect bounds = board.GetBounds();
        pos.x = Mathf.Max(bounds.x+cc.radius, (Mathf.Min(bounds.width-cc.radius, pos.x)));
        pos.z = Mathf.Max(bounds.y+cc.radius, (Mathf.Min(bounds.height-cc.radius, pos.z)));

        // look towards the direction the user's pressing
        if(canControl) {
            Vector3 lookTarget = new Vector3(inputHorizontal, 0, inputVertical);
            if(lookTarget != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(lookTarget);
            }
        }
        GetComponent<Rigidbody>().MovePosition(pos);
        if(velocity.y != 0) {
            velocity += Physics.gravity * Time.deltaTime;
        }
	}
}
