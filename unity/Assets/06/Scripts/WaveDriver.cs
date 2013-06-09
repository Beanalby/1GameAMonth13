using UnityEngine;
using System.Collections;

public class WaveDriver : MonoBehaviour {
    public TextCreator tc;

    GameObject[] letters=null;
    void Update() {
        if(letters == null) {
            letters = tc.GetText(transform, "When dangers near you always hold your stance");
        }
    }


}
