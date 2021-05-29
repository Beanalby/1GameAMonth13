using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour {

    public float requiredVelocity = 0;

    private AudioSource source;

    public void Start() {
        source = GetComponent<AudioSource>();
    }
    public void BallHit(BallHitInfo hitInfo) {
        if(requiredVelocity != 0) {
            if(hitInfo.ball.GetComponent<Rigidbody>().velocity.magnitude <= requiredVelocity) {
                return;
            }
        }
        source.Play();
    }
}
