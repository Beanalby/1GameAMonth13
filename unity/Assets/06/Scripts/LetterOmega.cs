using UnityEngine;
using System.Collections;

public class LetterOmega : Letter {

    public Material disabedMaterial;

    private float spinAmount = .15f;
    private float shakeAmount = .1f;

    private Quaternion baseRotation;

    private Vector3 basePos;
    private bool isShaking = false;
    public GameObject shakeEffectPrefab;
    private GameObject shakeEffectInstance;
    
    public override void Start() {
        base.Start();
        baseRotation = transform.localRotation;
        invincible = true;
        scoreValue = 100;
        if(WaveDriver.DebugOmegaEasy) {
            DebugSetHealth(10); // +++ die fast for testing
        }
    }

    public virtual void Update() {
        ShakeLetter();
    }

    public void Hidden() {
        if(isShaking) {
            StopShaking();
        }
    }

    protected override void HandleDeath(Damage damage) {
        if(Ship.ship != null) {
            Ship.ship.AddScore(scoreValue);
        }
        // just note the fact that we're dead instead of leaving
        invincible = true;
        isAlive = false;
        StartShaking();
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

    public void StartShaking() {
        if(!isShaking) {
            isShaking = true;
            basePos = transform.localPosition;
            if(shakeEffectPrefab) {
                shakeEffectInstance = Instantiate(shakeEffectPrefab) as GameObject;
                shakeEffectInstance.transform.parent = transform;
                shakeEffectInstance.transform.localRotation = Quaternion.identity;
                shakeEffectInstance.transform.localPosition = new Vector3(0, 3.5f, 0);
            }
        }
    }
    private void ShakeLetter() {
        if(isShaking) {
            // add some random positions to the base position
            transform.localPosition = new Vector3(
                basePos.x + Random.Range(-shakeAmount, shakeAmount),
                basePos.y + Random.Range(-shakeAmount, shakeAmount),
                basePos.z + Random.Range(-shakeAmount, shakeAmount));
        }
    }
    private void StopShaking() {
        isShaking = false;
        transform.localPosition = basePos;
        if(shakeEffectInstance) {
            shakeEffectInstance.SendMessage("StopShaking");
        }
    }
}
