using UnityEngine;
using System.Collections;

public class BigBoard : Board {

    public void Start() {
        spots = new Spot[9];
        for(int i=0;i<transform.childCount; i++) {
            Transform t = transform.GetChild(i);
            spots[i] = t.GetComponent<TinyBoard>();
        }
    }

    protected override void FoundWinner(int move1, int move2, int move3) {
        return;
    }
}
