using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class OmegaShakeEffect : MonoBehaviour {
    public void StopShaking() {
        GetComponent<ParticleSystem>().Stop();
        StartCoroutine(KillSelf());
    }
    private IEnumerator KillSelf() {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration);
        Destroy(gameObject);
    }
}
