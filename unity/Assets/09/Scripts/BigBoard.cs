using UnityEngine;
using System.Collections;
using System.Linq;

public class BigBoard : Board {

    public void Start() {
        spots = new Spot[9];
        Spot spot;
        int currentSpot = 0;
        foreach(Transform t in transform.Cast<Transform>().OrderBy(t=>t.name)) {
            spot = t.GetComponent<TinyBoard>();
            if(spot != null) {
                spots[currentSpot++] = spot;
            }
        }
    }

    protected override void FoundWinner(int move1, int move2, int move3) {
        return;
    }
}
