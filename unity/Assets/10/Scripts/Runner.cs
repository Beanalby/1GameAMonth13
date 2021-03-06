﻿using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

    public AudioClip shiftSound;
    public GameObject IntroPrefab;

    private const int MAX_SHIFT_DISTANCE = 2;

    private float acceleration = .5f;
    private float speedStart = 5;
    private float speed;

    private float startTime, startPos;

    private float maxHealth = 4;
    private float health;
    public float Health {
        get { return health; }
    }
    public float Speed {
        get { return speed; }
    }

    public bool CanControl;

    private float shiftStartPos;
    private float shiftStart = -1;
    private const float shiftDuration = .25f;
    private const float shipRotate = 15f;
    private float shiftOffset;
    private Interpolate.Function shiftEase = Interpolate.Ease(Interpolate.EaseType.EaseInOutCubic);

    private float blinkStart = -1f;
    private float blinkDuration = 1f;
    private float blinkRate = .25f;

    private Transform ship;
    [HideInInspector]
    public RunnerInfo info;

    public float distanceTravelled {
        get {
            if(!CanControl) {
                return 0;
            } else {
                return transform.position.z - startPos;
            }
        }
    }

    public void Awake() {
        ship = transform.Find("mesh");
    }

    public void Start() {
        health = maxHealth;
        speed = speedStart;
        CanControl = false;
    }

    public void Update() {
        HandleBlink();
        HandlePitch();
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
        if(shiftStart != -1 || !CanControl) {
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
        float newPos = GetComponent<Rigidbody>().transform.position.x + offset;
        if(newPos > MAX_SHIFT_DISTANCE || newPos < -MAX_SHIFT_DISTANCE) {
            return;
        }
        ship.rotation = Quaternion.Euler(0, 0, shipRotate * -offset);
        shiftOffset = offset;
        shiftStart = Time.time;
        shiftStartPos = GetComponent<Rigidbody>().transform.position.x;
        AudioSource.PlayClipAtPoint(shiftSound, Camera.main.transform.position);
    }

    void HandleBlink() {
        if(blinkStart == -1) {
            return;
        }
        float percent = (Time.time - blinkStart) / blinkDuration;
        if(percent >= 1) {
            ship.gameObject.SetActive(true);
            blinkStart = -1;
        } else {
            ship.gameObject.SetActive((Time.time - blinkStart) % blinkRate > blinkRate / 2f);
        }
    }

    void HandlePitch() {
        GetComponent<AudioSource>().pitch = speed * .1f + .45f;
    }

    void HandleAcceleration() {
        if(!CanControl) // don't speed up before they control
            return;
        // slow down acceleration if we're going fast
        if(speed >= 12) {
            speed += (acceleration / 5) * Time.deltaTime;
        } else if(speed >= 8) {
            speed += (acceleration / 2) * Time.deltaTime;
        } else {
            speed += acceleration * Time.deltaTime;
        }
    }

    void HandleRunning() {
        Vector3 newPos = GetComponent<Rigidbody>().position;
        newPos.x = ApplyShift(newPos.x);
        newPos.z += +Time.deltaTime * speed;
        GetComponent<Rigidbody>().MovePosition(newPos);
    }

    public void EnableControl() {
        startPos = transform.position.z;
        CanControl = true;
        // make the title stop following us, and stick to the room
        Transform title = transform.Find("Title");
        if(title) {
            RunDriver.instance.AttachToNextRoom(title);
        }
        StartCoroutine(AddIntro());
    }

    private IEnumerator AddIntro() {
        yield return new WaitForSeconds(.5f);
        // make a room with the help text
        Transform intro = (GameObject.Instantiate(IntroPrefab) as GameObject).transform;
        Vector3 pos = intro.position;
        intro.parent = transform;
        intro.localPosition = pos;
        yield return new WaitForSeconds(2);
        RunDriver.instance.AttachToNextRoom(intro);
    }

    public void Crashed(float damage) {
        health -= damage;
        speed /= 2;
        blinkStart = Time.time;
        if(health <= 0) {
            health = 0;
            Die();
        }
    }

    public void Die() {
        if(info) {
            info.RunnerDied();
        }
        // remove the camera from ourselves
        GetComponentInChildren<Camera>().transform.parent = null;
        Destroy(gameObject);
    }
}
