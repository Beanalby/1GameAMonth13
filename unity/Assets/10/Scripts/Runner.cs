using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

    private bool isRunning = true;

    private float speed = 1;

    private float shiftStartPos;
    private float shiftStart = -1;
    private const float shiftDuration = .25f;
    private float shiftOffset;
    private Interpolate.Function shiftEase = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic);

    public void Update() {
        HandleShift();
    }

    void FixedUpdate() {
        HandleRunning();
    }

    float ApplyShift(float current) {
        if(shiftStart == -1) {
            return current;
        }
        // interpolate between our starting point and the end
        float percent = (Time.time - shiftStart) / shiftDuration;
        if(percent >= 1) {
            shiftStart = -1;
            return shiftStartPos + shiftOffset;
        } else {
            return shiftEase(shiftStartPos, shiftOffset, percent, 1);
        }
    }
    void HandleShift() {
        if(shiftStart != -1) {
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
        shiftOffset = offset;
        shiftStart = Time.time;
        shiftStartPos = rigidbody.transform.position.x;
    }


    void HandleRunning() {
        if(isRunning) {
            Vector3 newPos = rigidbody.position;
            newPos.x = ApplyShift(newPos.x);
            newPos.z += +Time.deltaTime * speed;
            rigidbody.MovePosition(newPos);
        }
    }
}
