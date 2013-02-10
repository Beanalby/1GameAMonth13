using UnityEngine;

public class ProgressCircle : MonoBehaviour {
    private float percent = 0;
    public Color FullColor = Color.green;
    public Color EmptyColor = Color.red;
    public float Percent {
        // don't let it go to full 100%, as that will cause all the solid
        // black that should stay transparent to show.  Max out at 99%,
        // which shows the full circle.
        set { percent = Mathf.Min(.99f, value); }
        get { return percent; }
    }

    void Start() {
        UpdateCircle();
    }
	void Update() {
        UpdateCircle();
	}

    public void UpdateCircle() {
        renderer.materials[1].SetFloat("_Cutoff", 1-percent);
        renderer.materials[1].color = Color.Lerp(EmptyColor, FullColor, percent);
    }
}
