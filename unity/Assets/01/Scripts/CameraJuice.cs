using UnityEngine;
using System.Collections;

public class CameraJuice : MonoBehaviour {

    private Vector3 basePos;

    private Vector3 poundEffectFrom, poundEffectTo;
    private float poundEffectAmount = .1f;
    private float poundEffectDuration = .1f;
    private float poundEffectStart = -1;

    void Start() {
        basePos = transform.position;
    }
    void Update() {
        HandlePoundEffect();
    }

    void DidPound() {
        poundEffectStart = Time.time;
        poundEffectFrom = basePos;
        poundEffectTo = poundEffectFrom;
        poundEffectTo.y += poundEffectAmount;
    }
    void HandlePoundEffect() {
        if(poundEffectStart == -1)
            return;
        if(poundEffectStart + poundEffectDuration < Time.time) {
            poundEffectStart = -1;
            transform.position = basePos;
            return;
        }
        float percent = (Time.time - poundEffectStart) / poundEffectDuration;
        Vector3 pos;
        if(percent < .5f)
            pos = Vector3.Lerp(poundEffectFrom, poundEffectTo, percent * 2f);
        else
            pos = Vector3.Lerp(poundEffectTo, poundEffectFrom, (percent-.5f) * 2f);
        transform.position = pos;
    }
}
