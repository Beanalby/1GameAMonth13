using UnityEngine;
using System.Collections;
using System.Linq;

public class TinyBoard : Board, Spot {

    public TinySpot spotPrefab;
    public WinEffect winPrefab;

    public void Start() {
        int currentSpot = 0;
        Spot spot;
        spots = new Spot[9];
        foreach(Transform t in transform.Cast<Transform>().OrderBy(t=>t.name)) {
            spot = t.GetComponent<TinySpot>();
            if(spot != null) {
                spots[currentSpot++] = spot;
            }
        }
    }

    protected override void FoundWinner(int move1, int move2, int move3) {
        GameObject tmp = GameObject.Instantiate(winPrefab.gameObject) as GameObject;
        tmp.transform.parent = transform;
        tmp.transform.localPosition = Vector3.zero;
        WinEffect we = tmp.GetComponent<WinEffect>();
        we.winner = Winner;
        we.pos1 = IndexToCoordinate(move1);
        we.pos2 = IndexToCoordinate(move3);
    }

    public SpotValue GetValue() {
        return Winner;
    }
}
