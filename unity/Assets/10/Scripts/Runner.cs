using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

    private const int MAX_SHIFT_DISTANCE = 2;

    private float acceleration = .5f;
    private float speedStart = 5;
    private float speed;

    private float maxHealth = 4;
    private float health;
    public float Health {
        get { return health; }
    }
    public float Speed {
        get { return speed; }
    }

    private float shiftStartPos;
    private float shiftStart = -1;
    private const float shiftDuration = .25f;
    private const float shipRotate = 15f;
    private float shiftOffset;
    private Interpolate.Function shiftEase = Interpolate.Ease(Interpolate.EaseType.EaseInOutCubic);

    private bool isRunning = true;
    private Transform ship;
    [HideInInspector]
    public RunnerInfo info;

    public float distanceTravelled {
        get { return transform.position.z; }
    }

    public void Awake() {
        ship = transform.FindChild("mesh");
    }

    public void Start() {
        health = maxHealth;
        speed = speedStart;
    }

    public void Update() {
        HandleShift();
    }

    void FixedUpdate() {
        HandleAcceleration();
        HandleRunning();
    }

    float ApplyShift(float current) {
        if(shiftStart == -1) {
            return current;
        }
        // interpolate between our starting point and the end
        float percent = (Time.time - shiftStart) / shiftDuration;
        if(percent >= 1) {
            ship.rotation = Quaternion.Euler(Vector3.zero);
            shiftStart = -1;
            return shiftStartPos + shiftOffset;
        } else {
            if(percent >= .25f) {
                ship.rotation = Quaternion.Euler(new Vector3(
                    0, 0, shiftEase(shipRotate * -shiftOffset, -(shipRotate * -shiftOffset), percent - .25f, .75f)));
            }
            return shiftEase(shiftStartPos, shiftOffset, percent, 1);
        }
    }
    void HandleShift() {
        if(shiftStart != -1 || isRunning == false) {
            return;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        if(horizontal >= 1) {
            Shift(1);
        } else if(horizontal <= -1) {
            Shift(-1);
        }
    }

    void Shift(int offset) {
        if(shiftStart != -1) {
            return;
        }
        // don't allow shifting beyond the max position
        float newPos = rigidbody.transform.position.x + offset;
        if(newPos > MAX_SHIFT_DISTANCE || newPos < -MAX_SHIFT_DISTANCE) {
            return;
        }
        ship.rotation = Quaternion.Euler(0, 0, shipRotate * -offset);
        shiftOffset = offset;
        shiftStart = Time.time;
        shiftStartPos = rigidbody.transform.position.x;
    }

    void HandleAcceleration() {
        if(isRunning) {
            // slow down acceleration if we're going fast
            if(speed >= 12) {
                speed += (acceleration / 10) * Time.deltaTime;
            } else if(speed >= 8) {
                speed += (acceleration / 2) * Time.deltaTime;
            } else {
                speed += acceleration * Time.deltaTime;
            }
        }
    }

    void HandleRunning() {
        if(isRunning) {
            Vector3 newPos = rigidbody.position;
            newPos.x = ApplyShift(newPos.x);
            newPos.z += +Time.deltaTime * speed;
            rigidbody.MovePosition(newPos);
        }
    }

    public void Crashed(float damage) {
        health -= damage;
        speed /= 2;
        if(health <= 0) {
            health = 0;
            Die();
        }
    }

    public void Die() {
        Debug.Log("Down she goes!");
        if(info) {
            info.RunnerDied();
        }
        isRunning = false;
        // remove the camera from ourselves
        GetComponentInChildren<Camera>().transform.parent = null;
        Destroy(gameObject);
    }
}
