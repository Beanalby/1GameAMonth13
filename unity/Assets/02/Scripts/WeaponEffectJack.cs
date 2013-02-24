using UnityEngine;
using System.Collections;
using SysDebug = System.Diagnostics.Debug;

[RequireComponent(typeof(LineRenderer))]
public class WeaponEffectJack : WeaponEffectBase {
    public float duration = .5f;
    public float scrollSpeed = 10f;

    private LineRenderer line;
    Vector2 offset;
    private float started;

    public void Start() {
        if (target == null || launcher == null) {
            Destroy(gameObject);
            return;
        }
        line = GetComponent<LineRenderer>();
        offset = line.material.GetTextureOffset("_MainTex");
        line.SetPosition(0, launcher.transform.position);
        line.SetPosition(1, target.transform.position);
        started = Time.time;
    }
    public void Update() {
        if (Time.time > started + duration) {
            Destroy(gameObject);
            return;
        }
        offset.x -= scrollSpeed * Time.deltaTime;
        line.material.SetTextureOffset("_MainTex", offset);
    }
}
