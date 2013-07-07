using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OmegaDriver : MonoBehaviour {

    protected float showStart = -1;
    protected float hideStart = -1;

    private Vector3 hiddenPos;
    private Vector3 livePos;
    private float totalDist;
    private float baseOffset = 9;

    private float slop;
    private float duration;

    public virtual void Start() {
        livePos = transform.position;
        hiddenPos = transform.position;
        hiddenPos.z += baseOffset;
        hiddenPos.y += baseOffset;
        transform.position = hiddenPos;
        totalDist = (livePos - hiddenPos).magnitude;
        // we normally show Omega for almost the full WAVE_DURATION, starting
        // to pull up JUST before the next text appears.  If we're doing
        // omega only, start much earlier so it can go through the normal
        // full hide/show cycle.
        if(WaveDriver.DebugOmegaOnly) {
            duration = WaveDriver.WAVE_DURATION - .5f;
        } else {
            duration = WaveDriver.WAVE_DURATION - .1f;
        }
    }

    public void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        if(showStart != -1) {
            if(Time.time > showStart + duration) {
                showStart = -1;
                hideStart = Time.time;
            }
            // slow speed as it gets closer
            Vector3 delta = (livePos - transform.position) * .2f;
            transform.position += delta;
        }
        if(hideStart != -1) {
            // go faster the further away we get
            float percent = (transform.position - livePos).magnitude / totalDist;
            if(percent >= .95f) {
                transform.position = hiddenPos;
                hideStart = -1;
                foreach(Transform t in transform) {
                    t.SendMessage("Hidden", SendMessageOptions.DontRequireReceiver);
                }
            } else {
                float scale = .02f + percent;
                Vector3 delta = (hiddenPos - transform.position) * scale;
                transform.position += delta;
            }
        }
    }

    public void LetterDisabled() {
        foreach(Transform t in transform) {
            if(t.GetComponent<LetterOmega>().isAlive) {
                return;
            }
        }
        GameObject.Find("WaveDriver").SendMessage("OmegaDead");
        foreach(Transform t in transform) {
            t.SendMessage("StartShaking");
        }
    }

    public virtual void ShowOmega(int index) {
        Debug.Log(name + " Showing omega wave #" + index);
        hideStart = -1;
        showStart = Time.time;
        foreach(Transform t in transform) {
            t.SendMessage("ShowOmega", SendMessageOptions.DontRequireReceiver);
        }
    }
}
