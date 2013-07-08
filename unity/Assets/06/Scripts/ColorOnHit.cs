using UnityEngine;
using System.Collections;

public class ColorOnHit : MonoBehaviour {

    public MeshRenderer mesh = null;
    public Color hitColor = Color.white;
    public float hitDuration = .1f;

    private Material mat;
    private Color baseColor;
    private float hitTime = -1f;
    private Letter letter;

    public void Start() {
        if(mesh == null) {
            mesh = GetComponent<MeshRenderer>();
        }
        if(mesh == null) {
            Debug.LogError(name + ": no mesh found!");
            Debug.Break();
        }
        mat = mesh.material;
        baseColor = mesh.material.color;
        letter = GetComponent<Letter>();
    }
    public void Update() {
        if(hitTime != -1) {
            // hack: if we're a letter that's died, stop adjusting the color
            //if(letter != null && !letter.isAlive) {
            //    hitTime = -1;
            //    return;
            //}                
            float percent = (Time.time - hitTime) / hitDuration;
            mesh.material.color = Color.Lerp(hitColor, baseColor, percent);
            if(percent >= 1) {
                hitTime = -1;
            }
        }
    }

    protected void HandleDeath(Damage damage) {
        mat.color = baseColor;
        hitTime = -1;
    }

    public void TakeDamage() {
        // hack to check if we're an invincible invincible
        if(letter != null && letter.invincible) {
            return;
        }
        hitTime = Time.time;
    }
}
