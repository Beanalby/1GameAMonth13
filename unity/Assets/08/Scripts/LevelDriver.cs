using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelDriver : MonoBehaviour {

    static public Vector3 CAMERA_OFFSET_START = new Vector3(0, 20, 20);
    static public Vector3 CAMERA_OFFSET_CLOSE = new Vector3(0, 3.5f, -1.5f);
    static public Vector3 CAMERA_OFFSET_FAR = new Vector3(0, 7, -3);

    public Button zoomButton;

    private float moveStart = -1f;
    private Vector3 moveStartPos, moveDelta;
    private float moveDuration = 3f;
    private bool finishWhenDoneMoving = false;
    private Interpolate.Function moveEase;

    private float zoomStart = -1f;
    private Vector3 zoomStartPos;
    private float zoomDuration = 1f;
    private Interpolate.Function zoomEase =
        Interpolate.Ease(Interpolate.EaseType.EaseOutExpo);
    private PlayerDriver player;
    private GameObject playerLights, roomLights;
    private List<Light> playerGroundLights;
    Camera cam;

    public void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerDriver>();
        playerLights = player.transform.Find("PlayerLights").gameObject;
        roomLights = GameObject.Find("RoomLights");
        cam = Camera.main;
        cam.transform.position = CAMERA_OFFSET_FAR;
        zoomButton.buttonListeners += ButtonPressed;

        playerLights.SetActive(false);
        roomLights.SetActive(true);
        InitMovement();
    }

    public void Update() {
        HandleMovement();
        HandleZoom();
    }

    public void ExitActivated() {
        moveStart = Time.time;
        moveStartPos = cam.transform.position;
        moveDelta = CAMERA_OFFSET_START;
        moveEase = Interpolate.Ease(Interpolate.EaseType.EaseInExpo);
        finishWhenDoneMoving = true;
        player.EnableInput = false;
    }

    private void HandleMovement() {
        if(moveStart == -1) {
            return;
        }
        if(zoomStart != -1) {
            // zoom takes precedence over movement, stop this
            moveStart = -1;
            return;
        }
        float percent = (Time.time - moveStart) / moveDuration;
        if(percent >= 1) {
            cam.transform.position = moveStartPos + moveDelta;
            moveStart = -1;
            if(finishWhenDoneMoving) {
                GameDriver8.instance.LevelFinished();
            }
        } else {
            cam.transform.position = Interpolate.Ease(moveEase,
                moveStartPos, moveDelta, percent, 1);
        }
    }
    private void HandleZoom() {
        if(zoomStart == -1) {
            return;
        }
        float percent = (Time.time - zoomStart) / zoomDuration;
        if(percent >= 1) {
            cam.transform.parent = player.transform;
            cam.transform.localPosition = CAMERA_OFFSET_CLOSE;
            zoomStart = -1;
            return;
        } else {
            cam.transform.position = Interpolate.Ease(zoomEase, zoomStartPos,
                (player.transform.position + CAMERA_OFFSET_CLOSE) - zoomStartPos,
                percent, 1);
        }
    }
    private void InitMovement() {
        moveStart = Time.time;
        moveStartPos = CAMERA_OFFSET_FAR + CAMERA_OFFSET_START;
        moveDelta = -CAMERA_OFFSET_START;
        moveEase = Interpolate.Ease(Interpolate.EaseType.EaseOutExpo);
    }

    public void ButtonPressed(bool isPressed) {
        zoomStart = Time.time;
        zoomStartPos = cam.transform.position;
        playerLights.SetActive(true);
        roomLights.SetActive(false);
    }
}
