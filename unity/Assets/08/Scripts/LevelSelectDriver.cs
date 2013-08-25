using UnityEngine;
using System.Collections;

public class LevelSelectDriver : MonoBehaviour {

    public Level current, target;
    
    private float moveStart = -1f;
    private float moveDuration = .5f;
    private Interpolate.Function ease = Interpolate.Ease(Interpolate.EaseType.EaseOutExpo);
    private GameDriver8 driver;

    public void Awake() {
        driver = GameDriver8.instance;
        if(driver.lastLevel == null) {
            current = GameObject.Find("Level-test").GetComponent<Level>();
        } else {
            current = GameObject.Find("Level-" + driver.lastLevel).GetComponent<Level>();
        }
    }
    public void Start() {
        MoveToLevel(current);
    }
    public void Update() {
        HandleInput();
        HandleMovement();
    }

    private void HandleInput() {
        if(moveStart != -1) {
            return;
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
    private void HandleMovement() {
        if(moveStart == -1) {
            return;
        }
        float percent = (Time.time - moveStart) / moveDuration;
        if(percent >= 1) {
            moveStart = -1;
            transform.position = target.transform.position;
            current = target;
        } else {
            transform.position = Interpolate.Ease(ease, current.transform.position,
                target.transform.position - current.transform.position,
                percent, 1);
        }
    }

    private void MoveToLevel(Level newLevel) {
        target = newLevel;
        moveStart = Time.time;
        if(current != null) {
            current.Unselected();
        }
        target.Selected();
    }

}
