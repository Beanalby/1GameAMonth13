using UnityEngine;
using System.Collections;

public class CuteAttackable : MonoBehaviour {
    public GameObject DeathEffect;
    public bool MustDestroy = true;
    public AudioClip pushedSound;
    public AudioClip dieSound;

    private float moveSpeed = 4f;
    private Interpolate.Function moveEase = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic);

    private float travelDuration;
    private float travelStart = -1;
    private Vector3 travelDelta = Vector3.zero, travelBase = Vector3.zero;
    private Interpolate.Function travelEase = null;
    private int groundMask, attackableMask;

    public void Start() {
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        attackableMask = 1 << LayerMask.NameToLayer("Attackable");
    }
    public void Update() {
        HandleTravel();
    }

    public void Attacked() {
        Debug.Log(name + " got attacked!");
        if(DeathEffect != null) {
            GameObject obj = Instantiate(DeathEffect) as GameObject;
            obj.transform.position = transform.position;
        }
        AudioSource.PlayClipAtPoint(dieSound, Camera.main.transform.position);
        Destroy(gameObject);
    }

    public bool IsValidMovementDir(Vector3 dir) {
        return IsValidMovementTarget(transform.position + dir);
    }
    public bool IsValidMovementTarget(Vector3 pos) {
        // check we're not moving into something we shouldn't.
        // attackable things can't push other objects, so we always test both
        int mask = groundMask |= attackableMask;
        if(Physics.OverlapSphere(pos, .2f, mask).Length != 0) {
            return false;
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

    public void Pushed(Vector3 delta) {
        if(!IsValidMovementDir(delta)) {
            Debug.LogError("Got pushed invalid! Shouldn't happen!");
            return;
        }
        travelDelta = delta;
        travelBase = transform.position;
        travelStart = Time.time;
        travelDuration = 1 / moveSpeed;
        travelEase = moveEase;
        AudioSource.PlayClipAtPoint(pushedSound, Camera.main.transform.position);
    }
}
