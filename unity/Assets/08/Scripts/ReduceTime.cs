using UnityEngine;
using System.Collections;

public class ReduceTime : MonoBehaviour {
    public GUISkin skin;

    private float fadeStart = -1;
    private float fadeDuration = 1f;
    private GUIStyle levelStyle, totalStyle;

    private Color colorStart, colorEnd, colorCurrent;
    private GameDriver8 driver;
    private bool loadOnFadeFinish;

    public void Start() {
        driver = GameDriver8.instance;
        levelStyle = new GUIStyle(skin.box);
        levelStyle.normal.textColor = new Color(1, 0, 0, 1);
        totalStyle = new GUIStyle(levelStyle);
        totalStyle.fontSize *= 2;
        FadeIn();
    }
    public void OnGUI() {
        // level amount only fades on fadeOut
        if(loadOnFadeFinish)
            levelStyle.normal.textColor = colorCurrent;
        totalStyle.normal.textColor = colorCurrent;
        DrawTime();
    }
    public void Update() {
        HandleFade();
        SubtractTime();
    }

    private void DrawTime() {
        int displayTime = (int)(driver.LastLevelTime * 100);
        GUI.Box(new Rect(0, 0, Screen.width, 50),
            displayTime.ToString(), levelStyle);
        GUI.Box(new Rect(0, 50, Screen.width, 100),
            driver.TotalTime.ToString(".00"), totalStyle);
    }
    private void HandleFade() {
        if(fadeStart == -1) {
            return;
        }
        float percent = (Time.time - fadeStart) / fadeDuration;
        if(percent >= 1) {
            colorCurrent = colorEnd;
            fadeStart = -1;
            if(loadOnFadeFinish) {
                driver.ReduceFinished();
            }
        } else {
            colorCurrent = Color.Lerp(colorStart, colorEnd, percent);
        }
    }
    private void FadeIn() {
        colorStart = new Color(1, 0, 0, 0);
        colorEnd = new Color(1, 0, 0, 1);
        fadeStart = Time.time;
    }
    private IEnumerator FadeOut() {
        yield return new WaitForSeconds(1f);
        colorStart = new Color(1, 0, 0, 1);
        colorEnd = new Color(1, 0, 0, 0);
        fadeStart = Time.time;
        loadOnFadeFinish = true;
    }
    private void SubtractTime() {
        if(fadeStart != -1) {
            return;
        }
        if(driver.LastLevelTime == 0) {
            return;
        }
        driver.DecrementTime(Time.deltaTime * 4);
        if(driver.LastLevelTime <= 0) {
            StartCoroutine(FadeOut());
        }
    }
}
