using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TextCreator : MonoBehaviour {

    private static float LETTER_SPACING = .4f;
    public GameObject TextMesh;

    Dictionary<char, Mesh> letters;

    public void Awake() {
        InitLetters();
    }

    private void InitLetters() {
        letters = new Dictionary<char, Mesh>();
        foreach(Transform t in TextMesh.transform) {
            char letter = (char)int.Parse(t.name);
            letters[letter] = t.GetComponent<MeshFilter>().sharedMesh;
        }
    }

    public GameObject MakeLetter(Wave parent, string text, int index) {
        if(letters == null) {
            InitLetters();
        }
        if(text[index] == ' ') {
            return null;
        }
        float baseOffset = -LETTER_SPACING * text.Length / 2;
        GameObject obj = Instantiate(parent.GetLetterPrefab(text, index)) as GameObject;
        obj.transform.parent = parent.transform;
        obj.transform.localPosition =
            new Vector3(baseOffset + LETTER_SPACING * index, 0, 0);
        obj.transform.rotation = Quaternion.Euler(315, 180, 0);
        obj.name = index.ToString() + "-" + text[index];
        MeshFilter mf = obj.GetComponent<MeshFilter>();
        mf.mesh = letters[text[index]];
        return obj;
    }
}
