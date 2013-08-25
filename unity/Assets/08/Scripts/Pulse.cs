using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {
    public Color Color;
    private Color color;

    public float pulseRate;

    private Color dim;
    private Renderer rend;
    private Light pulseLight;
    private float lightMin, lightMax;

    public void Start() {
        pulseLight = transform.Find("pulseLight").GetComponent<Light>();
        pulseLight.color = color;
        lightMax = pulseLight.intensity;
        lightMin = lightMax / 2;
        SetColor(Color);

        if(rend == null) {
            rend = GetComponent<MeshRenderer>();
            if(rend == null) {
                rend = GetComponentInChildren<MeshRenderer>();
            }
        }
    }

    public void Update() {
        pulseLight.color = color;
        float percent = Time.time / pulseRate % 1;
        if(percent < .5f) {
            pulseLight.intensity = Mathf.Lerp(lightMax, lightMin, percent / .5f);
            rend.sharedMaterial.color = Color.Lerp(color, dim, percent / .5f);
        } else {
            pulseLight.intensity = Mathf.Lerp(lightMin, lightMax, (percent-.5f) / .5f);
            rend.sharedMaterial.color = Color.Lerp(dim, color, (percent-.5f) / .5f);
        }
    }
    public void SetColor(Color colorParam) {
        color = Color = colorParam;
        dim = color;
        dim /= 1.5f;
        dim.a = 1;
        if(pulseLight) {
            pulseLight.color = color;
        }
    }
    public void OnEnable() {
        if(pulseLight)
            pulseLight.enabled = true;
    }
    public void OnDisable() {
        if(pulseLight)
            pulseLight.enabled = false;
    }
}
