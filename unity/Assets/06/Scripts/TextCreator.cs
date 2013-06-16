using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TextCreator : MonoBehaviour {

    public GameObject TextMesh;

    Dictionary<char, GameObject> letters;

    public void Awake() {
        InitLetters();
    }

    private void InitLetters() {
        letters = new Dictionary<char, GameObject>();
        foreach(Transform t in TextMesh.transform) {
            letters[(char)int.Parse(t.name)] = t.gameObject;
        }
    }
    public GameObject[] GetText(Transform parent, string text) {
        if(letters == null) {
            InitLetters();
        }
        float step = .4f;
        float baseOffset = -step * text.Length / 2;
        GameObject[] ret = new GameObject[text.Length];
        for(int i=0;i<text.Length;i++) {
            if(text[i] == ' ') {
                continue;
            }
            ret[i] = (GameObject)Instantiate(letters[text[i]]);
            ret[i].transform.parent = parent;
            ret[i].transform.localPosition = new Vector3(baseOffset + step * i, 0, 0);
            ret[i].transform.rotation = Quaternion.Euler(0, 180,0);
            ret[i].name = i.ToString() + "-" + text[i];
        }
        return ret;
    }
}
