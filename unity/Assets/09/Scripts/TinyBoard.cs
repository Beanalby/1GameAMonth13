using UnityEngine;
using System.Collections;

public class TinyBoard : MonoBehaviour {

    public Spot spotPrefab;
    public WinEffect winPrefab;

    private Spot[] spots;

    // used to cache the winner, any usage should be through GetWinner()
    private SpotValue _winner = SpotValue.None;

    private static int[,] winningMoves = new int[8,3] {
        {0, 1, 2},
        {3, 4, 5},
        {6, 7, 8},
        {0, 3, 6},
        {1, 4, 7},
        {2, 5, 8},
        {0, 4, 8},
        {2, 4, 6}
    };

    private Vector3 IndexToCoordinate(int pos) {
        return new Vector3(
            (pos % 3) - 1,
            (pos / 3) - 1,
            0);
    }
    public void Start() {
        spots = new Spot[9];
        GameObject tmp;
        for(int i = 0; i < 9; i++) {
            tmp = Instantiate(spotPrefab.gameObject) as GameObject;
            spots[i] = tmp.GetComponent<Spot>();
            tmp.name = name + "-spot" + i;
            tmp.transform.parent = transform;
            tmp.transform.localPosition = IndexToCoordinate(i);
        }
    }

    public SpotValue GetWinner() {
        // if we've already found a winner, just use that
        if(_winner != SpotValue.None) {
            return _winner;
        }

        SpotValue[] values = new SpotValue[spots.Length];
        for(int i = 0; i < spots.Length; i++) {
            values[i] = spots[i].GetValue();
        }
        for(int i = 0; i < winningMoves.GetLength(0); i++) {
            bool xWon = values[winningMoves[i,0]] == SpotValue.X
                && values[winningMoves[i,1]] == SpotValue.X
                && values[winningMoves[i,2]] == SpotValue.X;
            if(xWon) {
                MadeWinner(winningMoves[i, 0], winningMoves[i, 1],
                    winningMoves[i, 2], SpotValue.X);
                return _winner;
            }
            bool yWon = values[winningMoves[i,0]] == SpotValue.O
                && values[winningMoves[i,1]] == SpotValue.O
                && values[winningMoves[i,2]] == SpotValue.O;
            if(yWon) {
                MadeWinner(winningMoves[i, 0], winningMoves[i, 1],
                    winningMoves[i, 2], SpotValue.O);
                return _winner;
            }
        }
        return SpotValue.None;
    }
    

    public void MadeMove(Spot spot) {
        // force the board to recognize if there's a winner
        GetWinner();
    }

    public void MadeWinner(int move1, int move2, int move3, SpotValue winner) {
        _winner = winner;
        GameObject tmp = GameObject.Instantiate(winPrefab.gameObject) as GameObject;
        tmp.transform.parent = transform;
        tmp.transform.localPosition = Vector3.zero;
        WinEffect we = tmp.GetComponent<WinEffect>();
        we.winner = winner;
        we.pos1 = IndexToCoordinate(move1);
        we.pos2 = IndexToCoordinate(move3);
    }
}
