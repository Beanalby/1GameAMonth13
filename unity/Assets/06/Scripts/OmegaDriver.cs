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

    public virtual void Start() {
        livePos = transform.position;
        hiddenPos = transform.position;
        hiddenPos.z += baseOffset;
        hiddenPos.y += baseOffset;
        transform.position = hiddenPos;
        totalDist = (livePos - hiddenPos).magnitude;
    }

    public void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        if(showStart != -1) {
            if(Time.time > showStart + WaveDriver.WAVE_DURATION-.1f) {
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
