using UnityEngine;
using System.Collections;

public class ColorOnHit : MonoBehaviour {

    public MeshRenderer mesh;
    public Color hitColor;

    private Color baseColor;
    private float hitTime = -1f;
    private float hitDuration = 1f;

    public void Start() {
        baseColor = mesh.material.color;
    }
    public void Update() {
        if(hitTime != -1) {
            float percent = (Time.time - hitTime) / hitDuration;
            Debug.Log("Setting hit color to " + percent);
            mesh.material.color = Color.Lerp(hitColor, baseColor, percent);
            if(percent >= 1) {
                hitTime = -1;
            }
        }
    }
    public void TakeDamage() {
        hitTime = Time.time;
    }
}
