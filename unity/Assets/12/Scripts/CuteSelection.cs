using UnityEngine;
using System.Collections;

public class CuteSelection : MonoBehaviour {

    private Material mat;
    private Color color;
    private float cycleSpeed = 10;

    public void Start() {
        mat = GetComponentInChildren<Renderer>().material;
        color = mat.color;
    }

    public void Update() {
        color.a = Mathf.Sin(Time.time * cycleSpeed) / 4 + .75f;
        mat.color = color;
    }
}
