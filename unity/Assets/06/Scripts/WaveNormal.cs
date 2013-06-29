using UnityEngine;
using System.Collections;

public class WaveNormal : Wave {
    public GameObject letterPrefab;

    private float invincibleGracePeriod = .5f;

    public void Start() {
        foreach(Transform t in transform) {
            t.gameObject.AddComponent<Letter>();
        }
        StartCoroutine(RemoveInvincibility());
        StartCoroutine(KillSelf());
    }
    void Update() {
        Vector3 pos = transform.position;
        pos.z -= speed * Time.deltaTime;
        transform.position = pos;
    }

    override public GameObject GetLetterPrefab(string text, int index) {
        return letterPrefab;
    }

    IEnumerator KillSelf() {
        yield return new WaitForSeconds(duration);
        Debug.Log("+++ " + name + " killing self!");
        foreach(Transform t in transform) {
            Destroy(t.gameObject);
        }
        Destroy(gameObject);
    }

    private IEnumerator RemoveInvincibility() {
        yield return new WaitForSeconds(invincibleGracePeriod);
        foreach(Transform t in transform) {
            t.GetComponent<Letter>().invincible = false;
        }
    }
}
