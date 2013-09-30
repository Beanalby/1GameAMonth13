using UnityEngine;
using System.Collections;

public class GameDriver9 : MonoBehaviour {

    private static GameDriver9 _instance;
    public static GameDriver9 instance {
        get { return _instance; }
    }
    private SpotValue currentTurn;
    public SpotValue CurrentTurn {
        get { return currentTurn; }
    }
    private TinyBoard currentBoard;
    public HighlightBoard highlight;

    private bool isPlaying = true;
    public bool IsPlaying {
        get { return isPlaying; }
    }

    private int tinySpotMask;
    public BigBoard bigBoard;

    private TinyBoard forcedBoard = null;

    private bool didDebug = false;
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
        HandleDebug();
        HandleClick();
    }

    public TinySpot GetHovered() {
        // find out if they clicked on anything clickable
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, tinySpotMask)) {
            TinySpot spot = hit.collider.GetComponent<TinySpot>();
            return spot;
        }
        return null;
    }
    private void HandleClick() {
        if(!Input.GetMouseButtonDown(0)) {
            return;
        }
        TinySpot spot = GetHovered();
        if(IsValidMove(spot)) {
            MakeMove(spot);
        }
    }

    private void HandleDebug() {
        if(didDebug) {
            return;
        }
        didDebug = true;
        // make some moves for testing
        {
            // tie board
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(0)).GetSpot(0));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(0)).GetSpot(1));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(0)).GetSpot(2));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(0)).GetSpot(3));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(0)).GetSpot(4));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(0)).GetSpot(8));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(0)).GetSpot(7));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(0)).GetSpot(6));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(0)).GetSpot(5));

            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(3)).GetSpot(0));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(4)).GetSpot(1));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(3)).GetSpot(3));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(4)).GetSpot(4));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(3)).GetSpot(6));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(4)).GetSpot(7));

            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(6)).GetSpot(0));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(7)).GetSpot(1));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(6)).GetSpot(3));
            MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(7)).GetSpot(4));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(6)).GetSpot(6));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(7)).GetSpot(7));
        }

        {
            // 1 move from victory
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(3)).GetSpot(0));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(4)).GetSpot(1));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(3)).GetSpot(3));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(4)).GetSpot(4));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(3)).GetSpot(6));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(4)).GetSpot(7));

            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(0)).GetSpot(0));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(1)).GetSpot(1));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(0)).GetSpot(3));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(1)).GetSpot(4));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(0)).GetSpot(6));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(1)).GetSpot(7));

            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(6)).GetSpot(0));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(7)).GetSpot(1));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(6)).GetSpot(3));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(7)).GetSpot(4));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(6)).GetSpot(6));
            //MakeMove((TinySpot)((TinyBoard)bigBoard.GetSpot(7)).GetSpot(7));
        }
    }

    public bool IsValidMove(TinySpot spot) {
        // nothing's valid if we're not playing!
        if(!isPlaying) {
            return false;
        }
        // don't change if this spot has a value
        if(spot.GetValue() != SpotValue.None) {
            return false;
        }
        // don't change if the spot's board isn't the current forced one
        if(forcedBoard != null && forcedBoard != spot.Board) {
            return false;
        }
        //don't change if the board already has a winner
        if(spot.Board.Winner != SpotValue.None) {
            return false;
        }
        return true;
    }

    public void MakeMove(TinySpot spot) {
        spot.MakeMark(currentTurn);
        spot.Board.MadeMove(spot);

        // check if this ends the game
        bigBoard.CheckWinner();
        if(bigBoard.Winner != SpotValue.None) {
            switch(bigBoard.Winner) {
                case SpotValue.X:
                    Debug.Log("Game Over, X won!"); break;
                case SpotValue.O:
                    Debug.Log("Game Over, O won!"); break;
                case SpotValue.Tie:
                    Debug.Log("Game Over, TIED!"); break;
            }
            isPlaying = false;
        } else {
            // find the next forced board
            int index = spot.Board.GetIndex(spot);
            TinyBoard targetBoard = bigBoard.GetSpot(index) as TinyBoard;
            forcedBoard = targetBoard;
            // but don't force it if it's already completed
            if(targetBoard.Winner != SpotValue.None) {
                forcedBoard = null;
            }
            highlight.Highlight(currentTurn, spot, targetBoard,
                targetBoard == forcedBoard);
            // next turn is opposite player's
            if(currentTurn == SpotValue.X) {
                currentTurn = SpotValue.O;
            } else {
                currentTurn = SpotValue.X;
            }
        }
    }
}
