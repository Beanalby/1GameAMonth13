using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplatEffect : MonoBehaviour {
    public FruitType Type;
    public float fadeDelay=3, fadeDuration=1;
    private float start;

    public static Dictionary<FruitType, Color> type2color = new Dictionary<FruitType,Color>
    { 
        { FruitType.Apple, Color.red },
        { FruitType.Lime, Color.green },
        { FruitType.Orange, new Color(1, .5f, 0) }
    };

    private Material splatMat, particleMat;
    Color color;
    public void Start() {
        start = Time.time;

        // tweak the colors based on the FruitType
        color = type2color[Type];
        Renderer r = GetComponentInChildren<MeshRenderer>();
        if(r == null) {
            r = GetComponentInChildren<SkinnedMeshRenderer>();
        }
        splatMat = r.material;
        splatMat.color = color;
        splatMat.SetColor("_Emission", color);
        particleMat = GetComponentInChildren<ParticleSystemRenderer>().material;
        particleMat.SetColor("_TintColor", color);
        
        // rotate the splatter
        Transform t = transform.FindChild("splat");
        if(t != null)
            t.rotation = Quaternion.Euler(90, 0, Random.Range(0, 360));
    }

    public void Update() {
        HandleFade();
    }

    private void HandleFade() {
        if(Time.time < start + fadeDelay) {
            return;
        }
        float percent = (Time.time - (start + fadeDelay)) / fadeDuration;
        if(percent > 1) {
            Destroy(gameObject);
        } else {
            color.a = 1 - percent;
            splatMat.color = color;
            splatMat.SetColor("_Emission", color);
            particleMat.SetColor("_TintColor", color);
        }
    }
}
