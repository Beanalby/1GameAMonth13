using UnityEngine;
using System.Collections;

public class GameDriver9 : MonoBehaviour {

    private static GameDriver9 _instance;
    public static GameDriver9 instance {
        get { return _instance; }
    }
    private SpotValue currentTurn;
    private TinyBoard currentBoard;

    private int tinySpotMask;
    public BigBoard bigBoard;

    private TinyBoard forcedBoard = null;
    public void Awake() {
        if(_instance != null) {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    public void Start() {
        currentTurn = SpotValue.X;
        tinySpotMask = 1 << LayerMask.NameToLayer("TinySpot");
    }
    public void Update() {
        HandleClick();
    }

    private void HandleClick() {
        if(!Input.GetMouseButtonDown(0)) {
            return;
        }

        // find out if they clicked on anything clickable
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, tinySpotMask)) {
            TryMove(hit.collider.GetComponent<TinySpot>());
        }
    }

    public void MadeMove(TinySpot spot) {
        spot.MakeMark(currentTurn);
        spot.Board.MadeMove(spot);

        // find the next forced board
        int index = spot.Board.GetIndex(spot);
        forcedBoard = bigBoard.GetSpot(index) as TinyBoard;
        // but don't force it if it's already completed
        if(forcedBoard.Winner != SpotValue.None) {
            forcedBoard = null;
        }
        // next turn is opposite player's
        if(currentTurn == SpotValue.X) {
            currentTurn = SpotValue.O;
        } else {
            currentTurn = SpotValue.X;
        }
    }

    private void TryMove(TinySpot spot) {
        // don't change if this spot has a value
        if(spot.GetValue() != SpotValue.None) {
            return;
        }
        // don't change if the spot's board isn't the current forced one
        if(forcedBoard != null && forcedBoard != spot.Board) {
            return;
        }
        //don't change if the board already has a winner
        if(spot.Board.Winner != SpotValue.None) {
            return;
        }

        // no reason not to, apply it
        MadeMove(spot);
    }
}
