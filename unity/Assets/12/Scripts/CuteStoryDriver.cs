using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CuteStoryDriver : MonoBehaviour {
    public GUISkin skin;
    public string message;
    public string nextStage;
    public Vector3 cameraStart, cameraFinish;
    public float duration;
    private float start;

    Interpolate.Function ease = Interpolate.Ease(Interpolate.EaseType.Linear);

    private Vector3 cameraDelta;

    public void Start() {
        cameraDelta = cameraFinish - cameraStart;
        start = Time.time;
        Debug.Log("Playing music via " + CuteMusicPlayer.Instance);
        CuteMusicPlayer.Instance.PlayMusic("titleMusic");
    }

    public void Update() {
        float percent = (Time.time - start) / duration;
        if(percent >= 1) {
            Application.LoadLevel(nextStage);
        } else {
            transform.position = Interpolate.Ease(ease, cameraStart,
                cameraDelta, percent, 1);
        }
    }
    public void OnGUI() {
        Rect messageRect = new Rect(0, 70, Screen.width, 70);
        GUI.Label(messageRect, message, skin.customStyles[0]);
    }
}
