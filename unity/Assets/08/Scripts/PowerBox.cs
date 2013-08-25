using UnityEngine;
using System.Collections;

public class PowerBox : MonoBehaviour {

    public AudioClip SoundSlide;

    private float moveStart=-1, moveDuration;
    private Vector3 moveStartPos, moveDelta;
    private Interpolate.Function ease = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic);
    private bool movingForward = true;

    private PlayerDriver player;
    private int powerTileMask;
    GameObject currentPowerTile;

    public float MoveStart {
        get { return moveStart; }
    }


    public void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerDriver>();
        powerTileMask = 1 << LayerMask.NameToLayer("PowerTile");
    }
    public void Update() {
        HandleMovement();
    }
    private void CheckPowerTile() {
        Vector3 pos = transform.position;
        pos.z += .1f;
        // if we were previously on a power tile, we're certainly off it now
        if(currentPowerTile != null) {
            currentPowerTile.SendMessage("DeactivateTile");
            currentPowerTile = null;
        }
        Ray ray = new Ray(pos, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, powerTileMask)) {
            hit.collider.SendMessage("ActivateTile");
            currentPowerTile = hit.collider.gameObject;
        }
    }

    public void HandleMovement() {
        if(moveStart == -1) {
            return;
        }
        float percent = (Time.time - moveStart) / moveDuration;
        if(percent >= 1) {
            rigidbody.MovePosition(moveStartPos + moveDelta);
            moveStart = -1;
            CheckPowerTile();
        } else {
            rigidbody.MovePosition(Interpolate.Ease(ease, moveStartPos,
                moveDelta, percent, 1));
        }
    }

    public void ReverseMovement() {
        if(moveStart != -1 && movingForward) {
            // undo the movement
            moveStart = Time.time;
            moveDelta = moveStartPos - transform.position;
            moveStartPos = transform.position;
            movingForward = false;
            // if I'M reversing, then the player will also need to.
            player.ReverseMovement(false);
        }
    }

    public void OnCollisionEnter(Collision col) {
        PlayerDriver player = col.collider.GetComponent<PlayerDriver>();
        if(player == null) {
            // uh oh, collided with something besides player.  ABORT MOVE!
            ReverseMovement();
        } else {
            moveStart = Time.time;
            moveStartPos = transform.position;
            moveDelta = player.MoveDelta;
            moveDuration = player.MoveDuration;
            movingForward = true;
            AudioSource.PlayClipAtPoint(SoundSlide, transform.position);
        }
    }
}
