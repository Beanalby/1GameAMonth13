using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    static public Vector3 OPEN_LEFT = new Vector3(-.5f, 0, 0),
        OPEN_RIGHT = new Vector3(.5f, 0, 0);

    public AudioClip soundOpen;
    public AudioClip soundClose;

    public Switch button;

    public bool invert = false;

    private float openStarted = -1;
    private float closeStarted = -1f;
    private float moveDuration = 1f;
    private Interpolate.Function ease = Interpolate.Ease(Interpolate.EaseType.EaseInOutCubic);
    private bool isClosed;

    private Transform doorLeft, doorRight;

    public void Start() {
        foreach(MeshRenderer rend in GetComponentsInChildren<MeshRenderer>()) {
            rend.material.color = button.color;
        }
        button.switchListeners += ButtonPressed;
        doorLeft = transform.Find("doorLeft");
        doorRight = transform.Find("doorRight");
        if(doorLeft.localPosition == Vector3.zero && doorRight.localPosition == Vector3.zero) {
            isClosed = true;
        } else {
            isClosed = false;
        }
    }

    public void OnDrawGizmos() {
        if(button == null) {
            return;
        }
        Gizmos.color = button.color;
        // draw from the middle of the door, not on the ground
        Vector3 pos = transform.position;
        pos.y += .5f;
        Gizmos.DrawLine(pos, button.transform.position);
    }

    public void Update() {
        HandleMovement();
    }

    private void ButtonPressed(bool isPressed) {
        if(invert) {
            isPressed = !isPressed;
        }
        if(isPressed && isClosed) {
            OpenDoors();
        } else if(!isPressed && !isClosed) {
            CloseDoors();
        }
    }

    private void CloseDoors() {
        closeStarted = Time.time;
        AudioSource.PlayClipAtPoint(soundClose, transform.position);
    }

    private void HandleMovement() {
        if(openStarted != -1) {
            float percent = (Time.time - openStarted) / moveDuration;
            if(percent >= 1) {
                doorLeft.localPosition = OPEN_LEFT;
                doorRight.localPosition = OPEN_RIGHT;
                openStarted = -1;
                isClosed = false;
            } else {
                doorLeft.localPosition = Interpolate.Ease(ease,
                    Vector3.zero, OPEN_LEFT, percent, 1f);
                doorRight.localPosition = Interpolate.Ease(ease,
                    Vector3.zero, OPEN_RIGHT, percent, 1f);
            }
        }
        if(closeStarted != -1) {
            float percent = (Time.time - closeStarted) / moveDuration;
            if(percent >= 1) {
                doorLeft.localPosition = Vector3.zero;
                doorRight.localPosition = Vector3.zero;
                closeStarted = -1;
                isClosed = true;
            } else {
                doorLeft.localPosition = Interpolate.Ease(ease,
                    OPEN_LEFT, -OPEN_LEFT, percent, 1f);
                doorRight.localPosition = Interpolate.Ease(ease,
                    OPEN_RIGHT, -OPEN_RIGHT, percent, 1f);
            }
        }
    }

    private void OpenDoors() {
        openStarted = Time.time;
        AudioSource.PlayClipAtPoint(soundOpen, transform.position);
    }
}
