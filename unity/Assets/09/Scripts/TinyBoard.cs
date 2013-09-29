using UnityEngine;
using System.Collections;

public class TinyBoard : Board, Spot {

    public TinySpot spotPrefab;
    public WinEffect winPrefab;

    public void Start() {
        spots = new Spot[9];
        GameObject tmp;
        for(int i = 0; i < 9; i++) {
            tmp = Instantiate(spotPrefab.gameObject) as GameObject;
            spots[i] = tmp.GetComponent<TinySpot>();
            tmp.name = name + "-spot" + i;
            tmp.transform.parent = transform;
            tmp.transform.localPosition = IndexToCoordinate(i);
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
