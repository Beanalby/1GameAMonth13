using UnityEngine;
using System.Collections;

public class CutePlayer : MonoBehaviour {

    private const float INPUT_THRESHOLD = .5f;

    public GameObject aimPrefab, aimAnyPrefab;
    private GameObject aimObject = null, aimAnyObject = null;

    private float moveStart = -1;
    private Vector3 moveDelta = Vector3.zero, moveBase = Vector3.zero;
    private float moveDuration = .25f;
    private Interpolate.Function moveEase = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic);
    private int groundMask;
    private Vector3 aimDir;

    public bool CanMove {
        get { return moveStart == -1 && !IsAiming; }
    }
    private bool IsAiming = false;

    public void Start() {
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        aimObject = Instantiate(aimPrefab) as GameObject;
        aimObject.SetActive(false);
        aimAnyObject = Instantiate(aimAnyPrefab) as GameObject;
        aimAnyObject.SetActive(false);
    }

    public void Update() {
        HandleInput();
        HandleMovement();
    }

    private void HandleInput() {
        HandleInputAim();
        HandleInputMovement();
    }

    private void HandleInputAim() {
        if(Input.GetButtonDown("Fire1")) {
            IsAiming = true;
            aimObject.SetActive(false);
            aimAnyObject.SetActive(true);
            aimAnyObject.transform.position = transform.position;
            aimObject.transform.position = transform.position;
            aimDir = Vector3.zero;
        } else if(Input.GetButtonUp("Fire1")) {
            if(aimDir == Vector3.zero) {
                Debug.Log("Didn't move aim, cancelling.");
            } else {
                Debug.Log("Firing in direction " 
                    + aimDir);
            }
            IsAiming = false;
            aimObject.SetActive(false);
            aimAnyObject.SetActive(false);
            aimDir = Vector3.zero;
            return;
        }
        if(IsAiming) {
            // we're currently aiming, see if there's a directional input
            aimDir = Vector3.zero;
            if(Input.GetAxisRaw("Horizontal") <= -INPUT_THRESHOLD) {
                aimDir = Vector3.left;
            } else if(Input.GetAxisRaw("Horizontal") >= INPUT_THRESHOLD) {
                aimDir = Vector3.right;
            } else if(Input.GetAxisRaw("Vertical") <= -INPUT_THRESHOLD) {
                aimDir = new Vector3(0, 0, -1);
            } else if(Input.GetAxisRaw("Vertical") >= INPUT_THRESHOLD) {
                aimDir = new Vector3(0, 0, 1);
            }
            if(aimDir != Vector3.zero) {
                aimAnyObject.SetActive(false);
                aimObject.SetActive(true);
                aimObject.transform.position = transform.position + aimDir;
            } else {
                aimAnyObject.SetActive(true);
                aimObject.SetActive(false);
            }
        }
    }

    private void HandleInputMovement() {
        if(!CanMove) {
            return;
        }
        Vector3 delta = Vector3.zero;
        if(Input.GetAxisRaw("Horizontal") <= -INPUT_THRESHOLD) {
            delta = Vector3.left;
        } else if(Input.GetAxisRaw("Horizontal") >= INPUT_THRESHOLD) {
            delta = Vector3.right;
        } else if(Input.GetAxisRaw("Vertical") <= -INPUT_THRESHOLD) {
            delta = new Vector3(0, 0, -1);
        } else if(Input.GetAxisRaw("Vertical") >= INPUT_THRESHOLD) {
            delta = new Vector3(0, 0, 1);
        }
        if(delta != Vector3.zero) {
            TryMovement(delta);
        }
    }
    private void TryMovement(Vector3 delta) {
        Vector3 testPos = transform.position + delta;
        // don't move into a ground block
        if(Physics.OverlapSphere(testPos, .2f, groundMask).Length != 0) {
            //Debug.Log("Skipping movement into a ground block");
            return;
        }
        // don't move to a position that doesn't have ground beneath it
        testPos.y -= 1;
        if(Physics.OverlapSphere(testPos, .2f, groundMask).Length == 0) {
            //Debug.Log("Skipping movement over non-existent ground");
            return;
        }
        // everything checks out, let it move
        ApplyMovement(delta);
    }

    private void ApplyMovement(Vector3 delta) {
        moveDelta = delta;
        moveBase = transform.position;
        moveStart = Time.time;
    }

    private void HandleMovement() {
        if(moveStart != -1) {
            float percent = (Time.time - moveStart) / moveDuration;
            if(percent >= 1) {
                transform.position = moveBase + moveDelta;
                moveStart = -1;
                moveBase = Vector3.zero;
                moveDelta = Vector3.zero;
            } else {
                transform.position = Interpolate.Ease(moveEase,
                    moveBase, moveDelta, percent, 1);
            }
        }
    }
}
