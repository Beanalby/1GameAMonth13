using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class BoardController : MonoBehaviour {

    public GameObject squareTemplate;

    public static int BoardSize = 5;
    private float squareX, squareZ;

    private List<SquareController> squares;

    private List<int> silentPattern1, silentPattern2, silentPattern3;

	void Start() {
        squareX = squareTemplate.GetComponent<BoxCollider>().size.x;
        squareZ = squareTemplate.GetComponent<BoxCollider>().size.z;

        squares = new List<SquareController>(BoardSize * BoardSize);
        for(int i = 0; i < BoardSize*BoardSize; i++) {
            squares.Add(((GameObject)GameObject.Instantiate(squareTemplate)).GetComponent<SquareController>());
            squares[i].boardIndex = i;
            squares[i].transform.parent = transform;
        }
        InitSilentPatterns();
        List<List<int>> testBoards = new List<List<int>>() {
            new List<int>() { 0 },
            new List<int>() { 0,1 },
            new List<int>() { 0,2 }
        };

        foreach(List<int> board in testBoards) {
            Debug.Log(IsSolvable(board) + " for " + board);
        }
	}
    void InitSilentPatterns() {
        /* XX.XX
         * .....
         * XX.XX
         * .....
         * XX.XX */
        silentPattern1 = new List<int>() {
            0,  1,  3,  4,
            10, 11, 13, 14,
            20, 21, 23, 24
        };
        /* X.X.X
         * X.X.X
         * .....
         * X.X.X
         * X.X.X */
        silentPattern2 = new List<int>() {
            0,  2,  4,
            5,  7,  9,
            15, 17, 19,
            20, 22, 24
        };
        /* 3 is combination of first two
         * .XXX.
         * X.X.X
         * XX.XX
         * X.X.X
         * .XXX. */
        silentPattern3 = new List<int>() {
            1, 2, 3,
            5, 7, 9,
            10, 11, 13, 14,
            15, 17, 19,
            21, 22, 23
        };
    }

    public Rect GetBounds() {
        return new Rect(0, 0, squareX * BoardSize, squareZ * BoardSize);
    }


    private List<int> Board2list() {
        List<int> boardState = new List<int>();
        foreach(SquareController sc in squares) {
            if(sc.IsCorrupted) {
                boardState.Add(sc.boardIndex);
            }
        }
        return boardState;
    }
    public bool IsSolvable(List<int> boardState) {
        /* a board is unsolvable if either of the silent patterns
         * have an odd number marked. */
        if(boardState.Intersect(silentPattern1).Count() % 2 != 0) {
            return false;
        }
        if(boardState.Intersect(silentPattern2).Count() % 2 != 0) {
            return false;
        }
        return true;
    }

    public void SquareHit(SquareController sc) {
        sc.Toggle();
        // toggle any additional squares around them
        if(sc.boardY > 0)
            squares[sc.boardIndex-1].Toggle();
        if(sc.boardY < BoardSize - 1)
            squares[sc.boardIndex+1].Toggle();
        if(sc.boardX > 0)
            squares[sc.boardIndex-BoardSize].Toggle();
        if(sc.boardX < BoardSize - 1)
            squares[sc.boardIndex+BoardSize].Toggle();
    }

    public void OnDrawGizmos() {
        Vector3 size = squareTemplate.GetComponent<BoxCollider>().size;
        Gizmos.color = Color.green / 2;
        for(int i = 0; i < BoardSize; i++) {
            for(int j = 0; j < BoardSize; j++) {
                Vector3 pos = new Vector3((i + .5f) * size.x, -.5f * size.y, (j + .5f) * size.z);
                Gizmos.DrawWireCube(pos, size);
            }
        }
    }
}
