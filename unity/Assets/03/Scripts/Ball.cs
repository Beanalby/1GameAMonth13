using UnityEngine;
using System.Collections;

public class BallHitInfo {
    public Collision info;
    public GameObject ball;
    public BallHitInfo(Collision info, GameObject ball) {
        this.info = info;
        this.ball = ball;
    }
}

public class Ball : MonoBehaviour {

    public float duration = 4f;

    private Transform shadow;
    private Vector3 offset;
    private float timeStart;

    // keeps the shadow at its original position, pointed at us
    void Start() {
        shadow = transform.Find("BallShadow");
        offset = shadow.localPosition;
        shadow.parent = null;
        timeStart = Time.time;
    }
    void OnDestroy() {
        // shadow may already be destroyed on scene end
        if(shadow) {
            Destroy(shadow.gameObject);
        }
    }
    void Update () {
        shadow.position = transform.position + offset;
        shadow.rotation = Quaternion.LookRotation(transform.position - shadow.position);
        if(timeStart + duration < Time.time) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision info) {
        BallHitInfo hitInfo = new BallHitInfo(info, gameObject);
        info.gameObject.SendMessage("BallHit", hitInfo, SendMessageOptions.DontRequireReceiver);
    }
    void OnTriggerEnter(Collider other) {
        BallHitInfo hitInfo = new BallHitInfo(null, gameObject);
        other.gameObject.SendMessage("BallHit", hitInfo, SendMessageOptions.DontRequireReceiver);
    }
}
