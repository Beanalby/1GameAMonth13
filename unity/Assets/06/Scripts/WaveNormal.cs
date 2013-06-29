using UnityEngine;
using System.Collections;

public class WaveNormal : Wave {
    public GameObject letterPrefab;

    override public GameObject GetLetterPrefab(string text, int index) {
        return letterPrefab;
    }

}
