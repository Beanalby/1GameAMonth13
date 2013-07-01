using UnityEngine;
using System.Collections;

public class WaveKillerWord : Wave {
    public GameObject letterNormalPrefab;
    public GameObject letterKillerPrefab;
    public GameObject letterHealPrefab;

    public bool isKiller = true;

    private int killerWordStart, killerWordEnd;

    public override void Start() {
        ChoosekillerWord();
        base.Start();
    }

    public void ChoosekillerWord() {
        // randomly choose one of the words to be killer
        int killerWord;
        if(text.Length == 0) {
            return;
        }
        int numWords = 1;
        for(int i = 0; i < text.Length; i++) {
            if(text[i] == ' ') {
                numWords++;
            }
        }
        killerWord = Random.Range(0, numWords);

        // find the start and end index of the chosen word
        int currentWord = 0;
        killerWordStart=0;
        while(currentWord < killerWord) {
            if(text[killerWordStart] == ' ') {
                currentWord++;
            }
            killerWordStart++;
        }
        killerWordEnd = killerWordStart;
        while(killerWordEnd < text.Length && text[killerWordEnd] != ' ') {
            killerWordEnd++;
        }
        //int len = killerWordEnd - killerWordStart;
        //Debug.Log("KillerWord #" + killerWord
        //    + " (" + killerWordStart + "," + killerWordEnd + ")"
        //    + " [" + text.Substring(killerWordStart, len) + "] for "
        //    + text);
    }

    public override GameObject GetLetterPrefab(string text, int index) {
        if(index >= killerWordStart && index < killerWordEnd) {
            if(isKiller) {
                return letterKillerPrefab;
            } else {
                return letterHealPrefab;
            }
        } else {
            return letterNormalPrefab;
        }
    }
}
