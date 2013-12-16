using UnityEngine;
using System.Collections;

public class CutePlayer : MonoBehaviour {

    private const float INPUT_THRESHOLD = .5f;

    public GameObject aimPrefab, aimAnyPrefab;
    private GameObject aimObject = null, aimAnyObject = null;

    public AudioClip moveSound, chargeSound, headbuttSound;

    private Transform mesh;
    private Vector3 savedRotation, savedScale;

    private float flingSpeed = 5f;
    private Interpolate.Function flingEase = Interpolate.Ease(Interpolate.EaseType.Linear);

    private float moveSpeed = 4f;
    private Interpolate.Function moveEase = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic);

    private float travelDuration;
    private float travelStart = -1;
    private Vector3 travelDelta = Vector3.zero, travelBase = Vector3.zero;
    private Interpolate.Function travelEase = null;
    private int groundMask, attackableMask;

    private Vector3 aimDir;
    public bool IsFlinging {
        get { return isFlinging; }
    }
    private bool isFlinging;

    public bool CanMove {
        get { return stageDriver.IsPlaying && travelStart == -1 && !IsAiming; }
    }
    public bool CanControl {
        get { return stageDriver.IsPlaying && travelStart == -1; }
    }

    private bool IsAiming = false;
    private CuteStageDriver stageDriver;

    public void Start() {
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        attackableMask = 1 << LayerMask.NameToLayer("Attackable");
        aimObject = Instantiate(aimPrefab) as GameObject;
        aimObject.SetActive(false);
        aimAnyObject = Instantiate(aimAnyPrefab) as GameObject;
        aimAnyObject.SetActive(false);
        mesh = transform.Find("mesh");
        stageDriver = CuteStageDriver.Instance;
    }

    public void Update() {
        HandleInput();
        HandleTravel();
        HandleFlinging();
    }

    private void HandleInput() {
        HandleInputAim();
        HandleInputMovement();
    }

    private void HandleInputAim() {
        if(!CanControl) {
            return;
        }
        if(!IsAiming && Input.GetButton("Fire1")) {
            IsAiming = true;
            aimObject.SetActive(false);
            aimAnyObject.SetActive(true);
            aimAnyObject.transform.position = transform.position;
            aimObject.transform.position = transform.position;
            foreach(Transform t in aimAnyObject.transform) {
                t.gameObject.SetActive(IsValidMovementTarget(t.position, true));
            }
            aimDir = Vector3.zero;
            //AudioSource.PlayClipAtPoint(chargeSound, Camera.main.transform.position);

        } else if(IsAiming && !Input.GetButton("Fire1")) {
            if(aimDir != Vector3.zero) {
                StartFling();
            }
            IsAiming = false;
            aimObject.SetActive(false);
            aimAnyObject.SetActive(false);
        }
        if(IsAiming) {
            // we're currently aiming, see if there's a directional input
            Vector3 checkDir = Vector3.zero;
            if(Input.GetAxisRaw("Horizontal") <= -INPUT_THRESHOLD) {
                checkDir = Vector3.left;
            } else if(Input.GetAxisRaw("Horizontal") >= INPUT_THRESHOLD) {
                checkDir = Vector3.right;
            } else if(Input.GetAxisRaw("Vertical") <= -INPUT_THRESHOLD) {
                checkDir = new Vector3(0, 0, -1);
            } else if(Input.GetAxisRaw("Vertical") >= INPUT_THRESHOLD) {
                checkDir = new Vector3(0, 0, 1);
            }
            // if there was direction input and it's valid, aim there
            if(checkDir != Vector3.zero && IsValidMovementTarget(transform.position + checkDir, true)) {
                aimDir = checkDir;
                aimAnyObject.SetActive(false);
                aimObject.SetActive(true);
                aimObject.transform.position = transform.position + checkDir;
            } else {
                aimDir = Vector3.zero;
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
        if(!IsValidMovementTarget(transform.position + delta, false)) {
            return;
        }
        // everything checks out, let it move
        ApplyMovement(delta);
    }

    private void ApplyMovement(Vector3 delta) {
        // if there's an object in that direction, push it
        Vector3 pos = transform.position + delta;
        foreach(Collider col in Physics.OverlapSphere(pos, .2f, attackableMask)) {
            col.SendMessage("Pushed", delta);
        }
        travelDelta = delta;
        travelBase = transform.position;
        travelStart = Time.time;
        travelDuration = 1 / moveSpeed;
        travelEase = moveEase;
        AudioSource.PlayClipAtPoint(moveSound, Camera.main.transform.position);
    }

    private bool IsValidMovementTarget(Vector3 pos, bool testFling) {
        // check we're not moving into something we shouldn't.  If we're
        // flinging then we only worry about ground, but movement can't
        // go through objects either
        if(Physics.OverlapSphere(pos, .2f, groundMask).Length != 0) {
            return false;
        }
        // if we're not flinging, look for an attackable object.
        // make sure the object can be pushed
        if(!testFling) {
            Collider[] objs = Physics.OverlapSphere(pos, .2f, attackableMask);
            if(objs.Length != 0) {
                CuteAttackable tmp = objs[0].GetComponent<CuteAttackable>();
                Vector3 dir = pos - transform.position;
                if(!tmp.IsValidMovementDir(dir)) {
                    return false;
                }
            }
        }
        // don't move to a position that doesn't have ground beneath it
        pos.y -= 1;
        if(Physics.OverlapSphere(pos, .2f, groundMask).Length == 0) {
            return false;
        }
        return true;
    }
    private void HandleTravel() {
        if(travelStart != -1) {
            float percent = (Time.time - travelStart) / travelDuration;
            if(IsFlinging && percent >= 1) {
                // get the next fling target and re-evaluate percent
                FlingToNext();
                percent = (Time.time - travelStart) / travelDuration;
            }
            if(percent >= 1) {
                transform.position = travelBase + travelDelta;
                travelStart = -1;
                travelBase = Vector3.zero;
                travelDelta = Vector3.zero;
            } else {
                transform.position = Interpolate.Ease(travelEase,
                    travelBase, travelDelta, percent, 1);
            }
        }
    }

    private void StartFling() {
        savedRotation = mesh.localRotation.eulerAngles;
        savedScale = mesh.localScale;

        travelBase = transform.position;
        travelEase = flingEase;
        if(aimDir == Vector3.right) {
            mesh.localRotation = Quaternion.Euler(new Vector3(
                savedRotation.x, savedRotation.y, -90));
        } else if(aimDir == Vector3.left) {
            mesh.localRotation = Quaternion.Euler(new Vector3(
                savedRotation.x, savedRotation.y, 90));
        } else if(aimDir.z == 1) {
            mesh.localScale = new Vector3(
                savedScale.x, 1f-savedScale.y, savedScale.z);
        } else {
            mesh.localScale = new Vector3(
                savedScale.x, savedScale.y-1, savedScale.z);
        }
        isFlinging = true;
        travelDelta = aimDir;
        travelDuration = 1 / flingSpeed;
        AudioSource.PlayClipAtPoint(headbuttSound, Camera.main.transform.position);
        FlingToNext();
    }
    private void StopFling() {
        mesh.localRotation = Quaternion.Euler(savedRotation) ;
        mesh.localScale = savedScale;
        isFlinging = false;
        stageDriver.FlingFinished();
        // let the movement stuff finish normally
    }

    private void FlingToNext() {
        // keep going to the next location
        if(travelStart == -1) {
            // we're starting to fling, make sure it's valid
            if(!IsValidMovementTarget(travelBase + aimDir, true)) {
                StopFling();
                return;
            }
            travelBase = transform.position;
        } else {
            // we're midair, and see whether we can continue beyond the
            // current movement
            if(!IsValidMovementTarget((travelBase + aimDir) + aimDir, true)) {
                StopFling();
                return;
            }
            travelBase = travelBase + aimDir;
        }
        travelStart = Time.time;
        // check if there's anything that would get destroyed by this
        foreach(Collider col in Physics.OverlapSphere(
                travelBase + aimDir, .2f, attackableMask)) {
            col.gameObject.SendMessage("Attacked");
        }
    }
    private void HandleFlinging() {
        // make sure the place we're about to go is over ground
    }
}
