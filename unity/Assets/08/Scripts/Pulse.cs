using UnityEngine;
using System.Collections;

public delegate void PulseCustom(Color color);

public class Pulse : MonoBehaviour {
    public Color Color;
    private Color color;

    public float pulseRate;
    public PulseCustom custom;

    private Color dim;
    private Renderer rend;
    private Light pulseLight;
    private float lightMin, lightMax;

    public void Start() {
        Transform tmp = transform.Find("pulseLight");
        if(tmp) {
            pulseLight = tmp.GetComponent<Light>();
            pulseLight.color = color;
            lightMax = pulseLight.intensity;
            lightMin = lightMax / 2;
        }
        SetColor(Color);

        if(rend == null) {
            rend = GetComponent<MeshRenderer>();
            if(rend == null) {
                rend = GetComponentInChildren<MeshRenderer>();
            }
        }
    }

    public void Update() {
        float percent = Time.time / pulseRate % 1;
        if(percent < .5f) {
            Color newColor = Color.Lerp(color, dim, percent / .5f);
            if(pulseLight) {
                pulseLight.intensity = Mathf.Lerp(lightMax, lightMin, percent / .5f);
            }
            if(rend) {
                rend.sharedMaterial.color = newColor;
            }
            if(custom != null) {
                custom(newColor);
            }
        } else {
            Color newColor = Color.Lerp(dim, color, (percent - .5f) / .5f);
            if(pulseLight) {
                pulseLight.intensity = Mathf.Lerp(lightMin, lightMax, (percent - .5f) / .5f);
            }
            if(rend) {
                rend.sharedMaterial.color = newColor;
            }
            if(custom != null) {
                custom(newColor);
            }
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
