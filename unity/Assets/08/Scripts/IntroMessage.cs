using UnityEngine;
using System.Collections;

public class IntroMessage : MonoBehaviour {

    public GUISkin skin;

    public string message;
    public string retryText;
    public bool hideWhenDone = true;

    private float amount = 0;
    private float displayDuration = 3f;
    private float introStart = -1, introDone = -1f;
    private int charsPerSecond = 20;
    private bool isFastRate = false;

    public void Start() {
        introStart = Time.time;
        audio.Play();
    }
    public void Update() {
        isFastRate = Input.GetButton("Fire1");
        float delta = (Time.deltaTime) * charsPerSecond;
        if(isFastRate) {
            delta *= 5;
        }
        // 6C617374686F7065
        amount = Mathf.Min(message.Length, amount + delta);
    }
    public void OnGUI() {
        GUI.skin = skin;

        if(hideWhenDone && introDone != -1 && (Time.time - introDone) > displayDuration) {
            return;
        }
        if(introStart == -1) {
            return;
        }
        string smallMsg = message.Substring(0, (int)amount);
        GUI.Box(new Rect(0, Screen.height - 100, Screen.width, 100),
            smallMsg);
        if(amount == message.Length) {
            if(introDone == -1) {
                introDone = Time.time;
                audio.Stop();
            }
        }
        if(introDone != -1  && retryText != "") {
            int width = 200, height = 50;
            Rect retryRect = new Rect(Screen.width / 2 - width / 2,
                Screen.height / 2 - height / 2, width, height);
            if(GUI.Button(retryRect, retryText)) {
                GameDriver8.instance.Restart();
            }
        }
    }
}
