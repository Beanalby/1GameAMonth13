using UnityEngine;
using System.Collections;

public class TinySpot : Spot {

    private SpotValue value = SpotValue.None;
    public GameObject prefabX;
    public GameObject prefabO;

    private GameObject mark = null;
    private TinyBoard board;

    public void Start() {
        board = transform.parent.GetComponent<TinyBoard>();
    }

    public override SpotValue GetValue() {
        return value;
    }

    public void Chosen(SpotValue currentTurn) {
        // don't change if this spot has a value, or if the board has a winner
        if(value != SpotValue.None) {
            return;
        }
        if(board.GetWinner() != SpotValue.None) {
            return;
        }
        value = currentTurn;
        MakeMark();
        GameDriver9.instance.SendMessage("MadeMove", this);
        transform.parent.SendMessage("MadeMove", this);
    }

    private void MakeMark() {
        if(value == SpotValue.X) {
            mark = Instantiate(prefabX) as GameObject;
        } else {
            mark = Instantiate(prefabO) as GameObject;
        }
        mark.transform.parent = transform;
        mark.transform.localPosition = Vector3.zero;
    }
}
