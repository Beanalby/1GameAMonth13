using UnityEngine;
public class ProgressCircle : MonoBehaviour {
    public float percent = 1;
    void Start() {
        renderer.materials[1].SetFloat("_Cutoff", 1);
    }
	void Update () {
        renderer.materials[1].SetFloat("_Cutoff", 1-percent);
	}
}
