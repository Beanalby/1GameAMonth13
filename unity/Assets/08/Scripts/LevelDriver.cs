using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelDriver : MonoBehaviour {

    static public Vector3 CAMERA_OFFSET_START = new Vector3(0, 20, 20);
    static public Vector3 CAMERA_OFFSET_CLOSE = new Vector3(0, 3.5f, -1.5f);
    static public Vector3 CAMERA_OFFSET_FAR = new Vector3(0, 7, -3);

    public GUISkin skin;
    public Button zoomButton;

    private float timeStart=-1f, timeLevel=-1f;
    private GUIStyle completeStyle, timeStyle;
    private float fadeAlphaStart, fadeAlphaEnd;
    private float moveStart = -1f;
    private Vector3 moveStartPos, moveDelta;
    private float moveDuration = 3f;
    private Interpolate.Function moveEase;

    private float fadeStart = -1f;
    private float fadeCurrent = 1;
    private float fadeDelay = 2f;
    private float fadeDuration = 1f;

    private float zoomStart = -1f;
    private Vector3 zoomStartPos;
    private float zoomDuration = 1f;
    private Interpolate.Function zoomEase =
        Interpolate.Ease(Interpolate.EaseType.EaseOutExpo);
    private PlayerDriver player;
    private GameObject playerLights, roomLights;
    private List<Light> playerGroundLights;
    private bool isExiting = false;
    Camera cam;

    public void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerDriver>();
        playerLights = player.transform.Find("PlayerLights").gameObject;
        roomLights = GameObject.Find("RoomLights");
        cam = Camera.main;
        cam.transform.position = CAMERA_OFFSET_FAR;
        zoomButton.buttonListeners += ButtonPressed;

        timeStyle = new GUIStyle(skin.box);
        timeStyle.normal.textColor = Color.red;
        completeStyle = new GUIStyle(skin.box);

        playerLights.SetActive(false);
        roomLights.SetActive(true);
        InitMovement();
    }

    public void Update() {
        HandleFade();
        HandleMovement();
        HandleZoom();
    }

    public void OnGUI() {
        GUI.skin = skin;
        DrawTime();
        if(isExiting) {
            DrawExitMessage();
        }
    }

    private void DrawTime() {
        if(timeStart == -1) {
            return;
        }
        int timeDisplay;
        if(timeLevel != -1) {
            timeDisplay = (int)(timeLevel * 100);
        } else {
            timeDisplay = (int)((Time.time - timeStart) * 100);
        }
        //timeStyle.normal.textColor = new Color(1, 0, 0, fadeCurrent);
        GUI.Box(new Rect(Screen.width/2 - 100, 0, 200, 50),
            timeDisplay.ToString(), timeStyle);
    }
    public void DrawExitMessage() {
        completeStyle.normal.textColor = new Color(1, 1, 1, fadeCurrent);
        GUI.Box(new Rect(0, 50, Screen.width, 100), "Level\nComplete",
            completeStyle);
    }
    public void ExitActivated() {
        moveStart = Time.time;
        moveStartPos = cam.transform.position;
        moveDelta = CAMERA_OFFSET_START;
        moveEase = Interpolate.Ease(Interpolate.EaseType.EaseInExpo);
        player.EnableInput = false;
        timeLevel = Time.time - timeStart;
        isExiting = true;
        StartCoroutine(FadeText());
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
            if(isExiting) {
                GameDriver8.instance.LevelFinished(timeLevel);
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
    private void HandleFade() {
        if(fadeStart == -1) {
            return;
        }
        float percent = (Time.time - fadeStart) / fadeDuration;
        if(percent >= 1) {
            fadeCurrent = 0;
            fadeStart = -1;
        } else {
            fadeCurrent = 1 - percent;
        }
    }
    public IEnumerator FadeText() {
        yield return new WaitForSeconds(fadeDelay);
        fadeCurrent = 1;
        fadeStart = Time.time;
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
        timeStart = Time.time;
    }
}
