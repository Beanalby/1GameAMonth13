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

    Transform shadow;
    Vector3 offset;

    // keeps the shadow at its original position, pointed at us
    void Start() {
        shadow = transform.Find("BallShadow");
        offset = shadow.localPosition;
        shadow.parent = null;
    }
    void OnDestroy() {
        Destroy(shadow);
    }
    void Update () {
        shadow.position = transform.position + offset;
        shadow.rotation = Quaternion.LookRotation(transform.position - shadow.position);
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
