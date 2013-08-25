using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    static private Vector3 openLeft = new Vector3(-.5f, 0, 0),
        openRight = new Vector3(.5f, 0, 0);

    public Button button;

    private float openStarted = -1;
    private float closeStarted = -1f;
    private float moveDuration = 1f;
    private Interpolate.Function ease = Interpolate.Ease(Interpolate.EaseType.EaseInOutCubic);

    private Transform doorLeft, doorRight;

    public void Start() {
        foreach(MeshRenderer rend in GetComponentsInChildren<MeshRenderer>()) {
            rend.material.color = button.color;
        }
        button.buttonListeners += ButtonPressed;
        doorLeft = transform.Find("doorLeft");
        doorRight = transform.Find("doorRight");
    }

    public void OnDrawGizmos() {
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
        if(isPressed) {
            OpenDoors();
        } else {
            CloseDoors();
        }
    }

    private void CloseDoors() {
        closeStarted = Time.time;
    }

    private void HandleMovement() {
        if(openStarted != -1) {
            float percent = (Time.time - openStarted) / moveDuration;
            if(percent >= 1) {
                doorLeft.localPosition = openLeft;
                doorRight.localPosition = openRight;
                openStarted = -1;
            } else {
                doorLeft.localPosition = Interpolate.Ease(ease,
                    Vector3.zero, openLeft, percent, 1f);
                doorRight.localPosition = Interpolate.Ease(ease,
                    Vector3.zero, openRight, percent, 1f);
            }
        }
        if(closeStarted != -1) {
            float percent = (Time.time - closeStarted) / moveDuration;
            if(percent >= 1) {
                doorLeft.localPosition = Vector3.zero;
                doorRight.localPosition = Vector3.zero;
                closeStarted = -1;
            } else {
                doorLeft.localPosition = Interpolate.Ease(ease,
                    openLeft, -openLeft, percent, 1f);
                doorRight.localPosition = Interpolate.Ease(ease,
                    openRight, -openRight, percent, 1f);
            }
        }
    }

    private void OpenDoors() {
        openStarted = Time.time;
    }
}
