using UnityEngine;
using System.Collections;

public class SelectLevelDriver : MonoBehaviour {

    public Level current;

    [HideInInspector]
    public bool EnableInput = true;

    private Vector3 moveStartPos, moveTargetPos;
    private float moveStart = -1f;
    private float moveDuration = .5f;
    private Interpolate.Function ease; 
    private GameDriver8 driver;

    private bool loadWhenMoved = false;
    public void Awake() {
        driver = GameDriver8.instance;
        
        // fixup left & bottom links based on top & right links
        Object[] tmp = Resources.FindObjectsOfTypeAll(typeof(Level));
        Level[] levels = System.Array.ConvertAll(tmp, o => (Level)o);
        foreach(Level level in levels) {
            if(level.levelRight != null) {
                level.levelRight.levelLeft = level;
            }
            if(level.levelTop != null) {
                level.levelTop.levelBottom = level;
            }
        }
        current = GameObject.Find(driver.LastLevel).GetComponent<Level>();
        current.Selected();
    }
    public void Start() {
        InitMovement();
    }
    public void Update() {
        HandleInputMovement();
        HandleInputSelect();
        HandleMovement();
    }

    private Vector3 GetInsidePoint(Level level) {
        /// when we're moving into/outof a level, we go slightly further than
        /// the stage's actual position to "get past" the lineRenderer
        Ray ray = new Ray(level.transform.position,
            -LevelDriver.CAMERA_OFFSET_FAR);
        return ray.GetPoint(LevelDriver.CAMERA_OFFSET_FAR.magnitude * 1.05f);
    }

    private void HandleInputMovement() {
        if(!EnableInput) {
            return;
        }
        /// Ignore any movement inputs if we're already moving and less than
        /// a certain percentage done
        if(moveStart != -1) {
            if(((Time.time - moveStart) / moveDuration < .85f)) {
                return;
            }
        }

        float horizontal = Input.GetAxisRaw("Horizontal"),
            vertical = Input.GetAxisRaw("Vertical");
        Level moveTarget = null;
        if(horizontal == 1) {
            moveTarget = current.levelRight;
        } else if(horizontal == -1) {
            moveTarget = current.levelLeft;
        } else if(vertical == 1) {
            moveTarget = current.levelTop;
        } else if(vertical == -1) {
            moveTarget = current.levelBottom;
        }

        if(moveTarget != null) {
            if(driver.IsLinkActive(current, moveTarget)) {
                MoveToLevel(moveTarget);
            } else {
                // TODO: make it nudge in that direction
            }
        }
    }
    private void HandleInputSelect() {
        if(!EnableInput) {
            return;
        }
        /// Don't allow more input if it's already moving and it's not more
        /// than some percent done
        if(Input.GetButtonDown("Fire1")) {
            if(driver.IsLevelFinished(current)) {
                return;
            }
            ChooseLevel();
        }
    }
    private void HandleMovement() {
        if(moveStart == -1) {
            return;
        }
        float percent = (Time.time - moveStart) / moveDuration;
        if(percent >= 1) {
            moveStart = -1;
            transform.position = moveTargetPos;
            if(loadWhenMoved) {
                driver.LoadLevel(current);
            }
        } else {
            if(percent >= .9f && loadWhenMoved) {
                current.ShowLabel = false;
            }
            transform.position = Interpolate.Ease(ease, moveStartPos,
                moveTargetPos - moveStartPos,
                percent, 1);
        }
    }

    private void InitMovement() {
        /// Performs initial movement (zooming out of level)
        MoveToLevel(current);
        moveStart = Time.time;
        ease = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic);
        moveStartPos = GetInsidePoint(current);
        moveTargetPos = current.transform.position;
    }
    private void MoveToLevel(Level newLevel) {
        moveStartPos = current.transform.position;
        moveTargetPos = newLevel.transform.position;
        moveStart = Time.time;
        ease = Interpolate.Ease(Interpolate.EaseType.EaseOutCirc);
        if(current != null) {
            current.Unselected();
        }
        current = newLevel;
        current.Selected();
    }

    private void ChooseLevel() {
        moveStart = Time.time;
        ease = Interpolate.Ease(Interpolate.EaseType.EaseInCubic);
        moveStartPos = current.transform.position;
        moveTargetPos = GetInsidePoint(current);
        loadWhenMoved = true;
        EnableInput = false;
    }
}
