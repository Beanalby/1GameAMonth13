using UnityEngine;
using System.Collections;

public abstract class Board : MonoBehaviour {
    
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

    public static Vector3 IndexToCoordinate(int pos) {
        return new Vector3(
            (pos % 3) - 1,
            (pos / 3) - 1,
            0);
    }

    // used to cache the winner, any usage should be through GetWinner()
    private SpotValue _winner = SpotValue.None;
    public SpotValue Winner {
        get { return _winner; }
    }
    protected Spot[] spots;

    public void MadeMove(Spot spot) {
        // force the board to recognize if there's a winner
        CheckWinner();
    }

    public void CheckWinner() {
        SpotValue[] values = new SpotValue[spots.Length];
        bool hasNone = false;
        for(int i = 0; i < spots.Length; i++) {
            values[i] = spots[i].GetValue();
            if(values[i] == SpotValue.None) {
                hasNone = true;
            }
        }
        for(int i = 0; i < winningMoves.GetLength(0); i++) {
            bool xWon = (values[winningMoves[i,0]] == SpotValue.X || values[winningMoves[i,0]] == SpotValue.Tie)
                && (values[winningMoves[i,1]] == SpotValue.X || values[winningMoves[i,1]] == SpotValue.Tie)
                && (values[winningMoves[i,2]] == SpotValue.X || values[winningMoves[i,2]] == SpotValue.Tie);
            if(xWon) {
                _winner = SpotValue.X;
                FoundWinner(winningMoves[i, 0], winningMoves[i, 1],
                    winningMoves[i, 2]);
                return;
            }
            bool yWon = (values[winningMoves[i, 0]] == SpotValue.O || values[winningMoves[i, 0]] == SpotValue.Tie)
                && (values[winningMoves[i, 1]] == SpotValue.O || values[winningMoves[i, 1]] == SpotValue.Tie)
                && (values[winningMoves[i, 2]] == SpotValue.O || values[winningMoves[i, 2]] == SpotValue.Tie);
            if(yWon) {
                _winner = SpotValue.O;
                FoundWinner(winningMoves[i, 0], winningMoves[i, 1],
                    winningMoves[i, 2]);
                return;
            }
        }
        // if there's no spots left to be determined with no winner, it's tied
        if(!hasNone) {
            _winner = SpotValue.Tie;
            FoundWinner(-1, -1, -1);
        }
    }

    public int GetIndex(Spot spot) {
        for(int i=0;i<spots.Length; i++) {
            if(spot == spots[i]) {
                return i;
            }
        }
        throw new System.ArgumentException("spot not found");
    }

    public Spot GetSpot(int index) {
        return spots[index];
    }
    protected abstract void FoundWinner(int move1, int move2, int move3);
}
