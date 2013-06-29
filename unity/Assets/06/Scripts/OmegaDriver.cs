using UnityEngine;
using System.Collections;

public class OmegaDriver : MonoBehaviour {

    private float showDuration = 3.428f;
    private float showStart = -1;
    private float hideStart = -1;

    private Vector3 hiddenPos;
    private Vector3 livePos;
    private float totalDist;
    private float baseOffset = 9;

    private float slop;

    public void Start() {
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
            if(Time.time > showStart + showDuration) {
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

    public void ShowOmega(int index) {
        hideStart = -1;
        showStart = Time.time;
    }
}
