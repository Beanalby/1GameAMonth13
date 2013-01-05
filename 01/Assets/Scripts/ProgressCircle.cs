using UnityEngine;
public class ProgressCircle : MonoBehaviour {
    public float percent = 0;
	void Update () {
        renderer.materials[1].SetFloat("_Cutoff", percent);
	}
}
