using UnityEngine;
using System.Collections;

public class HealthGlobe : MonoBehaviour {
    public Texture globe;

    private Transform target;
    private int amount = 10;
    private float speed = 10f;
    private Vector3 startPos;
    private Vector3 dir;
    private float totalDist;

    private float startTime;

    public void Start() {
        target = GameObject.Find("HealthGlobeTarget").transform;
        dir = (target.position - transform.position);
        totalDist = dir.sqrMagnitude;
        dir.Normalize();
        startPos = transform.position;
    }

    public void Update() {
        transform.Translate(dir * speed * Time.deltaTime);
        speed += Time.deltaTime * 10;
        // check if we're there
        if((transform.position - startPos).sqrMagnitude > totalDist) {
            if(Ship.ship != null) {
                Ship.ship.AddHealth(amount);
            }
            Destroy(gameObject);
        }
    }
    public void OnGUI() {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos.y = Screen.height - pos.y;
        GUI.DrawTexture(new Rect(pos.x, pos.y, globe.width, globe.height), globe);
    }
}
