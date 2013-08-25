using UnityEngine;
using System.Collections;

public class IntroMessage : MonoBehaviour {

    public GUISkin skin;

    public string message;
    private float amount = 0;
    private float displayDuration = 3f;
    private float introStart = -1, introDone = -1f;
    private int charsPerSecond = 20;
    private bool isFastRate = false;

    public void Start() {
        introStart = Time.time;
    }
    public void Update() {
        isFastRate = Input.GetButton("Fire1");
        float delta = (Time.deltaTime) * charsPerSecond;
        if(isFastRate) {
            delta *= 2;
        }
        // 6C617374686F7065
        amount = Mathf.Min(message.Length, amount + delta);
    }
    public void OnGUI() {
        GUI.skin = skin;

        if(introDone != -1 && (Time.time - introDone) > displayDuration) {
            return;
        }
        if(introStart == -1) {
            return;
        }
        string smallMsg = message.Substring(0, (int)amount);
        GUI.Box(new Rect(128, Screen.height - 100, Screen.width - 128, 100),
            smallMsg);
        if(amount == message.Length && introDone == -1) {
            introDone = Time.time;
        }
    }
}
