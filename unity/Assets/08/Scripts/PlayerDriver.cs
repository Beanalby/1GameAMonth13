using UnityEngine;
using System.Collections;

public class PlayerDriver : MonoBehaviour {

    public AudioClip soundMove;
    public AudioClip soundMoveRevert;

    [HideInInspector]
    public bool EnableInput = true;

    private float moveDuration = .25f;

    private bool movingForward = false;
    private float moveStart=-1;
    private Vector3 moveFrom, moveDelta;
    private Interpolate.Function ease = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic);

    Transform mesh;

    public Vector3 MoveDelta {
        get { return moveDelta; }
    }
    public float MoveDuration {
        get { return moveDuration; }
    }

    public void Start() {
        mesh = transform.Find("PlayerMesh");
    }
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
            mesh.rotation = Quaternion.LookRotation(moveDelta);
            AudioSource.PlayClipAtPoint(soundMove, transform.position);
        }
    }

    private void HandleMovement() {
        if(moveStart == -1) {
            return;
        }
        float percent = (Time.time - moveStart) / moveDuration;
        if(percent >= 1) {
            GetComponent<Rigidbody>().MovePosition(moveFrom + moveDelta);
            moveStart = -1;
            moveDelta = Vector3.zero;
        } else {
            GetComponent<Rigidbody>().MovePosition(Interpolate.Ease(ease, moveFrom, moveDelta,
                percent, 1));
        }
    }
    public void ReverseMovement(bool playSound) {
        if(moveStart != -1 && movingForward) {
            // undo the movement
            moveStart = Time.time;
            moveDelta = moveFrom - transform.position;
            moveFrom = transform.position;
            movingForward = false;
            if(playSound) {
                AudioSource.PlayClipAtPoint(soundMoveRevert,
                    transform.position);
            }
        }
    }
    private void OnCollisionEnter(Collision col) {
        PowerBox box = col.collider.GetComponent<PowerBox>();
        if(box != null) {
            // don't reverse if it isn't moving, just push it
            if(box.MoveStart == -1) {
                return;
            }
            // don't play a sound for boxes, they make their own
            ReverseMovement(false);
        } else {
            ReverseMovement(true);
        }
    }
}
