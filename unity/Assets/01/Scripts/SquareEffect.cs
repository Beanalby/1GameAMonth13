using UnityEngine;
using System.Collections;

public class SquareEffect : MonoBehaviour {

    public bool isCorrupted;

    public Material cleanMaterial;
    public Material corruptedMaterial;

    private float effectStart;
    private float effectDuration = 1f;
    private float upPercent = .1f;

	// Use this for initialization
	void Start () {
        if(isCorrupted) {
            renderer.material = corruptedMaterial;
        } else {
            renderer.material = cleanMaterial;
        }
        effectStart = Time.time;
	}
	 
	// Update is called once per frame
	void Update () {
        if(effectStart + effectDuration < Time.time) {
            Destroy(gameObject);
            return;
        }
        float percent = (Time.time - effectStart) / effectDuration;
        float from, to, total, currentProgress;
        if(percent < upPercent) {
            from = 0;
            to = 1;
            total = upPercent;
            currentProgress = percent;
        } else {
            from = 1;
            to = 0;
            total = effectDuration - upPercent;
            currentProgress = percent - upPercent;
        }
        float zScale = Mathf.Lerp(from, to, currentProgress / total);
        transform.localScale = new Vector3(1, 1, zScale);
	}
}
