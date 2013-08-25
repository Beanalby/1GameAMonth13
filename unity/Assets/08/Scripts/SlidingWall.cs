using UnityEngine;
using System.Collections;

public class SlidingWall : MonoBehaviour {
    public Switch button;

    public Vector3 delta;
    private float slideStarted = -1;
    private Vector3 slideStartPos;
    private float slideDuration = 1;
    Interpolate.Function ease = Interpolate.Ease(Interpolate.EaseType.EaseInOutCubic);

    public void Start() {
        button.switchListeners += ButtonPressed;
    }
    public void Update() {
        HandleSlide();
    }

    private void ButtonPressed(bool isPressed) {
        if(isPressed) {
            slideStarted = Time.time;
            slideStartPos = transform.position;
        }
    }
    private void HandleSlide() {
        if(slideStarted == -1) {
            return;
        }
        float percent = (Time.time - slideStarted) / slideDuration;
        if(percent >= 1) {
            transform.position = slideStartPos + delta;
            slideStarted = -1;
        } else {
            transform.position = Interpolate.Ease(ease, slideStartPos,
                delta, percent, 1);
        }
    }
}
