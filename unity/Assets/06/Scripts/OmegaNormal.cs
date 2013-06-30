using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OmegaNormal : OmegaDriver {

    public override void ShowOmega(int index) {
        base.ShowOmega(index);

        if(index > 2) {
            // choose a live letter to become vulnerable
            List<LetterOmega> alive = new List<LetterOmega>();
            foreach(Transform t in transform) {
                LetterOmega l = t.GetComponent<LetterOmega>();
                if(l.isAlive) {
                    alive.Add(l);
                }
            }
            int letterIndex = Random.Range(0, alive.Count);
            StartCoroutine(alive[letterIndex].MakeVulnerable());
        }
    }
}
