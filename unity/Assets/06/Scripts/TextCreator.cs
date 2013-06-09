using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TextCreator : MonoBehaviour {

    public GameObject TextMesh;

    Dictionary<char, GameObject> letters;

    public void Awake() {
        Debug.Log("Starting TextCreator Awake");
        letters = new Dictionary<char, GameObject>();
        foreach(Transform t in TextMesh.transform) {
            letters[(char)int.Parse(t.name)] = t.gameObject;
            Debug.Log("Added in [" + (char)int.Parse(t.name)
                + "] (" + int.Parse(t.name)+")");
        }
        Debug.Log("Finished awake, populated with " + letters.Keys.Count + " letters");
    }
    public GameObject[] GetText(Transform parent, string text) {
        float step = .5f;
        float baseOffset = -step * text.Length / 2;
        GameObject[] ret = new GameObject[text.Length];
        Debug.Log("pulling from " + letters.Keys.Count + " letters");
        for(int i=0;i<text.Length;i++) {
            if(text[i] == ' ') {
                continue;
            }
            Debug.Log("Pulling [" + text[i]);
            GameObject obj = letters[text[i]];
            Debug.Log("Pulled " + obj + " for " + text[i]);
            ret[i] = (GameObject)Instantiate(letters[text[i]]);
            ret[i].transform.parent = parent;
            ret[i].transform.localPosition = new Vector3(baseOffset + step * i, 0, 0);
            ret[i].transform.rotation = Quaternion.Euler(0, 180,0);
        }
        return ret;
    }
}
