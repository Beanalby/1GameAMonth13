using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Wave : MonoBehaviour {
    [HideInInspector]
    public string text;
    [HideInInspector]
    public TextCreator tc;

    private float invincibleGracePeriod = .5f;
    protected float speed = 1f;
    protected float duration = 4f;

    private GameObject[] letters;

    public abstract GameObject GetLetterPrefab(string text, int index);

    public virtual void Start() {
        for(int i = 0; i < text.Length; i++) {
            tc.MakeLetter(this, text, i);
        }
        StartCoroutine(RemoveInvincibility());
        StartCoroutine(KillSelf());
    }

    public virtual void Update() {
        Vector3 pos = transform.position;
        pos.z -= speed * Time.deltaTime;
        transform.position = pos;
    }

    private IEnumerator RemoveInvincibility() {
        yield return new WaitForSeconds(invincibleGracePeriod);
        foreach(Transform t in transform) {
            t.GetComponent<Letter>().invincible = false;
        }
    }

    IEnumerator KillSelf() {
        yield return new WaitForSeconds(duration);
        // make a copy of the children array; WaveEnd might remove us as
        // their parent, which screws up the iteration
        Transform[]children = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++) {
            children[i] = transform.GetChild(i);
        }

        foreach(Transform t in children) {
            t.SendMessage("WaveEnd", SendMessageOptions.DontRequireReceiver);
        }
        // any letters that didn't detach themselves in WaveEnd die with us
        Destroy(gameObject);
    }
}