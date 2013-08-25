using UnityEngine;
using System.Collections;

public class PlayerDriver : MonoBehaviour {

    [HideInInspector]
    public bool EnableInput = true;

    private float moveDuration = .3f;

    private bool movingForward = false;
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
        if(moveStart != -1 || !EnableInput) {
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
            movingForward = true;
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
        if(moveStart != -1 && movingForward) {
            // undo the movement
            moveStart = Time.time;
            moveDelta = moveFrom - transform.position;
            moveFrom = transform.position;
            movingForward = false;
        }
    }
}
