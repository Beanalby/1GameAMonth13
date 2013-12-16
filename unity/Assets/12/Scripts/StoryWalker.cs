using UnityEngine;
using System.Collections;

public class StoryWalker : MonoBehaviour {
    public Vector3 walkDir;
    public float timeBetweenWalks;

    private float moveSpeed = 4f;
    private Interpolate.Function moveEase = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic);
    private float travelDuration;
    private float travelStart = -1;
    private Vector3 travelDelta = Vector3.zero, travelBase = Vector3.zero;
    private Interpolate.Function travelEase = null;

    public void Start() {
        StartCoroutine(Walk());
    }
    public void Update() {
        HandleTravel();
    }

    /// <summary>
    /// occasionally walks in the specified direction
    /// </summary>
    /// <returns></returns>
    IEnumerator Walk() {
        while(true) {
            travelDelta = walkDir;
            travelBase = transform.position;
            travelStart = Time.time;
            travelDuration = 1 / moveSpeed;
            travelEase = moveEase;
            yield return new WaitForSeconds(timeBetweenWalks);
        }
    }

    private void HandleTravel() {
        if(travelStart != -1) {
            float percent = (Time.time - travelStart) / travelDuration;
            if(percent >= 1) {
                transform.position = travelBase + travelDelta;
                travelStart = -1;
                travelBase = Vector3.zero;
                travelDelta = Vector3.zero;
            } else {
                transform.position = Interpolate.Ease(travelEase,
                    travelBase, travelDelta, percent, 1);
            }
        }
    }
}
