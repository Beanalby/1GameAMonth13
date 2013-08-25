﻿using UnityEngine;
using System.Collections;

public class PlayerDriver : MonoBehaviour {

    private float moveDuration = .3f;

    private float moveStart=-1;
    private Vector3 moveFrom, moveDelta;
    private Interpolate.Function ease = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic);
    public void Update() {
        HandleMovementInput();
    }

    public void FixedUpdate() {
        HandleMovement();
    }

    private void HandleMovementInput() {
        if(moveStart != -1) {
            return;
        }
        float horizontal = Input.GetAxisRaw("Horizontal"),
            vertical = Input.GetAxisRaw("Vertical");
        if(horizontal == -1) {
            moveDelta = Vector3.left;
        } else if(horizontal == 1) {
            moveDelta = Vector3.right;
        } else if(vertical == -1) {
            moveDelta = Vector3.back;
        } else if(vertical == 1) {
            moveDelta = Vector3.forward;
        }
        if(moveDelta != Vector3.zero) {
            moveStart = Time.time;
            moveFrom = transform.position;
        }
    }

    private void HandleMovement() {
        if(moveStart == -1) {
            return;
        }
        float percent = (Time.time - moveStart) / moveDuration;
        if(percent >= 1) {
            rigidbody.MovePosition(moveFrom + moveDelta);
            moveStart = -1;
            moveDelta = Vector3.zero;
        } else {
            rigidbody.MovePosition(Interpolate.Ease(ease, moveFrom, moveDelta,
                percent, 1));
        }
    }
    private void OnCollisionEnter(Collision col) {
        if(col.collider.name == "Ground") {
            return;
        }
        if(moveStart != -1) {
            // undo the movement
            Debug.Log("Undoing movement");
            moveStart = Time.time;
            moveDelta = moveFrom - transform.position;
            moveFrom = transform.position;
        }
        Debug.Log("Collided with " + col.collider.name);
    }
}
