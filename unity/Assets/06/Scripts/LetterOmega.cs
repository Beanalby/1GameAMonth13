using UnityEngine;
using System.Collections;

public class LetterOmega : Letter {

    public Material disabedMaterial;

    private float spinAmount = .15f;
    private Quaternion baseRotation;

    public override void Start() {
        base.Start();
        baseRotation = transform.localRotation;
        invincible = true;
        //DebugSetHealth(10); // +++ die fast for testing
    }

    protected override void HandleDeath(Damage damage) {
        // just note the fact that we're dead instead of leaving
        invincible = true;
        isAlive = false;
        renderer.material = disabedMaterial;
        SendMessageUpwards("LetterDisabled");
    }

    public IEnumerator MakeVulnerable() {
        // remove our invulnerability, and give us a little random spin
        // to tip off the players
        invincible = false;
        Vector3 spin = new Vector3();
        spin.x = Random.Range(spinAmount/2, spinAmount);
        if(Random.Range(0, 2) > 0) {
            spin.x = -spin.x;
        }
        spin.y = Random.Range(spinAmount/2, spinAmount);
        if(Random.Range(0, 2) > 0) {
            spin.y = -spin.y;
        }
        rigidbody.angularVelocity = spin;
        yield return new WaitForSeconds(WaveDriver.WAVE_DURATION);
        RemoveVulnerability();
    }

    private void RemoveVulnerability() {
        invincible = true;
        rigidbody.angularVelocity = Vector3.zero;
        transform.localRotation = baseRotation;
    }
}
