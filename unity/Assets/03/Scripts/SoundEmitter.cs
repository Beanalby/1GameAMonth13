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
            // make sure it's going the speed they requested
            Debug.Log("Velocity=" + hitInfo.ball.rigidbody.velocity.magnitude.ToString(".00") + " (" + hitInfo.ball.rigidbody.velocity + ")");
            if(hitInfo.ball.rigidbody.velocity.magnitude <= requiredVelocity) {
                return;
            }
        }
        source.Play();
        //AudioSource.PlayClipAtPoint(ballHit, transform.position);
    }
}
